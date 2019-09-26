using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrafficTrain
{
    public partial class ClosingWindow : Form
    {
        Timer tim;

        public ClosingWindow()
        {
            InitializeComponent();
            tim = new Timer();
            tim.Interval = 100;
            tim.Tick+=tim_Tick;
            tim.Start();
        }

        private void tim_Tick(object sender, EventArgs e)
        {
            if (MainWindow.CloseProg)
            {
                tim.Stop();
                Close();
            }
        }
    }
}
