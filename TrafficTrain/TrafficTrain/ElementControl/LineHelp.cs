using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using ARM_SHN.Interface;
using ARM_SHN.WorkWindow;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.ImpulsClient;
using System.Windows.Media.Media3D;

namespace ARM_SHN.ElementControl
{
    /// <summary>
    /// класс описывающий вспомагательную линию
    /// </summary>
    class LineHelp : BaseGraficElement, IInfoElement, ISelectElement, IIndicationEl
    {
        #region Переменные и свойства
        /// <summary>
        /// шестизначный номер станции справа
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement> Impulses { get; set; }

        //////цветовая палитра
        /// <summary>
        /// цвет линии по умолчанию, если она не активна
        /// </summary>
        Brush _colordefultlinehelp = Brushes.SaddleBrown;
        /// <summary>
        /// цвет замыкания
        /// </summary>
        public static Brush _colorlocking = Brushes.LightGreen;
        /// <summary>
        /// цвет замыкания маневровый
        /// </summary>
        public static Brush _colorlockingM = Brushes.White;
        /// <summary>
        /// цвет замыкания аварийный
        /// </summary>
        public static Brush _colorlockingY = Brushes.Yellow;
        /// <summary>
        /// цвет не занятого элемента
        /// </summary>
        public static Brush _color_pasive = new SolidColorBrush(Color.FromRgb(100, 100, 100));
        /// <summary>
        /// цвет  занятия
        /// </summary>
        public static Brush _color_active = Brushes.Red;
        /// <summary>
        /// цвет  ограждения
        /// </summary>
        public static Brush _colorfencing = Brushes.SaddleBrown;
        /// <summary>
        /// цвет неконтролируемого элемента
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(205, 205, 205));
        /// <summary>
        /// цвет проезда
        /// </summary>
        public static Brush _color_passage = Brushes.Black;
        /// <summary>
        /// цвет искуственной разделки первый
        /// </summary>
        public static Brush _color_cutting_one =  Brushes.LightGreen;
        /// <summary>
        /// цвет искуственной разделки второй
        /// </summary>
        public static Brush _color_cutting_ty = Brushes.Black;
         /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>() { Viewmode.occupation, Viewmode.cutting, Viewmode.locking, Viewmode.lockingM, Viewmode.lockingY, Viewmode.passage };

        private IDictionary<StatesControl, string> m_messages = new Dictionary<StatesControl, string>();

        private bool m_visible = true;

        public new bool IsVisible
        {
            get
            {
                return m_visible;
            }
        }

        public string NameUl
        {
            get
            {
                return NameObject;
            }
        }

        public string FileClick { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public LineHelp(PathGeometry geometry, double weight, string namecolor, string name, Dictionary<Viewmode, StateElement> impulses, bool isVisible):
            base(name, ViewElement.line, geometry, false)
        {
            Impulses = AnalisCollectionStateControl(impulses);
            m_visible = isVisible;
            NewColor();
            if (Impulses.Count > 0)
            {
                //обработка информации по импульсам и номерам поездов
                Connections.NewTart += StartFlashing;
            }
            //
            if (weight < 1)
                weight = LoadProject.ProejctGrafic.Scroll * SystemParameters.CaretWidth;
            ViewModel.StrokeThickness = weight;
            LoadColorControl.NewColor += NewColor;
        }

        public override void Dispose()
        {
            if (Impulses.Count > 0)
                Connections.NewTart -= StartFlashing;
            //
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

        private void StartFlashing()
        {
            Flashing();
        }
        /// <summary>
        /// обрабатываем мирцание
        /// </summary>
        private void Flashing()
        {
            //проверять ли рамкой
            StateElement control_stroke = CheckPriorityState(_priority_stroke);
            if (control_stroke != null)
                FlashingControl(SetColorsFlashing(control_stroke.Name), control_stroke);
        }

        private void FlashingControl(List<Brush> brushes, StateElement control)
        {
            if (brushes.Count == 2)
            {
                if (Connections.Taktupdate)
                {
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.stroke:
                            ViewModel.Stroke = brushes[0];
                            break;
                    }
                }
                else
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.stroke:
                            ViewModel.Stroke = brushes[1];
                            break;
                    }
            }
        }

        private static List<Brush> SetColorsFlashing(Viewmode mode)
        {
            switch (mode)
            {
                case Viewmode.cutting:
                    return new List<Brush> { _color_cutting_one, _color_pasive };
            }
            //
            return new List<Brush>();
        }

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

        public string InfoElement()
        {
            return Notes;
        }


        public IList<string> Analis()
        {
            bool update = false;
            var result = new List<string> ();
            foreach (var Imp in Impulses)
            {
                var state = Imp.Value.state;
                Imp.Value.state = Connections.ClientImpulses.Data.GetStateControl(StationControl, Imp.Value.Impuls);
                //
                if (state != Imp.Value.state)
                {
                    Imp.Value.Update = true;
                    update = true;
                    if (Imp.Key == Viewmode.passage)
                    {
                        if (Imp.Value.state == StatesControl.activ)
                            ViewModel.ZIndex = int.MaxValue;
                        else
                            ViewModel.ZIndex = int.MinValue;
                    }
                    //
                     result.AddRange(Diagnostic.DiagnosticControl(Imp.Value));
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
                bool _update_stroke = false;
                //
                foreach (var imp in Impulses)
                {
                    if ((CheckUpdate && imp.Value.Update) || !CheckUpdate)
                    {
                        switch (imp.Value.ViewControlDraw)
                        {
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
                        ViewModel.Stroke = _colornotcontrol;
                }
            }
            else
            {
                if (!CheckUpdate)
                    ViewModel.Stroke = _colornotcontrol;
            }
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
                            case Viewmode.locking:
                                ViewModel.Stroke = _colorlocking;
                                break;
                            case Viewmode.lockingM:
                                ViewModel.Stroke = _colorlockingM;
                                break;
                            case Viewmode.lockingY:
                                ViewModel.Stroke = _colorlockingY;
                                break;
                            case Viewmode.occupation:
                                ViewModel.Stroke = _color_active;
                                break;
                            case Viewmode.passage:
                                ViewModel.Stroke = _color_pasive;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.stroke:
                                if (Impulses.ContainsKey(Viewmode.passage))
                                    ViewModel.Stroke = _color_passage;
                                else ViewModel.Stroke = _color_pasive;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.stroke:
                                ViewModel.Stroke = _colornotcontrol;
                                break;
                        }
                        break;
                }
            }
        }

        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (!ImpulsesClientTCP.Connect)
                {
                    foreach (var imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    ViewModel.Stroke = _colornotcontrol;
                }
            }
        }

        private void NewColor()
        {
            if (Impulses.Count > 0)
                UpdateElement(false);
            else
            {
                if (IsVisible)
                    ViewModel.Stroke = _colordefultlinehelp;
                else
                    ViewModel.Stroke = _color_active;
            }
        }
    }
}
