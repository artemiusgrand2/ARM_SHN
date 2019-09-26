using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using TrafficTrain.DataGrafik;

namespace TrafficTrain
{
    public delegate void AddColor();
    public delegate void UpdateColorValue(string name);
    public delegate void UpdateColorName(string nameold, string namenew);

    public partial class WindowColor : Window
    {
        #region Variable
        /// <summary>
        /// обновление значения цвета
        /// </summary>
        public event UpdateColorValue UpdateColorValue;
        /// <summary>
        /// обновление имени цвета 
        /// </summary>
        public event UpdateColorName UpdateColorName;
        /// <summary>
        /// добавление нового имени цвета 
        /// </summary>
        public event AddColor AddColor;
        /// <summary>
        /// удаление имени цвета 
        /// </summary>
        public event UpdateColorValue RemoveColor;
        /// <summary>
        /// можно ли закрыть окно
        /// </summary>
        public bool YesClose { get; set; }
        #endregion

        public WindowColor()
        {
            InitializeComponent();
            LoadColorControl loadcolor = new LoadColorControl();
            loadcolor.CreatePanel(ComBoxColorNames, MainPanel, this);
        }

        private void Save()
        {
            try
            {
                if (!string.IsNullOrEmpty(LoadProject.ColorConfiguration.Name))
                {
                    using (Stream savestream = new FileStream(LoadProject.ColorConfiguration.File, FileMode.Create))
                    {
                        // Указываем тип того объекта, который сериализуем
                        XmlSerializer xml = new XmlSerializer(typeof(ColorConfiguration));
                        // Сериализуем
                        xml.Serialize(savestream, LoadProject.ColorConfiguration);
                        savestream.Close();
                    }
                }
            }
            catch { }
        }

        public void UpdatePalitra()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                LoadColorControl.AnalisLoad();
                ExsampleColorPanel.Background = Brushes.Black;
                ExsampleValueColor.Text = "0,0,0";
                ComBoxColorNames.ItemsSource = LoadProject.ColorConfiguration.ColorNames;
                //ComBoxColorNames.Items.Refresh();
                foreach (UIElement el in MainPanel.Children)
                {
                    if (el is ControlColorElement)
                    {
                        foreach (UIElement st in (el as ControlColorElement).GetPanel().Children)
                        {
                            if (st is ControlColorStatus)
                            {
                                foreach (UIElement color in (st as ControlColorStatus).GetPanel().Children)
                                {
                                    if (color is ControlColor)
                                        (color as ControlColor).UpdateControl();
                                }
                            }
                        }
                    }
                }
                //
                LoadColorControl.UpdateColor();
            }));
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_add_color_Click(object sender, RoutedEventArgs e)
        {
            AddNewColorWindow newcolor = new AddNewColorWindow(string.Empty);
            if (newcolor.ShowDialog().Value)
            {
                if (LoadProject.ColorConfiguration != null )
                {
                    if (!ControlColor.CheckColor(newcolor.NewNameColor))
                    {
                        LoadProject.ColorConfiguration.ColorValues.Add(new ValueColor() { Name = newcolor.NewNameColor , Value = new ColorN()});
                        LoadProject.ColorConfiguration.ColorNames.Add(newcolor.NewNameColor);
                        ComBoxColorNames.Items.Refresh();
                        if (ComBoxColorNames.Items.Count > 0)
                            ComBoxColorNames.SelectedIndex = ComBoxColorNames.Items.Count - 1;
                        //
                        if (AddColor != null)
                            AddColor();
                    }
                    else
                        MessageBox.Show("Повторение названий цветов недопустимо !!!");
                }
            }
        }

        private void button_remove_color_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ComBoxColorNames.Text))
            {
                if (LoadProject.ColorConfiguration != null)
                {
                    if (ControlColor.CheckColor(ComBoxColorNames.SelectedItem.ToString()))
                    {
                        string oldname = ComBoxColorNames.Text;
                        LoadProject.ColorConfiguration.ColorValues.Remove(ControlColor.FindColor(ComBoxColorNames.Text));
                        LoadProject.ColorConfiguration.ColorNames.Remove(ComBoxColorNames.Text);
                        ComBoxColorNames.Items.Refresh();
                        if (LoadProject.ColorConfiguration.ColorValues.Count > 0)
                            ComBoxColorNames.SelectedIndex = 0;
                        else
                        {
                            ExsampleValueColor.Text = string.Empty;
                            ComBoxColorNames.Text = string.Empty;
                            ExsampleColorPanel.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                            //
                            if (RemoveColor != null)
                                RemoveColor(oldname);
                        }
                    }
                }
            }
        }

        private void button_update_color_Click(object sender, RoutedEventArgs e)
        {
            if (ControlColor.CheckColor(ComBoxColorNames.Text))
            {
                AddNewColorWindow newcolor = new AddNewColorWindow( ComBoxColorNames.Text);
                if (newcolor.ShowDialog().Value)
                {
                    if (ComBoxColorNames.Text != newcolor.NewNameColor)
                    {
                        ColorN color = ControlColor.GetColor(ComBoxColorNames.Text);
                        LoadProject.ColorConfiguration.ColorValues.Remove(ControlColor.FindColor(ComBoxColorNames.Text));
                        LoadProject.ColorConfiguration.ColorNames.Remove(ComBoxColorNames.Text);
                        //
                        LoadProject.ColorConfiguration.ColorValues.Add(new ValueColor() { Name = newcolor.NewNameColor, Value = color });
                        LoadProject.ColorConfiguration.ColorNames.Add(newcolor.NewNameColor);
                        if (UpdateColorName != null)
                            UpdateColorName(ComBoxColorNames.Text, newcolor.NewNameColor);
                        ComBoxColorNames.Items.Refresh();
                        ComBoxColorNames.Text = newcolor.NewNameColor;
                    }
                }
            }
        }

        private void ComBoxColorNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComBoxColorNames.SelectedItem != null)
            {
                if (ControlColor.CheckColor(ComBoxColorNames.SelectedItem.ToString()))
                {
                    ColorN color = ControlColor.GetColor(ComBoxColorNames.SelectedItem.ToString());
                    ExsampleValueColor.Text = GetValuColor(color.R, color.G, color.B);
                    ExsampleColorPanel.Background = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
                }
            }
        }

        public static string GetValuColor(byte R, byte G, byte B)
        {
            return string.Format("{0}, {1}, {2}", R, G, B);
        }

        private void ExsampleColorPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(ComBoxColorNames.Text))
            {
                if (ControlColor.CheckColor(ComBoxColorNames.Text))
                {
                    ColorN color = ControlColor.GetColor(ComBoxColorNames.Text);
                    System.Windows.Forms.ColorDialog colordialog = new System.Windows.Forms.ColorDialog();
                    colordialog.Color = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
                    if (colordialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ControlColor.FindColor(ComBoxColorNames.Text).Value.R = colordialog.Color.R;
                        ControlColor.FindColor(ComBoxColorNames.Text).Value.G = colordialog.Color.G;
                        ControlColor.FindColor(ComBoxColorNames.Text).Value.B = colordialog.Color.B;
                        ExsampleValueColor.Text = GetValuColor(colordialog.Color.R, colordialog.Color.G, colordialog.Color.B);
                        ExsampleColorPanel.Background = new SolidColorBrush(Color.FromRgb(colordialog.Color.R, colordialog.Color.G, colordialog.Color.B));
                        //
                        if (UpdateColorValue != null)
                            UpdateColorValue(ComBoxColorNames.Text);
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Visibility = System.Windows.Visibility.Hidden;
            ShowInTaskbar = false;
            if (!YesClose)
                e.Cancel = true;
        }
    }
}
