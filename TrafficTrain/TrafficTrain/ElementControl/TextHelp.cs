using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.Interface;
using ARM_SHN.EditText;
using SCADA.Common.Enums;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;

namespace ARM_SHN.ElementControl
{
    class TextHelp : BaseTextGraficElement, ISelectElement, IText, IInfoElement
    {
        #region Переменные и свойства
        /// <summary>
        /// цвет фона
        /// </summary>
         Brush _color_fon = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush _color_text = Brushes.Black;
        /// <summary>
        /// цвет рамки
        /// </summary>
        Brush _color_stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        //////

        public string NameUl
        {
            get
            {
                return NameObject;
            }
        }

        public int StationTransition { get; set; }
        public string FileClick { get; set; } = string.Empty;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public TextHelp(PathGeometry geometry, double marginX, double marginY, double fontsize, string text, double rotate): base(text, ViewElement.texthelp, geometry, rotate)
        {
            ViewModel.FontSize = fontsize;
            ViewModel.Text = text;
            ViewModel.Foreground = _color_text;
            ViewModel.Margin = new Thickness(marginX, marginY, 0, 0);
            ViewModel.FontWeight = FontWeights.Bold;
            ViewModel.TextWrapping = TextWrapping.Wrap;
            ViewModel.TextAlignment = TextAlignment.Center;
            ViewModel.RenderTransform = new RotateTransform(RotateText);
            //обработка времени
            LoadColorControl.NewColor += NewColor;
        }

        public string InfoElement()
        {
            return Notes;
        }

        public override void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
        }

        private void NewColor()
        {
            ViewModel.Foreground = _color_text;
            ViewModel.Fill = _color_fon;
        }

    }
}
