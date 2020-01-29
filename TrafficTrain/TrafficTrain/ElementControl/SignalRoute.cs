using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Interface;
using TrafficTrain.Enums;
using TrafficTrain.WorkWindow;
using TrafficTrain.Constant;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.ImpulsClient;
using log4net;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий сигнал
    /// </summary>
    public class RouteSignal : Shape, IGraficElement, IInfoElement, ISelectElement, IDisposable
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
        //////основные свойства сигнала
        /// <summary>
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// название сигнала
        /// </summary>
        public string NameSignal { get; set; }
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
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет проверки маршрута на возможность задания
        /// </summary>
        public static Brush _color_check_route = Brushes.Blue;
        /// <summary>
        /// цвет задания мигания маршрута первый
        /// </summary>
        public static Brush _color_command_received_one = Brushes.Blue;
        /// <summary>
        /// цвет задания мигания маршрута второй
        /// </summary>
        public static Brush _color_command_received_ty = Brushes.Black;
        /// <summary>
        /// цвет задания ожидания установки маршрута первый
        /// </summary>
        public static Brush _color_wait_install_one = Brushes.Blue;
        /// <summary>
        /// цвет задания ожидания установки маршрута второй
        /// </summary>
        public static Brush _color_wait_install_ty = Brushes.Red;
        /// <summary>
        /// цвет ограждения
        /// </summary>
        public static Brush _color_fencing = Brushes.Brown;
        /// <summary>
        /// цвет  маневрового светофора
        /// </summary>
        public static Brush _color_shunting = Brushes.White;
        /// <summary>
        /// цвет при занятой секции
        /// </summary>
        public static Brush _color_active = Brushes.Red;
        /// <summary>
        /// цвет открытого сигнала
        /// </summary>
        public static Brush _color_open = Brushes.LightGreen;
        /// <summary>
        /// цвет при замкнутом маршруте
        /// </summary>
        public static Brush _color_locing = Brushes.Yellow;
        /// <summary>
        /// цвет при мигания при установке маршрута первый
        /// </summary>
        public static Brush _color_install_route_one = Brushes.Yellow;
        /// <summary>
        /// цвет при мигания при установке маршрута второй
        /// </summary>
        public static Brush _color_install_route_ty = Brushes.Black;
        /// <summary>
        /// цвет мигание пригласительного огня первый
        /// </summary>
        public static Brush _color_invitational_one = Brushes.LightGreen;
        /// <summary>
        /// цвет мигание пригласительного огня второй
        /// </summary>
        public static Brush _color_invitational_ty= Brushes.Red;
        /// <summary>
        /// цвет при сгоревшей лампе светофора
        /// </summary>
        public static Brush _color_fault = Brushes.Red;
        /// <summary>
        /// установленный проезд
        /// </summary>
        public static Brush _color_passage = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        /// <summary>
        /// цвет фона если путь свободен
        /// </summary>
        public static Brush _color_pasive = new SolidColorBrush(Color.FromRgb(220, 220, 220));
        /// <summary>
        /// цвет  рамки по умолчанию
        /// </summary>
        public static Brush _color_ramka_defult = Brushes.Black;
        /// <summary>
        /// цвет неконтролируемого  пути
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        /// <summary>
        /// цвет неконтролируемого  пути его рамка
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
        /// логирование
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(RouteSignal));

        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.invitational, Viewmode.signal, Viewmode.occupation, Viewmode.shunting, Viewmode.locking, Viewmode.fencing, Viewmode.passage };
        /// <summary>
        /// приоритет отображения рамки
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>() { Viewmode.fault, Viewmode.installation, Viewmode.assignment_command, Viewmode.check_route, Viewmode.passage_route };
        /// <summary>
        /// приоритеты состояния сигнала
        /// </summary>
        List<Viewmode> _priority_signal = new List<Viewmode>() { Viewmode.installation, Viewmode.signal, Viewmode.shunting};
        /// <summary>
        /// текущее состояние сигнала
        /// </summary>
        public ViewSostSignal CurrentSostSignal { get; set; }
        private DateTime _last_update_signal = DateTime.Now;
        /// <summary>
        /// последние время изменение состояния сигнала
        /// </summary>
        public DateTime LastUpdateSignal
        {
            get { return _last_update_signal; }
            set { _last_update_signal = value; }
        }

        /// <summary>
        /// обозначение
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
                return NameSignal;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public RouteSignal(PathGeometry geometry, string name, Dictionary<Viewmode, StateElement> impulses)
        {
            NameSignal = name;
            Impulses = AnalisCollectionStateControl(impulses);
            GeometryFigureCopy(geometry);
            Analis();
            //обработка импульсов
            Connections.NewTart += StartFlashing;
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            //ImpulsesClient.NewData -= NewInfomation;
            //ImpulsesClient.ConnectDisconnectionServer -= ConnectCloseServer;
            Connections.NewTart -= StartFlashing;
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

        public ViewElementDraw GetViewElementDrawImpuls(Viewmode mode)
        {
            foreach (Viewmode stroke_element in _priority_stroke)
            {
                if (stroke_element == mode)
                    return ViewElementDraw.stroke;
            }
            //
            return ViewElementDraw.fill;
        }

        public string InfoElement()
        {
            return string.Format("{0}", Notes);
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
            //проверяем иоргать ли фоном
            StateElement control_fill = CheckPriorityState(_priority_fill);
            if (control_fill != null)
                FlashingControl(SetColorsFlashing(control_fill.Name), control_fill);
            //проверять ли рамкой
            StateElement control_stroke = CheckPriorityState(_priority_stroke);
            if (control_stroke != null)
                FlashingControl(SetColorsFlashing(control_stroke.Name),control_stroke);
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
                case Viewmode.installation:
                    return new List<Brush> { _color_install_route_one, _color_install_route_ty };
                case Viewmode.fault:
                    return new List<Brush> { _color_fault, _color_ramka_defult };
                case Viewmode.assignment_command:
                    return new List<Brush> { _color_command_received_one, _color_command_received_ty };
                case Viewmode.passage_route:
                    return new List<Brush> { _color_wait_install_one, _color_wait_install_ty };
                case Viewmode.invitational:
                    return new List<Brush> { _color_invitational_one, _color_invitational_ty };
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
            Stroke = _colornotcontrolstroke;
            Fill = _colornotcontrol;
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
                    CurrentSostSignal = ViewSostSignal.none;
                    _last_update_signal = DateTime.Now;
                }
            }
        }

        public void Analis()
        {
            bool update = false;
            bool update_sost_signal = false;
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                StatesControl state = Imp.Value.state;
                Imp.Value.state = Connections.ClientImpulses.Data.GetStateControl(StationControl, Imp.Value.Impuls);
                //
                if (state != Imp.Value.state)
                {
                    Imp.Value.Update = true;
                    Imp.Value.LastUpdate = DateTime.Now;
                    update = true;
                }
                //проверяем был ли изменен режим контроля станции
                if (!update_sost_signal && IsControlSignal(Imp.Value.Name) && Imp.Value.Update)
                    update_sost_signal = true;
            }
            //
            if(update_sost_signal)
                SetStateSignal(CheckPriorityState(_priority_signal));
            //обновляем элемент
            if(update)
                UpdateElement(true);
        }

        private bool IsControlSignal(Viewmode mode)
        {
            foreach (Viewmode value in _priority_signal)
            {
                if (value == mode)
                    return true;
            }
            
            return false;
        }

        private void SetStateSignal(StateElement control)
        {
            if (control != null)
            {
                switch (control.Name)
                {
                    case Viewmode.shunting:
                         CurrentSostSignal = ViewSostSignal.shunting;
                         _last_update_signal = DateTime.Now;
                        break;
                    case Viewmode.invitational:
                         CurrentSostSignal = ViewSostSignal.invitational;
                         _last_update_signal = DateTime.Now;
                        break;
                    case Viewmode.signal:
                         CurrentSostSignal = ViewSostSignal.signal;
                         _last_update_signal = DateTime.Now;
                        break;
                }
            }
            else
            {
                if (CurrentSostSignal != ViewSostSignal.none)
                {
                    CurrentSostSignal = ViewSostSignal.none;
                    _last_update_signal = DateTime.Now;
                }
            }
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
                            case Viewmode.check_route:
                                Stroke = _color_check_route;
                                break;
                            case Viewmode.signal:
                                Fill = _color_open;
                                break;
                            case Viewmode.occupation:
                                Fill = _color_active;
                                break;
                            case Viewmode.locking:
                                Fill = _color_locing;
                                break;
                            case Viewmode.shunting:
                                Fill = _color_shunting;
                                break;
                            case Viewmode.passage:
                                Fill = _color_passage;
                                break;
                            case Viewmode.fencing:
                                Fill = _color_fencing;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _color_pasive;
                                break;
                            case ViewElementDraw.stroke:
                                Stroke = _color_ramka_defult;
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
