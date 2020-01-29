using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.ImpulsClient;

namespace TrafficTrain
{
    /// <summary>
    /// класс описывающий вспомагательную линию
    /// </summary>
    class LineHelp : Shape, IGraficElement, IInfoElement, ISelectElement, IDisposable, IIndicationEl
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
                return m_figure;
            }
        }
        private PathGeometry m_figure = new PathGeometry();
        /// <summary>
        /// геометрическое иписание фигуры
        /// </summary>
        public PathGeometry Figure
        {
            get
            {
                return m_figure;
            }
            set
            {
                m_figure = value;
            }
        }
      
        /// <summary>
        /// толщина контура объкта по умолчанию
        /// </summary>
        double m_strokethickness = 1 * SystemParameters.CaretWidth;
        /// <summary>
        /// Имя цвета
        /// </summary>
        public string NameColor { get; set; }
        //////основные свойства 
        /// <summary>
        /// шестизначный номер станции к которой принадлежит активная линия
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
        /// название активной линии
        /// </summary>
        public string NameLine { get; set; }
        /// <summary>
        /// коллекция возможных состояний элемента станционный путь
        /// </summary>
        public Dictionary<Viewmode, StateElement> Impulses { get; set; }

        //////цветовая палитра
        /// <summary>
        /// цвет линии по умолчанию, если она не активна
        /// </summary>
        Brush _colordefultlinehelp = Brushes.SaddleBrown;
        /// <summary>
        /// цвет замыкания
        /// </summary>
        public static Brush _colorlocking = Brushes.LightGreen;
        /// <summary>
        /// цвет замыкания маневровый
        /// </summary>
        public static Brush _colorlockingM = Brushes.White;
        /// <summary>
        /// цвет замыкания аварийный
        /// </summary>
        public static Brush _colorlockingY = Brushes.Yellow;
        /// <summary>
        /// цвет не занятого элемента
        /// </summary>
        public static Brush _color_pasive = new SolidColorBrush(Color.FromRgb(100, 100, 100));
        /// <summary>
        /// цвет  занятия
        /// </summary>
        public static Brush _color_active = Brushes.Red;
        /// <summary>
        /// цвет  ограждения
        /// </summary>
        public static Brush _colorfencing = Brushes.SaddleBrown;
        /// <summary>
        /// цвет неконтролируемого элемента
        /// </summary>
        public static Brush _colornotcontrol = new SolidColorBrush(Color.FromRgb(205, 205, 205));
        /// <summary>
        /// цвет проезда
        /// </summary>
        public static Brush _color_passage = Brushes.Black;
        /// <summary>
        /// цвет искуственной разделки первый
        /// </summary>
        public static Brush _color_cutting_one =  Brushes.LightGreen;
        /// <summary>
        /// цвет искуственной разделки второй
        /// </summary>
        public static Brush _color_cutting_ty = Brushes.Black;
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
        /// приоритет отображения фона
        /// </summary>
        List<Viewmode> _priority_stroke = new List<Viewmode>() { Viewmode.occupation, Viewmode.cutting, Viewmode.locking, Viewmode.lockingM, Viewmode.lockingY, Viewmode.passage };
        /// <summary>
        /// пояснения
        /// </summary>
        public string Notes { get; set; }

        private IDictionary<StatesControl, string> m_messages = new Dictionary<StatesControl, string>();
        /// <summary>
        /// Индекс слоя
        /// </summary>
        public int ZIntex { get; set; }

        private bool m_visible = true;

        public bool IsVisible
        {
            get
            {
                return m_visible;
            }
        }

        public string NameUl
        {
            get
            {
                return NameLine;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public LineHelp(PathGeometry geometry, double Weight, string namecolor, string name, Dictionary<Viewmode, StateElement> impulses, bool isVisible)
        {
            if (namecolor == null)
                NameColor = string.Empty;
            else NameColor = namecolor;
            NameLine = name;
            Impulses = AnalisCollectionStateControl(impulses);
            m_visible = isVisible;
            NewColor();
            if (Impulses.Count > 0)
            {
                //обработка информации по импульсам и номерам поездов
                Connections.NewTart += StartFlashing;
            }
            //
            GeometryFigureCopy(geometry, Weight);
            LoadColorControl.NewColor += NewColor;
        }

        public void Dispose()
        {
            if (Impulses.Count > 0)
            {
                Connections.NewTart -= StartFlashing;
            }
            //
            LoadColorControl.NewColor -= NewColor;
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

        private void StartFlashing()
        {
            Dispatcher.Invoke(new Action(() => Flashing()));
        }
        /// <summary>
        /// обрабатываем мирцание
        /// </summary>
        private void Flashing()
        {
            //проверять ли рамкой
            StateElement control_stroke = CheckPriorityState(_priority_stroke);
            if (control_stroke != null)
                FlashingControl(SetColorsFlashing(control_stroke.Name), control_stroke);
        }

        private void FlashingControl(List<Brush> brushes, StateElement control)
        {
            if (brushes.Count == 2)
            {
                if (Connections.Taktupdate)
                {
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.stroke:
                            Stroke = brushes[0];
                            break;
                    }
                }
                else
                    switch (control.ViewControlDraw)
                    {
                        case ViewElementDraw.stroke:
                            Stroke = brushes[1];
                            break;
                    }
            }
        }

        private List<Brush> SetColorsFlashing(Viewmode mode)
        {
            switch (mode)
            {
                case Viewmode.cutting:
                    return new List<Brush> { _color_cutting_one, _color_pasive };
            }
            //
            return new List<Brush>();
        }

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

        public string InfoElement()
        {
            return string.Format("{0}", Notes);
        }


        public IList<string> Analis()
        {
            if(NameLine == "СТП.Achtung")
            {

            }
            bool update = false;
            var result = new List<string> ();
            foreach (KeyValuePair<Viewmode, StateElement> Imp in Impulses)
            {
                StatesControl state = Imp.Value.state;
                Imp.Value.state = Connections.ClientImpulses.Data.GetStateControl(StationControl, Imp.Value.Impuls);
                //
                if (state != Imp.Value.state)
                {
                    Imp.Value.Update = true;
                    update = true;
                    if (Imp.Key == Viewmode.passage)
                    {
                        if (Imp.Value.state == StatesControl.activ)
                            Canvas.SetZIndex(this, int.MaxValue);
                        else
                            Canvas.SetZIndex(this, int.MinValue);
                    }
                    //
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
                bool _update_stroke = false;
                //
                foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                {
                    if ((CheckUpdate && imp.Value.Update) || !CheckUpdate)
                    {
                        switch (imp.Value.ViewControlDraw)
                        {
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
                        Stroke = _colornotcontrol;
                }
            }
            else
            {
                if (!CheckUpdate)
                    Stroke = _colornotcontrol;
            }
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
                            case Viewmode.locking:
                                Stroke = _colorlocking;
                                break;
                            case Viewmode.lockingM:
                                Stroke = _colorlockingM;
                                break;
                            case Viewmode.lockingY:
                                Stroke = _colorlockingY;
                                break;
                            case Viewmode.occupation:
                                Stroke = _color_active;
                                break;
                            case Viewmode.passage:
                                Stroke = _color_pasive;
                                break;
                        }
                        break;
                    case StatesControl.pasiv:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.stroke:
                                if (Impulses.ContainsKey(Viewmode.passage))
                                    Stroke = _color_passage;
                                else Stroke = _color_pasive;
                                break;
                        }
                        break;
                    case StatesControl.nocontrol:
                        switch (control.ViewControlDraw)
                        {
                            case ViewElementDraw.stroke:
                                Stroke = _colornotcontrol;
                                break;
                        }
                        break;
                }
            }
        }

        public void ServerClose()
        {
            if (Impulses.Count > 0)
            {
                if (!ImpulsesClientTCP.Connect)
                {
                    foreach (KeyValuePair<Viewmode, StateElement> imp in Impulses)
                        imp.Value.state = StatesControl.nocontrol;
                    Stroke = _colornotcontrol;
                }
            }
        }

        private void NewColor()
        {
            if (Impulses.Count > 0)
                UpdateElement(false);
            else
            {
                if (IsVisible)
                    Stroke = _colordefultlinehelp;
                else
                    Stroke = _color_active;
            }
        }

        /// <summary>
        /// формируем геометрию объкта
        /// </summary>
        /// <param name="geometry"></param>
        private void GeometryFigureCopy(PathGeometry geometry, double Weight)
        {
            foreach (PathFigure geo in geometry.Figures)
            {
                PathFigure newfigure = new PathFigure(){IsClosed = false};
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
                m_figure.Figures.Add(newfigure);
            }
            //
            if (Weight < 1)
                StrokeThickness = m_strokethickness;
            else
            {
                StrokeThickness = Weight;
                m_strokethickness = Weight;
            }
            //
            m_strokethickness *= LoadProject.ProejctGrafic.Scroll;
            StrokeThickness = m_strokethickness;
        }

        /// <summary>
        /// создаем коллекцию линий и точек
        /// </summary>
        public void CreateCollectionLines()
        {
            _points.Clear();
            _lines.Clear();
            foreach (PathFigure geo in m_figure.Figures)
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
