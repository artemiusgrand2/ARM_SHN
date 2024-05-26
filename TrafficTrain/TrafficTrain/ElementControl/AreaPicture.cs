using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using ARM_SHN.Interface;
using System.Reflection;

namespace ARM_SHN.ElementControl
{
    /// <summary>
    /// Класс описывающий геометрию рамку станции
    /// </summary>
    public  class AreaPicture : BaseGraficElement, ISelectElement, IInfoElement
    {
        #region Переменные и свойства
      
        //////цветовая палитра
        /// <summary>
        /// цвет заливки
        /// </summary>
        public static Brush _colorfill = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        /// <summary>
        /// цвет рамки
        /// </summary>
        public static Brush _colorstroke = Brushes.Black;
        /// <summary>
        /// толщина контура объкта
        /// </summary>
        double _strokethickness = 1 * SystemParameters.CaretWidth;
        /// <summary>
        /// путь к файлу картинки
        /// </summary>
        string path = string.Empty;
        /// <summary>
        /// угол поворота картинки
        /// </summary>
        double angle;
        /// <summary>
        /// обозначение
        /// </summary>
        public string Nodes { get; set; }
     
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }

        public string NameUl
        {
            get
            {
                return string.Empty;
            }
        }

        public string FileClick { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public AreaPicture(PathGeometry geometry, string path, double angle) : base(geometry)
        {
            this.path = path;
            this.angle = angle;
            ViewModel.Stroke = _colorstroke;
            if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                SetFillColor(path);
            else
                ViewModel.Fill = _colorfill;
        }

        public string InfoElement()
        {
            return Nodes;
        }

        private void NewColor()
        {
            ViewModel.Fill = _colorfill;
            ViewModel.Stroke = _colorstroke;
        }
       

        private void SetFillColor(string path)
        {
            try
            {
                TransformedBitmap tb = new TransformedBitmap();
                // Create the source to use as the tb source.
                BitmapImage bi = new BitmapImage(new Uri(path));
                // Properties must be set between BeginInit and EndInit calls.
                tb.BeginInit();
                tb.Source = bi;
                RotateTransform transform = new RotateTransform(angle);
                tb.Transform = transform;
                tb.EndInit();
                ViewModel.Fill = new ImageBrush(tb);

            }
            catch { }
        }

    }
}
