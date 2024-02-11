//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Windows;
//using System.Windows.Data;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Shapes;

//using ARM_SHN.Interface;

//using SCADA.Common.Models;
//using SCADA.Common.Enums;

//namespace ARM_SHN.ElementControl
//{
//    public class BaseGraficElement : Shape, IGraficElement, IDisposable
//    {
//        /////геометрия элемента
//        /// <summary>
//        /// отрисовываемая геометрия
//        /// </summary>
//        protected override Geometry DefiningGeometry
//        {
//            get
//            {
//                return _geometry;
//            }
//        }
//        /// <summary>
//        /// геометрическое иписание фигуры
//        /// </summary>
//        private PathGeometry _geometry  = new PathGeometry();

//        /// <summary>
//        /// обозначение
//        /// </summary>
//        public string Notes { get; set; }
//        /// <summary>
//        /// Индекс слоя
//        /// </summary>
//        public int ZIntex
//        {
//            get
//            {
//                return ViewModel.ZIndex;
//            }
//            set
//            {
//                ViewModel.ZIndex = value;
//            }
//        }

//        /// <summary>
//        /// графическая модель
//        /// </summary>
//        public GraficElementModel ViewModel { get; private set; }


//        public int StationNumber
//        {
//            get
//            {
//                if (ViewModel != null)
//                    return ViewModel.StationNumber;
//                //
//                return 0;
//            }
//        }


//        public string NameObject
//        {
//            get
//            {
//                if (ViewModel != null)
//                    return ViewModel.Name;
//                //
//                return string.Empty;
//            }
//        }

//        public ViewElement View
//        {
//            get
//            {
//                if (ViewModel != null)
//                    return ViewModel.ViewElement;
//                //
//                return ViewElement.none;
//            }
//        }

//        public BaseGraficElement(PathGeometry geometry, bool IsClosed = false, bool IsFilled = true, int indexFigure = -1)
//        {
//            ViewModel = new GraficElementModel();
//            SetBindins(ViewModel);
//            CreateGeometry(geometry, IsClosed, IsFilled, indexFigure);
//        }

//        public BaseGraficElement(string name, int stationNumber, ViewElement view, TypeView typeView, PathGeometry geometry, bool IsClosed = false, bool IsFilled = true, int indexFigure = -1)
//        {
//            ViewModel = new GraficElementModel(name, stationNumber, view, typeView);
//            SetBindins(ViewModel);
//            CreateGeometry(geometry, IsClosed, IsFilled, indexFigure);
//        }

//        public BaseGraficElement(PathGeometry geometry, int indexFigure)
//        {
//            ViewModel = new GraficElementModel();
//            SetBindins(ViewModel);
//            CreateGeometry(geometry, false, true, indexFigure);
//        }

//        public BaseGraficElement(string name, int stationNumber, ViewElement view, TypeView typeView, PathGeometry geometry, int indexFigure)
//        {
//            ViewModel = new GraficElementModel(name, stationNumber, view, typeView);
//            SetBindins(ViewModel);
//            CreateGeometry(geometry, false, true, indexFigure);
//        }

//        public BaseGraficElement(string name, int stationNumber, ViewElement view, TypeView typeView, PathGeometry geometry)
//        {
//            ViewModel = new GraficElementModel(name, stationNumber, view, typeView);
//            SetBindins(ViewModel);
//            CreateGeometry(geometry, false, true, -1);
//        }

//        public virtual void Dispose() { }

//        protected virtual void SetBindins(GraficElementModel model)
//        {
//            var bindingStroke = new Binding("Stroke"); 
//            bindingStroke.Source = model;
//            this.SetBinding(StrokeProperty, bindingStroke);
//            //
//            var bindingFill = new Binding("Fill");
//            bindingFill.Source = model;
//            this.SetBinding(FillProperty, bindingFill);
//            //
//            var bindingStrokeThickness = new Binding("StrokeThickness");
//            bindingStrokeThickness.Source = model;
//            this.SetBinding(StrokeThicknessProperty, bindingStrokeThickness);
//            ////
//            var bindingVisibility = new Binding("Visibility");
//            bindingVisibility.Source = model;
//            this.SetBinding(VisibilityProperty, bindingVisibility);
//            ////
//            var bindingZIndex = new Binding("ZIndex");
//            bindingZIndex.Source = model;
//            this.SetBinding(Canvas.ZIndexProperty, bindingZIndex);
//        }

//        protected virtual void CreateGeometry(PathGeometry geometry, bool IsClosed, bool IsFilled, int indexFigure)
//        {
//            if (geometry == null)
//                return;
//            //
//            int index = 0;
//            foreach (PathFigure geo in geometry.Figures)
//            {
//                PathFigure newfigure = new PathFigure() { IsClosed = IsClosed };
//                if (index == 0 && !IsFilled)
//                    newfigure.IsFilled = IsFilled;
//                newfigure.StartPoint = new Point(geo.StartPoint.X, geo.StartPoint.Y);
//                var bufferPoints = new List<Point>();
//                bufferPoints.Add(newfigure.StartPoint);
//                foreach (PathSegment seg in geo.Segments)
//                {
//                    Point newPointSegment;
//                    //сегмент линия
//                    LineSegment lin = seg as LineSegment;
//                    if (lin != null)
//                    {
//                        newPointSegment = new Point(lin.Point.X, lin.Point.Y);
//                        newfigure.Segments.Add(new LineSegment() { Point = newPointSegment });
//                        bufferPoints.Add(newPointSegment);
//                        continue;
//                    }
//                    //сегмент арка
//                    ArcSegment arc = seg as ArcSegment;
//                    if (arc != null)
//                    {
//                        newPointSegment = new Point(arc.Point.X, arc.Point.Y);
//                        newfigure.Segments.Add(new ArcSegment() { Point = new Point(arc.Point.X, arc.Point.Y), Size = new Size(arc.Size.Width, arc.Size.Height) });
//                        bufferPoints.Add(newPointSegment);
//                        continue;
//                    }
//                }
//                //
//                if (indexFigure == -1)
//                    _geometry.Figures.Add(newfigure);
//                else if (index == indexFigure)
//                {
//                    _geometry.Figures.Add(newfigure);
//                    break;
//                }
//                index++;
//            }
//        }

//    }
//}
