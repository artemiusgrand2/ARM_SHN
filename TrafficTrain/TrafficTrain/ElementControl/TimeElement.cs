using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.Interface;
using ARM_SHN.WorkWindow;



namespace ARM_SHN.ElementControl
{
    class TimeElement : BaseTextGraficElement, IText, IInfoElement
    {
        #region Переменные и свойства
        
        //////цветовая фона
        public static Brush _color_fon = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush _color_text = Brushes.Black;
        /// <summary>
        /// цвет рамки
        /// </summary>
        public static Brush _color_stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

        public string NameUl
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public TimeElement(PathGeometry geometry, double marginX, double marginY, double fontsize):base(geometry)
        {;
            ViewModel.FontSize = fontsize;
            ViewModel.Margin = new Thickness(marginX, marginY, 0, 0);
            ViewModel.FontWeight = FontWeights.Bold;
            ViewModel.TextWrapping = TextWrapping.Wrap;
            ViewModel.TextAlignment = TextAlignment.Center;
            //
            ViewModel.Stroke = _color_stroke;
            Fill = _color_fon;
            ViewModel.Foreground = _color_text;
            //обработка времени
            Connections.NewSecond += StartTime;
            LoadColorControl.NewColor += NewColor;
        }

        public override void Dispose()
        {
            Connections.NewSecond -= StartTime;
            LoadColorControl.NewColor -= NewColor;
        }

        private void NewColor()
        {
            ViewModel.Stroke = _color_stroke;
            ViewModel.Foreground = _color_text;
            ViewModel.Fill = _color_fon;
        }

        public string InfoElement()
        {
            var result = string.Format("{0}", DateTime.Now.Day);
            switch (DateTime.Now.Month)
            {
                case 1:
                    result = string.Format("{0} {1}", result, "Января");
                    break;
                case 2:
                    result = string.Format("{0} {1}", result, "Февраля");
                    break;
                case 3:
                    result = string.Format("{0} {1}", result, "Марта");
                    break;
                case 4:
                    result = string.Format("{0} {1}", result, "Апреля");
                    break;
                case 5:
                    result = string.Format("{0} {1}", result, "Мая");
                    break;
                case 6:
                    result = string.Format("{0} {1}", result, "Июня");
                    break;
                case 7:
                    result = string.Format("{0} {1}", result, "Июля");
                    break;
                case 8:
                    result = string.Format("{0} {1}", result, "Августа");
                    break;
                case 9:
                    result = string.Format("{0} {1}", result, "Сентября");
                    break;
                case 10:
                    result = string.Format("{0} {1}", result, "Отктября");
                    break;
                case 11:
                    result = string.Format("{0} {1}", result, "Ноября");
                    break;
                case 12:
                    result = string.Format("{0} {1}", result, "Декабря");
                    break;
            }
            result = string.Format("{0} {1} года", result, DateTime.Now.Year);
            //
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = string.Format("{0} {1}", result, "Понедельник");
                    break;
                case DayOfWeek.Tuesday:
                    result = string.Format("{0} {1}", result, "Вторинк");
                    break;
                case DayOfWeek.Wednesday:
                    result = string.Format("{0} {1}", result, "Среда");
                    break;
                case DayOfWeek.Thursday:
                    result = string.Format("{0} {1}", result, "Четверг");
                    break;
                case DayOfWeek.Friday:
                    result = string.Format("{0} {1}", result, "Пятница");
                    break;
                case DayOfWeek.Saturday:
                    result = string.Format("{0} {1}", result, "Суббота");
                    break;
                case DayOfWeek.Sunday:
                    result = string.Format("{0} {1}", result, "Воскресенье");
                    break;
            }
            //
            return $"{result} {Notes}";
        }

        private void StartTime()
        {
            ViewModel.Text = string.Format("{0:D2}.{1:D2}.{2} {3:D2}:{4:D2}:{5:D2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year.ToString().Remove(0, 2), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

    }
}
