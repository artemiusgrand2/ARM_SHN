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

    public partial class ControlColorElement : UserControl
    {
        public ControlColorElement(string name, List<ControlColorStatus> controls)
        {
            InitializeComponent();
            NameElement.Header = name;
            //
            foreach (ControlColorStatus el in controls)
            {
                PanelStatus.Children.Add(el);
                PanelStatus.Children.Add(new Label() { Content = "  "});
            }
        }

        public WrapPanel GetPanel()
        {
            return PanelStatus;
        }
    }
}
