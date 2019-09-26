using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Impulsesver.Client;
using TrafficTrain.Interface;
using TrafficTrain.Enums;
using TrafficTrain.Delegate;
using TrafficTrain.WorkWindow;
using TrafficTrain.EditText;
using SCADA.Common.Enums;

namespace TrafficTrain
{

    public struct Interval
    {
        int start;
        public int Start
        {
            get
            {
                return start;
            }
        }

        int end;
        public int End
        {
            get
            {
                return end;
            }
        }

        public Interval(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }

    public class CommandButton : Shape, IGraficElement, ISelectElement,  IInfoElement, IText
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
        private string nameObject = string.Empty;
        /// <summary>
        /// справочный текст
        /// </summary>
        private string help = string.Empty;
        /// <summary>
        /// Количество сработок по фильтрам
        /// </summary>
        public int CountFilter { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет нормальной работы при диагностики
        /// </summary>
        public static Brush _color_no_message = new LinearGradientBrush(Color.FromArgb(255,144, 180, 144), Color.FromArgb(127,144, 255, 144), 45);
        /// <summary>
        /// цвет не нормальной работы при диагностики
        /// </summary>
        public static Brush _color_yes_message = new LinearGradientBrush(Color.FromArgb(255, 255, 0, 0), Color.FromArgb(127, 160, 0, 0), 45);
        /// <summary>
        /// цвет текста справочного элемента
        /// </summary>
        public static Brush _color_text_help = Brushes.Black;
        /// <summary>
        /// цвет текста кнопок переключения
        /// </summary>
        public static Brush _color_text_switch_button = Brushes.Black;
        /// <summary>
        /// цвет текста журналов
        /// </summary>
        public static Brush _color_text_journal = Brushes.Black;
        /// <summary>
        /// цвет  рамки
        /// </summary>
        public static Brush _color_stroke = Brushes.Black;
        /// <summary>
        /// цвет фона если кнопка не нажата
        /// </summary>
        public static Brush _colorpasiv = new LinearGradientBrush(Color.FromRgb(170, 170, 170), Color.FromRgb(225, 225, 225), 45);
        /// <summary>
        /// цвет фона если кнопка нажата
        /// </summary>
        public static Brush _coloractiv = new LinearGradientBrush(Color.FromRgb(128, 0, 128), Color.FromRgb(128, 0, 200), 45);
        /// <summary>
        /// цвет фона справочной строки
        /// </summary>
        public static Brush _color_helpstring_fon = new LinearGradientBrush(Color.FromRgb(170, 170, 170), Color.FromRgb(225, 225, 225), 45);
        /// <summary>
        /// цвет границы справочной строки
        /// </summary>
        public static Brush _color_helpstring_stroke = new LinearGradientBrush(Color.FromRgb(128, 0, 128), Color.FromRgb(128, 0, 200), 45);
        //////
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1 * SystemParameters.CaretWidth;

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
        /// шестизначный номер станции контроля
        /// </summary>
        public int StationControl { get; set; }
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
         /// <summary>
        /// вид управляющей команды
        /// </summary>
        private ViewCommand viewcommand = ViewCommand.none;
        public ViewCommand ViewCommand
        {
            get
            {
                return viewcommand;
            }
            set
            {
                viewcommand = value;
            }
        }
        /// <summary>
        /// вид управляющей панели
        /// </summary>
        private ViewPanel viewpanel = ViewPanel.none;
        public ViewPanel ViewPanel
        {
            get
            {
                return viewpanel;
            }
            set
            {
                viewpanel = value;
            }
        }

        IList<Interval> intervals = new List<Interval>();
        /// <summary>
        /// Интервалы поездов
        /// </summary>
        public IList<Interval> Intervals
        {
            get
            {
                return intervals;
            }
        }
       
        bool downclick = false;
        /// <summary>
        /// нажата ли клавиша
        /// </summary>
        public bool DownClik
        {
            get
            {
                return downclick;
            }
        }

        /// <summary>
        /// показать диалоговое окно цвета
        /// </summary>
        public static event NewColor OpenColorDialog;
        /// <summary>
        /// событие показывать ли свойство объекта
        /// </summary>
        public static event ShowCommand ShowObject;
        /// <summary>
        /// событие показывать ли объект
        /// </summary>
        public static event CommandObject VisibleObject;
        /// <summary>
        /// событие включать или выключать объект
        /// </summary>
        public static event OnOffObject OnOffObject;
        /// <summary>
        /// событие показывать поездную обстановку
        /// </summary>
        public static event FilterNumberTrain TrainLayout;
        /// <summary>
        /// пояснения
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }
        /// <summary>
        /// Заблакированна ли клавиша
        /// </summary>
        private bool m_block = false;

        public string NameUl
        {
            get
            {
                return nameObject;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public CommandButton(PathGeometry geometry, string name, string help, double marginX, double marginY, double fontsize, double rotate, ViewCommand viewcommand, ViewPanel viewpanel)
        {
            nameObject = name;
            this.help = help;
            this.viewcommand = viewcommand;
            this.viewpanel = viewpanel;
            SetText(fontsize, rotate, marginX, marginY);
            //первоначальные координаты
            _startfontsize = fontsize;
            _startmargin = new Thickness(marginX, marginY, 0, 0);
            GeometryFigureCopy(geometry);
            //
            switch (viewpanel)
            {
                case ViewPanel.tabletrain:
                    {
                        VisibleObject += ShowElementTable;
                    }
                    break;
            }
            if (viewcommand == ViewCommand.style && !MainWindow.Admin)
            {
                Visibility = System.Windows.Visibility.Collapsed;
                Text.Visibility = Visibility;
            }
            //
            if(viewcommand == ViewCommand.pass || viewcommand == ViewCommand.electro)
                TrafficTrain.StationPath.CancelShow += CancelShow;
            //устанавливаем цвета
            NewColor();
            LoadColorControl.NewColor += NewColor;
        }

        public string InfoElement()
        {
            return string.Format("{0}", Notes);
        }

        ~CommandButton()
        {
            switch (viewpanel)
            {
                case ViewPanel.tabletrain:
                    {
                        VisibleObject -= ShowElementTable;
                    }
                    break;
            }
            if (viewcommand == ViewCommand.pass)
                TrafficTrain.StationPath.CancelShow -= CancelShow;
            //  
            LoadColorControl.NewColor -= NewColor;
        }


        private void SetText(double fontsize, double rotate, double marginX, double marginY)
        {
            _text.Text = string.Empty;
            if (!string.IsNullOrEmpty(nameObject))
            {
                string[] names = nameObject.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (names.Length > 0)
                {
                    _text.Text = names[0];
                    if (viewcommand == ViewCommand.viewtrain && names.Length > 1)
                    {
                        for (int i = 1; i < names.Length; i++)
                        {
                            string[] interval = names[i].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            if (interval.Length > 1)
                            {
                                int min, max;
                                if (int.TryParse(interval[0], out min) && int.TryParse(interval[1], out max))
                                    intervals.Add(new Interval(min, max));
                            }
                        }
                    }
                }
            }
            _text.Foreground = _color_text_help;
            _text.FontSize = fontsize;
            RotateText = rotate;
            _text.Margin = new Thickness(marginX, marginY, 0, 0);
            _text.RenderTransform = new RotateTransform(RotateText);
        }

        private void ShowElementTable(EventVisibleElement visible)
        {
            if (visible == EventVisibleElement.traintable)
            {
                if (viewcommand != ViewCommand.show_table_train)
                {
                    if (this.Visibility == System.Windows.Visibility.Visible)
                    {
                        this.Visibility = System.Windows.Visibility.Hidden;
                        this.Text.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        this.Visibility = System.Windows.Visibility.Visible;
                        this.Text.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
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
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
        }


        public void UpdateState()
        {
            downclick = !downclick;
            if (downclick)
                Fill = _coloractiv;
            else
                Fill = _colorpasiv;
        }

        public void Click(ContentControl content)
        {
            if (!m_block)
            {
                UpdateState();
                //
                switch (viewcommand)
                {
                    case ViewCommand.exit:
                        {
                            if (content != null)
                            {
                                if (content is Window)
                                    (content as Window).Close();
                                Click(null);
                            }
                        }
                        break;
                    case ViewCommand.diagnostics:
                        {
                            if (downclick)
                            {
                                try
                                {
                                    if (this is ISelectElement)
                                    {
                                        LoadProject.UpdateStation((this as ISelectElement).StationControl, nameObject);
                                        Click(null);
                                    }
                                }
                                catch { }
                            }
                        }
                        break;
                    case ViewCommand.style:
                        {
                            if (downclick)
                            {
                                if (OpenColorDialog != null)
                                    OpenColorDialog();
                                Click(null);
                            }
                            break;
                        }
                }
            }
        }

        private void NewColor()
        {
            Stroke = _color_stroke;
            //
            switch (viewcommand)
            {
                case ViewCommand.diagnostics:
                    if (YesFlashingDiagnostic())
                        Fill = _color_yes_message;
                    else Fill = _color_no_message;
                    _text.Foreground = _color_text_journal;
                    break;
                case ViewCommand.sound:
                    if (YesFlashingSound())
                        Fill = _color_yes_message;
                    else Fill = _color_no_message;
                    _text.Foreground = _color_text_journal;
                    break;
                case ViewCommand.content_exchange:
                    Fill = _color_helpstring_fon;
                    Stroke = _color_helpstring_stroke;
                    _text.Foreground = _color_text_help;
                    break;
                case ViewCommand.content_help:
                    Fill = _color_helpstring_fon;
                    Stroke = _color_helpstring_stroke;
                    _text.Foreground = _color_text_help;
                    break;
                default:
                    if (downclick)
                        Fill = _coloractiv;
                    else Fill = _colorpasiv;
                    _text.Foreground = _color_text_switch_button;
                    break;
            }     
        }

        private void NewDiagnostic(ViewMessageJournal view)
        {
            switch (view)
            {
                case ViewMessageJournal.sound:
                    {
                        if (viewcommand == ViewCommand.sound)
                        {
                            if (YesFlashingSound())
                            {
                                if (Fill != _color_yes_message)
                                    Fill = _color_yes_message;
                            }
                            else
                            {
                                if (Fill != _color_no_message)
                                    Fill = _color_no_message;
                            }
                        }
                    }
                    break;
                case ViewMessageJournal.diagnostik:
                    {
                        if (viewcommand == ViewCommand.diagnostics)
                        {
                            if (YesFlashingDiagnostic())
                            {
                                if (Fill != _color_yes_message)
                                    Fill = _color_yes_message;
                            }
                            else
                            {
                                if (Fill != _color_no_message)
                                    Fill = _color_no_message;
                            }
                        }
                    }
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


        private bool YesFlashingSound()
        {
            foreach (MessageInfo message in LoadProject.JournalSoundMessage)
            {
                if (!message.OpenMessage)
                    return true;
            }
            //
            return false;
        }

        private bool YesFlashingDiagnostic()
        {
            foreach (MessageInfo message in LoadProject.JournalDiagnosticMessage)
            {
                if (!message.OpenMessage)
                    return true;
            }
            //
            return false;
        }

        private void CancelShow(ViewCommand view)
        {
            if (viewcommand == ViewCommand.pass || viewcommand == ViewCommand.electro)
            {
                if (m_block)
                {
                    m_block = !m_block;
                    UpdateState();
                }
            }
        }

        /// <summary>
        /// Центрируем текст по центру
        /// </summary>
        public void LocationText()
        {
            //центрируем надпись
            double width = AlingmentText.LenghtStorona(((ArcSegment)_figure.Figures[0].Segments[_figure.Figures[0].Segments.Count - 2]).Point, _figure.Figures[0].StartPoint);
            double height = AlingmentText.LenghtStorona(((ArcSegment)_figure.Figures[0].Segments[2]).Point, _figure.Figures[0].StartPoint);
            _text.FontSize = AlingmentText.FontSizeText(_ktextweight, _ktextheight,
                new Rectangle()
                {
                    Width = width,
                    Height = height
                },
            _text.Text, RotateText);
            _text.Margin = AlingmentText.AlingmentCenter(_figure.Figures[0].StartPoint.X, _figure.Figures[0].StartPoint.Y, width, height, _text, RotateText);
            //
        }

      
    }
}
