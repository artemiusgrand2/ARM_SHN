using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using TrafficTrain.Interface;
using TrafficTrain.Enums;
using TrafficTrain.Delegate;

namespace TrafficTrain
{
    /// <summary>
    /// выбираем перегон или элементы для отправки команды ТУ
    /// </summary>
    class SelectElements
    {
        /// <summary>
        /// информация о правильности построения команды
        /// </summary>
        public static event Info InfoCommand;

        /// <summary>
        /// определяем попадание мышки на блок участок
        /// </summary>
        /// <param name="collection">коллекция отрисованных объектов</param>
        /// <param name="pointclick">точка касания мышью</param>
        public static BlockSection SelectBlockSection(IList<UIElement> collection, Point pointclick)
        {
            foreach (var el in collection)
            {
                //главный путь станции
                BlockSection _block = el as BlockSection;
                if (_block != null)
                {
                    _block.CreateCollectionLines();
                    //
                    if (HitStroke(_block.Lines, pointclick, _block.StrokeThickness))
                        return _block;

                }
            }
            return null;
        }

        /// <summary>
        /// определяем попадание мышки на название станции
        /// </summary>
        /// <param name="collection">коллекция отрисованных объектов</param>
        /// <param name="pointclick">точка касания мышью</param>
        public static NameStation SelectNameStation(IList<UIElement> collection, Point pointclick)
        {
            foreach (var el in collection)
            {
                 NameStation _namestation = el as NameStation;
                if (_namestation != null)
                {
                    _namestation.CreateCollectionLines();
                    //
                    if (HitObject(_namestation.Lines, _namestation.Points, pointclick))
                        return _namestation;
                }
            }
            return null;
        }

        /// <summary>
        /// выбираем один элемент
        /// </summary>
        /// <param name="collection">коллекция нарисованных объектов</param>
        /// <param name="pointclick">точка нажатия на мышь</param>
        public static bool ClickElement(IList<UIElement> collection, Point pointclick, ContentControl content)
        {
            //if (InfoCommand != null)
            //    InfoCommand(string.Empty);
            ////
            foreach (var el in collection)
            {
                ISelectElement command = el as ISelectElement;
                if (command != null)
                {
                    command.CreateCollectionLines();
                    if (HitObject(command.Lines, command.Points, pointclick) || HitStroke(command.Lines, pointclick, command.StrokeThickness))
                    {
                        return Commands.AnalisCommand(command, content);
                    }
                }
            }
            //
            return false;
        }

        public static bool FindInfoElement(IList<UIElement> collection, Point pointclick, CommandButton commandbutton)
        {
            var _selectElement = new List<IInfoElement>();
            var result = false;
            foreach (var el in collection)
            {
                ISelectElement command = el as ISelectElement;
                if (command != null)
                {
                    command.CreateCollectionLines();
                    if (el is BlockSection || el is LineHelp)
                    {
                        if (HitStroke(command.Lines, pointclick, command.StrokeThickness))
                        {
                            result = true;
                            if (el is IInfoElement)
                                _selectElement.Add(el as IInfoElement);
                        }
                    }
                    else
                    {
                        if (HitObject(command.Lines, command.Points, pointclick))
                        {
                            result = true;
                            //если выбран путь или сигнал
                            if (el is IInfoElement)
                                _selectElement.Add(el as IInfoElement);
                        }
                    }
                }
            }
            //
            var note = string.Empty;
            if (_selectElement.Count > 0)
            {
                _selectElement.Sort(Sort);
                note = (_selectElement[_selectElement.Count - 1] as IInfoElement).InfoElement();  
                result = true;
            }
            if (commandbutton != null)
            {
                if (!string.IsNullOrEmpty(note))
                {
                    commandbutton.Visibility = Visibility.Visible;
                    commandbutton.Text.Visibility = Visibility.Visible;
                }
                else if (commandbutton.Visibility == Visibility.Visible)
                {
                    commandbutton.Visibility = Visibility.Collapsed;
                    commandbutton.Text.Visibility = Visibility.Collapsed;
                }
                //
                commandbutton.Text.Text = note;
                commandbutton.LocationText();
            }
            //
            return result;
        }

        private static int Sort(IInfoElement x, IInfoElement y)
        {
            if (x.ZIntex > y.ZIntex)
                return 1;
            if (x.ZIntex < y.ZIntex)
                return -1;
            return 0;
        }

        /// <summary>
        /// определяем попадания в рамку горизонтальную и вертикальную
        /// </summary>
        /// <param name="collection">коллекция нарисованных объектов</param>
        /// <param name="pointclick">точка нажатия на мышь</param>
        public static SizeAligment SelectRectangleXY(Rectangle rectangle, Point pointclick, double strokethickness)
        {
            if (IsIntersectedBorderP(new Point(rectangle.Margin.Left, rectangle.Margin.Top), new Point(rectangle.Margin.Left + rectangle.Width, rectangle.Margin.Top), pointclick, strokethickness))
                return SizeAligment.vectical;
            //
            if (IsIntersectedBorderP(new Point(rectangle.Margin.Left, rectangle.Margin.Top), new Point(rectangle.Margin.Left, rectangle.Margin.Top + rectangle.Height), pointclick, strokethickness))
                return SizeAligment.horizontal;
            //
            return  SizeAligment.none;
        }

        /// <summary>
        /// определяем попадания в рамку горизонтальную
        /// </summary>
        /// <param name="collection">коллекция нарисованных объектов</param>
        /// <param name="pointclick">точка нажатия на мышь</param>
        public static SizeAligment SelectRectangleX(Rectangle rectangle, Point pointclick, double strokethickness)
        {
            if (IsIntersectedBorderP(new Point(rectangle.Margin.Left, rectangle.Margin.Top), new Point(rectangle.Margin.Left + rectangle.Width, rectangle.Margin.Top), pointclick, strokethickness))
                return SizeAligment.vectical;
            //
            return SizeAligment.none;
        }



        /// <summary>
        /// определяем попадание награницу объект
        /// </summary>
        /// <param name="_lines">коллекция линий</param>
        /// <param name="pointclick">точка касания</param>
        /// <returns></returns>
        static bool HitStroke(List<Line> _lines, Point pointclick, double strokethickness)
        {
            foreach (Line line in _lines)
            {
                if (IsIntersectedBorder(new Point(line.X1, line.Y1), new Point(line.X2, line.Y2), pointclick, strokethickness))
                    return true;
            }
            //
            return false;
        }

        private static Point MaxHeightWidth(PointCollection points)
        {
            Point max = new Point(double.MinValue, double.MinValue);
            Point min = new Point(double.MaxValue, double.MaxValue);
            foreach (Point point in points)
            {
                if (point.X > max.X)
                    max.X = point.X;
                //
                if (point.Y > max.Y)
                    max.Y = point.Y;
                //
                if (point.X < min.X)
                    min.X = point.X;
                //
                if (point.Y < min.Y)
                    min.Y = point.Y;
            }
            //
            return new Point(max.X - min.X, max.Y - min.Y);
        }

        /// <summary>
        /// определяем попадание в объект
        /// </summary>
        /// <param name="_lines">коллекция линий</param>
        /// <param name="_points">коллекция точек</param>
        /// <param name="pointclick">точка касания</param>
        /// <returns></returns>
        static bool HitObject(List<Line> _lines, PointCollection _points, Point pointclick)
        {
            Point max = MaxHeightWidth(_points);
            //направления
            bool Top = false;
            bool Bottom = false;
            bool Left = false;
            bool Right = false;
            //
            foreach (Line lin in _lines)
            {
                if (IsLinePartsIntersected(pointclick, new Point(pointclick.X, pointclick.Y - max.Y), new Point(lin.X1, lin.Y1), new Point(lin.X2, lin.Y2)))
                    Top = true;
                //
                if (IsLinePartsIntersected(pointclick, new Point(pointclick.X, pointclick.Y + max.Y), new Point(lin.X1, lin.Y1), new Point(lin.X2, lin.Y2)))
                    Bottom = true;
                //
                if (IsLinePartsIntersected(pointclick, new Point(pointclick.X - max.X, pointclick.Y), new Point(lin.X1, lin.Y1), new Point(lin.X2, lin.Y2)))
                    Left = true;
                //
                if (IsLinePartsIntersected(pointclick, new Point(pointclick.X + max.X, pointclick.Y), new Point(lin.X1, lin.Y1), new Point(lin.X2, lin.Y2)))
                    Right = true;
            }
            //
            if (Top && Bottom && Left && Right)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Определяем пересекаются ли линии
        /// </summary>
        /// <param name="pa1">первая точка первой линии</param>
        /// <param name="pa2">вторая точка первой линии</param>
        /// <param name="pb1">первая точка второй линии</param>
        /// <param name="pb2">второй точка второй линии</param>
        /// <returns></returns>
        static bool IsLinePartsIntersected(Point pa1, Point pa2, Point pb1, Point pb2)
        {
            double v1 = (pb2.X - pb1.X) * (pa1.Y - pb1.Y) - (pb2.Y - pb1.Y) * (pa1.X - pb1.X);
            double v2 = (pb2.X - pb1.X) * (pa2.Y - pb1.Y) - (pb2.Y - pb1.Y) * (pa2.X - pb1.X);
            double v3 = (pa2.X - pa1.X) * (pb1.Y - pa1.Y) - (pa2.Y - pa1.Y) * (pb1.X - pa1.X);
            double v4 = (pa2.X - pa1.X) * (pb2.Y - pa1.Y) - (pa2.Y - pa1.Y) * (pb2.X - pa1.X);

            if ((v1 * v2 < 0) && (v3 * v4 < 0))
                return true;
            else return false;
        }

        /// <summary>
        /// определяем попадание на границу объекта принадлежит ли точка объекту
        /// </summary>
        /// <returns></returns>
        static bool IsIntersectedBorder(Point p1, Point p2, Point PointX, double strokethickness)
        {
            double a = DistancePoint(p1, PointX);
            double b = DistancePoint(p2, PointX);
            double c = DistancePoint(p1, p2);
            double angle1 = AngleTriangle(a, b, c);
            double angle2 = AngleTriangle(b, a, c);
            //
            if (angle1 >= 0 && angle2 > 0)
            {
                double sin = Sin(angle1) * b;
                if ((Sin(angle1) * b) <= (strokethickness))
                    return true;
            }
            return false;
        }

        static bool IsIntersectedBorderP(Point p1, Point p2, Point PointX, double strokethickness)
        {
            //для вертикального отрезка
            if (p1.X == p2.X)
            {
                if (Math.Abs(PointX.X - p1.X) <= strokethickness && PointX.Y > p1.Y )
                    return true;
            }
            //для горизонтального отрезка
            if (p1.Y == p2.Y)
            {
                if (Math.Abs(PointX.Y - p1.Y) <= strokethickness && PointX.X > p1.X)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// определяем расстояние между точками
        /// </summary>
        /// <param name="p1">точка номер один</param>
        /// <param name="p2">точка номер два</param>
        /// <returns></returns>
        static private double DistancePoint(Point p1, Point p2)
        {
            double X = Math.Pow(p2.X - p1.X, 2);
            double Y = Math.Pow(p2.Y - p1.Y, 2);
            double answer = Math.Sqrt(X + Y);
            return answer;
        }

        static private double AngleTriangle(double a, double b, double c)
        {
            return ((Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(a, 2)) / (2 * b * c));
        }

        static private double Sin(double cos)
        {
            return (Math.Sqrt(1 - cos * cos));
        }

        /// <summary>
        /// вычисление координат вершин линий стрелки
        /// </summary>
        /// <param name="basepoint">базовая точка центральной линии</param>
        /// <param name="toppoint">вершинная точка центральной линии</param>
        /// <param name="lenght">длина основания стрелки</param>
        /// <param name="height">высота стрелки</param>
        /// <param name="arrowlineone">вершинная точка первой линии стрелки</param>
        /// <param name="arrowlinety">вершинная точка второй линии стрелки</param>
        public static bool PointsArrow(Point basepoint, Point toppoint, double lenght, double height, ref Point arrowlineone, ref Point arrowlinety)
        {
            Point top_middle = new Point();
            if (GetPointIntersection(toppoint, toppoint, basepoint, lenght, ref top_middle))
            {
                if (Math.Round(toppoint.Y - basepoint.Y, 3) == 0)
                {
                    arrowlineone.X = top_middle.X ;
                    arrowlineone.Y = top_middle.Y + height;
                    //
                    arrowlinety.X = top_middle.X;
                    arrowlinety.Y = top_middle.Y - height;
                    return true;
                }
                //
                if (Math.Round(toppoint.X - basepoint.X, 3) == 0)
                {
                    arrowlineone.X = top_middle.X+ height;
                    arrowlineone.Y = top_middle.Y ;
                    //
                    arrowlinety.X = top_middle.X - height;
                    arrowlinety.Y = top_middle.Y ;
                    return true;
                }
                //коэффициент наклона основной линии
                double Ke = (toppoint.Y - basepoint.Y) / (toppoint.X - basepoint.X);
                //коэффициент наклона вспомагательной линии
                double Kp = -(1 / Ke);
                //коэффициент переноса
                double Bp = top_middle.Y - Kp * top_middle.X;
                //координаты первой точки
                arrowlineone.X = top_middle.X + height / (Math.Sqrt(Kp * Kp + 1));
                arrowlineone.Y = Kp * arrowlineone.X + Bp;
                //координаты второй точки
                arrowlinety.X = top_middle.X - height / (Math.Sqrt(Kp * Kp + 1));
                arrowlinety.Y = Kp * arrowlinety.X + Bp;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// определяем точку пересечения прямой и окружности
        /// </summary>
        /// <param name="center">центр окружности</param>
        /// <param name="P1">первая точка линии</param>
        /// <param name="P2">вторая точка линии</param>
        /// <param name="R">радиус окружности</param>
        /// <returns>точка пересечения</returns>
        public static bool GetPointIntersection(Point center, Point P1, Point P2, double R, ref Point result)
        {
            if (R == 0)
            {
                result.X = center.X;
                result.Y = center.Y;
                return true;
            }
            //коэффициент наклона линии
            if (Math.Round(P2.X - P1.X, 3) == 0)
            {
                result.X = center.X;
                double Y1 = center.Y + R;
                double Y2 = center.Y - R;
                if ((Y1 <= P2.Y && Y1 >= P1.Y) || (Y1 >= P2.Y && Y1 <= P1.Y))
                    result.Y = Y1;
                else result.Y = Y2;
                return true;
            }
            else
            {
                double K = (P2.Y - P1.Y) / (P2.X - P1.X);
                //
                double C = (P1.Y - (P1.X * (P2.Y - P1.Y)) / (P2.X - P1.X));
                double m = C - center.Y;
                //дискременант  квадратного уравнения
                double D = Math.Pow((2 * m * K - 2 * center.X), 2) - 4 * (1 + K * K) * (m * m + center.X * center.X - R * R);
                if (D > 0)
                {
                    //первая точка
                    double X1 = (2 * center.X - 2 * m * K - Math.Sqrt(D)) / (2 * (1 + K * K));
                    double Y1 = K * X1 + C;
                    //вторая точка
                    double X2 = (2 * center.X - 2 * m * K + Math.Sqrt(D)) / (2 * (1 + K * K));
                    double Y2 = K * X2 + C;
                    //
                    if ((X1 <= P2.X && X1 >= P1.X) || (X1 >= P2.X && X1 <= P1.X))
                    {
                        result.X = X1;
                        result.Y = Y1;
                    }
                    else
                    {
                        result.X = X2;
                        result.Y = Y2;
                    }
                    return true;
                }
                if (D == 0)
                {
                    result.X = (2 * center.X - 2 * m * K) / (2 * (1 + K * K));
                    result.Y = K * result.X + C;
                    return true;
                }
            }

            //
            return false;
            ////коэффициент наклона линии
            //if (P2.X - P1.X == 0)
            //{
            //    result.X = center.X;
            //    double Y1 = center.Y + R;
            //    double Y2 = center.Y - R;
            //    if ((Y1 <= P2.Y && Y1 >= P1.Y) || (Y1 >= P2.Y && Y1 <= P1.Y))
            //        result.Y = Y1;
            //    else result.Y = Y2;
            //    return true;
            //}
            //else
            //{
            //    double K = (P2.Y - P1.Y) / (P2.X - P1.X);
            //    //
            //    double C = (P1.Y - (P1.X * (P2.Y - P1.Y)) / (P2.X - P1.X));
            //    double m = C - center.Y;
            //    //дискременант  квадратного уравнения
            //    double D = Math.Pow((2 * m * K - 2 * center.X), 2) - 4 * (1 + K * K) * (m * m + center.X * center.X - R * R);
            //    if (D > 0)
            //    {
            //        //первая точка
            //        double X1 = (2 * center.X - 2 * m * K - Math.Sqrt(D)) / (2 * (1 + K * K));
            //        double Y1 = K * X1 + C;
            //        //вторая точка
            //        double X2 = (2 * center.X - 2 * m * K + Math.Sqrt(D)) / (2 * (1 + K * K));
            //        double Y2 = K * X2 + C;
            //        //
            //        if ((X1 <= P2.X && X1 >= P1.X) || (X1 >= P2.X && X1 <= P1.X))
            //        {
            //            result.X = X1;
            //            result.Y = Y1;
            //        }
            //        else
            //        {
            //            result.X = X2;
            //            result.Y = Y2;
            //        }
            //        return true;
            //    }
            //}
            ////
            //return false;
        }
    }
}
