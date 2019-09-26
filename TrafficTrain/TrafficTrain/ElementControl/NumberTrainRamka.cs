using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Impulsesver.Client;

using TrafficTrain.Interface;
using TrafficTrain.EditText;
using TrafficTrain.WorkWindow;
using TrafficTrain.Constant;
using TrafficTrain.Enums;

using SCADA.Common.Enums;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий рамку вывода номеров поездов
    /// </summary>
    public class NumberTrainRamka : Shape, IGraficElement, ISelectElement, IInfoElement, IText
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

        //////основные свойства 
        /// <summary>
        /// Название пути перегона
        /// </summary>
        public string NameMovePath { get; set; }
        /// <summary>
        /// шестизначный номер станции слева
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции справа
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// название левого участка приближения пути
        /// </summary>
        public string LeftBorder { get; set; }
        /// <summary>
        /// название правого участка приближения пути
        /// </summary>
        public string RightBorder { get; set; }
        /// <summary>
        /// угол поворота текста
        /// </summary>
        private double angle;
        /// <summary>
        /// занятость перегона
        /// </summary>
        private StatesControl _employment_move = StatesControl.nocontrol;
        /// <summary>
        /// поворот перегона влево
        /// </summary>
        private bool _left = false;
        /// <summary>
        /// поворот перегона вправо
        /// </summary>
        private bool _right = false;
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет свободного перегона
        /// </summary>
        public static Brush _color_pasiv = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// цвет  занятого перегона
        /// </summary>
        public static Brush _coloractiv = new SolidColorBrush(Color.FromArgb(150, Brushes.Pink.Color.R, Brushes.Pink.Color.G, Brushes.Pink.Color.B));
        /// <summary>
        /// цвет  рамки
        /// </summary>
        public static Brush _color_ramka = Brushes.Black;
        /// <summary>
        /// цвет текста для номера поезда при нештатной ситуации
        /// </summary>
        public static Brush _color_text_notnormal = Brushes.Yellow;
        /// <summary>
        /// цвет текста для номера поезда по умолчанию
        /// </summary>
        public static Brush _color_text_defult = Brushes.Black;
        /// <summary>
        /// цвет текста для номера поезда при наличии плана
        /// </summary>
        public static Brush _color_text_plan = Brushes.Blue;
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
        private TextBlock _text = new TextBlock();
        /// <summary>
        /// тескт для списка номеров поездов
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

        static double _ktextweight = 1.6;
        public static double Kwtext
        {
            get
            {
                return _ktextweight;
            }
            set
            {
                _ktextweight = value;
            }
        }
        static double _ktextheight = 0.7;
        public static double Khtext
        {
            get
            {
                return _ktextheight;
            }
            set
            {
                _ktextheight = value;
            }
        }
        /// <summary>
        /// последняя реакция по занятию
        /// </summary>
        private DateTime _lastupdate = DateTime.Now;
        /// <summary>
        /// номера поездов показывать по умолчанию
        /// </summary>
        private bool isvisible_train = true;
        /// <summary>
        /// номера пути показывать по умолчанию
        /// </summary>
        private bool isvisible_track = true;
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
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumberleft">шестизначный номер станции слева</param>
        /// <param name="stationnumberright">шестизначный номер станции справа</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="border">название участков приближения пути</param>
        /// <param name="angle">угол поворота</param>
        public NumberTrainRamka(PathGeometry geometry, string leftborder, string rightborder, string nameelement, double angle)
        {
            LeftBorder = leftborder;
            RightBorder = rightborder;
            NameMovePath = nameelement;
            this.angle = angle;
            _text.Foreground = _color_text_defult;
            //
            GeometryFigureCopy(geometry);
            //импульс сервер и сервер номеров поездов
           // ImpulsesClient.ConnectDisconnectionServer += ConnectCloseServer;
            Connections.NewTart += StartFlashing;
            LoadColorControl.NewColor += NewColor;
            CommandButton.VisibleObject += VisibleObject;
        }

        ~NumberTrainRamka()
        {
           // ImpulsesClient.ConnectDisconnectionServer -= ConnectCloseServer;
            Connections.NewTart -= StartFlashing;
            LoadColorControl.NewColor -= NewColor;
            CommandButton.VisibleObject += VisibleObject;
        }

        private void NewColor()
        {
            ServerClose(true);
        }

        private void VisibleObject(EventVisibleElement visible)
        {
            switch (visible)
            {
                case EventVisibleElement.train:
                    isvisible_train = !isvisible_train;
                    break;
                case EventVisibleElement.track:
                    isvisible_track = !isvisible_track;
                    break;
            }
        }

        public string InfoElement()
        {
            string result = string.Format("{0} путь", NameMovePath);
            //
            return string.Format("{0} {1}", result, Notes);
        }

        /// <summary>
        /// изменяем состояние элемента
        /// </summary>
        public void UpdateState(StatesControl state)
        {
            switch (state)
            {
                case StatesControl.activ:
                    Fill = _coloractiv;
                    _employment_move = StatesControl.activ;
                    break;
                case StatesControl.pasiv:
                    Fill = _color_pasiv;
                    _employment_move = StatesControl.pasiv;
                    break;
                case StatesControl.nocontrol:
                    Fill = _colornotcontrol;
                    _employment_move = StatesControl.nocontrol;
                    break;
            }
            //
            _lastupdate = DateTime.Now;
        }
        /// <summary>
        /// изменяем направление левого поворота
        /// </summary>
        /// <param name="direction">повернут или нет перегон влево</param>
        public void LeftDirection(bool direction)
        {
            _left = direction;
        }
        /// <summary>
        /// изменяем направление правого поворота
        /// </summary>
        /// <param name="direction">повернут или нет перегон вправо</param>
        public void RightDirection(bool direction)
        {
            _right = direction;
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
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                    {
                        newfigure.Segments.Add(new ArcSegment() { Point = new Point(arc.Point.X, arc.Point.Y), Size = new Size(arc.Size.Width, arc.Size.Height) });
                        continue;
                    }
                }
                _figure.Figures.Add(newfigure);
            }
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            NewColor();
            Fill = _colornotcontrol;
            //
           // LocationText();
            _text.RenderTransform = new RotateTransform(angle);
        }

        /// <summary>
        /// Реагируем на подключение к серверу импульсов
        /// </summary>
        private void ConnectCloseServer()
        {
            Dispatcher.Invoke(new Action(() => ServerClose(false)));
        }

        private void ServerClose(bool iscolor)
        {
            if (ImpulsesClient.Connect)
                Stroke = _color_ramka;
            else
            {
                Stroke = _colornotcontrolstroke;
                Fill = _colornotcontrol;
                if (!iscolor)
                {
                    _employment_move = StatesControl.nocontrol;
                    _lastupdate = DateTime.Now;
                    _left = false;
                    _right = false;
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

        /// <summary>
        /// вычисляем размер текста (высоту)
        /// </summary>
        public static double FontSizeText(double textKw, double textKh, Rectangle ramka, string _text, double RotateText)
        {
            double fontsizeWr = textKw * ramka.Width / _text.Length;
            double fontsizeHr = ramka.Height * textKh;
            if ((fontsizeHr) >= (fontsizeWr))
                return fontsizeWr;
            else return fontsizeHr;
        }


        private void Aligment(double width, double height)
        {
            //если направление перегона не определено
            if (((!_left) && (!_right)) || ((_right) && (_left)))
                _text.Margin = AlingmentText.AlingmentCenter(_figure.Figures[0].StartPoint.X, _figure.Figures[0].StartPoint.Y, width, height, _text, angle);
            else
            {
                //если перегон развернут влево
                if (_left)
                    _text.Margin = AlingmentText.AlingmentLeft(_figure.Figures[0].StartPoint.X, _figure.Figures[0].StartPoint.Y, width, height, _text, angle);
                //если перегон развернут вправо
                if (_right)
                    _text.Margin = AlingmentText.AlingmentRight(_figure.Figures[0].StartPoint.X, _figure.Figures[0].StartPoint.Y, width, height, _text, angle);
            }
        }

        private void StartFlashing()
        {
            Dispatcher.Invoke(new Action(() => Flashing()));
        }
        /// <summary>
        /// обрабатываем мирцание номеров поездов
        /// </summary>
        private void Flashing()
        {
            //if (_text.Foreground != StationPath._color_path)
            //    _text.Foreground = StationPath._color_path;
        }
    }
}
