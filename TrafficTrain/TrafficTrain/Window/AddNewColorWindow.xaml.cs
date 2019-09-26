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
using System.Windows.Shapes;

namespace TrafficTrain
{
    public partial class AddNewColorWindow : Window
    {
        /// <summary>
        /// новое название цвета
        /// </summary>
        public string NewNameColor { get; set; }

        public AddNewColorWindow(string name)
        {
            InitializeComponent();
            Width /= System.Windows.SystemParameters.CaretWidth;
            Height /= System.Windows.SystemParameters.CaretWidth;
            text_box_new_name.Text = name;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewNameColor))
                DialogResult = true;
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void text_box_new_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewNameColor = text_box_new_name.Text;
        }
    }
}
