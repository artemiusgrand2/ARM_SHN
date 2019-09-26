using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.EditText;
using TrafficTrain.Interface;
using SCADA.Common.Enums;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий элемент название станции
    /// </summary>
    public class NameStation : Shape, IGraficElement, ISelectElement, IDisposable, IInfoElement, IText
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

        //////основные свойства элемента названия станции

        /// <summary>
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// название станции
        /// </summary>
        public string StationName { get; set; }
        ////////

        //////цветовая палитра
        Brush _colorfon = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// нормальное состояние элемента
        /// </summary>
        public static Brush _color_track = Brushes.Black;
        /// <summary>
        /// есть неустановленные поезда
        /// </summary>
        public static Brush _color_train = Brushes.Red;
        //////

        private TextBlock m_text = new TextBlock(){ FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center };
        /// <summary>
        /// тескт названия объекта
        /// </summary>
        public TextBlock Text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }
        /// <summary>
        /// начальный размер текста
        /// </summary>
        double m_startfontsize;
        /// <summary>
        /// первоначальное разположение текста
        /// </summary>
        Thickness m_startmargin;
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
        /// относиться ли станция точкой выхода
        /// </summary>
        public bool PointOutput { get; set; }
        /// <summary>
        /// Является ли станция не диспетчерской
        /// </summary>
        public bool IsNotDispatcherStation { get; set; }
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
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public NameStation(PathGeometry geometry, string name, double marginX, double marginY, double fontsize)
        {
            StationName = name;
            //
            m_text.Text = name;
            m_text.FontSize = fontsize;
            m_text.Margin = new Thickness(marginX, marginY,0,0);
            m_startfontsize = fontsize;
            m_startmargin = new Thickness(marginX, marginY, 0, 0);
            m_text.Foreground = _color_track;
            //
            GeometryFigureCopy(geometry);
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
        }

        public string InfoElement()
        {
            return string.Format("{0}", Notes);
        }

        private void NewColor()
        {
            m_text.Foreground = _color_track;
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
            Stroke = _colorfon;
            Fill = _colorfon;
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
            //
            _pointCenter.X = m_text.Margin.Left + AlingmentText.WidthText(m_text) / 2;
            _pointCenter.Y = m_text.Margin.Top + AlingmentText.HeightText(m_text) / 2; 
        }
    }
}
