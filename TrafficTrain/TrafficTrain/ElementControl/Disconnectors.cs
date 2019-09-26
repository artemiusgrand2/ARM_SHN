using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using TrafficTrain.Impulsesver.Client;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;


namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий разъединители
    /// </summary>
    class Disconnectors : Shape, IGraficElement, ISelectElement, IInfoElement, IDisposable, IIndicationEl
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
        //////основные свойства переезда
        /// <summary>
        /// шестизначный номер станции к которой принадлежит путь
        /// </summary>
        public int StationControl { get; set; }
        private int m_stationRight = -1;
        /// <summary>
        /// шестизначный номер станции справа
        /// </summary>
        public int StationTransition
        {
            get
            {
                return m_stationRight;
            }
            set
            {
                m_stationRight = value;
            }
        }
        /// <summary>
        /// название разъеденителя
        /// </summary>
        public string NameDisconnector { get; set; }
        ////////

        //////цветовая палитра
        /// <summary>
        /// цвет  занятия 
        /// </summary>
        public static Brush m_colorOccupation = Brushes.Red;
        /// <summary>
        /// цвет  замыкания
        /// </summary>
        public static Brush m_colorLocking = Brushes.Green;
        /// <summary>
        /// цвет неконтролируемого  элемента
        /// </summary>
        public static Brush m_colornotcontrol = Brushes.White;
        /// <summary>
        /// цвет неконтролируемого элемента его рамка
        /// </summary>
        public static Brush m_colornotcontrolstroke = Brushes.Black;
        /// <summary>
        /// цвет фона по умолчанию
        /// </summary>
        public static Brush m_colorDefult = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет рамки по умолчанию
        /// </summary>
        public static Brush m_colorStrokeDefult = Brushes.Black;
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement> Impulses { get; set; }
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1.3 * SystemParameters.CaretWidth;
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
        /// Тип разъеденителя
        /// </summary>
        //TypeDisconnectors Type{get;set;}
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
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_fill = new List<Viewmode>() { Viewmode.occupation, Viewmode.locking };
        /// <summary>
        /// приоритет отображения рамки
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>();
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
                return NameDisconnector;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public Disconnectors(PathGeometry geometry, string name, Dictionary<Viewmode, StateElement> impulses, TypeDisconnectors type)
        {
            NameDisconnector = name;
            //Type = type;
            Impulses = AnalisCollectionStateControl(impulses);
            GeometryFigureCopy(geometry);
            //обработка импульсов
            if (!string.IsNullOrEmpty(NameDisconnector) && Impulses.Count > 0)
            {
                Analis();
            }
            //LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(NameDisconnector) && Impulses.Count > 0)
            {
                //LoadColorControl.NewColor -= NewColor;
            }
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
            return Notes;
        }

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
            foreach (PathFigure geo in geometry.Figures)
            {
                PathFigure newfigure = new PathFigure() { IsClosed = true };
                newfigure.StartPoint = new Point(geo.StartPoint.X, geo.StartPoint.Y);
                foreach (PathSegment seg in geo.Segments)
                {
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
            {
                Stroke = m_colornotcontrolstroke;
                Fill = m_colornotcontrol;
            }
            //
            _strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = _strokethickness;
        }

        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (!ImpulsesClient.Connect)
                {
                    foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                    {
                        imp.Value.state = StatesControl.nocontrol;
                    }
                    //
                    Stroke = m_colornotcontrolstroke;
                    Fill = m_colornotcontrol;
                }
                else
                {
                    Stroke = m_colorStrokeDefult;
                }
            }
        }

        public IList<string> Analis()
        {
            bool update = false;
            var result = new List<string>();
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                StatesControl state = Imp.Value.state;
                Imp.Value.state = GetImpuls.GetStateControl(StationControl, Imp.Value.Impuls);
                //
                if (state != Imp.Value.state)
                {
                    Imp.Value.Update = true;
                    update = true;
                    result.AddRange(Diagnostic.DiagnosticControl(Imp.Value));
                }
            }
            //обновляем элемент
            if (update)
                UpdateElement(true);
            //
            return result;
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
                        Stroke = m_colornotcontrolstroke;
                    if (!_update_fill)
                        Fill = m_colornotcontrol;
                }
            }
            else
            {
                if (!CheckUpdate)
                {
                    Stroke = m_colornotcontrolstroke;
                    Fill = m_colornotcontrol;
                }
            }
        }

        private void UpdateColor()
        {
            UpdateElement(false);
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
                            case Viewmode.occupation:
                                Fill = m_colorOccupation;
                                break;
                            case Viewmode.locking:
                                Fill = m_colorLocking;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = m_colorDefult;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.fill:
                                Fill = m_colornotcontrol;
                                break; 
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
            for (int i = 0; i < _points.Count; i++)
            {
                if (i < _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[i + 1].X, Y2 = _points[i + 1].Y });
                else if (i == _points.Count - 1)
                    _lines.Add(new Line() { X1 = _points[i].X, Y1 = _points[i].Y, X2 = _points[0].X, Y2 = _points[0].Y });
            }
        }
    }
}
