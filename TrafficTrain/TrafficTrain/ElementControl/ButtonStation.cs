using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;
using TrafficTrain.Constant;
using TrafficTrain.Enums;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.ImpulsClient;
using log4net;


namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий кнопку станции
    /// </summary>
    public class ButtonStation : Shape, IGraficElement, ISelectElement, IDisposable, IInfoElement//, IIndicationEl
    {
        #region Переменные и свойства
        /// <summary>
        /// логирование
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(ButtonStation));
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
        //////основные свойства пути
        /// <summary>
        /// <summary>
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// название кнопки станции
        /// </summary>
        public string NameButton { get; set; }
        /// <summary>
        /// наличие автономного управления
        /// </summary>
        private bool _autonomous_control = false;
        /// <summary>
        /// наличие автономного управления свойство
        /// </summary>
        public bool AutonomousControl 
        {
            get
            {
                return _autonomous_control;
            }
            set
            {
                _autonomous_control = value;
            }
        }
        /// <summary>
        /// не входит станция в диспетчерский круг
        /// </summary>
        private bool _not_supervisory_control = false;
        /// <summary>
        ///не входит станция в диспетчерский круг
        /// </summary>
        public bool NotSupervisoryControl
        {
            get
            {
                return _not_supervisory_control;
            }
            set
            {
                _not_supervisory_control = value;
            }
        }
        /// <summary>
        /// поддержка автоуправления
        /// </summary>
        private bool _auto_pilot = false;
        /// <summary>
        /// поддержка автоуправления свойство
        /// </summary>
        public bool AutoPilot
        {
            get
            {
                return _auto_pilot;
            }
            set
            {
                _auto_pilot = value;
            }
        }
        
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет фона умолчанию
        /// </summary>
        public static Brush _color_defult = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// цвет  при диспетчерской работе 
        /// </summary>
        public static  Brush _color_dispatcher = Brushes.LightGreen;
        /// <summary>
        /// цвет  при работе автопилота
        /// </summary>
        public static Brush _color_auto_dispatcher = Brushes.Blue;
        /// <summary>
        /// цвет при аварии
        /// </summary>
        public static Brush _color_accident = Brushes.Red;
        /// <summary>
        /// цвет при неисправности
        /// </summary>
        public static Brush _color_fault = Brushes.Yellow;
        /// <summary>
        /// цвет при пожаре
        /// </summary>
        public static Brush _color_fire = Brushes.Red;
        /// <summary>
        /// цвет  подложки при мигании пожара
        /// </summary>
        public static Brush _color_fire_ty = _color_defult;
        /// <summary>
        /// цвет при потери связи со станцией
        /// </summary>
        public static Brush _color_notlink = new SolidColorBrush(Color.FromArgb(190, Brushes.Blue.Color.R, Brushes.Blue.Color.G, Brushes.Blue.Color.B));
        /// <summary>
        /// цвет при сезонном управлении
        /// </summary>
        public static Brush _color_sesoncontol = Brushes.White;
        /// <summary>
        /// цвет при автономном управлении
        /// </summary>
        public static Brush _color_autonomous_control = Brushes.Black;
        /// <summary>
        /// цвет при резервном управлении
        /// </summary>
        public static Brush _color_reserve_control = Brushes.Yellow;
        /// <summary>
        /// цвет рамки при нормальной работе
        /// </summary>
        public static Brush _color_stroke_normal = Brushes.Black;
        /// <summary>
        /// цвет фона если станция не входит в диспетчерский круг
        /// </summary>
        public static Brush _color_not_dispatcher = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        /// <summary>
        /// цвет неконтролируемого рамки
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
        /// текущее управление на станции
        /// </summary>
        public ViewStateStation CurrentControlStation { get; set; }
        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> m_priority_fill = new List<Viewmode>() { Viewmode.not_station, Viewmode.autonomous_control, Viewmode.fire, Viewmode.reserve_control, Viewmode.start_seasonal_management, 
            Viewmode.seasonal_management, Viewmode.auto_pilot, Viewmode.supervisory_control };
        /// <summary>
        /// приоритет отображения рамки
        /// </summary>
        List<Viewmode> m_priority_stroke = new List<Viewmode>() { Viewmode.accident, Viewmode.fault };
        /// <summary>
        /// приоритеты видов контролей станции
        /// </summary>
        List<Viewmode> m_priority_view_control_station = new List<Viewmode>() { Viewmode.not_station, Viewmode.autonomous_control, Viewmode.reserve_control, 
            Viewmode.seasonal_management, Viewmode.auto_pilot, Viewmode.supervisory_control };
        /// <summary>
        /// пояснения
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
                return NameButton;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public ButtonStation(PathGeometry geometry, string name, Dictionary<Viewmode, StateElement> impulses)
        {
            NameButton = name;
            Impulses = AnalisCollectionStateControl(AnalisLoadDann(impulses));
            GeometryFigureCopy(geometry);
            Analis();
            //обработка импульсов
            Connections.NewTart += StartFlashing;
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            Connections.NewTart -= StartFlashing;
            LoadColorControl.NewColor -= NewColor;
        }

        private Dictionary<Viewmode, StateElement> AnalisLoadDann(Dictionary<Viewmode, StateElement> impulses)
        {
            //проверяем находится ли станция на автономном управлении
            if (impulses.ContainsKey(Viewmode.autonomous_control))
                AutonomousControl = true;
            //входит ли станция в диспетчерский круг
            if (impulses.ContainsKey(Viewmode.not_supervisory_control))
            {
                NotSupervisoryControl = true;
                impulses.Remove(Viewmode.not_supervisory_control);
            }
            //проверяем поддерживает ли станция автопилот
            if (impulses.ContainsKey(Viewmode.auto_pilot))
            {
                if (Connections.ClientImpulses.Data.Stations.ContainsKey(StationControl))
                {
                    if (Connections.ClientImpulses.Data.Stations[StationControl].TS.ContainsTS(impulses[Viewmode.auto_pilot].Impuls))
                        AutoPilot = true;
                    else
                    {
                        if (Connections.ClientImpulses.Data.Stations[StationControl].TS.AddTSImpuls(impulses[Viewmode.auto_pilot].Impuls))
                            AutoPilot = true;
                    }
                }

            }
            //если у станции нет контроля подллючения
            if (!impulses.ContainsKey(Viewmode.not_station))
                impulses.Add(Viewmode.not_station, new StateElement() { Name = Viewmode.not_station });
            //
            return impulses;
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
                foreach (Viewmode stroke_element in m_priority_stroke)
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

        private void NewColor()
        {
            UpdateElement(false);
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

            StateElement control =  CheckPriorityState(m_priority_fill);
            if (control != null)
                FlashingControl(SetColorsFlashing(control.Name));
        }

        private void FlashingControl(List<Brush> brushes)
        {
            if (brushes.Count == 2)
            {
                if (Connections.Taktupdate)
                    Fill = brushes[0];
                else
                    Fill = brushes[1];
            }
        }
        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry)
        {
            foreach (PathFigure geo in geometry.Figures)
            {
                PathFigure newfigure = new PathFigure() {  IsClosed = true};
                newfigure.StartPoint = new Point(geo.StartPoint.X, geo.StartPoint.Y);
                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                    {
                        newfigure.Segments.Add(new LineSegment() { Point = new Point(lin.Point.X, lin.Point.Y) });
                        continue;
                    }
                }
                _figure.Figures.Add(newfigure);
            }
            //
            if (Impulses.Count == 0)
            {
                Stroke = _colornotcontrolstroke;
                Fill = _color_not_dispatcher;
            }
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
        }


        public string InfoElement()
        {
            return string.Format("{0}", Notes);
        }

        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (ImpulsesClientTCP.Connect)
                    Stroke = _color_stroke_normal;
                else
                {
                    foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    Stroke = _colornotcontrolstroke;
                    CurrentControlStation = ViewStateStation.not_connect;
                }
            }
        }

        public IList<string> Analis()
        {
            bool update = false;
            bool update_sost_station = false;
            //
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                StatesControl state = Imp.Value.state;
                if (Imp.Value.Name != Viewmode.autonomous_control)
                    Imp.Value.state = Connections.ClientImpulses.Data.GetStateControl(StationControl, Imp.Value.Impuls);
                else
                {
                    switch (Imp.Value.Name)
                    {
                        case Viewmode.autonomous_control:
                            Imp.Value.state = StatesControl.activ;
                            break;
                    }
                }
                //
                if (state != Imp.Value.state)
                {
                    Imp.Value.LastUpdate = DateTime.Now;
                    Imp.Value.Update = true;
                    update = true;
                }
                //проверяем был ли изменен режим контроля станции
                if (!update_sost_station && IsControlStation(Imp.Value.Name) && Imp.Value.Update)
                    update_sost_station = true;
                   
            }
            //
            if(update_sost_station)
                SetStateStation(CheckPriorityState(m_priority_view_control_station));
            //обновляем элемент
            if(update)
                UpdateElement(true);
            //
            return new List<string>();
        }

        private bool IsControlStation(Viewmode mode)
        {
            foreach (Viewmode value in m_priority_view_control_station)
            {
                if (value == mode)
                    return true;
            }
            return false;
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
        public void UpdateElement(bool CheckUpdate)
        {
             if (Impulses.Count > 0)
            {
                if (!NotSupervisoryControl)
                {
                    if (!AutonomousControl)
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
                                            UpdateCurrentState(m_priority_fill, ref _update_fill);
                                        break;
                                    case ViewElementDraw.stroke:
                                        if (!_update_stroke)
                                            UpdateCurrentState(m_priority_stroke, ref _update_stroke);
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
                                Fill = _color_not_dispatcher;
                        }
                    }
                    else
                    {
                        //if (Impulses.ContainsKey(Viewmode.autonomous_control))
                        //{
                        if (Fill != _color_autonomous_control && CurrentControlStation == ViewStateStation.autonomous_control)
                            Fill = _color_autonomous_control;
                        //}
                    }
                }
                else
                {
                    if (Fill != _color_not_dispatcher && CurrentControlStation != ViewStateStation.not_connect)
                        Fill = _color_not_dispatcher;
                }
            }
            else
            {
                if (!CheckUpdate)
                {
                    Stroke = _colornotcontrolstroke;
                    Fill = _color_not_dispatcher;
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
            foreach (Viewmode control in priority_control)
            {
                if (Impulses.ContainsKey(control))
                {
                    if (Impulses[control].state == StatesControl.activ)
                    {
                        if (Impulses[control].Name != Viewmode.not_station)
                            return Impulses[control];
                    }
                    else
                    {
                        if (Impulses[control].Name == Viewmode.not_station)
                            return Impulses[control];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// изменяем цвет графического элемента в зависимости от контролей
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
                            case Viewmode.not_station:
                                Fill = _color_defult;
                                break;
                            case Viewmode.supervisory_control:
                                Fill = _color_dispatcher;
                                break;
                            case Viewmode.seasonal_management:
                                Fill = _color_sesoncontol;
                                break;
                            case Viewmode.reserve_control:
                                Fill = _color_reserve_control;
                                break;
                            case Viewmode.auto_pilot:
                                Fill = _color_auto_dispatcher;
                                break;
                            case Viewmode.fault:
                                Stroke = _color_fault;
                                break;
                            case Viewmode.accident:
                                Stroke = _color_accident;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _color_defult;
                                break;
                            case ViewElementDraw.stroke:
                                Stroke = _color_stroke_normal;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _color_not_dispatcher;
                                break;
                            case ViewElementDraw.stroke:
                                Stroke = _colornotcontrolstroke;
                                break;
                        }
                        break;
                }
            }
        }

        private List<Brush> SetColorsFlashing(Viewmode mode)
        {
            switch (mode)
            {
                case Viewmode.not_station:
                     return new List<Brush> { _color_notlink, _color_defult };
                case Viewmode.fire:
                    return new List<Brush> {_color_fire, _color_fire_ty};
                case Viewmode.start_seasonal_management:
                    return new List<Brush> { _color_dispatcher, _color_sesoncontol };
            }
            //
            return new List<Brush>();
        }

        /// <summary>
        /// определяем цвет мигания при пожаре в завмсимости от текущего управления станцией
        /// </summary>
        /// <param name="index"></param>
        private void SetStateStation(StateElement control)
        {
            if (control != null)
            {
                switch (control.Name)
                {
                    case Viewmode.supervisory_control:
                        _color_fire_ty = _color_dispatcher;
                        CurrentControlStation = ViewStateStation.supervisory_control;
                        break;
                    case Viewmode.seasonal_management:
                        _color_fire_ty = _color_sesoncontol;
                        CurrentControlStation = ViewStateStation.seasonal_management;
                        break;
                    case Viewmode.reserve_control:
                        _color_fire_ty = _color_reserve_control;
                        CurrentControlStation = ViewStateStation.reserve_control;
                        break;
                    case Viewmode.auto_pilot:
                        _color_fire_ty = _color_auto_dispatcher;
                        CurrentControlStation = ViewStateStation.auto_supervisory;
                        break;
                    case Viewmode.not_station:
                        CurrentControlStation = ViewStateStation.not_connect;
                        break;
                    case Viewmode.autonomous_control:
                        CurrentControlStation = ViewStateStation.autonomous_control;
                        break;
                }
                //
                if (AutoPilot && (CurrentControlStation != ViewStateStation.auto_supervisory && Impulses[Viewmode.auto_pilot].state == StatesControl.activ))
                    Impulses[Viewmode.auto_pilot].state = StatesControl.pasiv;
            }
            else _color_fire_ty = _color_defult;
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
                }
            }
            //
            double Xmin = double.MaxValue;
            double Xmax = double.MinValue;
            double Ymin = double.MaxValue;
            double Ymax = double.MinValue;
            //
            for (int i = 0; i < _points.Count; i++)
            {
                if (_points[i].X > Xmax)
                    Xmax = _points[i].X;
                //
                if (_points[i].X < Xmin)
                    Xmin = _points[i].X;
                //
                if (_points[i].Y > Ymax)
                    Ymax = _points[i].Y;
                //
                if (_points[i].Y < Ymin)
                    Ymin = _points[i].Y;
                //
                if (i < _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[i + 1].X, Y2 = _points[i + 1].Y });
                else if (i == _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[0].X, Y2 = _points[0].Y });
            }
            //
            _pointCenter.X = Xmin + (Xmax - Xmin) / 2;
            _pointCenter.Y = Ymin + (Ymax - Ymin) / 2;
            //
        }
    }
}
