using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.Impulsesver.Client;
using TrafficTrain.EditText;
using TrafficTrain.Interface;
using TrafficTrain.Enums;
using TrafficTrain.Constant;
using TrafficTrain.WorkWindow;
using TrafficTrain.Delegate;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий станционный путь
    /// </summary>
    public class StationPath : Shape,  IGraficElement, ISelectElement, IInfoElement, IText, IIndicationEl
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
       
        //////основные свойства пути
        /// <summary>
        /// шестизначный номер станции к которой принадлежит путь
        /// </summary>
        public int StationControl { get; set; }

        public int StationTransition { get; set; }

        private string nametrack = string.Empty;
        /// <summary>
        /// название пути
        /// </summary>
        public string NameTrack
        {
            get
            {
                return nametrack;
            }
        }

        /// <summary>
        /// вид тяги для пути по умлчанию автономная
        /// </summary>
        public ViewTraction ViewTraction { get; set; }
        /// <summary>
        /// имеет ли путь платформу
        /// </summary>
        public bool IsPlatform { get; set; }
        ////////

        #region Colors
        //////цветовая палитра
        /// <summary>
        /// цвет огражденного пути
        /// </summary>
        public static Brush _colorfencing = Brushes.SaddleBrown;
        /// <summary>
        /// цвет не занятого пути
        /// </summary>
        public static Brush _colorpassiv = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет  занятого пути
        /// </summary>
        public static Brush _coloractiv = Brushes.Red;
        /// <summary>
        /// цвет неконтролируемого  пути
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(230, 230, 230));
        /// <summary>
        /// цвет неконтролируемого  пути его рамка
        /// </summary>
        public static Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(205, 205, 205));
        /// <summary>
        /// цвет пути при замыкании
        /// </summary>
        public static Brush _color_loking = Brushes.LightGreen;
        /// <summary>
        /// цвет пути при замыкании маневровом
        /// </summary>
        public static Brush _color_lokingM = Brushes.White;
        /// <summary>
        /// цвет пути при замыкании аварийном
        /// </summary>
        public static Brush _color_lokingY = Brushes.Yellow;
        /// <summary>
        /// цвет  пути с автономной тягой
        /// </summary>
        public static Brush m_colordiesel_traction = Brushes.Black;
        /// <summary>
        /// цвет  пути если есть наличие платформы
        /// </summary>
        public static Brush m_color_platform = Brushes.White;
        /// <summary>
        /// цвет контура пути с электрической тягой
        /// </summary>
        public static Brush _colorelectric_traction = Brushes.Blue;
        /// <summary>
        /// цвет текста для номера поезда
        /// </summary>
        public static Brush _color_train = Brushes.Yellow;
        /// <summary>
        /// цвет текста для номера поезда при наличии плана
        /// </summary>
        public static Brush _color_train_plan = Brushes.Blue;
        /// <summary>
        /// цвет текста для названия главного пути
        /// </summary>
        public static Brush _color_path = Brushes.Black;
        /// <summary>
        /// цвет текста для номера поезда если есть вектор (роспуска, выставления, транзита)
        /// </summary>
        public static Brush _color_vertor_train = Brushes.Blue;

        #endregion
        //////
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement > Impulses { get; set; }
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1 * SystemParameters.CaretWidth;

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
        /// поворот текста
        /// </summary>
        public  double RotateText { get; set; }
        /// <summary>
        /// время последнего занятия
        /// </summary>
        DateTime m_timelastactiv ;
        /// <summary>
        /// задержка в минутах перед началом мигания после освобождения пути
        /// </summary>
        const double _count_time_pause = 2;
        bool _emptyPath = true;
        /// <summary>
        /// показываем является ли  путь не пустым 
        /// </summary>
        public bool EmptyPath
        {
            get
            {
                return _emptyPath;
            }
            set
            {
                _emptyPath = value;
            }
        }
        /// <summary>
        /// показывать ли номера поездов
        /// </summary>
        bool isvisibletrain = true;
        /// <summary>
        /// показывать ли номера путей
        /// </summary>
        bool isvisibletrack = true;
        /// <summary>
        /// количество обновлений
        /// </summary>
        private byte m_updateCurState = 0;
        private StatesControl m_currentstate = StatesControl.nocontrol;
        /// <summary>
        /// текущее состояние пути по занятости
        /// </summary>
        public StatesControl CurrentControl { get { return m_currentstate; } }
        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.fencing, Viewmode.occupation, Viewmode.locking, Viewmode.lockingM, Viewmode.lockingY};
        /// <summary>
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>() { Viewmode.electrification};
        /// <summary>
        /// текущее выравнивание текста
        /// </summary>
        TextAlignment _current_alignment = TextAlignment.Center;

        /// <summary>
        /// обозначение
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }
        private byte m_countShowP = 0;
        /// <summary>
        /// показывать ли настройки
        /// </summary>
        private IDictionary<ViewCommand, bool> m_startShow = new Dictionary<ViewCommand, bool>() { { ViewCommand.electro, false }, { ViewCommand.pass, false } };
        private ViewCommand m_currentView;
        public static event ShowCommand CancelShow;

        public string NameUl
        {
            get
            {
                return nametrack;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="text">название объекта</param>
        public StationPath(PathGeometry geometry, string text, string name, double marginX, double marginY, double fontsize, 
                            double rotate, Dictionary<Viewmode, StateElement> impulses)
        {
            nametrack = name;
            m_text.Text = text;
            //проверяем является ли путь пустым
            if (iSNullOrEmpty(nametrack))
                _emptyPath = false;
            //
            m_text.Foreground = _color_path;
            m_text.FontSize = fontsize;
            RotateText = rotate;
            m_text.Margin = new Thickness(marginX, marginY, 0, 0);
            m_text.RenderTransform = new RotateTransform(RotateText);
            //первоначальные координаты
            m_startfontsize = fontsize;
            m_startmargin = new Thickness(marginX, marginY, 0, 0);
            Impulses = AnalisCollectionStateControl(impulses);
            //графика
            GeometryFigureCopy(geometry);
            m_timelastactiv = DateTime.Now;
            Analis();
            //обработка информации по импульсам и номерам поездов
            if (Impulses.Count > 0)
            {
                Connections.NewTart += StartFlashing;
            }
            //
            LoadColorControl.NewColor += NewColor;
            CommandButton.ShowObject += ShowObject;
        }

        ~StationPath()
        {
            Connections.NewTart -= StartFlashing;
            LoadColorControl.NewColor -= NewColor;
            CommandButton.ShowObject -= ShowObject;
        }

        /// <summary>
        /// анализируем коллекцию 
        /// </summary>
        /// <param name="impulses"></param>
        private Dictionary<Viewmode, StateElement> AnalisCollectionStateControl(Dictionary<Viewmode, StateElement> impulses)
        {
            foreach (KeyValuePair<Viewmode, StateElement> control in impulses)
            {
                //смотрим с каким графическим объектом работает контроль
                foreach (Viewmode stroke_element in _priority_stroke)
                {
                    if (stroke_element == control.Value.Name)
                    {
                        control.Value.ViewControlDraw = ViewElementDraw.stroke;
                        break;
                    }
                }
            }
            //
            return impulses;
        }

        public string InfoElement()
        {
            return string.Format("{0}", Notes);
        }

        private string GetTimeOccupation()
        {
            if (m_updateCurState >= 2)
            {
                return " c " + GetTimeUpdateStatus();
            }
            //
            return string.Empty;
        }

        private string GetTimeUpdateStatus()
        {
            if (Impulses.ContainsKey(Viewmode.occupation))
                return Impulses[Viewmode.occupation].LastUpdate.ToLongTimeString();
            else return "Время неизвестно !!!";
        }

        private bool iSNullOrEmpty(string str)
        {
            if (str == null)
                return true;
            else
            {
                if (str.Trim(new char[] { ' ' }).Length == 0)
                    return true;
                else return false;
            }
        }

        private void ShowObject(ViewCommand view)
        {
            if (m_currentView == ViewCommand.none)
            {
                switch (view)
                {
                    case ViewCommand.electro:
                        {
                            if (ViewTraction == Enums.ViewTraction.electric_traction)
                            {
                                m_startShow[view] = true;
                                m_currentView = view;
                                Stroke = m_color_platform;
                            }
                        }
                        break;
                    case ViewCommand.pass:
                        {
                            if (IsPlatform)
                            {
                                m_startShow[view] = true;
                                m_currentView = view;
                                Stroke = m_color_platform;
                            }
                        }
                        break;
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
            if (Impulses.Count == 0)
                Stroke = _colornotcontrolstroke;
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
            Fill = _colornotcontrol;
            Stroke = m_colordiesel_traction;
            //LocationText();
        }

        private void NewColor()
        {
            UpdateElement(false);
            //
            if (m_text.Text == NameTrack)
                m_text.Foreground = _color_path;

        }


        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (ImpulsesClient.Connect)
                    //GetColorStroke();
                    Stroke = m_colordiesel_traction;
                else
                {
                    foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                    {
                        imp.Value.state = StatesControl.nocontrol;
                        imp.Value.LastUpdate = DateTime.Now;
                    }
                    //обнуляем значения
                    Stroke = _colornotcontrolstroke;
                    Fill = _colornotcontrol;
                    m_currentstate = StatesControl.nocontrol;
                    if (m_updateCurState <= 1)
                        m_updateCurState++;
                    //выравниваем текст по центру
                    UpdateAlignment(TextAlignment.Center);
                }
            }
        }

        public IList<string> Analis()
        {
            bool update = false;
            if (nametrack == "Аварийное отключение")
            {
            }
            var result = new List<string>();
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                if (Imp.Value.Name != Viewmode.electrification)
                {
                    StatesControl state = Imp.Value.state;
                    Imp.Value.state = GetImpuls.GetStateControl(StationControl, Imp.Value.Impuls);
                    //
                    if (state != Imp.Value.state)
                    {
                        if (Imp.Value.Name == Viewmode.occupation)
                        {
                            m_currentstate = Imp.Value.state;
                            if (m_updateCurState <= 1)
                                m_updateCurState++;
                            if (state == StatesControl.activ && Imp.Value.state != StatesControl.activ)
                                m_timelastactiv = DateTime.Now;
                        }
                        Imp.Value.Update = true;
                        update = true;
                        result.AddRange(Diagnostic.DiagnosticControl(Imp.Value));
                    }
                }
            }
            //обновляем элемент
            if (update)
            {
                UpdateElement(true);
                //анализируем где находится голова поезда
                AnalisHeadSide();
            }
            //
            return result;
        }

        /// <summary>
        /// анализ состояния для первого запуска дочерних элементов
        /// </summary>
        public void Analis(StationPath track)
        {
            bool update = false;
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                if (Imp.Value.Name != Viewmode.electrification && track.Impulses.ContainsKey(Imp.Key))
                {
                    StatesControl state = Imp.Value.state;
                    Imp.Value.state = track.Impulses[Imp.Key].state;
                    //
                    if (state != Imp.Value.state)
                    {
                        if (Imp.Value.Name == Viewmode.occupation)
                        {
                            m_currentstate = Imp.Value.state;
                            if (m_updateCurState <= 1)
                                m_updateCurState++;
                            if (state == StatesControl.activ && Imp.Value.state != StatesControl.activ)
                                m_timelastactiv = track.Impulses[Imp.Key].LastUpdate;
                        }
                        Imp.Value.LastUpdate = track.Impulses[Imp.Key].LastUpdate;
                        Imp.Value.Update = true;
                        update = true;
                    }
                }
            }
            //обновляем элемент
            if (update)
            {
                UpdateElement(true);
                //анализируем где находится голова поезда
                AnalisHeadSide();
            }
        }

        /// <summary>
        /// анализируем где находится голова поезда
        /// </summary>
        private void AnalisHeadSide()
        {
   
            if ((Impulses[Viewmode.head_left].state == StatesControl.activ && Impulses[Viewmode.head_right].state != StatesControl.activ) ||
                (Impulses[Viewmode.head_left].state != StatesControl.activ && Impulses[Viewmode.head_right].state == StatesControl.activ))
            {
                if (Impulses[Viewmode.head_left].state == StatesControl.activ && Impulses[Viewmode.head_right].state != StatesControl.activ)
                    UpdateAlignment(TextAlignment.Left);
                else UpdateAlignment(TextAlignment.Right);
            }
            else
                UpdateAlignment(TextAlignment.Center);
        }

        private void UpdateAlignment(TextAlignment alignment)
        {
            if (_current_alignment != alignment)
            {
                _current_alignment = alignment;
               // LocationText();
            }
        }

        private void UpdateCurrentState(List<Viewmode> list_priority, ref bool update)
        {
            StateElement control = CheckPriorityState(list_priority);
            if (control != null)
                SetState(control);
            else
            {
                foreach (Viewmode mode in list_priority)
                {
                    if (Impulses.ContainsKey(mode))
                    {
                        SetState(Impulses[mode]);
                        break;
                    }
                }
            }
            update = true;
        }


        /// <summary>
        /// Изменяем цвет элемента
        /// </summary>
        private void UpdateElement(bool CheckUpdate)
        {
            if (Impulses.Count > 0)
            {
                bool _update_fill = false;
                bool _update_stroke = false;
                //
                foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                {
                    if ((CheckUpdate && imp.Value.Update) || !CheckUpdate)
                    {
                        switch (imp.Value.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                if (!_update_fill)
                                    UpdateCurrentState(_priority_fill, ref _update_fill);
                                break;
                            case ViewElementDraw.stroke:
                                if (!_update_stroke)
                                    UpdateCurrentState(_priority_stroke, ref _update_stroke);
                                break;
                        }
                    }
                    //
                    if (CheckUpdate && imp.Value.Update)
                        imp.Value.Update = false;
                }
                //при изменении цветовой расскраски
                if (!CheckUpdate)
                {
                    if (!_update_stroke)
                        Stroke = m_colordiesel_traction;
                    if (!_update_fill)
                        Fill = _colornotcontrol;
                }
            }
            else
            {
                if (!CheckUpdate)
                {
                    Stroke = _colornotcontrolstroke;
                    Fill = _colornotcontrol;
                }
            }
        }


        private void SetText(string numbertrain)
        {
            if (m_text.Text != numbertrain)
                m_text.Text = numbertrain;
        }


        /// <summary>
        /// находим более приоритетное состояние из представленного перечьня
        /// </summary>
        /// <param name="priority_control">перечень контролей в порядке убывания приоритета</param>
        /// <returns></returns>
        private StateElement CheckPriorityState(List<Viewmode> priority_control)
        {
            foreach (Viewmode control in priority_control)
            {
                if (Impulses.ContainsKey(control))
                {
                    if (Impulses[control].state == StatesControl.activ)
                        return Impulses[control];
                }
            }
            return null;
        }

        /// <summary>
        /// устанавливаем активное состояние для различных контролей
        /// </summary>
        /// <param name="index"></param>
        private void SetState(StateElement control)
        {
            if (control != null)
            {
                switch (control.state)
                {
                    case StatesControl.activ:
                        switch (control.Name)
                        {
                            case Viewmode.fencing:
                                Fill = _colorfencing;
                                break;
                            case Viewmode.occupation:
                                Fill = _coloractiv;
                                break;
                            case Viewmode.locking:
                                Fill = _color_loking;
                                break;
                            case Viewmode.lockingM:
                                Fill = _color_lokingM;
                                break;
                            case Viewmode.lockingY:
                                Fill = _color_lokingY;
                                break;
                            //case Viewmode.electrification:
                            //    Stroke = _colorelectric_traction;
                            //    break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _colorpassiv;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = _colornotcontrol;
                                break;;
                        }
                        break;
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


        private void StartFlashing()
        {
            Dispatcher.Invoke(new Action(() => Flashing()));
        }
        /// <summary>
        /// обрабатываем мирцание
        /// </summary>
        private void Flashing()
        {
           
        }

        /// <summary>
        /// определяем занят ли в данный момент путь
        /// </summary>
        /// <returns></returns>
        public string OccupationPath()
        {
            if (Impulses.ContainsKey(Viewmode.occupation))
            {
                switch (Impulses[Viewmode.occupation].state)
                {
                    case StatesControl.activ:
                        return "занят";
                    case StatesControl.pasiv:
                        return "не занят";
                    case StatesControl.nocontrol:
                        return "не контролируется";
                }
            }
            return "не контролируется";
        }
    }
}
