using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.EditText;
using ARM_SHN.Interface;
using ARM_SHN.Enums;
using ARM_SHN.Constant;
using ARM_SHN.WorkWindow;
using ARM_SHN.Delegate;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.ImpulsClient;

namespace ARM_SHN.ElementControl
{
    /// <summary>
    /// класс описывающий станционный путь
    /// </summary>
    public class StationPath : BaseTextGraficElement, ISelectElement, IInfoElement,  IIndicationEl, IText
    {
        #region Colors
        //////цветовая палитра
        /// <summary>
        /// цвет огражденного пути
        /// </summary>
        public static Brush _colorfencing = Brushes.SaddleBrown;
        /// <summary>
        /// цвет не занятого пути
        /// </summary>StationTransition
        public static Brush _colorpassiv = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет  занятого пути
        /// </summary>
        public static Brush _coloractiv = Brushes.Red;
        /// <summary>
        /// цвет неконтролируемого  пути
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(230, 230, 230));
        /// <summary>
        /// цвет неконтролируемого  пути его рамка
        /// </summary>
        public static Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(205, 205, 205));
        /// <summary>
        /// цвет пути при замыкании
        /// </summary>
        public static Brush _color_loking = Brushes.LightGreen;
        /// <summary>
        /// цвет пути при замыкании маневровом
        /// </summary>
        public static Brush _color_lokingM = Brushes.White;
        /// <summary>
        /// цвет пути при замыкании аварийном
        /// </summary>
        public static Brush _color_lokingY = Brushes.Yellow;
        /// <summary>
        /// цвет  пути с автономной тягой
        /// </summary>
        public static Brush m_colordiesel_traction = Brushes.Black;
        /// <summary>
        /// цвет контура пути с электрической тягой
        /// </summary>
        public static Brush _colorelectric_traction = Brushes.Blue;
        /// <summary>
        /// цвет текста для номера поезда
        /// </summary>
        public static Brush _color_train = Brushes.Yellow;
        /// <summary>
        /// цвет текста для номера поезда при наличии плана
        /// </summary>
        public static Brush _color_train_plan = Brushes.Blue;
        /// <summary>
        /// цвет текста для названия главного пути
        /// </summary>
        public static Brush _color_path = Brushes.Black;
        /// <summary>
        /// цвет текста для номера поезда если есть вектор (роспуска, выставления, транзита)
        /// </summary>
        public static Brush _color_vertor_train = Brushes.Blue;

        #endregion
        //////
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement > Impulses { get; set; }

        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.fencing, Viewmode.occupation, Viewmode.locking, Viewmode.lockingM, Viewmode.lockingY};
        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>() { Viewmode.electrification};

        public string NameUl
        {
            get
            {
                return NameObject;
            }
        }

        public string FileClick { get; set; } = string.Empty;

        public int StationTransition { get; set; }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="text">название объекта</param>
        public StationPath(PathGeometry geometry, string text, string name, double marginX, double marginY, double fontsize, 
                            double rotate, Dictionary<Viewmode, StateElement> impulses):
             base(name, ViewElement.chiefroad, geometry, rotate)
        {
            ViewModel.Text = text;
            ViewModel.Foreground = _color_path;
            ViewModel.FontSize = fontsize;
            ViewModel.Margin = new Thickness(marginX, marginY, 0, 0);
            ViewModel.RenderTransform = new RotateTransform(RotateText);
            ViewModel.StrokeThickness = LoadProject.ProejctGrafic.Scroll * SystemParameters.CaretWidth;
            Impulses = AnalisCollectionStateControl(impulses);
            Analis();
            //
            LoadColorControl.NewColor += NewColor;
        }

        public override void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
        }

        /// <summary>
        /// анализируем коллекцию 
        /// </summary>
        /// <param name="impulses"></param>
        private Dictionary<Viewmode, StateElement> AnalisCollectionStateControl(Dictionary<Viewmode, StateElement> impulses)
        {
            foreach (var control in impulses)
            {
                //смотрим с каким графическим объектом работает контроль
                foreach (var stroke_element in _priority_stroke)
                {
                    if (stroke_element == control.Value.Name)
                    {
                        control.Value.ViewControlDraw = ViewElementDraw.stroke;
                        break;
                    }
                }
            }
            //
            return impulses;
        }

        public string InfoElement()
        {
            return Notes;
        }

        private void NewColor()
        {
            UpdateElement(false);
            //
            if (ViewModel.Text == NameObject)
                ViewModel.Foreground = _color_path;
        }

        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (ImpulsesClientTCP.Connect)
                    //GetColorStroke();
                    ViewModel.Stroke = m_colordiesel_traction;
                else
                {
                    foreach (var imp in Impulses)
                    {
                        imp.Value.state = StatesControl.nocontrol;
                        imp.Value.LastUpdate = DateTime.Now;
                    }
                    //обнуляем значения
                    ViewModel.Stroke = _colornotcontrolstroke;
                    ViewModel.Fill = _colornotcontrol;
                }
            }
        }

        public IList<string> Analis()
        {
            bool update = false;
            var result = new List<string>();
            foreach (var Imp in Impulses)
            {
                if (Imp.Value.Name != Viewmode.electrification)
                {
                    var state = Imp.Value.state;
                    Imp.Value.state = Connections.ClientImpulses.Data.GetStateControl(StationControl, Imp.Value.Impuls);
                    //
                    if (state != Imp.Value.state)
                    {
                        Imp.Value.Update = true;
                        update = true;
                        result.AddRange(Diagnostic.DiagnosticControl(Imp.Value));
                    }
                }
            }
            //обновляем элемент
            if (update)
                UpdateElement(true);

            //
            return result;
        }



        private void UpdateCurrentState(List<Viewmode> list_priority, ref bool update)
        {
            var control = CheckPriorityState(list_priority);
            if (control != null)
                SetState(control);
            else
            {
                foreach (var mode in list_priority)
                {
                    if (Impulses.TryGetValue(mode, out var imp))
                    {
                        SetState(imp);
                        break;
                    }
                }
            }
            update = true;
        }


        /// <summary>
        /// Изменяем цвет элемента
        /// </summary>
        private void UpdateElement(bool CheckUpdate)
        {
            if (Impulses.Count > 0)
            {
                bool _update_fill = false;
                bool _update_stroke = false;
                //
                foreach (var imp in Impulses)
                {
                    if ((CheckUpdate && imp.Value.Update) || !CheckUpdate)
                    {
                        switch (imp.Value.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                if (!_update_fill)
                                    UpdateCurrentState(_priority_fill, ref _update_fill);
                                break;
                            case ViewElementDraw.stroke:
                                if (!_update_stroke)
                                    UpdateCurrentState(_priority_stroke, ref _update_stroke);
                                break;
                        }
                    }
                    //
                    if (CheckUpdate && imp.Value.Update)
                        imp.Value.Update = false;
                }
                //при изменении цветовой расскраски
                if (!CheckUpdate)
                {
                    if (!_update_stroke)
                        ViewModel.Stroke = m_colordiesel_traction;
                    if (!_update_fill)
                        ViewModel.Fill = _colornotcontrol;
                }
            }
            else
            {
                if (!CheckUpdate)
                {
                    ViewModel.Stroke = _colornotcontrolstroke;
                    ViewModel.Fill = _colornotcontrol;
                }
            }
        }


        /// <summary>
        /// находим более приоритетное состояние из представленного перечьня
        /// </summary>
        /// <param name="priority_control">перечень контролей в порядке убывания приоритета</param>
        /// <returns></returns>
        private StateElement CheckPriorityState(List<Viewmode> priority_control)
        {
            foreach (var control in priority_control)
            {
                if (Impulses.TryGetValue(control, out var imp))
                {
                    if (imp.state == StatesControl.activ)
                        return imp;
                }
            }
            return null;
        }

        /// <summary>
        /// устанавливаем активное состояние для различных контролей
        /// </summary>
        /// <param name="index"></param>
        private void SetState(StateElement control)
        {
            if (control != null)
            {
                switch (control.state)
                {
                    case StatesControl.activ:
                        switch (control.Name)
                        {
                            case Viewmode.fencing:
                                ViewModel.Fill = _colorfencing;
                                break;
                            case Viewmode.occupation:
                                ViewModel.Fill = _coloractiv;
                                break;
                            case Viewmode.locking:
                                ViewModel.Fill = _color_loking;
                                break;
                            case Viewmode.lockingM:
                                ViewModel.Fill = _color_lokingM;
                                break;
                            case Viewmode.lockingY:
                                ViewModel.Fill = _color_lokingY;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                ViewModel.Fill = _colorpassiv;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                ViewModel.Fill = _colornotcontrol;
                                break;;
                        }
                        break;
                }
            }
        }
    }
}
