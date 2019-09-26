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
using sdm.diagnostic_section_model;
using sdm.diagnostic_section_model.client_impulses;
using Move;
using log4net;

namespace TrafficTrain
{
    /// <summary>
    /// делегат сообщает показывать или нет номера поездов
    /// </summary>
    public delegate void VisibleNumberTrain();
    /// <summary>
    /// делегат сообщает какой из фильтров приминен
    /// </summary>
    public delegate void FilterNumberTrain(TrafficTrain.DataGrafik.ViewPanel ViewPanel, TrafficTrain.DataGrafik.ViewCommand ViewCommand);

    class CommandButton : Shape, GraficElement, CommandElement
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
        /// <summary>
        /// показывет выбран ли элемент для построения команды
        /// </summary>
        public bool SelectElement { get; set; }

        /// <summary>
        /// название элемента
        /// </summary>
        public string NameCommand { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет нормальной работы при диагностики
        /// </summary>
        public static Brush _colortestnormal = Brushes.LightGreen;
        /// <summary>
        /// цвет не нормальной работы при диагностики
        /// </summary>
        public static Brush _colortestnotnormal = Brushes.Red;
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush _colorfont = Brushes.Black;
        /// <summary>
        /// цвет  рамки
        /// </summary>
        public static Brush _colorstroke = Brushes.Black;
        /// <summary>
        /// цвет фона если кнопка не нажата
        /// </summary>
        public static Brush _colorpasiv = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        /// <summary>
        /// цвет фона если кнопка нажата
        /// </summary>
        public static Brush _coloractiv = Brushes.MediumPurple;
        //////
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1;

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
        /// поворот текста
        /// </summary>
        public  double RotateText { get; set; }
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
        static double _ktextheight = 0.8;
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
        /// вид управляющей команды
        /// </summary>
        private TrafficTrain.DataGrafik.ViewCommand _viewcommand = TrafficTrain.DataGrafik.ViewCommand.none;
        public TrafficTrain.DataGrafik.ViewCommand ViewCommand
        {
            get
            {
                return _viewcommand;
            }
            set
            {
                _viewcommand = value;
            }
        }
        /// <summary>
        /// вид управляющей панели
        /// </summary>
        private TrafficTrain.DataGrafik.ViewPanel _viewpanel = TrafficTrain.DataGrafik.ViewPanel.none;
        public TrafficTrain.DataGrafik.ViewPanel ViewPanel
        {
            get
            {
                return _viewpanel;
            }
            set
            {
                _viewpanel = value;
            }
        }
        /// <summary>
        /// нажата ли клавиша
        /// </summary>
        bool _downclick = false;
        /// <summary>
        /// событие показывать ли номер поезда
        /// </summary>
        public static event VisibleNumberTrain VisibleNumberTrain;
        /// <summary>
        /// событие показывать поездную обстановку
        /// </summary>
        public static event FilterNumberTrain TrainLayout;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public CommandButton(PathGeometry geometry, string name, double marginX, double marginY, double fontsize, double rotate, TrafficTrain.DataGrafik.ViewCommand viewcommand, TrafficTrain.DataGrafik.ViewPanel viewpanel)
        {
            NameCommand = name;
            _viewcommand = viewcommand;
            _viewpanel = viewpanel;
            _text.Text = NameCommand;
            _text.Foreground = _colorfont;
            _text.FontSize = fontsize;
            RotateText = rotate;
            _text.Margin = new Thickness(marginX, marginY, 0, 0);
            _text.RenderTransform = new RotateTransform(RotateText);
            //первоначальные координаты
            _startfontsize = fontsize;
            _startmargin = new Thickness(marginX, marginY, 0, 0);
            GeometryFigureCopy(geometry);
            //
            if (_viewpanel == DataGrafik.ViewPanel.drawtrain)
            {
                switch (_viewcommand)
                {
                    case DataGrafik.ViewCommand.diagnostics:
                        MainWindow.NewDiagnostic += NewDiagnostic;
                        JournalMessageDiagnostik.UpdateInfo += NewDiagnostic;
                        break;
                    case DataGrafik.ViewCommand.sound:
                        Fill = _colortestnormal;
                        JournalMessageSound.UpdateInfo += NewSound;
                        LightElementControl.UpdateCollectionSound += NewSound;
                        break;
                    default:
                        Fill = _colorpasiv;
                        break;
                }
            }
            //  
            LoadProject.NewColor += NewColor;
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
            Stroke = _colorstroke;
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
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
            Point point = scaletransform.Transform(new Point(_text.Margin.Left, _text.Margin.Top));
            _text.Margin = new Thickness(point.X, point.Y, 0, 0);
            _text.FontSize *= scale;
            //
            StrokeThickness *= scale;
        }

        public void Click()
        {
            _downclick = !_downclick;
            switch (_viewcommand)
            {
                case DataGrafik.ViewCommand.help:
                    {
                        if (_downclick)
                        {
                            try
                            {
                                if(MainWindow.HelpFile!=null)
                                    System.Diagnostics.Process.Start(MainWindow.HelpFile);
                                Fill = _coloractiv;
                            }
                            catch { };
                        }
                        else
                        {
                            Fill = _colorpasiv;
                        }
                    }
                    break;
                case DataGrafik.ViewCommand.numbertrain:
                    {
                        if (_downclick)
                        {
                            Fill = _coloractiv;
                            if (VisibleNumberTrain != null)
                                VisibleNumberTrain();
                        }
                        else
                        {
                            Fill = _colorpasiv;
                            if (VisibleNumberTrain != null)
                                VisibleNumberTrain();
                        }
                    }
                    break;
                case DataGrafik.ViewCommand.filter_train:
                    {
                        if (_downclick)
                        {
                            Fill = _coloractiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                        else
                        {
                            Fill = _colorpasiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                    }
                    break;
                case DataGrafik.ViewCommand.train_even:
                    {
                        if (_downclick)
                        {
                            Fill = _coloractiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                        else
                        {
                            Fill = _colorpasiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                    }
                    break;
                case DataGrafik.ViewCommand.train_odd:
                    {
                        if (_downclick)
                        {
                            Fill = _coloractiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                        else
                        {
                            Fill = _colorpasiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                    }
                    break;
                case DataGrafik.ViewCommand.train_unknow:
                    {
                        if (_downclick)
                        {
                            Fill = _coloractiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                        else
                        {
                            Fill = _colorpasiv;
                            if (TrainLayout != null)
                                TrainLayout(_viewpanel, _viewcommand);
                        }
                    }
                    break;
            }
        }

        private void NewColor()
        {
            UpdateColor();
        }

        private void NewDiagnostic()
        {
            if (YesFlashingDiagnostic())
            {
                if (Fill != _colortestnotnormal)
                    Fill = _colortestnotnormal;
            }
            else
            {
                if (Fill != _colortestnormal)
                    Fill = _colortestnormal;
            }
        }

        private void NewSound()
        {
            if (YesFlashingSound())
            {
                if (Fill != _colortestnotnormal)
                    Fill = _colortestnotnormal;
            }
            else
            {
                if (Fill != _colortestnormal)
                    Fill = _colortestnormal;
            }
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

        /// <summary>
        /// откат графики к начальнвым координатам
        /// </summary>
        public void StartPosition(Point center, double scroll)
        {
            foreach (PathFigure geo in _figure.Figures)
            {
                geo.StartPoint = new Point((geo.StartPoint.X - center.X) / scroll, (geo.StartPoint.Y - center.Y) / scroll);

                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                    {
                        lin.Point = new Point((lin.Point.X - center.X) / scroll, (lin.Point.Y - center.Y) / scroll);
                        continue;
                    }
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                    {
                        arc.Point = new Point((arc.Point.X - center.X) / scroll, (arc.Point.Y - center.Y) / scroll);
                        arc.Size = new Size(arc.Size.Width / scroll, arc.Size.Height / scroll);
                        continue;
                    }
                }
            }
            //
            _text.Margin = new Thickness((_text.Margin.Left - center.X) / scroll, (_text.Margin.Top - center.Y) / scroll, 0, 0);
            _text.FontSize = (_text.FontSize / scroll);
            StrokeThickness = _strokethickness;
        }

        /// <summary>
        /// Изменяем цвет элемента
        /// </summary>
        private void UpdateColor()
        {
          
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

        //private void StartFlashing()
        //{
        //    Dispatcher.Invoke(new Action(() => Flashing()));
        //}
        ///// <summary>
        ///// обрабатываем мирцание
        ///// </summary>
        //private void Flashing()
        //{
        //    switch (_viewcommand)
        //    {
        //        case DataGrafik.ViewCommand.sound:
        //            {
        //                if (YesFlashingSound())
        //                {
        //                    if(Fill!=_colortestnotnormal)
        //                      Fill = _colortestnotnormal;
        //                }
        //                else
        //                {
        //                    if (Fill != _colortestnormal)
        //                        Fill = _colortestnormal;
        //                }
        //            }
        //            break;
        //    }  
        //}

        private bool YesFlashingSound()
        {
            foreach (SoundMessage message in LoadProject.JournalSoundMessage)
            {
                if (!message.OpenMessage)
                    return true;
            }
            //
            return false;
        }

        private bool YesFlashingDiagnostic()
        {
            foreach (SoundMessage message in LoadProject.JournalDiagnosticMessage)
            {
                if (!message.OpenMessage)
                    return true;
            }
            //
            return false;
        }

        /// <summary>
        /// Центрируем текст по центру
        /// </summary>
        private void LocationText()
        {
            //центрируем надпись
            double width = LenghtStorona(((ArcSegment)_figure.Figures[0].Segments[_figure.Figures[0].Segments.Count - 2]).Point, _figure.Figures[0].StartPoint);
            double height = LenghtStorona(((ArcSegment)_figure.Figures[0].Segments[2]).Point, _figure.Figures[0].StartPoint);
            _text.FontSize = FontSizeText(_ktextweight, _ktextheight,
                new Rectangle()
                {
                    Width = width,
                    Height = height
                },
            _text.Text, RotateText);
            _text.Margin = AlingmentCenter(_figure.Figures[0].StartPoint.X, _figure.Figures[0].StartPoint.Y, width, height, _text, RotateText);
            //
        }

        /// <summary>
        /// определяем длину отрезка между двумя точками
        /// </summary>
        /// <param name="p1">точка один</param>
        /// <param name="p2">точка два</param>
        /// <returns></returns>
        public static double LenghtStorona(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
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

        /// <summary>
        /// Выравнивание название главного пути по центру при изменении длины или ширины
        /// </summary>
        /// <returns></returns>
        public static Thickness AlingmentCenter(double ramkaX, double ramkaY, double width, double height, TextBlock _text, double RotateText)
        {
            if (RotateText == 0 || Math.Abs(RotateText) == 360)
                return new Thickness(ramkaX + (width - WidthText(_text)) / 2, ramkaY + (height - HeightText(_text)) / 2, 0, 0);
            //
            if (Math.Abs(RotateText) == 90)
                return new Thickness(ramkaX - (height - HeightText(_text)) / 2, ramkaY + (width - WidthText(_text)) / 2, 0, 0);
            //
            if (Math.Abs(RotateText) == 180)
                return new Thickness(ramkaX - (width - WidthText(_text)) / 2, ramkaY - (height - HeightText(_text)) / 2, 0, 0);
            //
            if (Math.Abs(RotateText) == 270)
                return new Thickness(ramkaX + (height - HeightText(_text)) / 2, ramkaY - (width - WidthText(_text)) / 2, 0, 0);
            //
            return new Thickness(0, 0, 0, 0);
        }


        /// <summary>
        /// ширина текста в пикселях
        /// </summary>
        /// <param name="textblock"></param>
        /// <returns></returns>
        public static double WidthText(TextBlock textblock)
        {
            Typeface typeface = new Typeface(textblock.FontFamily, textblock.FontStyle, textblock.FontWeight, textblock.FontStretch);
            System.Windows.Media.Brush brush = new SolidColorBrush();
            FormattedText formatedText = new FormattedText(textblock.Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, textblock.FontSize, brush);
            return formatedText.Width;
        }
        /// <summary>
        /// высота текста в пикселях
        /// </summary>
        /// <param name="textblock"></param>
        /// <returns></returns>
        public static double HeightText(TextBlock textblock)
        {
            Typeface typeface = new Typeface(textblock.FontFamily, textblock.FontStyle, textblock.FontWeight, textblock.FontStretch);
            System.Windows.Media.Brush brush = new SolidColorBrush();
            FormattedText formatedText = new FormattedText(textblock.Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, textblock.FontSize, brush);
            return formatedText.Height;
        }
    }
}
