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
    public partial class UpdateRemarka : Form
    {

        #region Переменные и свойства
        int _x = 0;
        int _y = 0;
        string _remarka = string.Empty;
        public string Remarka
        {
            get
            {
                return _remarka;
            }
        }
        #endregion

        public UpdateRemarka(string remarka)
        {
            InitializeComponent();
            textBox_remarka.Text = remarka;
            _remarka = remarka;
            //
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
         ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void UpdateRemarka_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, Height));
        }

        private void UpdateRemarka_MouseDown(object sender, MouseEventArgs e)
        {
            _x = PointToScreen(e.Location).X;
            _y = PointToScreen(e.Location).Y;
        }

        private void UpdateRemarka_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                Location = new Point(Location.X + (PointToScreen(e.Location).X - _x), Location.Y + (PointToScreen(e.Location).Y - _y));
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            _remarka = textBox_remarka.Text;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
