﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.Interface;
using ARM_SHN.WorkWindow;
using ARM_SHN.EditText;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.ImpulsClient;

namespace ARM_SHN.ElementControl
{
    /// <summary>
    /// класс индикации шильд
    /// </summary>
    class HelpElement : BaseTextGraficElement, ISelectElement, IText, IIndicationEl
    {
        #region Переменные и свойства

        //////основные свойства пути
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет фона по умолчанию
        /// </summary>
        public static Brush _color_fon_defult = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет рамки по умолчанию
        /// </summary>
        public static Brush _color_stroke_defult = Brushes.Black;
        /// <summary>
        /// цвет  при контроле красном
        /// </summary>
        public static Brush _colorRed = Brushes.Red;
        /// <summary>
        /// цвет  при контроле Желтом
        /// </summary>
        public static Brush _colorYellow = Brushes.Yellow;
        /// <summary>
        /// цвет  при контроле белом
        /// </summary>
        public static Brush _colorWhite = Brushes.White;
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush _colortext = Brushes.Black;
        /// <summary>
        /// цвет неконтролируемого  элемента
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        /// <summary>
        /// цвет неконтролируемого элемента его рамка
        /// </summary>
        public static Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(205, 205, 205));
        //////
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement> Impulses { get; set; }
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1 * SystemParameters.CaretWidth;

        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.controlRedF, Viewmode.controlYellowF, Viewmode.controlRed, Viewmode.controlYellow, Viewmode.controlWhite };

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
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public HelpElement(PathGeometry geometry, string text, string name, double marginX, double marginY, double fontsize, double rotate, Dictionary<Viewmode, StateElement> impulses) :
                  base(name, ViewElement.help_element, geometry, rotate)
        {
            ViewModel.Text = name;
            ViewModel.Foreground = _colortext;
            //ViewModel.FontSize = fontsize;
            //ViewModel.Margin = new Thickness(marginX, marginY, 0, 0);
            ViewModel.RenderTransform = new RotateTransform(RotateText);
            LocationText();
            ViewModel.StrokeThickness = (LoadProject.ProejctGrafic.Scroll * SystemParameters.CaretWidth);
            //
            Impulses = impulses;
            Analis();
            //обработка информации по импульсам и номерам поездов
            LoadColorControl.NewColor += NewColor;
            Connections.NewTart += StartFlashing;
        }

        public override void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
            Connections.NewTart -= StartFlashing;
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
            //проверяем иоргать ли фоном
            StateElement control_fill = CheckPriorityState(_priority_fill);
            if (control_fill != null)
                FlashingControl(SetColorsFlashing(control_fill.Name), control_fill);
        }

        private void FlashingControl(List<Brush> brushes, StateElement control)
        {
            if (brushes.Count == 2)
            {
                if (Connections.Taktupdate)
                {
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.fill:
                            ViewModel.Fill = brushes[0];
                            break;
                    }
                }
                else
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.fill:
                            ViewModel.Fill = brushes[1];
                            break;
                    }
            }
        }

        private static List<Brush> SetColorsFlashing(Viewmode mode)
        {
            switch (mode)
            {
                case Viewmode.controlRedF:
                    return new List<Brush> { _colorRed, _color_fon_defult };
                case Viewmode.controlYellowF:
                    return new List<Brush> { _colorRed, _color_fon_defult };
            }
            //
            return new List<Brush>();
        }

        private void NewColor()
        {
            UpdateElement(false);
        }

        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (!ImpulsesClientTCP.Connect)
                {
                    foreach (var imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    ViewModel.Stroke = _colornotcontrolstroke;
                    ViewModel.Fill = _colornotcontrol;

                }
                else
                    ViewModel.Stroke = _color_stroke_defult;
            }
        }

        public IList<string> Analis()
        {
            bool update = false;
            foreach (var Imp in Impulses)
            {
                var state = Imp.Value.state;
                Imp.Value.state = Connections.ClientImpulses.Data.GetStateControl(StationControl, Imp.Value.Impuls);
                //
                if (state != Imp.Value.state)
                {
                    Imp.Value.Update = true;
                    update = true;
                }
            }
            //обновляем элемент
            if (update)
                UpdateElement(true);
            //
            return new List<string>();
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
                        ViewModel.Stroke = _colornotcontrolstroke;
                    if (!_update_fill)
                        ViewModel.Fill = _colornotcontrol;
                    ViewModel.Foreground = _colortext;
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
            {
                switch (control.state)
                {
                    case StatesControl.activ:
                        switch (control.Name)
                        {
                            case Viewmode.controlRed:
                                ViewModel.Fill = _colorRed;
                                break;
                            case Viewmode.controlYellow:
                                ViewModel.Fill = _colorYellow;
                                break;
                            case Viewmode.controlWhite:
                                ViewModel.Fill = _colorWhite;
                                break;
                            case Viewmode.controlRedF:
                                ViewModel.Fill = _colorRed;
                                break;
                            case Viewmode.controlYellowF:
                                ViewModel.Fill = _colorYellow;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                ViewModel.Fill = _color_fon_defult;
                                break;
                                //case ViewElementDraw.stroke:
                                //    Stroke = _color_stroke_defult;
                                //    break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                ViewModel.Fill = _colornotcontrol;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
