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

namespace ARM_SHN.ElementControl
{
    /// <summary>
    /// класс описывающий разъединители
    /// </summary>
    class Disconnectors : BaseGraficElement, ISelectElement, IInfoElement, IIndicationEl
    {

        #region Переменные и свойства
     
        ///////

        //////цветовая палитра
        /// <summary>
        /// цвет  занятия 
        /// </summary>
        public static Brush m_colorOccupation = Brushes.Red;
        /// <summary>
        /// цвет  замыкания
        /// </summary>
        public static Brush m_colorLocking = Brushes.Green;
        /// <summary>
        /// цвет неконтролируемого  элемента
        /// </summary>
        public static Brush m_colornotcontrol = Brushes.White;
        /// <summary>
        /// цвет неконтролируемого элемента его рамка
        /// </summary>
        public static Brush m_colornotcontrolstroke = Brushes.Black;
        /// <summary>
        /// цвет фона по умолчанию
        /// </summary>
        public static Brush m_colorDefult = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет рамки по умолчанию
        /// </summary>
        public static Brush m_colorStrokeDefult = Brushes.Black;
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement> Impulses { get; set; }
        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.occupation, Viewmode.locking };
        /// <summary>
        /// приоритет отображения рамки
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>();

        public string NameUl
        {
            get
            {
                return NameObject;
            }
        }
        public int StationTransition { get; set; }
        public string FileClick { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public Disconnectors(PathGeometry geometry, string name, Dictionary<Viewmode, StateElement> impulses, TypeDisconnectors type):
             base(name, ViewElement.disconnectors, geometry)
        {
            ViewModel.StrokeThickness = (1.3 * SystemParameters.CaretWidth * LoadProject.ProejctGrafic.Scroll);
            Impulses = AnalisCollectionStateControl(impulses);
            if (Impulses.Count == 0)
            {
                ViewModel.Stroke = m_colornotcontrolstroke;
                ViewModel.Fill = m_colornotcontrol;
            }
            //обработка импульсов
            if (!string.IsNullOrEmpty(NameObject) && Impulses.Count > 0)
                Analis();
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
            UpdateColor();
        }


        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (!ImpulsesClientTCP.Connect)
                {
                    foreach (var imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    //
                    ViewModel.Stroke = m_colornotcontrolstroke;
                    ViewModel.Fill = m_colornotcontrol;
                }
                else
                    ViewModel.Stroke = m_colorStrokeDefult;
            }
        }

        public IList<string> Analis()
        {
            var update = false;
            var result = new List<string>();
            foreach (var Imp in Impulses)
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
                foreach (Viewmode mode in list_priority)
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
                var _update_fill = false;
                var _update_stroke = false;
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
                        ViewModel.Stroke = m_colornotcontrolstroke;
                    if (!_update_fill)
                        ViewModel.Fill = m_colornotcontrol;
                }
            }
            else
            {
                if (!CheckUpdate)
                {
                    ViewModel.Stroke = m_colornotcontrolstroke;
                    ViewModel.Fill = m_colornotcontrol;
                }
            }
        }

        private void UpdateColor()
        {
            UpdateElement(false);
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
                            case Viewmode.occupation:
                                ViewModel.Fill = m_colorOccupation;
                                break;
                            case Viewmode.locking:
                                ViewModel.Fill = m_colorLocking;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                ViewModel.Fill = m_colorDefult;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                ViewModel.Fill = m_colornotcontrol;
                                break; 
                        }
                        break;
                }
            }
        }

    }
}
