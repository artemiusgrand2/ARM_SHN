using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Impulsesver.Client;
using TrafficTrain.Interface;
using TrafficTrain.Enums;
using TrafficTrain.WorkWindow;
using SCADA.Common.Enums;
using SCADA.Common.SaveElement;

namespace TrafficTrain
{
    /// <summary>
    /// класс отвечает за левую стрелку поворота
    /// </summary>
   public class Direction : Shape, IGraficElement, IDisposable, ISelectElement
    {
         #region Переменные и свойства
        //геометрия
        private PathGeometry _figure = new PathGeometry();
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
        public PathGeometry Figure { get { return _figure; } }
        ///////
        //////основные свойства перегонной стрелки
        /// <summary>
        /// шестизначный номер станции слева
        /// </summary>
        public int StationNumberLeft { get; set; }
        /// <summary>
        /// шестизначный номер станции справа
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// шестизначный номер станции импульсов ТС
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// название граничного участка
        /// </summary>
        public string NameTrack { get; set; }
        /// <summary>
        /// рамка для номеров поездов к которой относиться элемент
        /// </summary>
        NumberTrainRamka trainramka;
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет  при отправлениии
        /// </summary>
        public static Brush _color_departure = Brushes.LightGreen;
        /// <summary>
        /// цвет разрешения отправления 
        /// </summary>
        public static Brush _color_ok_departure = Brushes.LightGreen;
        /// <summary>
        /// цвет при ожидания отправления
        /// </summary>
        public static Brush _color_wait_departure = Brushes.Yellow;
        /// <summary>
        /// цвет при рамки по умолчанию
        /// </summary>
        public static Brush _color_ramka = Brushes.Black;
        /// <summary>
        /// цвет при нормальной работе по умолчанию (если перегон не занят)
        /// </summary>
        public static Brush _color_pasive = new SolidColorBrush(Color.FromRgb(190, 190, 190));
        /// <summary>
        /// цвет неконтролируемого  пути
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        /// <summary>
        /// цвет неконтролируемого  пути его рамка
        /// </summary>
        public static Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /////
        /// <summary>
        /// коллекция возможных состояний элемента перегонная стрелка со станции слева
        /// </summary>
        public Dictionary<Viewmode, StateElement> Impulses { get; set; }
        /// <summary>
        /// толщина контура объектов
        /// </summary>
        double _strokethickness = 1 * SystemParameters.CaretWidth;
        bool _rotate = false;
        /// <summary>
        /// показывает повернут ли перегон влево или в право в зависимости от типа
        /// </summary>
        public bool Rotate
        {
            get
            {
                return _rotate;
            }
        }
        /// <summary>
        /// постоянно ли развернут перегон влево или вправо
        /// </summary>
        public bool ConstRotate { get; set; }
       /// <summary>
       /// Вид стрелки поворота
       /// </summary>
        public ViewDirection ViewDirection { get; set; }
        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.waiting_for_departure, Viewmode.departure };
        /// <summary>
        /// пояснения
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }
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

        public string NameUl
        {
            get
            {
                return NameTrack;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumberleft">шестизначный номер станции слева</param>
        /// <param name="stationnumberright">шестизначный номер станции справа</param>
        /// <param name="geometry">название пути</param>
        /// <param name="name">граница</param>
        public Direction(int stationnumberleft, int stationnumberright, int station, PathGeometry geometry, string nametrack, Dictionary<Viewmode, StateElement> impulses, NumberTrainRamka train, bool constrotate,
            ViewDirection viewdirection, bool IsVisible, string Notes)
        {
            StationNumberLeft = stationnumberleft;
            StationTransition = stationnumberright;
            StationControl = station;
            NameTrack = nametrack;
            this.Notes = Notes;
            ConstRotate = constrotate;
            Impulses = impulses;
            trainramka = train;
            ViewDirection = viewdirection;
            if (!IsVisible)
                Visibility = System.Windows.Visibility.Hidden;
            //
            GeometryFigureCopy(geometry);
            //если перегон всегда развернут влево
            if (ConstRotate)
            {
                _rotate = true;
                if (trainramka != null)
                {
                    if (ViewDirection == ViewDirection.left)
                        trainramka.LeftDirection(_rotate);
                    else trainramka.RightDirection(_rotate);
                }
            }
            //обработка импульсов
            Connections.NewTart += StartFlashing;
            LoadColorControl.NewColor += NewColor;
        }

        public Direction() { }

        public string InfoElement()
        {
            return Notes;
        }

        public void Dispose()
        {
            Connections.NewTart -= StartFlashing;
            LoadColorControl.NewColor -= NewColor;
        }

        private void NewColor()
        {
            UpdateElement(false); ;
        }
        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry)
        {
            //номер фигуры
            int index = 0;
            foreach (PathFigure geo in geometry.Figures)
            {
                _figure = new PathGeometry();
                PathFigure newfigure = new PathFigure() { IsClosed = true };
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
                //
                if (ViewDirection == ViewDirection.left)
                {
                    if (index == 0)
                        break;
                }
                else
                {
                    if (index == 2)
                        break;
                }
                //
                index++;
            }
            //
            Fill = _colornotcontrol;
            Stroke = _colornotcontrolstroke;
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
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
            if (Impulses!=null && Impulses.Count > 0 && !ConstRotate)
            {
                if (!ImpulsesClient.Connect)
                {
                    foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    Fill = _colornotcontrol;
                    Stroke = _colornotcontrolstroke;
                    //
                    _rotate = false;
                }
                else
                    Stroke = _color_ramka;
            }
            //если перегон развернут постоянно влево
            if (ConstRotate)
            {
                if (!ImpulsesClient.Connect)
                    _rotate = false;
                else
                {
                    _rotate = true;
                    if (trainramka != null)
                    {
                        if (ViewDirection == ViewDirection.left)
                            trainramka.LeftDirection(_rotate);
                        else trainramka.RightDirection(_rotate);
                    }
                }
            }
           
        }

        private void StartFlashing()
        {
            Dispatcher.Invoke(new Action(() => Flashing()));
        }
        /// <summary>
        /// мигание 
        /// </summary>
        private void Flashing()
        {
            //проверяем моргать ли фоном
            StateElement control_fill = CheckPriorityState(_priority_fill);
            if (control_fill != null)
                FlashingControl(SetColorsFlashing(control_fill.Name), control_fill);
        }

        private void FlashingControl(List<Brush> brushes, StateElement control)
        {
            if (brushes.Count == 2)
            {
                if (Connections.Taktupdate)
                    Fill = brushes[0];
                else
                    Fill = brushes[1];
            }
        }

        private List<Brush> SetColorsFlashing(Viewmode mode)
        {
            switch (mode)
            {
                case Viewmode.resolution_of_origin:
                    return new List<Brush> { Direction._color_ok_departure, Direction._color_pasive };
            }
            //
            return new List<Brush>();
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

        public void Analis()
        {
            if (!ConstRotate && Impulses != null && Impulses.Count > 0)
            {
                bool update = false;
                //проверяем импульсы с левой станции
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
                if (update)
                {
                    UpdateElement(true);
                    //произошло ли изменение направления
                    AnalisDirection();
                }
            }
        }

        /// <summary>
        /// проверяем произошло ли изменение направления
        /// </summary>
        private void AnalisDirection()
        {
            bool answer = false;
            foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
            {
                if (imp.Value.state == StatesControl.activ)
                {
                    answer = true; break;
                }
            }
            //
            if (_rotate != answer)
            {
                _rotate = answer;
                if (trainramka != null)
                {
                    if (ViewDirection == ViewDirection.left)
                        trainramka.LeftDirection(_rotate);
                    else trainramka.RightDirection(_rotate);
                }
            }
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
                            case Viewmode.departure:
                                Fill = _color_departure;
                                break;
                            case Viewmode.resolution_of_origin:
                                break;
                            case Viewmode.waiting_for_departure:
                                Fill = _color_wait_departure;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _color_pasive;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _colornotcontrol;
                                break;
                        }
                        break;
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
