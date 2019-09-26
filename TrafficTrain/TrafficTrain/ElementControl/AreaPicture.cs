using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using TrafficTrain.Interface;

namespace TrafficTrain
{
    /// <summary>
    /// Класс описывающий геометрию рамку станции
    /// </summary>
    public  class AreaPicture : Shape, IGraficElement, ISelectElement, IInfoElement
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
        //////цветовая палитра
        /// <summary>
        /// цвет заливки
        /// </summary>
        public static Brush _colorfill = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        /// <summary>
        /// цвет рамки
        /// </summary>
        public static Brush _colorstroke = Brushes.Black;
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1 * SystemParameters.CaretWidth;
        /// <summary>
        /// путь к файлу картинки
        /// </summary>
        string path = string.Empty;
        /// <summary>
        /// угол поворота картинки
        /// </summary>
        double angle;
        /// <summary>
        /// обозначение
        /// </summary>
        public string Nodes { get; set; }
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
        /// пояснения
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }


        /// <summary>
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }

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
        /// <param name="geometry">геометрия объекта</param>
        public AreaPicture(PathGeometry geometry, string path, double angle)
        {
            this.path = path;
            this.angle = angle;
            GeometryFigureCopy(geometry);
        }

        public string InfoElement()
        {
            return Nodes;
        }

        private void NewColor()
        {
            Fill = _colorfill;
            Stroke = _colorstroke;
        }
        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry)
        {
            foreach (PathFigure geo in geometry.Figures)
            {
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
            }
            Stroke = _colorstroke;
            if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                SetFillColor(path);
            else
                Fill = _colorfill;
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
        }

        private void SetFillColor(string path)
        {
            try
            {
                TransformedBitmap tb = new TransformedBitmap();
                // Create the source to use as the tb source.
                BitmapImage bi = new BitmapImage(new Uri(path));
                // Properties must be set between BeginInit and EndInit calls.
                tb.BeginInit();
                tb.Source = bi;
                RotateTransform transform = new RotateTransform(angle);
                tb.Transform = transform;
                tb.EndInit();
                Fill = new ImageBrush(tb);

            }
            catch { }
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
