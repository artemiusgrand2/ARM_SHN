using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.Interface;
using ARM_SHN.WorkWindow;
using ARM_SHN.DataServer;
using ARM_SHN.Enums;
using ARM_SHN.EditText;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using static System.Net.Mime.MediaTypeNames;


namespace ARM_SHN.ElementControl
{
    /// <summary>
    /// класс индикации шильд
    /// </summary>
    class AnalogCell : BaseTextGraficElement, IText, ISelectElement, IInfoElement
    {
        #region Переменные и свойства
       

        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }

        //////цветовая палитра
        /// <summary>
        /// цвет фона по умолчанию
        /// </summary>
        public static Brush m_color_fon_defult = new SolidColorBrush(Color.FromRgb(195, 195, 195));
        /// <summary>
        /// цвет рамки по умолчанию
        /// </summary>
        public static Brush m_color_stroke_defult = Brushes.Black;
        /// <summary>
        /// цвет текста
        /// </summary>
        public static Brush m_colortext = Brushes.Black;
        /// толщина контура объкта
        /// </summary>
        double m_strokethickness = 1 * SystemParameters.CaretWidth;

 

        public string NameUl
        {
            get
            {
                return NameObject;
            }
        }
        private string m_noData = "--";
        /// <summary>
        /// id таблицы
        /// </summary>
        private int m_tableId;
        /// <summary>
        /// название измерения
        /// </summary>
        private string m_nameItem;

        public bool IsFind { get; set; }
        /// <summary>
        /// тип аналоговый данных
        /// </summary>
        private FieldType m_type = FieldType.UintType;

        private double m_factor;

        /// <summary>
        /// число символов до запятой
        /// </summary>
        private byte m_wholeCount = 3;
        /// <summary>
        /// число символов после запятой
        /// </summary>
        private byte m_fractionalCount = 2;

        public string FileClick { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stationnumber">шестизначный номер станции</param>
        /// <param name="geometry">геометрия объекта</param>
        /// <param name="name">название объекта</param>
        public AnalogCell(PathGeometry geometry, string name, double marginX, double marginY, double fontsize, double rotate, int tableId, string nameItem, string format, string factor, FieldType type)
            :base(name, ViewElement.analogCell, geometry, rotate)
        {
            ViewModel.Text = m_noData;
            ViewModel.Foreground = m_colortext;
            ViewModel.FontSize = fontsize;
            ViewModel.Margin = new Thickness(marginX, marginY, 0, 0);
            ViewModel.RenderTransform = new RotateTransform(RotateText);
            ViewModel.StrokeThickness = LoadProject.ProejctGrafic.Scroll * SystemParameters.CaretWidth;
            ViewModel.Fill = m_color_fon_defult;
            ViewModel.Stroke = m_color_stroke_defult;
            //
            m_tableId = tableId;
            m_nameItem = nameItem;
            m_type = type;
            m_factor = AnalisFactor(factor);
            AnalisFormat(format);
            LocationText();
        }

        private void AnalisFormat(string format)
        {
            var indexPoint = format.IndexOf('.');
            if (indexPoint != -1 && indexPoint > 0)
            {
                m_wholeCount = (byte)indexPoint;
                m_fractionalCount = (byte)(format.Length - (indexPoint + 1));
            }
            else if (format.Length > 0)
            {
                m_wholeCount = (byte)format.Length;
                m_fractionalCount = 0;
            }
        }

        public string InfoElement()
        {
            return Notes;
        }

        static double AnalisFactor(string factor)
        {
            double result;
            if(!double.TryParse(SCADA.Common.HelpCommon.HelpFuctions.GetFormatString(factor), out result))
                result =1;
            //
            return result;
        }


        public void AnalisNewData()
        {
            if (IsFind)
            {
                if (Core.Stations.TryGetValue(StationControl, out var stationfind))
                {
                    if (stationfind.TryGetValue(m_tableId, out var tablefind))
                    {
                        var data = tablefind.GetValue(m_nameItem);
                        //
                        if (data.Count > 0)
                        {
                            bool isNotValue;
                            double val = GetValue(data[0].Value, out isNotValue);
                            var valStr = (isNotValue) ? m_noData : GetFormatString(tablefind.Corrector * val * m_factor);
                            if (valStr != ViewModel.Text)
                            {
                                ViewModel.Text = valStr;
                                LocationText();
                            }
                        }
                    }
                }
            }
        }

        private string GetFormatString(double data)
        {
            try
            {
                var wholeNumber = (long)Math.Truncate(data);
                var whole = wholeNumber.ToString(string.Format("D{0}", m_wholeCount));
                if (m_fractionalCount > 0)
                    return $"{whole}.{data.ToString(string.Format("F{0}", m_fractionalCount)).Split(new char[] { ',', '.' })[1]}";
                else
                    return whole;
            }
            catch
            {
                return "ERROR";
            }
        }

        private float GetValue(byte[] data, out bool isNotValue)
        {
            var buffer = new byte [data.Length];
            isNotValue = false;
            isNotValue = CheckNotValue(data);
            switch (data.Length)
            {
                case 4:
                    {
                        switch (m_type)
                        {
                            case FieldType.floatType:
                                return BitConverter.ToSingle(FormaterToSingle(data), 0);
                            case FieldType.intType:
                                return BitConverter.ToInt32(data, 0);
                            default:
                                return BitConverter.ToUInt32(data, 0);
                        }
                    }
                case 2:
                    {
                        switch (m_type)
                        {
                            case FieldType.floatType:
                                return BitConverter.ToSingle(FormaterToSingle(data), 0);
                            case FieldType.intType:
                                return BitConverter.ToInt16(data, 0);
                            default:
                                return BitConverter.ToUInt16(data, 0);
                        }
                    }
                default:
                    {
                        return (m_type == FieldType.floatType) ? BitConverter.ToSingle(FormaterToSingle(data), 0) : data[data.Length - 1];
                    }
            }
        }

        static bool CheckNotValue(byte[] data)
        {
            var countF = 0;
            foreach (var bit in data)
            {
                if (bit == 0xFF)
                    countF++;
            }
            if (countF > data.Length / 2)
                return true;
            //
            return false;
        }

        static byte[] FormaterToSingle(byte [] data)
        {
            var buffer = new List<byte>();
            buffer.AddRange(data);
            if (data.Length < 4)
            {
                while (buffer.Count < 4)
                {
                    buffer.Insert(0, 0);
                }
            }
            //
            return buffer.ToArray();
        }

    
    }
}
