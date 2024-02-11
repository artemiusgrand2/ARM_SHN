using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ARM_SHN.DataGrafik;

namespace ARM_SHN
{
    /// <summary>
    /// Контрол управления цветом
    /// </summary>
    public partial class ControlColor : UserControl
    {
        #region Variable
        /// <summary>
        /// текущеее название используемого цвета
        /// </summary>
        string _current_name_color = string.Empty;
        /// <summary>
        /// базовый цвет
        /// </summary>
        Brush _base_color;
        /// <summary>
        /// итендификатор цвета
        /// </summary>
        EnumColor _id;
        #endregion

        public ControlColor(string name, WindowColor window, Brush basecolor, EnumColor id)
        {
            InitializeComponent();
            window.UpdateColorValue += UpdateValue;
            window.UpdateColorName += UpdateName;
            window.AddColor += AddColor;
            window.RemoveColor += RemoveColor;
            _id = id;
            _base_color = basecolor;
            LoadColor(basecolor, name, false, false);
        }

        public void UpdateControl()
        {
            _current_name_color = string.Empty;
            LoadColor(_base_color,CommandColor.GetNameColor(_id), true, false);
        }

        private int GetNumberName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (LoadProject.ColorConfiguration != null)
                {
                    for (int i = 0; i < LoadProject.ColorConfiguration.ColorNames.Count; i++)
                    {
                        if (name == LoadProject.ColorConfiguration.ColorNames[i])
                            return i;
                    }
                }
            }
            //
            return -1;
        }

        private void LoadColor(Brush basecolor, string name, bool update, bool updatesceen)
        {
            if (LoadProject.ColorConfiguration != null)
                NameColor.ItemsSource = LoadProject.ColorConfiguration.ColorNames;
            //
            if (CheckColor(name))
                NameColor.SelectedIndex = GetNumberName(name);
            else
            {
                if (basecolor is SolidColorBrush)
                    ColorCanvas.Background = new SolidColorBrush(Color.FromRgb((basecolor as SolidColorBrush).Color.R, (basecolor as SolidColorBrush).Color.G, (basecolor as SolidColorBrush).Color.B));
            }
        }

        public static ColorN GetColor(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                foreach (ValueColor value in LoadProject.ColorConfiguration.ColorValues)
                {
                    if (value.Name == name)
                        return value.Value;
                }
            }
            return new ColorN();
        }

        public static ValueColor FindColor(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                foreach (ValueColor value in LoadProject.ColorConfiguration.ColorValues)
                {
                    if (value.Name == name)
                        return value;
                }
            }
            //
            return null;
        }

        private void NewColor(bool update, bool updatesceen)
        {
            ColorN color = GetColor(_current_name_color);
            SetColor(color.R, color.G, color.B, update, updatesceen);
        }

        private void SetColor(byte R, byte G, byte B, bool update, bool updatesceen)
        {
            ColorCanvas.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            if (update)
            {
                CommandColor.SetColor(_id, Color.FromRgb(R, G, B));
                CommandColor.SetNameColor(_id, _current_name_color);
                if(updatesceen)
                    LoadColorControl.UpdateColor();
            }
        }

      
        private void UpdateValue(string name)
        {
            if (_current_name_color == name)
            {
                if (CheckColor(_current_name_color))
                    NewColor(true, true);
            }
            //
            NameColor.Items.Refresh();
        }

        private void UpdateName(string nameold, string namenew)
        {
            if (_current_name_color == nameold)
            {
                NameColor.Items.Refresh();
                NameColor.SelectedIndex = GetNumberName(namenew);
            }
        }

        private void AddColor()
        {
            NameColor.Items.Refresh();
        }

        private void RemoveColor(string name)
        {
            if (name == _current_name_color)
                _current_name_color = string.Empty;
            NameColor.Items.Refresh();
        }

        public static bool CheckColor(string name)
        {
            if (LoadProject.ColorConfiguration != null)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    foreach (ValueColor value in LoadProject.ColorConfiguration.ColorValues)
                    {
                        if (value.Name == name)
                            return true;
                    }
                }
            }
            return false;
        }

        private void NameColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (NameColor.SelectedItem != null)
                {
                    if (CheckColor(NameColor.SelectedItem.ToString()))
                    {
                        if (_current_name_color != NameColor.SelectedItem.ToString())
                        {
                            _current_name_color = NameColor.SelectedItem.ToString();
                            //обновление основного цвета
                            ColorN color = GetColor(_current_name_color);
                            SetColor(color.R, color.G, color.B, true, true);
                        }
                    }
                }
            }
            catch { }
        }
    }
}
