﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.ImpulsClient;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий переезд на основной схеме
    /// </summary>
    class Moves : Shape, IGraficElement, ISelectElement, IInfoElement, IDisposable
    {
        #region Переменные и свойства
        /////геометрия элемента
        /// <summary>
        /// отрисовываемая геометрия
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                return _figure;
            }
        }
        private PathGeometry _figure = new PathGeometry();
        /// <summary>
        /// геометрическое иписание фигуры
        /// </summary>
        public PathGeometry Figure
        {
            get
            {
                return _figure;
            }
            set
            {
                _figure = value;
            }
        }
        ///////
        //////основные свойства переезда
        /// <summary>
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// название переезда
        /// </summary>
        public string NameMove { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет открытого переезда
        /// </summary>
        public static Brush _color_moveopen = Brushes.Black;
        /// <summary>
        /// цвет  закрытого переезда  автоматически 
        /// </summary>
        public static Brush _color_closingmove_auto = Brushes.Red;
        /// <summary>
        /// цвет  закрытого переезда кнопкой 
        /// </summary>
        public static Brush _color_closingmove_button = Brushes.Purple;
        /// <summary>
        /// цвет аварии на переезде
        /// </summary>
        public static Brush _color_accident = Brushes.Red;
        /// <summary>
        /// цвет неисправного переезда
        /// </summary>
        public static Brush _color_faultmove = Brushes.Yellow;
        /// <summary>
        /// цвет неконтролируемого  пути
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        /// <summary>
        /// цвет неконтролируемого  пути его рамка
        /// </summary>
        public static Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(205, 205, 205));
        /// <summary>
        /// цвет фона по умолчанию
        /// </summary>
        public static Brush _color_fon_defult = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        //////
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement> Impulses { get; set; }
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1.3 * SystemParameters.CaretWidth;
        /// <summary>
        /// коллекция используемых линий
        /// </summary>
        List<Line> _lines = new List<Line>();
        public List<Line> Lines
        {
            get
            {
                return _lines;
            }
        }
        /// <summary>
        /// коллекция используемых точек
        /// </summary>
        PointCollection _points = new PointCollection();
        public PointCollection Points
        {
            get
            {
                return _points;
            }
        }
        /// <summary>
        /// центр фигуры
        /// </summary>
        Point _pointCenter = new Point();
        public Point PointCenter
        {
            get
            {
                return _pointCenter;
            }
        }
        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.closing_button, Viewmode.closing };
        /// <summary>
        /// приоритет отображения рамки
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>() { Viewmode.accident, Viewmode.fault };
        /// <summary>
        /// Пояснения
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }

        public string NameUl
        {
            get
            {
                return NameMove;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public Moves(int stationnumber, PathGeometry geometry, string name, Dictionary<Viewmode, StateElement> impulses)
        {
            StationTransition = stationnumber;
            NameMove = name;
            Impulses = AnalisCollectionStateControl(impulses);
            GeometryFigureCopy(geometry);
            Analis();
            //обработка импульсов
            //ImpulsesClient.NewData += NewInfomation;
            //ImpulsesClient.ConnectDisconnectionServer += ConnectCloseServer;
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            //ImpulsesClient.NewData -= NewInfomation;
            //ImpulsesClient.ConnectDisconnectionServer -= ConnectCloseServer;
            LoadColorControl.NewColor -= NewColor;
        }

        /// <summary>
        /// анализируем коллекцию 
        /// </summary>
        /// <param name="impulses"></param>
        private Dictionary<Viewmode, StateElement> AnalisCollectionStateControl(Dictionary<Viewmode, StateElement> impulses)
        {
            foreach (KeyValuePair<Viewmode, StateElement> control in impulses)
            {
                //смотрим с каким графическим объектом работает контроль
                foreach (Viewmode stroke_element in _priority_stroke)
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
            return string.Format("Переезд - {0} {1}", NameMove, Notes);
        }

        private void NewColor()
        {
            UpdateColor();
        }

        private void StartFlashing()
        {
            Dispatcher.Invoke(new Action(() => Flashing()));
        }
        /// <summary>
        /// мигание светофора
        /// </summary>
        private void Flashing()
        {
            //проверяем иоргать ли фоном
            StateElement control_fill = CheckPriorityState(_priority_fill);
            if (control_fill != null)
                FlashingControl(SetColorsFlashing(control_fill.Name), control_fill);
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
                        case ViewElementDraw.fill:
                            Fill = brushes[0];
                            break;
                        case ViewElementDraw.stroke:
                            Stroke = brushes[0];
                            break;
                    }
                }
                else
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.fill:
                            Fill = brushes[1];
                            break;
                        case ViewElementDraw.stroke:
                            Stroke = brushes[1];
                            break;
                    }
            }
        }

        private List<Brush> SetColorsFlashing(Viewmode mode)
        {
            switch (mode)
            {
                case Viewmode.fault:
                    return new List<Brush> { _color_faultmove, _color_moveopen };
                case Viewmode.closing_button:
                    return new List<Brush> { _color_closingmove_auto, _color_fon_defult };
                case Viewmode.accident:
                    return new List<Brush> { _color_accident, _color_moveopen };
            }
            //
            return new List<Brush>();
        }

        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry)
        {
            int index = 0;
            foreach (PathFigure geo in geometry.Figures)
            {
                PathFigure newfigure = new PathFigure() { IsClosed = true };
                newfigure.StartPoint = new Point(geo.StartPoint.X, geo.StartPoint.Y);
                if (index == 0)
                    newfigure.IsFilled = false;
                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                    {
                        newfigure.Segments.Add(new LineSegment() { Point = new Point(lin.Point.X, lin.Point.Y) });
                        continue;
                    }
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                    {
                        newfigure.Segments.Add(new ArcSegment() { Point = new Point(arc.Point.X, arc.Point.Y), Size = new Size(arc.Size.Width, arc.Size.Height) });
                        continue;
                    }
                }
                _figure.Figures.Add(newfigure);
                index++;
            }
            //
            if (Impulses.Count == 0)
            {
                Stroke = _colornotcontrolstroke;
                Fill = _colornotcontrol;
            }
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
        }

        /// <summary>
        /// Получаем новые данные по импульсам
        /// </summary>
        private void NewInfomation()
        {
            Dispatcher.Invoke(new Action(() => ProcessingData()));
        }

        /// <summary>
        /// Обработываем новые данные
        /// </summary>
        private void ProcessingData()
        {
            Analis();
        }

        /// <summary>
        /// Реагируем на подключение к серверу импульсов
        /// </summary>
        private void ConnectCloseServer()
        {
            Dispatcher.Invoke(new Action(() => ServerClose()));
        }

        private void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (!ImpulsesClientTCP.Connect)
                {
                    foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    Stroke = _colornotcontrolstroke;
                    Fill = _colornotcontrol;
                }
                else
                {
                    Stroke = _color_moveopen;
                    Fill = _color_fon_defult;
                }
            }
        }

        public void Analis()
        {
            bool update = false;
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                StatesControl state = Imp.Value.state;
                Imp.Value.state = Connections.ClientImpulses.Data.GetStateControl(StationControl, Imp.Value.Impuls);
                //
                if (state != Imp.Value.state)
                {
                    Imp.Value.Update = true;
                    update = true;
                }
            }
            //обновляем элемент
            if(update)
                UpdateElement(true);
        }

        private void UpdateCurrentState(List<Viewmode> list_priority, ref bool update)
        {
            StateElement control = CheckPriorityState(list_priority);
            if (control != null)
                SetState(control);
            else
            {
                foreach (Viewmode mode in list_priority)
                {
                    if (Impulses.ContainsKey(mode))
                    {
                        SetState(Impulses[mode]);
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
                foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
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
                        Stroke = _colornotcontrolstroke;
                    if (!_update_fill)
                        Fill = _colornotcontrol;
                }
            }
            else
            {
                if (!CheckUpdate)
                {
                    Stroke = _colornotcontrolstroke;
                    Fill = _colornotcontrol;
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
            foreach (Viewmode control in priority_control)
            {
                if (Impulses.ContainsKey(control))
                {
                    if (Impulses[control].state == StatesControl.activ)
                        return Impulses[control];
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
                            case Viewmode.accident:
                                Stroke = _color_accident;
                                break;
                            case Viewmode.fault:
                                Stroke = _color_faultmove;
                                break;
                            case Viewmode.closing:
                                Fill = _color_closingmove_auto;
                                break;
                            case Viewmode.closing_button:
                                Fill = _color_closingmove_button;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _color_fon_defult;
                                break;
                            case ViewElementDraw.stroke:
                                Stroke = _color_moveopen;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _colornotcontrol;
                                break;
                            case ViewElementDraw.stroke:
                                Stroke = _colornotcontrolstroke;
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// создаем коллекцию линий и точек
        /// </summary>
        public void CreateCollectionLines()
        {
            _points.Clear();
            _lines.Clear();
            foreach (PathFigure geo in _figure.Figures)
            {
                _points.Add(geo.StartPoint);
                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                        _points.Add(lin.Point);
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                        _points.Add(arc.Point);
                }
            }
            //
            for (int i = 0; i < _points.Count; i++)
            {
                if (i < _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[i + 1].X, Y2 = _points[i + 1].Y });
                else if (i == _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[0].X, Y2 = _points[0].Y });
            }
        }
    }
}
