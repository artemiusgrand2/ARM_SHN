using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.Interface;

namespace ARM_SHN.ElementControl
{
    /// <summary>
    /// Класс описывающий геометрию рамку станции
    /// </summary>
    class RamkaStation : BaseGraficElement
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

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public RamkaStation(PathGeometry geometry) : base(geometry)
        {
            ViewModel.Fill = _colorfill;
            ViewModel.Stroke = _colorstroke;
            ViewModel.StrokeThickness = (SystemParameters.CaretWidth * LoadProject.ProejctGrafic.Scroll);
            LoadColorControl.NewColor += NewColor;
        }

        public override void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
        }

        private void NewColor()
        {
            ViewModel.Fill = _colorfill;
            ViewModel.Stroke = _colorstroke;
        }
    }
}
