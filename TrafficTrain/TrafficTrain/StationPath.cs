using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий возможные состояния элементов и комбинации импульсов при которых они возможны
    /// </summary>
    public class StateElement
    {
        /// <summary>
        /// состояние элемента
        /// </summary>
        public StatesControl state { get; set; }
        /// <summary>
        /// комбинация импульсов
        /// </summary>
        public string Impuls { get; set; }
        /// <summary>
        /// название режима
        /// </summary>
        public Viewmode Name { get; set; }
        public bool _update = false;
        public StateElement()
        {
            state = StatesControl.nocontrol;
            Impuls = string.Empty;
        }
    }
    [Serializable]
    /// <summary>
    /// возможные состояния элементов перегона
    /// </summary>
    public enum Viewmode
    {
        //авария переезда
        accident = 0,
        //неисправность переезда
        fault = 1,
        //закрытие переезда
        closing = 2,
        //занятие блок участка, перегона , поворот перегона вправо или лево
        occupation = 3,
        //открыт или закрыт светофор (горит зеленым или красным) на перегоне
        opencloselight = 4,
        //срабатывание КГУ
        kguplay = 5,
        //неисправность КГУ
        faultkgu = 6,
        //срабатывание КТСМ
        ktcmplay = 7,
        //неисправность КТСМ
        faultktcm = 8
    }
    [Serializable]
    /// состояния элементов
    /// </summary>
    public enum StatesControl
    {
        //активный
        activ = 1,
        //пассивный
        pasiv = 0,
        //нет контроля
        nocontrol = 2
    }
    /// <summary>
    /// различные виды пути в зависимости от тяги
    /// </summary>
    public enum ViewTraction
    {
        //путь с автономной тягой
        diesel_traction = 0,
        //путь с электрической тягой
        electric_traction = 1
    }
    /// <summary>
    /// класс описывающий станционный путь
    /// </summary>
    class StationPath : Shape, GraficElement
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

        //////основные свойства пути
        /// <summary>
        /// шестизначный номер станции к которой принадлежит путь
        /// </summary>
        public int StationNumber { get; set; }
        /// <summary>
        /// название пути
        /// </summary>
        public string NamePath { get; set; }

        /// <summary>
        /// вид тяги для пути по умлчанию автономная
        /// </summary>
        public ViewTraction ViewTraction { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет не занятого пути
        /// </summary>
        Brush _colorpassiv = new SolidColorBrush(Color.FromRgb(190, 190, 190));
        /// <summary>
        /// цвет  занятого пути
        /// </summary>
        Brush _coloractiv = Brushes.Red;
        /// <summary>
        /// цвет неконтролируемого  пути
        /// </summary>
        Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(230, 230, 230));
        /// <summary>
        /// цвет пути при автодействии
        /// </summary>
        Brush _colorauto_action = Brushes.LightGreen;
        /// <summary>
        /// цвет контура пути с автономной тягой
        /// </summary>
        Brush _colordiesel_traction = Brushes.Black;
        /// <summary>
        /// цвет контура пути с электрической тягой
        /// </summary>
        Brush _colorelectric_traction = Brushes.Blue;
        //////
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public List<StateElement> Impulses { get; set; }
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double strokethickness = 1;

        private TextBlock _text = new TextBlock();
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
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public StationPath(int stationnumber, PathGeometry geometry, string name)
        {
            StationNumber = stationnumber;
            NamePath = name;
            GeometryFigureCopy(geometry);
        }

        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry)
        {
            foreach (PathFigure geo in geometry.Figures)
            {
                PathFigure newfigure = new PathFigure();
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
            Stroke = _colordiesel_traction;
            StrokeThickness = strokethickness;
            Fill = _colornotcontrol;
        }

        /// <summary>
        /// масштабироание объекта
        /// </summary>
        /// <param name="scale">масштаб</param>
        public void ScrollFigure(ScaleTransform scaletransform, double scale)
        {
            foreach (PathFigure geo in _figure.Figures)
            {
                geo.StartPoint = scaletransform.Transform(geo.StartPoint);
                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                        lin.Point = scaletransform.Transform(lin.Point);
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                    {
                        arc.Point = scaletransform.Transform(arc.Point);
                        arc.Size = new Size(arc.Size.Width * scale, arc.Size.Height * scale);
                    }
                }
            }
            //
            Point point = scaletransform.Transform(new Point(_text.Margin.Left, _text.Margin.Top));
            _text.Margin = new Thickness(point.X, point.Y, 0, 0);
            _text.FontSize *= scale;
            //
            StrokeThickness *= scale;
        }

        /// <summary>
        /// перемещение объекта
        /// </summary>
        /// <param name="deltaX">изменение по оси X</param>
        /// <param name="deltaY">изменение по оси Y</param>
        public void SizeFigure(double deltaX, double deltaY)
        {
            foreach (PathFigure geo in _figure.Figures)
            {
                geo.StartPoint = new Point(geo.StartPoint.X + deltaX, geo.StartPoint.Y + deltaY);
                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                        lin.Point = new Point(lin.Point.X + deltaX, lin.Point.Y + deltaY);
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                        arc.Point = new Point(arc.Point.X + deltaX, arc.Point.Y + deltaY);
                }
            }
            //
            _text.Margin = new Thickness(_text.Margin.Left + deltaX, _text.Margin.Top + deltaY, 0, 0);
        }

    }
}
