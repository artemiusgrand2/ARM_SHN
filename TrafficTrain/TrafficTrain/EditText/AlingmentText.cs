using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;

namespace TrafficTrain.EditText
{
    class AlingmentText
    {


        /// <summary>
        /// Выравнивание название главного пути по центру при изменении длины или ширины
        /// </summary>
        /// <returns></returns>
        public static Thickness AlingmentCenter(double ramkaX, double ramkaY, double width, double height, TextBlock text, double RotateText)
        {
            if (RotateText == 0 || Math.Abs(RotateText) == 360)
                return new Thickness(ramkaX + (width - WidthText(text)) / 2, ramkaY + (height - HeightText(text)) / 2, 0, 0);
            //
            if (Math.Abs(RotateText) == 90)
                return new Thickness(ramkaX - (height - HeightText(text)) / 2, ramkaY + (width - WidthText(text)) / 2, 0, 0);
            //
            if (Math.Abs(RotateText) == 180)
                return new Thickness(ramkaX - (width - WidthText(text)) / 2, ramkaY - (height - HeightText(text)) / 2, 0, 0);
            //
            if (Math.Abs(RotateText) == 270)
                return new Thickness(ramkaX + (height - HeightText(text)) / 2, ramkaY - (width - WidthText(text)) / 2, 0, 0);
            //
            return new Thickness(0, 0, 0, 0);
        }

        /// <summary>
        /// ширина текста в пикселях
        /// </summary>
        /// <param name="textblock"></param>
        /// <returns></returns>
        public static double WidthText(TextBlock textblock)
        {
            Typeface typeface = new Typeface(textblock.FontFamily, textblock.FontStyle, textblock.FontWeight, textblock.FontStretch);
            System.Windows.Media.Brush brush = new SolidColorBrush();
            FormattedText formatedText = new FormattedText(textblock.Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, textblock.FontSize, brush);
            return formatedText.Width;
        }
        /// <summary>
        /// высота текста в пикселях
        /// </summary>
        /// <param name="textblock"></param>
        /// <returns></returns>
        public static double HeightText(TextBlock textblock)
        {
            Typeface typeface = new Typeface(textblock.FontFamily, textblock.FontStyle, textblock.FontWeight, textblock.FontStretch);
            System.Windows.Media.Brush brush = new SolidColorBrush();
            FormattedText formatedText = new FormattedText(textblock.Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, textblock.FontSize, brush);
            return formatedText.Height;
        }

        /// <summary>
        /// определяем длину отрезка между двумя точками
        /// </summary>
        /// <param name="p1">точка один</param>
        /// <param name="p2">точка два</param>
        /// <returns></returns>
        public static double LenghtStorona(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        /// <summary>
        /// вычисляем размер текста (высоту)
        /// </summary>
        public static double FontSizeText(double textKw, double textKh, Rectangle ramka, string _text, double RotateText)
        {
            double fontsizeWr = textKw * ramka.Width / _text.Length;
            double fontsizeHr = ramka.Height * textKh;
            if ((fontsizeHr) >= (fontsizeWr))
                return fontsizeWr;
            else return fontsizeHr;
        }
    }
}
