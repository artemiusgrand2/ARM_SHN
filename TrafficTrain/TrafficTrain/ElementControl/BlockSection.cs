using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Interface;
using TrafficTrain.Enums;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий отдеьную рельсовую цепь
    /// </summary>
   public  class BlockSection : Shape, IGraficElement, ISelectElement, IDisposable, IInfoElement
    {
        #region Переменные и свойства
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
        /// название блок участка
        /// </summary>
        public string NameBlock { get; set; }
        /// <summary>
        /// название пути нахождения блок участка
        /// </summary>
        public string NameMove { get; set; }
        /// <summary>
        /// номер станции слева блок участка
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// номер станции справа блок участка
        /// </summary>
        public int StationTransition { get; set; }
        ////////

        //////цветовая палитра
          /// <summary>
        /// цвет не занятого блок участка
        /// </summary>
        public static Brush _colorpassiv = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет  занятого блок частка
        /// </summary>
        public static Brush _coloractiv = Brushes.Red;
        /// <summary>
        /// цвет неконтролируемого  блок участка
        /// </summary>
        public static  Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        //////
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
       // public List<StateElement> Impulses { get; set; }
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 9 * SystemParameters.CaretWidth;
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
        /// вид блок участка
        /// </summary>
        public BlockView BlockVid { get; set; }
        /// <summary>
        /// Координата начала участка
        /// </summary>
        public double LocationStart { get; set; }
        /// <summary>
        /// Координата окончания участка
        /// </summary>
        public double LocationEnd { get; set; }
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
                return NameBlock;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumberleft">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="nameblock">название объекта</param>
        public BlockSection(int stationnumberleft, int stationnumberright, PathGeometry geometry, string nameblock, string namepath, BlockView blockview, Visibility visible)
        {
            StationControl = stationnumberleft;
            StationTransition = stationnumberright;
            Visibility = visible;
            NameBlock = nameblock;
            NameMove = namepath;
            BlockVid = blockview;
            GeometryFigureCopy(geometry);
            //обработка импульсов
            LoadColorControl.NewColor += NewColor;
        }

        private void NewColor()
        {
            if (BlockVid == BlockView.notcontrol)
                Stroke = _colornotcontrolstroke;
        }

        public void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
        }

        public string InfoElement()
        {
            return string.Empty;
        }
   
        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry)
        {
            foreach (PathFigure geo in geometry.Figures)
            {
                PathFigure newfigure = new PathFigure() {  IsClosed = false};
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
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
        }
      

        public void GetStateBlock(StatesControl state)
        {
            switch (state)
            {
                case StatesControl.activ:
                    Stroke = _coloractiv;
                    break;
                case StatesControl.pasiv:
                    Stroke = _colorpassiv;
                    break;
                case StatesControl.nocontrol:
                    Stroke = _colornotcontrolstroke;
                    break;
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
            for (int i = 0; i <= _points.Count; i++)
            {
                if (i < _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[i + 1].X, Y2 = _points[i + 1].Y });
            }
        }
    }
}
