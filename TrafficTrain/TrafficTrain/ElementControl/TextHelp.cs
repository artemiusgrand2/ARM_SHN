using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.Interface;
using ARM_SHN.EditText;

namespace ARM_SHN.ElementControl
{
    class TextHelp : Shape, IGraficElement, IDisposable, IText, ISelectElement, IInfoElement
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

        /// <summary>
        /// цвет фона
        /// </summary>
         Brush _color_fon = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush _color_text = Brushes.Black;
        /// <summary>
        /// цвет рамки
        /// </summary>
        Brush _color_stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
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
                return m_text.Text;
            }
        }

        public int StationControl { get; set; }
        public int StationTransition { get; set; }
        public string FileClick { get; set; } = string.Empty;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public TextHelp(PathGeometry geometry, double marginX, double marginY, double fontsize, string text)
        {
            m_text.FontSize = fontsize;
            m_text.Text = text;
            m_text.Margin = new Thickness(marginX, marginY,0,0);
            m_startfontsize = fontsize;
            m_startmargin = new Thickness(marginX, marginY, 0, 0);
            //
            GeometryFigureCopy(geometry);
            //обработка времени
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
        }

        private void NewColor()
        {
            m_text.Foreground = _color_text;
            Fill = _color_fon;
        }

        public string InfoElement()
        {
            return Notes;
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
            m_text.Foreground = _color_text;
        }

    }
}
