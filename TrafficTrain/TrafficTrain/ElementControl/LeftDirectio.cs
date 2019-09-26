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
    /// <summary>
    /// класс отвечает за левую стрелку поворота
    /// </summary>
    class LeftDirection : Shape, GraficElement, ImpulsTSElement
    {
         #region Переменные и свойства
        //геометрия
        private PathGeometry _figure = new PathGeometry();
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
        ///////
        //////основные свойства перегонной стрелки
        /// <summary>
        /// шестизначный номер станции слева
        /// </summary>
        public int StationNumberLeft { get; set; }
        /// <summary>
        /// шестизначный номер станции справа
        /// </summary>
        public int StationNumberRight { get; set; }
        /// <summary>
        /// шестизначный номер станции импульсов ТС
        /// </summary>
        public int StationNumber { get; set; }
        /// <summary>
        /// название граничного участка
        /// </summary>
        public string NameMove { get; set; }
        ///// <summary>
        ///// событие при изменении направленея левого поворота
        ///// </summary>
        //public static event DicrectionMove LeftEvent;
        /// <summary>
        /// рамка для номеров поездов к которой относиться элемент
        /// </summary>
        NumberTrainRamka trainramka;
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет  при отправлениии
        /// </summary>
        public static Brush _color_departure = Brushes.LightGreen;
        /// <summary>
        /// цвет разрешения отправления
        /// </summary>
        public static Brush _color_ok_departure = Brushes.LightGreen;
        /// <summary>
        /// цвет при ожидания отправления
        /// </summary>
        public static Brush _color_wait_departure = Brushes.Yellow;
        /// <summary>
        /// цвет при рамки по умолчанию
        /// </summary>
        public static Brush _color_ramka = Brushes.Black;
        /// <summary>
        /// цвет при нормальной работе по умолчанию
        /// </summary>
        public static Brush _color_normal = new SolidColorBrush(Color.FromRgb(190, 190, 190));
        /// <summary>
        /// цвет неконтролируемого  пути
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(225, 225, 225));
        /// <summary>
        /// цвет неконтролируемого  пути его рамка
        /// </summary>
        public static Brush _colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        //////
        /// <summary>
        /// коллекция возможных состояний элемента перегонная стрелка со станции слева
        /// </summary>
        public List<StateElement> Impulses { get; set; }
        /// <summary>
        /// толщина контура объектов
        /// </summary>
        double _strokethickness = 1;
        bool _left = false;
        /// <summary>
        /// показывает повернут ли перегон влево
        /// </summary>
        public bool Left
        {
            get
            {
                return _left;
            }
        }
        /// <summary>
        /// постоянно ли развернут перегон влево
        /// </summary>
        public bool ConstLeftRotate { get; set; }
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumberleft">шестизначный номер станции слева</param>
        /// <param name="stationnumberright">шестизначный номер станции справа</param>
        /// <param name="geometry">название пути</param>
        /// <param name="name">граница</param>
        public LeftDirection(int stationnumberleft, int stationnumberright, int station, PathGeometry geometry, string namemove, List<StateElement> impulses, NumberTrainRamka train)
        {
            StationNumberLeft = stationnumberleft;
            StationNumberRight = stationnumberright;
            StationNumber = station;
            NameMove = namemove;
            //if (impulses == null)
            //    Impulses = new List<StateElement>();
            //else
                Impulses = impulses;
            trainramka = train;
            GeometryFigureCopy(geometry);
            //обработка импульсов
            ImpulsesClient.ConnectDisconnectionServer += ConnectCloseServer;
            MainWindow.NewTart += StartFlashing;
            LoadProject.NewColor += NewColor;
        }

        public LeftDirection() { }


        private void NewColor()
        {
            UpdateColor();
        }
        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry)
        {
            //номер фигуры
            int index = 0;
            foreach (PathFigure geo in geometry.Figures)
            {
                _figure = new PathGeometry();
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
                //
                if (index == 0)
                    break;
                //
                index++;
            }
            //
            Fill = _colornotcontrol;
            Stroke = _colornotcontrolstroke;
            //
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
                }
            }
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
                }
            }
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
                        lin.Point = new Point((lin.Point.X - center.X) / scroll, (lin.Point.Y - center.Y) / scroll);
                }
            }
            //
            StrokeThickness = _strokethickness;
        }

        /// <summary>
        /// Реагируем на подключение к серверу импульсов
        /// </summary>
        private void ConnectCloseServer()
        {
            Dispatcher.Invoke(new Action(() => ServerClose()));
        }

        private void ServerClose()
        {
            if (Impulses!=null && Impulses.Count > 0 && !ConstLeftRotate)
            {
                if (!ImpulsesClient.Connect)
                {
                    foreach (StateElement imp in Impulses)
                        imp.state = StatesControl.nocontrol;
                    //
                    Fill = _colornotcontrol;
                    Stroke = _colornotcontrolstroke;
                    //
                    _left = false;
                }
                else
                {
                    Stroke = _color_ramka;
                }
            }
            //если перегон развернут постоянно влево
            if (ConstLeftRotate)
            {
                if (!ImpulsesClient.Connect)
                    _left = false;
                else
                    _left = true;
            }
        }

        private void StartFlashing()
        {
            Dispatcher.Invoke(new Action(() => Flashing()));
        }
        /// <summary>
        /// мигание 
        /// </summary>
        private void Flashing()
        {
            if (Impulses != null && Impulses.Count > 0)
            {
                if (ImpulsesClient.Connect)
                {
                    //смотрю импульсы правой станции
                    foreach (StateElement imp in Impulses)
                    {
                        switch (imp.Name)
                        {
                            case Viewmode.resolution_of_origin:
                                {
                                    if (imp.state == StatesControl.activ && CheckPriorityState(new List<Move.Viewmode>() { Move.Viewmode.departure }))
                                    {
                                        if (MainWindow.Taktupdate)
                                            Fill = LeftDirection._color_ok_departure;
                                        else
                                            Fill = LeftDirection._color_normal;
                                    }
                                    //
                                }
                                break;
                        }
                    }
                }
            }
        }


        public void Analis()
        {

            if (!ConstLeftRotate && Impulses != null && Impulses.Count > 0)
            {
                //проверяем импульсы с левой станции
                foreach (StateElement Imp in Impulses)
                {
                    StatesControl state = Imp.state;
                    InfixNotation inNot;
                    try
                    {
                        inNot = new InfixNotation(Imp.Impuls);
                    }
                    catch { continue; }
                    //
                    switch (SetImpuls.SetValueImpuls(StationNumber, ref inNot))
                    {
                        case InfixNotation.infix_states.ActiveState:
                            Imp.state = StatesControl.activ;
                            break;
                        case InfixNotation.infix_states.PassiveState:
                            Imp.state = StatesControl.pasiv;
                            break;
                        default:
                            Imp.state = StatesControl.nocontrol;
                            break;
                    }
                    if (state != Imp.state)
                        Imp._update = true;

                }
                //обновляем элемент
                UpdateElement();
                //произошло ли изменение направления
                AnalisDirection();
            }
        }

        /// <summary>
        /// проверяем произошло ли изменение направления
        /// </summary>
        private void AnalisDirection()
        {
            bool answer = false;
            foreach (StateElement imp in Impulses)
            {
                if (imp.state == StatesControl.activ)
                {
                    answer = true; break;
                }
            }
            //
            if (_left != answer)
            {
                _left = answer;
                if (trainramka != null)
                    trainramka.LeftDirection(_left);
            }
        }

        /// <summary>
        /// Изменяем цвет элемента
        /// </summary>
        private void UpdateElement()
        {
            bool _update_fill = false;
            //импульсы левой станции
            foreach (StateElement imp in Impulses)
            {
                switch (imp.Name)
                {
                    case Viewmode.departure:
                        if (imp._update)
                        {
                            if (!_update_fill)
                            {
                                switch (imp.state)
                                {
                                    case StatesControl.activ:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.waiting_for_departure });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _color_departure;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                    case StatesControl.pasiv:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.waiting_for_departure });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _color_normal;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                    case StatesControl.nocontrol:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.waiting_for_departure });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _colornotcontrol;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                }
                            }
                            imp._update = false;
                        }
                        break;
                    case Viewmode.resolution_of_origin:
                        if (imp._update)
                        {
                            if (!_update_fill)
                            {
                                switch (imp.state)
                                {
                                    case StatesControl.activ:
                                        _update_fill = true;
                                        break;
                                    case StatesControl.pasiv:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.waiting_for_departure, Viewmode.departure });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _color_normal;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                    case StatesControl.nocontrol:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.waiting_for_departure, Viewmode.departure });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _colornotcontrol;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                }
                            }
                            imp._update = false;
                        }
                        break;
                    case Viewmode.waiting_for_departure:
                        if (imp._update)
                        {
                            if (!_update_fill)
                            {
                                switch (imp.state)
                                {
                                    case StatesControl.activ:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _color_wait_departure;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                    case StatesControl.pasiv:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.departure });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _color_normal;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                    case StatesControl.nocontrol:
                                        {
                                            int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.departure });
                                            if (index != -1)
                                                SetState(index);
                                            else
                                                Fill = _colornotcontrol;
                                            //
                                            _update_fill = true;
                                        }
                                        break;
                                }
                            }
                            imp._update = false;
                        }
                        break;
                }
            }
        }

        private void UpdateColor()
        {
            if (Impulses != null && Impulses.Count > 0)
            {
                if (ImpulsesClient.Connect)
                {
                    bool _update_fill = false;
                    //импульсы левой станции
                    foreach (StateElement imp in Impulses)
                    {
                        switch (imp.Name)
                        {
                            case Viewmode.departure:
                                if (!_update_fill)
                                {
                                    switch (imp.state)
                                    {
                                        case StatesControl.activ:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.waiting_for_departure });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _color_departure;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                        case StatesControl.pasiv:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.waiting_for_departure });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _color_normal;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                        case StatesControl.nocontrol:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.waiting_for_departure });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _colornotcontrol;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Viewmode.resolution_of_origin:
                                if (!_update_fill)
                                {
                                    switch (imp.state)
                                    {
                                        case StatesControl.activ:
                                            _update_fill = true;
                                            break;
                                        case StatesControl.pasiv:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.waiting_for_departure, Viewmode.departure });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _color_normal;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                        case StatesControl.nocontrol:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.waiting_for_departure, Viewmode.departure });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _colornotcontrol;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Viewmode.waiting_for_departure:
                                if (!_update_fill)
                                {
                                    switch (imp.state)
                                    {
                                        case StatesControl.activ:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _color_wait_departure;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                        case StatesControl.pasiv:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.departure });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _color_normal;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                        case StatesControl.nocontrol:
                                            {
                                                int index = FindActiveControl(new List<Viewmode>() { Viewmode.resolution_of_origin, Viewmode.departure });
                                                if (index != -1)
                                                    SetState(index);
                                                else
                                                    Fill = _colornotcontrol;
                                                //
                                                _update_fill = true;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    //
                    if (Stroke != _color_ramka)
                        Stroke = _color_ramka;
                }
                else
                {
                    Fill = _colornotcontrol;
                    Stroke = _colornotcontrolstroke;
                }
            }
            else
            {
                Fill = _colornotcontrol;
                Stroke = _colornotcontrolstroke;
            }
        }

        private int FindActiveControl(List<Move.Viewmode> controlsbottom)
        {
            //
            foreach (Move.Viewmode mode in controlsbottom)
            {
                if (Impulses.Count > 0)
                {
                    for (int i = 0; i < Impulses.Count; i++)
                    {
                        if (Impulses[i].Name == mode && Impulses[i].state == StatesControl.activ)
                            return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// устанавливаем активное состояние для различных контролей
        /// </summary>
        /// <param name="index"></param>
        private void SetState(int index)
        {
            switch (Impulses[index].Name)
            {
                case Viewmode.departure:
                    Fill = _color_departure;
                    break;
                case Viewmode.resolution_of_origin:
                    break;
                case Viewmode.waiting_for_departure:
                    Fill = _color_wait_departure;
                    break;
            }
        }

        /// <summary>
        /// провереям состояние более приоритетных контролей
        /// </summary>
        /// <param name="controls">перечень приоритетных контролей</param>
        /// <returns></returns>
        private bool CheckPriorityState(List<Move.Viewmode> controls)
        {
            foreach (Move.Viewmode mode in controls)
            {
                foreach (StateElement el in Impulses)
                {
                    if (el.Name == mode && el.state == StatesControl.activ)
                        return false;
                }
            }
            //
            return true;
        }
    }
}
