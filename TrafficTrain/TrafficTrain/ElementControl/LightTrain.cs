using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Impulsesver.Client;
using TrafficTrain.WorkWindow;
using TrafficTrain.Interface;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;

namespace TrafficTrain
{
    /// <summary>
    /// поездной светофор
    /// </summary>
    class LightTrain : Shape, IGraficElement, IInfoElement, ISelectElement, IDisposable
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
        private Path _addHead = new Path();
        /// <summary>
        /// дополнительная головка светофора
        /// </summary>
        public Path AddHead
        {
            get
            {
                return _addHead;
            }
        }
        ///////
        //////основные свойства кгу
        /// <summary>
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// название кгу
        /// </summary>
        public string NameLight { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет головки по умолчанию
        /// </summary>
        public static Brush m_color_defult = Brushes.Black;
        /// <summary>
        /// цвет разрешающего сигнала
        /// </summary>
        public static Brush m_color_open = Brushes.Green;
        /// <summary>
        /// цвет рамки при неисправности
        /// </summary>
        public static Brush m_color_fault = Brushes.Red;
        /// <summary>
        /// цвет запрещающего сигнала
        /// </summary>
        public static Brush m_color_close = Brushes.Red;
        /// <summary>
        /// цвет мигания пригласительного первый
        /// </summary>
        public static Brush m_color_invitation_one = Brushes.Green;
        /// <summary>
        /// цвет мигания пригласительного  второй
        /// </summary>
        public static Brush m_color_invitation_ty = Brushes.Black;
        /// <summary>
        /// цвет маневрового сигнала
        /// </summary>
        public static Brush m_color_shunting = Brushes.White;
        /// <summary>
        /// цвет рамки по умолчанию
        /// </summary>
        public static Brush m_color_stroke = Brushes.Black;
        /// <summary>
        /// цвет рамки при установке
        /// </summary>
        public static Brush m_color_install = Brushes.Green;
        /// <summary>
        /// цвет фона если нет контроля
        /// </summary>
        public static Brush m_color_fon_notcontrol = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        /// <summary>
        /// цвет рамки если нет контроля
        /// </summary>
        public static Brush m_color_stroke_notcontrol = new SolidColorBrush(Color.FromRgb(205, 205, 205));
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
        /// Является ли светофор входным
        /// </summary>
        public bool IsInput { get; set; }
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
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.invitational, Viewmode.signal, Viewmode.shunting };
        /// <summary>
        /// приоритет отображения рамки
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>() { Viewmode.fault, Viewmode.installation, Viewmode.assignment_command, Viewmode.check_route, Viewmode.passage_route};
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
                return NameLight;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public LightTrain(int stationnumber, PathGeometry geometry, string name, bool input, Dictionary<Viewmode, StateElement> impulses)
        {
            StationTransition = stationnumber;
            NameLight = name;
            IsInput = input;
            Impulses = AnalisCollectionStateControl(impulses);
            _addHead.Data = new PathGeometry();
            GeometryFigureCopy(geometry);
            Analis();
            //обработка импульсов
            //ImpulsesClient.NewData += NewInfomation;
            //ImpulsesClient.ConnectDisconnectionServer += ConnectCloseServer;
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
            return string.Format("Поездной светофор - {0} {1}", NameLight, Notes);
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
                            _addHead.Fill = brushes[0];
                            break;
                        case ViewElementDraw.stroke:
                             Stroke = brushes[0];
                            _addHead.Stroke = brushes[0];
                            break;
                    }
                }
                else
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.fill:
                            _addHead.Fill = brushes[1];
                            break;
                        case ViewElementDraw.stroke:
                            Stroke = brushes[1];
                            _addHead.Stroke = brushes[1];
                            break;
                    }
            }
        }

        private List<Brush> SetColorsFlashing(Viewmode mode)
        {
            switch (mode)
            {
                case Viewmode.installation:
                    return new List<Brush> { m_color_invitation_one, m_color_invitation_ty };
                case Viewmode.invitational:
                    return new List<Brush> { m_color_install, m_color_defult };
                case Viewmode.assignment_command:
                    return new List<Brush> { RouteSignal._color_command_received_one, RouteSignal._color_command_received_ty };
                case Viewmode.passage_route:
                    return new List<Brush> { RouteSignal._color_command_received_one, RouteSignal._color_wait_install_ty };
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
                PathFigure newfigure = new PathFigure() { IsClosed = false };
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
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                    {
                        newfigure.Segments.Add(new ArcSegment() { Point = new Point(arc.Point.X, arc.Point.Y), Size = new Size(arc.Size.Width, arc.Size.Height) });
                        continue;
                    }
                }
                //
                if (index < 3)
                    _figure.Figures.Add(newfigure);
                else (_addHead.Data as PathGeometry).Figures.Add(newfigure);
                //
                index++;
            }
            //
            Stroke = m_color_stroke_notcontrol;
            Fill = m_color_fon_notcontrol;
            //
            _addHead.Stroke = m_color_stroke_notcontrol;
            _addHead.Fill = m_color_fon_notcontrol;
            //
            StrokeThickness = _strokethickness;
            _addHead.StrokeThickness = _strokethickness;
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
                if (!ImpulsesClient.Connect)
                {
                    foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    Stroke = m_color_stroke_notcontrol;
                    Fill = m_color_fon_notcontrol;
                    //
                    _addHead.Stroke = m_color_stroke_notcontrol;
                    _addHead.Fill = m_color_fon_notcontrol;
                }
            }
        }

        public void Analis()
        {
            bool update = false;
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                StatesControl state = Imp.Value.state;
                Imp.Value.state = GetImpuls.GetStateControl(StationControl, Imp.Value.Impuls);
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
                                Stroke = RouteSignal._color_command_received_one;
                                _addHead.Stroke = RouteSignal._color_command_received_one;
                                break;
                            case Viewmode.installation:
                                Stroke = m_color_install;
                                _addHead.Stroke = m_color_install;
                                break;
                            case Viewmode.fault:
                                Stroke = m_color_fault;
                                _addHead.Stroke = m_color_fault;
                                break;
                            case Viewmode.signal:
                                Fill = m_color_defult; ;
                                _addHead.Fill = m_color_open;
                                break;
                            case Viewmode.shunting:
                                Fill = m_color_shunting;
                                _addHead.Fill = m_color_defult;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                if (IsInput)
                                {
                                    _addHead.Fill = m_color_defult;
                                    Fill = m_color_close;
                                }
                                else
                                {
                                    _addHead.Fill = m_color_defult;
                                    Fill = m_color_defult;
                                }
                                break;
                            case ViewElementDraw.stroke:
                                Stroke = m_color_stroke;
                                _addHead.Stroke = m_color_stroke;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                 Fill = m_color_fon_notcontrol;
                                _addHead.Fill = m_color_fon_notcontrol;
                                break;
                            case ViewElementDraw.stroke:
                                Stroke = m_color_stroke_notcontrol;
                                _addHead.Stroke = m_color_stroke_notcontrol;
                                break;
                        }
                        break;
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
                    {
                        Stroke = m_color_stroke_notcontrol;
                        _addHead.Stroke = m_color_stroke_notcontrol;
                    }
                    if (!_update_fill)
                    {
                        Fill = m_color_fon_notcontrol;
                        _addHead.Fill = m_color_fon_notcontrol;
                    }
                }
            }
            else
            {
                if (!CheckUpdate)
                {
                    Stroke = m_color_stroke_notcontrol;
                    _addHead.Stroke = m_color_stroke_notcontrol;
                    //
                    Fill = m_color_fon_notcontrol;
                    _addHead.Fill = m_color_fon_notcontrol;
                }
            }
        }

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
        /// создаем коллекцию линий и точек
        /// </summary>
        public void CreateCollectionLines()
        {
            _points.Clear();
            _lines.Clear();
            //
            double Xmin = double.MaxValue;
            double Xmax = double.MinValue;
            double Ymin = double.MaxValue;
            double Ymax = double.MinValue;
            //
            PointsFigure(_figure, _points);
            PointsFigure((_addHead.Data as PathGeometry), _points);
            //
            foreach (Point point in _points)
            {
                if (point.X > Xmax)
                    Xmax = point.X;
                //
                if (point.X < Xmin)
                    Xmin = point.X;
                //
                if (point.Y > Ymax)
                    Ymax = point.Y;
                //
                if (point.Y < Ymin)
                    Ymin = point.Y;
            }
            //
            _points.Clear();
            _points.Add(new Point(Xmin, Ymin)); _points.Add(new Point(Xmax, Ymin));
            _points.Add(new Point(Xmax, Ymax)); _points.Add(new Point(Xmin, Ymax));
            //
            for (int i = 0; i < _points.Count; i++)
            {
                if (i < _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[i + 1].X, Y2 = _points[i + 1].Y });
                else if (i == _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[0].X, Y2 = _points[0].Y });
            }
        }

        private void PointsFigure(PathGeometry geometry, PointCollection points)
        {
            foreach (PathFigure geo in geometry.Figures)
            {
                _points.Add(geo.StartPoint);
                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                        points.Add(lin.Point);
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                        points.Add(arc.Point);
                }
            }
        }
    }
}

