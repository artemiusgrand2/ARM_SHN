using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Interface;

namespace TrafficTrain
{
    /// <summary>
    /// Класс описывающий геометрию рамку станции
    /// </summary>
    class RamkaStation : Shape, IGraficElement, IDisposable
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
        /// обозначение
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public RamkaStation(PathGeometry geometry)
        {
            GeometryFigureCopy(geometry);
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
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
            Fill = _colorfill;
            Stroke = _colorstroke;
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
        }
    }
}
