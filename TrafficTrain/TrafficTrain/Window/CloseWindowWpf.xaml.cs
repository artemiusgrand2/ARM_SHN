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
using System.Timers;

namespace TrafficTrain
{
    /// <summary>
    /// Interaction logic for CloseWindowWpf.xaml
    /// </summary>
    public partial class CloseWindowWpf : Window
    {
        #region Variable
        Timer tim;
        #endregion
        public CloseWindowWpf()
        {
            InitializeComponent();
            //
            tim = new Timer(100);
            tim.Elapsed += tim_Tick;
            tim.Start();
        }

        private void tim_Tick(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
               {
                   if (MainWindow.CloseProg)
                   {
                       CloseWindow();
                   }
               }));
        }

        private void CloseWindow()
        {
            tim.Stop();
            tim.Close();
            Close();
        }
    }
}
