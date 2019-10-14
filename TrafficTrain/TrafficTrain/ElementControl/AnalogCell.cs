using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Impulsesver.Client;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;
using TrafficTrain.DataServer;
using TrafficTrain.Enums;
using TrafficTrain.EditText;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;


namespace TrafficTrain
{
    /// <summary>
    /// класс индикации шильд
    /// </summary>
    class AnalogCell : Shape, IGraficElement, IDisposable, IText, ISelectElement
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

        //////основные свойства пути

        private int m_stationControl = -1;
        /// <summary>
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl
        {
            get
            {
                return m_stationControl;
            }
            set
            {
               // m_stationControl = value;
            }
        }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
        /// <summary>
        /// название станции
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// название элемента
        /// </summary>
        public string NameElement { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет фона по умолчанию
        /// </summary>
        public static Brush m_color_fon_defult = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет рамки по умолчанию
        /// </summary>
        public static Brush m_color_stroke_defult = Brushes.Black;
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush m_colortext = Brushes.Black;
        /// толщина контура объкта
        /// </summary>
        double m_strokethickness = 1 * SystemParameters.CaretWidth;

        private TextBlock m_text = new TextBlock();
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
        static double _ktextweight = 1.6;
        static double _ktextheight = 0.8;
        /// <summary>
        /// начальный размер текста
        /// </summary>
        double m_startfontsize;
        /// <summary>
        /// первоначальное разположение текста
        /// </summary>
        Thickness m_startmargin;
        /// <summary>
        /// поворот текста
        /// </summary>
        public double RotateText { get; set; }
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
                return NameElement;
            }
        }

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

        private string m_noData = "--";
        /// <summary>
        /// id таблицы
        /// </summary>
        private int m_tableId;
        /// <summary>
        /// название измерения
        /// </summary>
        private string m_nameItem;

        public bool IsFind { get; set; }
        /// <summary>
        /// тип аналоговый данных
        /// </summary>
        private FieldType m_type = FieldType.UintType;

        private double m_factor;

        /// <summary>
        /// число символов до запятой
        /// </summary>
        private byte m_wholeCount = 3;
        /// <summary>
        /// число символов после запятой
        /// </summary>
        private byte m_fractionalCount = 2;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public AnalogCell(int station, PathGeometry geometry, string name, double marginX, double marginY, double fontsize, double rotate, int tableId, string nameItem, string format, string factor, FieldType type)
        {
            m_stationControl = station;
            NameElement = name;
            m_tableId = tableId;
            m_nameItem = nameItem;
            m_text.Foreground = m_colortext;
            m_text.FontSize = fontsize;
            RotateText = rotate;
            m_type = type;
            m_text.Margin = new Thickness(marginX, marginY, 0, 0);
            m_text.RenderTransform = new RotateTransform(RotateText);
            m_factor = AnalisFactor(factor);
            AnalisFormat(format);
            //первоначальные координаты
            m_startfontsize = fontsize;
            m_startmargin = new Thickness(marginX, marginY, 0, 0);
            GeometryFigureCopy(geometry);
            m_text.Text = m_noData;
            LocationText();
        }

        private void AnalisFormat(string format)
        {
            var indexPoint = format.IndexOf(".");
            if (indexPoint != -1 && indexPoint > 0)
            {
                m_wholeCount = (byte)indexPoint;
                m_fractionalCount = (byte)(format.Length - (indexPoint + 1));
            }
            else if (format.Length > 0)
            {
                m_wholeCount = (byte)format.Length;
                m_fractionalCount = 0;
            }
        }

        private double AnalisFactor(string factor)
        {
            double result;
            if(!double.TryParse(SCADA.Common.HelpCommon.HelpFuctions.GetFormatString(factor), out result))
                result =1;
            //
            return result;
        }

        public void Dispose()
        {

        }

        public void AnalisNewData()
        {
            if (m_tableId == 11)
            {
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (IsFind)
                {
                    if (Core.Stations.ContainsKey(StationControl))
                    {
                        if (m_tableId == 14)
                        {
                        }
                        if (Core.Stations[StationControl].ContainsKey(m_tableId))
                        {
                            var data = Core.Stations[StationControl][m_tableId].GetValue(m_nameItem);
                            //
                            if (data.Count > 0)
                            {
                                bool isNotValue;
                                double val = GetValue(data[0].Value, out isNotValue);
                                var valStr = (isNotValue) ? m_noData : GetFormatString(Core.Stations[StationControl][m_tableId].Corrector * val* m_factor);
                                if (valStr != m_text.Text)
                                {
                                    m_text.Text = valStr;
                                    LocationText();
                                }
                            }
                        }
                    }
                }
            }));
        }

        private string GetFormatString(double data)
        {
            try
            {
                var wholeNumber = (long)Math.Truncate(data);
                var whole = wholeNumber.ToString(string.Format("D{0}", m_wholeCount));
                if (data < 0 && wholeNumber == 0)
                    whole = whole.Insert(0, "-");
                if (m_fractionalCount > 0)
                    return string.Format("{0}.{1}", whole, data.ToString(string.Format("F{0}", m_fractionalCount)).Split(new char[] { ',', '.' })[1]);
                else
                    return whole;
            }
            catch
            {
                return "ERROR";
            }
        }

        private float GetValue(byte[] data, out bool isNotValue)
        {
            var buffer = new byte [data.Length];
            isNotValue = false;
            isNotValue = CheckNotValue(data);
            switch (data.Length)
            {
                case 4:
                    {
                        switch (m_type)
                        {
                            case FieldType.floatType:
                                return BitConverter.ToSingle(FormaterToSingle(data), 0);
                            case FieldType.intType:
                                return BitConverter.ToInt32(data, 0);
                            default:
                                return BitConverter.ToUInt32(data, 0);
                        }
                    }
                case 2:
                    {
                        switch (m_type)
                        {
                            case FieldType.floatType:
                                return BitConverter.ToSingle(FormaterToSingle(data), 0);
                            case FieldType.intType:
                                return BitConverter.ToInt16(data, 0);
                            default:
                                return BitConverter.ToUInt16(data, 0);
                        }
                    }
                default:
                    {
                        return (m_type == FieldType.floatType) ? BitConverter.ToSingle(FormaterToSingle(data), 0) : data[data.Length - 1];
                    }
            }
        }

        private bool CheckNotValue(byte[] data)
        {
            var countF = 0;
            foreach (var bit in data)
            {
                if (bit == 0xFF)
                    countF++;
            }
            if (countF > data.Length / 2)
                return true;
            //
            return false;
        }

        private byte[] FormaterToSingle(byte [] data)
        {
            var buffer = new List<byte>();
            buffer.AddRange(data);
            if (data.Length < 4)
            {
                while (buffer.Count < 4)
                {
                    buffer.Insert(0, 0);
                }
            }
            //
            return buffer.ToArray();
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
            Fill = m_color_fon_defult;
            Stroke = m_color_stroke_defult;
            m_strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = m_strokethickness;
        }


        private void LocationText()
        {
            //центрируем надпись
            double width = AlingmentText.LenghtStorona(((ArcSegment)_figure.Figures[0].Segments[_figure.Figures[0].Segments.Count - 2]).Point, _figure.Figures[0].StartPoint);
            double height = AlingmentText.LenghtStorona(((ArcSegment)_figure.Figures[0].Segments[2]).Point, _figure.Figures[0].StartPoint);
            m_text.FontSize = AlingmentText.FontSizeText(_ktextweight, _ktextheight,
                new Rectangle()
                {
                    Width = width,
                    Height = height
                },
            m_text.Text, RotateText);
            m_text.Margin = AlingmentText.AlingmentCenter(_figure.Figures[0].StartPoint.X, _figure.Figures[0].StartPoint.Y, width, height, m_text, RotateText);
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
