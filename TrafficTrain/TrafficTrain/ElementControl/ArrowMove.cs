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
using LogicalParse;
using Move;

namespace TrafficTrain
{
    //public delegate void DicrectionMove(bool direction, int StationNumberLeft, int StationNumberRight, string graniza);
    ///// <summary>
    ///// класс оисывающий перегонную стрелку
    ///// </summary>
    //class ArrowMove : Shape, ImpulsTSElement, GraficElement
    //{
    //    #region Переменные и свойства
    //    //геометрия
    //    private PathGeometry _figure = new PathGeometry();
    //    /////геометрия элемента
    //    /// <summary>
    //    /// отрисовываемая геометрия
    //    /// </summary>
    //    protected override Geometry DefiningGeometry
    //    {
    //        get
    //        {
    //            return _figure;
    //        }
    //    }
    //    //
    //    private Path _leftarrow = new Path();
    //    /// <summary>
    //    /// геометрия левой стрелки
    //    /// </summary>
    //    public Path LeftArrow
    //    {
    //        get
    //        {
    //            return _leftarrow;
    //        }
    //        set
    //        {
    //            _leftarrow = value;
    //        }
    //    }
    //    //
    //    private Path _rightarrow = new Path();
    //    /// <summary>
    //    /// геометрия правой стрелки
    //    /// </summary>
    //    public Path RightArrow
    //    {
    //        get
    //        {
    //            return _rightarrow;
    //        }
    //        set
    //        {
    //            _rightarrow = value;
    //        }
    //    }
    //    //
    //    private Path _center = new Path();
    //    /// <summary>
    //    /// геометрия правой стрелки
    //    /// </summary>
    //    public Path Center
    //    {
    //        get
    //        {
    //            return _center;
    //        }
    //        set
    //        {
    //            _center = value;
    //        }
    //    }
    //    ///////
    //    //////основные свойства перегонной стрелки
    //    /// <summary>
    //    /// шестизначный номер станции слева
    //    /// </summary>
    //    public int StationNumber { get; set; }
    //    /// <summary>
    //    /// шестизначный номер станции справа
    //    /// </summary>
    //    public int StationNumberRight { get; set; }
    //    /// <summary>
    //    /// название граничного участка
    //    /// </summary>
    //    public string Graniza { get; set; }
    //    /// <summary>
    //    /// событие при изменении занятия перегона
    //    /// </summary>
    //    public static event OccupationMove EventOccupation;
    //    /// <summary>
    //    /// событие при изменении направленея левого поворота
    //    /// </summary>
    //    public static event DicrectionMove LeftDirection;
    //    /// <summary>
    //    /// событие при изменении направленея правого поворота
    //    /// </summary>
    //    public static event DicrectionMove RightDirection;
    //    ////////

    //    //////цветовая палитра
    //    /// <summary>
    //    /// цвет  при отправлениии
    //    /// </summary>
    //    public static Brush _color_departure = Brushes.LightGreen;
    //    /// <summary>
    //    /// цвет при занятом перегоне
    //    /// </summary>
    //    public static Brush _color_occupation = Brushes.Red;
    //    /// <summary>
    //    /// цвет разрешения отправления
    //    /// </summary>
    //    public static Brush _color_ok_departure = Brushes.LightGreen;
    //    /// <summary>
    //    /// цвет при ожидания отправления
    //    /// </summary>
    //    public static Brush _color_wait_departure = Brushes.Yellow;
    //    /// <summary>
    //    /// цвет при рамки по умолчанию
    //    /// </summary>
    //    public static Brush _color_ramka = Brushes.Black;
    //    /// <summary>
    //    /// цвет при нормальной работе по умолчанию
    //    /// </summary>
    //    public static Brush _color_normal = new SolidColorBrush(Color.FromRgb(190, 190, 190));
    //    /// <summary>
    //    /// цвет неконтролируемого  пути
    //    /// </summary>
    //    public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(225, 225, 225));
    //    /// <summary>
    //    /// цвет неконтролируемого  пути его рамка
    //    /// </summary>
    //    public static Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(195, 195, 195));
    //    //////
    //    /// <summary>
    //    /// коллекция возможных состояний элемента перегонная стрелка со станции слева
    //    /// </summary>
    //    public List<StateElement> Impulses { get; set; }
    //    /// <summary>
    //    /// коллекция возможных состояний элемента перегонная стрелка со станции справа
    //    /// </summary>
    //    public List<StateElement> ImpulsesRight { get; set; }
    //    /// <summary>
    //    /// толщина контура объектов
    //    /// </summary>
    //    double strokethickness = 1;
    //    /// <summary>
    //    /// фон для рисования
    //    /// </summary>
    //    Canvas _drawcanvas = new Canvas();
    //    //занят ли перегон с левой станции
    //    StatesControl activ_busy_left = StatesControl.nocontrol;
    //    //занят ли перегон с правой станции
    //    StatesControl activ_busy_right = StatesControl.nocontrol;

    //    bool _left = false;
    //    bool _right = false;
    //    /// <summary>
    //    /// показывает повернут ли перегон влево
    //    /// </summary>
    //    public bool Left
    //    {
    //        get
    //        {
    //            return _left;
    //        }
    //    }
    //    /// <summary>
    //    /// показывает повернут ли перегон вправо
    //    /// </summary>
    //    public bool Right
    //    {
    //        get
    //        {
    //            return _right;
    //        }
    //    }
    //    #endregion

    //    /// <summary>
    //    /// Конструктор
    //    /// </summary>
    //    /// <param name="stationnumberleft">шестизначный номер станции слева</param>
    //    /// <param name="stationnumberright">шестизначный номер станции справа</param>
    //    /// <param name="geometry">название пути</param>
    //    /// <param name="name">граница</param>
    //    public ArrowMove(int stationnumberleft,int stationnumberright , PathGeometry geometry, string namemove)
    //    {
    //        StationNumber = stationnumberleft;
    //        StationNumberRight = stationnumberright;
    //        Graniza = namemove;
    //        Impulses = new List<StateElement>();
    //        ImpulsesRight = new List<StateElement>();
    //        GeometryFigureCopy(geometry);
    //        //обработка импульсов
    //        ImpulsesClient.NewData += NewInfomation;
    //        ImpulsesClient.ConnectDisconnectionServer += ConnectCloseServer;
    //        MainWindow.NewTart += StartFlashing;
    //        LoadProject.NewColor += NewColor;
    //    }

    //    private void NewColor()
    //    {

    //    }

    //    private void StartFlashing()
    //    {
    //        _drawcanvas.Dispatcher.Invoke(new Action(() => Flashing()));
    //    }
    //    /// <summary>
    //    /// мигание светофора
    //    /// </summary>
    //    private void Flashing()
    //    {
    //        if (ImpulsesClient.Connect )
    //        {
    //            //смотрю импульсы правой станции
    //            foreach (StateElement imp in Impulses)
    //            {
    //                switch (imp.Name)
    //                {
    //                    case Viewmode.resolution_of_origin:
    //                        {
    //                            if (imp.state == StatesControl.activ)
    //                            {
    //                                if (MainWindow.Taktupdate)
    //                                    RightArrow.Fill = _color_ok_departure;
    //                                else
    //                                    RightArrow.Fill = _color_normal;
    //                            }
    //                            //
    //                        }
    //                        break;
    //                }
    //            }
    //            //смотрю импульсы левой станции
    //            foreach (StateElement imp in ImpulsesRight)
    //            {
    //                switch (imp.Name)
    //                {
    //                    case Viewmode.resolution_of_origin:
    //                        {
    //                            if (imp.state == StatesControl.activ)
    //                            {
    //                                if (MainWindow.Taktupdate)
    //                                    LeftArrow.Fill = _color_ok_departure;
    //                                else
    //                                    LeftArrow.Fill = _color_normal;
    //                            }
    //                            //
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// формируем геометрию объкта
    //    /// </summary>
    //    /// <param name="geometry"></param>
    //    private void GeometryFigureCopy(PathGeometry geometry)
    //    {
    //        //номер фигуры
    //        int index = 0;
    //        foreach (PathFigure geo in geometry.Figures)
    //        {
    //            _figure = new PathGeometry();
    //            PathFigure newfigure = new PathFigure() { IsClosed = true };
    //            newfigure.StartPoint = new Point(geo.StartPoint.X, geo.StartPoint.Y);
    //            foreach (PathSegment seg in geo.Segments)
    //            {
    //                //сегмент линия
    //                LineSegment lin = seg as LineSegment;
    //                if (lin != null)
    //                {
    //                    newfigure.Segments.Add(new LineSegment() { Point = new Point(lin.Point.X, lin.Point.Y) });
    //                    continue;
    //                }
    //            }
    //            _figure.Figures.Add(newfigure);
    //            //
    //            switch (index)
    //            {
    //                case 0:
    //                    LeftArrow.Data = _figure;
    //                    break;
    //                case 1:
    //                    Center.Data = _figure;
    //                    break;
    //                case 2:
    //                    RightArrow.Data = _figure;
    //                    break;
    //            }
    //           //
    //            index++;
    //        }
    //        //
    //        if (Impulses.Count == 0 && ImpulsesRight.Count == 0)
    //        {
    //            //левая стрелка
    //            LeftArrow.Stroke = _colornotcontrolstroke;
    //            LeftArrow.Fill = _colornotcontrol;
    //            //середина
    //            Center.Stroke = _colornotcontrolstroke;
    //            Center.Fill = _colornotcontrol;
    //            //правая стрелка
    //            RightArrow.Stroke = _colornotcontrolstroke;
    //            RightArrow.Fill = _colornotcontrol;
    //        }
    //        //устанавливаем толщину объектов
    //        LeftArrow.StrokeThickness = strokethickness;
    //        Center.StrokeThickness = strokethickness;
    //        RightArrow.StrokeThickness = strokethickness;
    //    }
    //    /// <summary>
    //    /// масштабироание объекта
    //    /// </summary>
    //    /// <param name="scale">масштаб</param>
    //    public void ScrollFigure(ScaleTransform scaletransform, double scale)
    //    {
    //        ////перемещаем левую часть стрелки
    //        //PathGeometry pathleftarrow = (PathGeometry)_leftarrow.Data;
    //        //ScrollGeometry(scaletransform, pathleftarrow);
    //        //_leftarrow.StrokeThickness *= scale;
    //        ////перемещаем центральную часть
    //        //PathGeometry pathcenter = (PathGeometry)_center.Data;
    //        //ScrollGeometry(scaletransform, pathcenter);
    //        //_center.StrokeThickness *= scale;
    //        ////перемещаем правую часть стрелки
    //        //PathGeometry pathrightarrow = (PathGeometry)_rightarrow.Data;
    //        //ScrollGeometry(scaletransform, pathrightarrow);
    //        //_rightarrow.StrokeThickness *= scale;
    //    }

    //     /// <summary>
    //    /// перемещение объекта
    //    /// </summary>
    //    /// <param name="deltaX">изменение по оси X</param>
    //    /// <param name="deltaY">изменение по оси Y</param>
    //    public void SizeFigure(double deltaX, double deltaY)
    //    {
    //        ////перемещаем левую часть стрелки
    //        //PathGeometry pathleftarrow = (PathGeometry)_leftarrow.Data;
    //        //SizeGeometry(pathleftarrow, deltaX, deltaY);
    //        ////перемещаем центральную часть
    //        //PathGeometry pathcenter = (PathGeometry)_center.Data;
    //        //SizeGeometry(pathcenter, deltaX, deltaY);
    //        ////перемещаем правую часть стрелки
    //        //PathGeometry pathrightarrow = (PathGeometry)_rightarrow.Data;
    //        //SizeGeometry(pathrightarrow, deltaX, deltaY);
    //    }

    //    private void ScrollGeometry(ScaleTransform scaletransform, PathGeometry element)
    //    {
    //        foreach (PathFigure geo in element.Figures)
    //        {
    //            geo.StartPoint = scaletransform.Transform(geo.StartPoint);
    //            foreach (PathSegment seg in geo.Segments)
    //            {
    //                //сегмент линия
    //                LineSegment lin = seg as LineSegment;
    //                if (lin != null)
    //                    lin.Point = scaletransform.Transform(lin.Point);
    //            }
    //        }
    //    }

    //    private void SizeGeometry(PathGeometry element, double deltaX, double deltaY)
    //    {
    //        foreach (PathFigure geo in element.Figures)
    //        {
    //            geo.StartPoint = new Point(geo.StartPoint.X + deltaX, geo.StartPoint.Y + deltaY);
    //            foreach (PathSegment seg in geo.Segments)
    //            {
    //                //сегмент линия
    //                LineSegment lin = seg as LineSegment;
    //                if (lin != null)
    //                    lin.Point = new Point(lin.Point.X + deltaX, lin.Point.Y + deltaY);
    //            }
    //        }
    //    }

    //    public void StartPosition(Point center, double scroll)
    //    {
    //        //левая стрелка
    //        foreach (PathFigure geo in ((PathGeometry)LeftArrow.Data).Figures)
    //        {
    //            geo.StartPoint = new Point((geo.StartPoint.X - center.X) / scroll, (geo.StartPoint.Y - center.Y) / scroll);

    //            foreach (PathSegment seg in geo.Segments)
    //            {
    //                //сегмент линия
    //                LineSegment lin = seg as LineSegment;
    //                if (lin != null)
    //                    lin.Point = new Point((lin.Point.X - center.X) / scroll, (lin.Point.Y - center.Y) / scroll);
    //            }
    //        }
    //        //центральная часть
    //        foreach (PathFigure geo in ((PathGeometry)Center.Data).Figures)
    //        {
    //            geo.StartPoint = new Point((geo.StartPoint.X - center.X) / scroll, (geo.StartPoint.Y - center.Y) / scroll);

    //            foreach (PathSegment seg in geo.Segments)
    //            {
    //                //сегмент линия
    //                LineSegment lin = seg as LineSegment;
    //                if (lin != null)
    //                    lin.Point = new Point((lin.Point.X - center.X) / scroll, (lin.Point.Y - center.Y) / scroll);
    //            }
    //        }
    //        //правая стрелка
    //        foreach (PathFigure geo in ((PathGeometry)RightArrow.Data).Figures)
    //        {
    //            geo.StartPoint = new Point((geo.StartPoint.X - center.X) / scroll, (geo.StartPoint.Y - center.Y) / scroll);

    //            foreach (PathSegment seg in geo.Segments)
    //            {
    //                //сегмент линия
    //                LineSegment lin = seg as LineSegment;
    //                if (lin != null)
    //                    lin.Point = new Point((lin.Point.X - center.X) / scroll, (lin.Point.Y - center.Y) / scroll);
    //            }
    //        }
    //        //
    //        LeftArrow.StrokeThickness = strokethickness;
    //        Center.StrokeThickness = strokethickness;
    //        RightArrow.StrokeThickness = strokethickness;
    //    }

    //    /// <summary>
    //    /// Получаем новые данные по импульсам
    //    /// </summary>
    //    private void NewInfomation()
    //    {
    //        _drawcanvas.Dispatcher.Invoke(new Action(() => ProcessingData()));
    //    }

    //    /// <summary>
    //    /// Обработываем новые данные
    //    /// </summary>
    //    private void ProcessingData()
    //    {
    //        Analis();
    //    }

    //    /// <summary>
    //    /// Реагируем на подключение к серверу импульсов
    //    /// </summary>
    //    private void ConnectCloseServer()
    //    {
    //        _drawcanvas.Dispatcher.Invoke(new Action(() => ServerClose()));
    //    }

    //    private void ServerClose()
    //    {

    //        if (Impulses.Count > 0 && ImpulsesRight.Count > 0)
    //        {
    //            if (!ImpulsesClient.Connect)
    //            {
    //                foreach (StateElement imp in Impulses)
    //                    imp.state = StatesControl.nocontrol;
    //                //
    //                foreach (StateElement imp in ImpulsesRight)
    //                    imp.state = StatesControl.nocontrol;
    //                //
    //                activ_busy_left = StatesControl.nocontrol;
    //                activ_busy_right = StatesControl.nocontrol;
    //                //
    //                LeftArrow.Fill = _colornotcontrol;
    //                Center.Fill = _colornotcontrol;
    //                RightArrow.Fill = _colornotcontrol;
    //                //
    //                LeftArrow.Stroke = _colornotcontrolstroke;
    //                Center.Stroke = _colornotcontrolstroke;
    //                RightArrow.Stroke = _colornotcontrolstroke;
    //                //
    //                _left = false;
    //                _right = false;
    //            }
    //            else
    //            {
    //                LeftArrow.Stroke = _color_ramka;
    //                Center.Stroke = _color_ramka;
    //                RightArrow.Stroke= _color_ramka;
    //            }
    //        }
    //    }

    //    //public void Analis()
    //    //{
    //    //    //проверяем импульсы с левой станции
    //    //    foreach (StateElement Imp in Impulses)
    //    //    {
    //    //        StatesControl state = Imp.state;
    //    //        InfixNotation inNot;
    //    //        try
    //    //        {
    //    //            inNot = new InfixNotation(Imp.Impuls);
    //    //        }
    //    //        catch { continue; }
    //    //        //
    //    //        switch (SetImpuls.SetValueImpuls(StationNumber, ref inNot))
    //    //        {
    //    //            case InfixNotation.infix_states.ActiveState:
    //    //                Imp.state = StatesControl.activ;
    //    //                break;
    //    //            case InfixNotation.infix_states.PassiveState:
    //    //                Imp.state = StatesControl.pasiv;
    //    //                break;
    //    //            default:
    //    //                Imp.state = StatesControl.nocontrol;
    //    //                break;
    //    //        }
    //    //        if (state != Imp.state)
    //    //            Imp._update = true;

    //    //    }
    //    //    //проверяем импульсы с правой станции
    //    //    foreach (StateElement Imp in ImpulsesRight)
    //    //    {
    //    //        StatesControl state = Imp.state;
    //    //        InfixNotation inNot;
    //    //        try
    //    //        {
    //    //            inNot = new InfixNotation(Imp.Impuls);
    //    //        }
    //    //        catch { continue; }
    //    //        //
    //    //        switch (SetImpuls.SetValueImpuls(StationNumberRight, ref inNot))
    //    //        {
    //    //            case InfixNotation.infix_states.ActiveState:
    //    //                Imp.state = StatesControl.activ;
    //    //                break;
    //    //            case InfixNotation.infix_states.PassiveState:
    //    //                Imp.state = StatesControl.pasiv;
    //    //                break;
    //    //            default:
    //    //                Imp.state = StatesControl.nocontrol;
    //    //                break;
    //    //        }
    //    //        if (state != Imp.state)
    //    //            Imp._update = true;

    //    //    }
    //    //    //обновляем элемент
    //    //    UpdateElement();
    //    //    //
    //    //}

    //    public void Analis()
    //    {

    //    }

    //    /// <summary>
    //    /// изменяем отображение элемента
    //    /// </summary>
    //    private void UpdateElement()
    //    {

    //    }

    //    /// <summary>
    //    /// Изменяем цвет элемента
    //    /// </summary>
    //    //private void UpdateElement()
    //    //{
    //    //    //импульсы левой станции
    //    //    foreach (StateElement imp in Impulses)
    //    //    {
    //    //        switch (imp.Name)
    //    //        {
    //    //            case Viewmode.departure:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.activ:
    //    //                            RightArrow.Fill = _color_departure;
    //    //                            //
    //    //                            if (RightDirection != null)
    //    //                                RightDirection(true, StationNumber, StationNumberRight, Graniza);
    //    //                            break;
    //    //                        case StatesControl.pasiv:
    //    //                            //
    //    //                            if (RightDirection != null)
    //    //                                RightDirection(false, StationNumber, StationNumberRight, Graniza);
    //    //                            //
    //    //                            bool answer = false;
    //    //                            foreach (StateElement impul in Impulses)
    //    //                            {
    //    //                                if (impul.Name == Viewmode.waiting_for_departure && impul.state == StatesControl.activ)
    //    //                                {
    //    //                                    RightArrow.Fill = _color_wait_departure;
    //    //                                    answer = true;
    //    //                                    break;
    //    //                                }  
    //    //                            }
    //    //                            //
    //    //                            if (!answer)
    //    //                                RightArrow.Fill = _color_normal;
    //    //                            break;
    //    //                        case StatesControl.nocontrol:
    //    //                            if (RightDirection != null)
    //    //                                RightDirection(false, StationNumber, StationNumberRight, Graniza);
    //    //                            //
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //            case Viewmode.occupation:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.activ:
    //    //                            activ_busy_left = StatesControl.activ;
    //    //                            break;
    //    //                        case StatesControl.pasiv:
    //    //                            activ_busy_left = StatesControl.pasiv;
    //    //                            break;
    //    //                        case StatesControl.nocontrol:
    //    //                             activ_busy_left = StatesControl.nocontrol;;
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //            case Viewmode.waiting_for_departure:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.activ:
    //    //                            RightArrow.Fill = _color_wait_departure;
    //    //                            break;
    //    //                        case StatesControl.pasiv:
    //    //                            bool answer = false;
    //    //                            foreach (StateElement impul in Impulses)
    //    //                            {
    //    //                                if (impul.Name == Viewmode.departure && impul.state == StatesControl.activ)
    //    //                                {
    //    //                                    RightArrow.Fill = _color_departure;
    //    //                                    answer = true;
    //    //                                    break;
    //    //                                } 
    //    //                            }
    //    //                            //
    //    //                            if (!answer)
    //    //                                RightArrow.Fill = _color_normal;
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //            case Viewmode.resolution_of_origin:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.pasiv:
    //    //                            GetControlLeft();
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //        }
    //    //    }
    //    //    //импульсы правой станции
    //    //    foreach (StateElement imp in ImpulsesRight)
    //    //    {
    //    //        switch (imp.Name)
    //    //        {
    //    //            case Viewmode.departure:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.activ:
    //    //                            LeftArrow.Fill = _color_departure;
    //    //                            //
    //    //                            if (LeftDirection != null)
    //    //                                LeftDirection(true, StationNumber, StationNumberRight, Graniza);
    //    //                            break;
    //    //                        case StatesControl.pasiv:
    //    //                            bool answer = false;
    //    //                            //
    //    //                            if (LeftDirection != null)
    //    //                                LeftDirection(false, StationNumber, StationNumberRight, Graniza);
    //    //                            //
    //    //                            foreach (StateElement impul in ImpulsesRight)
    //    //                            {
    //    //                                if (impul.Name == Viewmode.waiting_for_departure && impul.state == StatesControl.activ)
    //    //                                {
    //    //                                    LeftArrow.Fill = _color_wait_departure;
    //    //                                    answer = true;
    //    //                                    break;
    //    //                                }
    //    //                            }
    //    //                            //
    //    //                            if (!answer)
    //    //                                LeftArrow.Fill = _color_normal;
    //    //                            break;
    //    //                        case StatesControl.nocontrol:
    //    //                             if (LeftDirection != null)
    //    //                                LeftDirection(false, StationNumber, StationNumberRight, Graniza);
    //    //                            //
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //            case Viewmode.occupation:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.activ:
    //    //                            activ_busy_right = StatesControl.activ;
    //    //                            break;
    //    //                        case StatesControl.pasiv:
    //    //                            activ_busy_right = StatesControl.pasiv;
    //    //                            break;
    //    //                        case StatesControl.nocontrol:
    //    //                            activ_busy_right = StatesControl.nocontrol;
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //            case Viewmode.waiting_for_departure:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.activ:
    //    //                            LeftArrow.Fill = _color_wait_departure;
    //    //                            break;
    //    //                        case StatesControl.pasiv:
    //    //                            bool answer = false;
    //    //                            foreach (StateElement impul in ImpulsesRight)
    //    //                            {
    //    //                                if (impul.Name == Viewmode.departure && impul.state == StatesControl.activ)
    //    //                                {
    //    //                                    LeftArrow.Fill = _color_departure;
    //    //                                    answer = true;
    //    //                                    break;
    //    //                                }
    //    //                            }
    //    //                            //
    //    //                            if (!answer)
    //    //                                LeftArrow.Fill = _color_normal;
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //            case Viewmode.resolution_of_origin:
    //    //                if (imp._update)
    //    //                {
    //    //                    switch (imp.state)
    //    //                    {
    //    //                        case StatesControl.pasiv:
    //    //                            GetControlRight();
    //    //                            break;
    //    //                    }
    //    //                    imp._update = false;
    //    //                }
    //    //                break;
    //    //        }
    //    //    }
    //    //    //занятие перегона
    //    //    if (Impulses.Count != 0 || ImpulsesRight.Count != 0)
    //    //    {
    //    //        //проверяем занятость перегона
    //    //        if (activ_busy_left == StatesControl.activ || activ_busy_right == StatesControl.activ && Center.Fill != _color_occupation)
    //    //        {
    //    //            Center.Fill = _color_occupation;
    //    //            if (EventOccupation != null)
    //    //                EventOccupation(StationNumber, StationNumberRight, Graniza, StatesControl.activ);
    //    //        }
    //    //        //
    //    //        if (activ_busy_left == StatesControl.pasiv && activ_busy_right == StatesControl.pasiv && Center.Fill != _color_normal)
    //    //        {
    //    //            Center.Fill = _color_normal;
    //    //            if (EventOccupation != null)
    //    //                EventOccupation(StationNumber, StationNumberRight, Graniza, StatesControl.pasiv);
    //    //        }
    //    //        //
    //    //        if (activ_busy_left == StatesControl.nocontrol && activ_busy_right == StatesControl.nocontrol && Center.Fill != _colornotcontrol)
    //    //        {
    //    //            Center.Fill = _colornotcontrol;
    //    //            if (EventOccupation != null)
    //    //                EventOccupation(StationNumber, StationNumberRight, Graniza, StatesControl.nocontrol);
    //    //        }
    //    //    }
    //    //} 

    //    ///// <summary>
    //    ///// контроль с левой станции
    //    ///// </summary>
    //    //private void GetControlLeft()
    //    //{
    //    //    bool activdeparture = false;
    //    //    bool activwaiting_for_departure = false;
    //    //    foreach (StateElement imp in Impulses)
    //    //    {
    //    //        switch (imp.Name)
    //    //        {
    //    //            case Viewmode.departure:
    //    //                switch (imp.state)
    //    //                {
    //    //                    case StatesControl.activ:
    //    //                        RightArrow.Fill = _color_departure ;
    //    //                        activdeparture = true;
    //    //                        break;
    //    //                }
    //    //                break;
    //    //            case Viewmode.waiting_for_departure:
    //    //                switch (imp.state)
    //    //                {
    //    //                    case StatesControl.activ:
    //    //                        RightArrow.Fill = _color_wait_departure;
    //    //                        activwaiting_for_departure = true;
    //    //                        break;
    //    //                }
    //    //                break;
    //    //        }

    //    //    }
    //    //    //
    //    //    if (!activwaiting_for_departure && !activdeparture)
    //    //        RightArrow.Fill = _color_normal;
    //    //}
    //    ///// <summary>
    //    ///// контроль с правой станции
    //    ///// </summary>
    //    //private void GetControlRight()
    //    //{
    //    //    bool activdeparture = false;
    //    //    bool activwaiting_for_departure = false;
    //    //    foreach (StateElement imp in ImpulsesRight)
    //    //    {
    //    //        switch (imp.Name)
    //    //        {
    //    //            case Viewmode.departure:
    //    //                switch (imp.state)
    //    //                {
    //    //                    case StatesControl.activ:
    //    //                        LeftArrow.Fill = _color_departure;
    //    //                        activdeparture = true;
    //    //                        break;
    //    //                }
    //    //                break;
    //    //            case Viewmode.waiting_for_departure:
    //    //                switch (imp.state)
    //    //                {
    //    //                    case StatesControl.activ:
    //    //                        LeftArrow.Fill = _color_wait_departure;
    //    //                        activwaiting_for_departure = true;
    //    //                        break;
    //    //                }
    //    //                break;
    //    //        }
    //    //    }
    //    //    //
    //    //    if(!activwaiting_for_departure && !activdeparture)
    //    //        LeftArrow.Fill = _color_normal;
    //    //}
    //}
}
