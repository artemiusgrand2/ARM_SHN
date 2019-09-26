using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;



namespace TrafficTrain
{
    class TimeElement : Shape, IGraficElement, IText, IInfoElement
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
        }

        //////цветовая фона
        public static Brush _color_fon = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush _color_text = Brushes.Black;
        /// <summary>
        /// цвет рамки
        /// </summary>
        public static Brush _color_stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        //////

        private TextBlock _text = new TextBlock(){ FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center };
        /// <summary>
        /// тескт названия объекта
        /// </summary>
        public TextBlock Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
        /// <summary>
        /// начальный размер текста
        /// </summary>
        double _startfontsize;
        /// <summary>
        /// первоначальное разположение текста
        /// </summary>
        Thickness _startmargin;
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
        /// обозначение
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }

        Window win;

        static bool isActive = false;

        public string NameUl
        {
            get
            {
                return string.Empty;
            }
        }

        public int StationNumber { get; set; }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public TimeElement(PathGeometry geometry, double marginX, double marginY, double fontsize, Window win)
        {
            this.win = win;
            _text.FontSize = fontsize;
            _text.Margin = new Thickness(marginX, marginY,0,0);
            _startfontsize = fontsize;
            _startmargin = new Thickness(marginX, marginY, 0, 0);
            Flashing();
            //
            GeometryFigureCopy(geometry);
            //обработка времени
            Connections.NewSecond += StartTime;
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            Connections.NewSecond -= StartTime;
            LoadColorControl.NewColor -= NewColor;
        }

        private void NewColor()
        {
            Stroke = _color_stroke;
            _text.Foreground = _color_text;
            Fill = _color_fon;
        }

        public string InfoElement()
        {
            string result = string.Format("{0}", DateTime.Now.Day);
            switch (DateTime.Now.Month)
            {
                case 1:
                    result = string.Format("{0} {1}", result, "Января");
                    break;
                case 2:
                    result = string.Format("{0} {1}", result, "Февраля");
                    break;
                case 3:
                    result = string.Format("{0} {1}", result, "Марта");
                    break;
                case 4:
                    result = string.Format("{0} {1}", result, "Апреля");
                    break;
                case 5:
                    result = string.Format("{0} {1}", result, "Мая");
                    break;
                case 6:
                    result = string.Format("{0} {1}", result, "Июня");
                    break;
                case 7:
                    result = string.Format("{0} {1}", result, "Июля");
                    break;
                case 8:
                    result = string.Format("{0} {1}", result, "Августа");
                    break;
                case 9:
                    result = string.Format("{0} {1}", result, "Сентября");
                    break;
                case 10:
                    result = string.Format("{0} {1}", result, "Отктября");
                    break;
                case 11:
                    result = string.Format("{0} {1}", result, "Ноября");
                    break;
                case 12:
                    result = string.Format("{0} {1}", result, "Декабря");
                    break;
            }
            result = string.Format("{0} {1} года", result, DateTime.Now.Year);
            //
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = string.Format("{0} {1}", result, "Понедельник");
                    break;
                case DayOfWeek.Tuesday:
                    result = string.Format("{0} {1}", result, "Вторинк");
                    break;
                case DayOfWeek.Wednesday:
                    result = string.Format("{0} {1}", result, "Среда");
                    break;
                case DayOfWeek.Thursday:
                    result = string.Format("{0} {1}", result, "Четверг");
                    break;
                case DayOfWeek.Friday:
                    result = string.Format("{0} {1}", result, "Пятница");
                    break;
                case DayOfWeek.Saturday:
                    result = string.Format("{0} {1}", result, "Суббота");
                    break;
                case DayOfWeek.Sunday:
                    result = string.Format("{0} {1}", result, "Воскресенье");
                    break;
            }
            //
            return string.Format("{0} {1}", result, Notes);
        }

        private void StartTime()
        {
            Dispatcher.Invoke(new Action(() => Flashing()));
        }
        /// <summary>
        /// обработка времени
        /// </summary>
        private void Flashing()
        {
            if (!isActive)
            {
                if (win.Visibility == System.Windows.Visibility.Visible)
                {
                    win.Activate();
                    isActive = !isActive;
                }
            }
            string hour, minute, second;
            if (DateTime.Now.Hour < 10)
                hour = string.Format("0{0}", DateTime.Now.Hour);
            else hour = DateTime.Now.Hour.ToString();
            //
            if (DateTime.Now.Minute < 10)
                minute = string.Format("0{0}", DateTime.Now.Minute);
            else minute = DateTime.Now.Minute.ToString();
            //
            if (DateTime.Now.Second < 10)
                second = string.Format("0{0}", DateTime.Now.Second);
            else second = DateTime.Now.Second.ToString();
            //
            _text.Text = string.Format("{0:D2}.{1:D2}.{2} {3:D2}:{4:D2}:{5:D2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year.ToString().Substring(2), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
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
            //делаем элемент бесцветным
            Stroke =  _color_stroke;
            Fill = _color_fon;
            _text.Foreground = _color_text;
            //
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
            double x_summa = 0;
            double y_summa = 0;
            //
            for (int i = 0; i < _points.Count; i++)
            {
                x_summa += _points[i].X;
                y_summa += _points[i].Y;
                //
                if (i < _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[i + 1].X, Y2 = _points[i + 1].Y });
                else if (i == _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[0].X, Y2 = _points[0].Y });
            }
            //
            if (_points.Count != 0)
            {
                _pointCenter.X = x_summa / _points.Count;
                _pointCenter.Y = y_summa / _points.Count;
            }
            //
        }
    }
}
