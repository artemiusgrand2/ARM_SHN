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

namespace TrafficTrain
{

    public partial class ControlColorStatus : UserControl
    {
        public ControlColorStatus(string Name, List<ControlColor> colors)
        {
            InitializeComponent();
            NameStatus.Text = Name;
            NameStatus.ToolTip = Name;
            //
            foreach (ControlColor color in colors)
            {
                color.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                PanelColor.Children.Add(color);
                PanelColor.Children.Add(new Label() { Content = " "});
            }
        }

        public WrapPanel GetPanel()
        {
            return PanelColor;
        }
    }
}
