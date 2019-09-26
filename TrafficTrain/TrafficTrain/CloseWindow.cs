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
    public partial class CloseWindow : Form
    {
        public CloseWindow()
        {
            InitializeComponent();
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
                ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
        }

        private void CloseWindow_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, Height));
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_CANCEL_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
