using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace TrafficTrain
{
    public partial class ShowMessageTrain : Form
    {
        #region Variable
        int _x = 0;
        int _y = 0;
        /// <summary>
        /// толщина рамки
        /// </summary>
        int _strokethickness = 4;
        #endregion

        public ShowMessageTrain(int id)
        {
            InitializeComponent();
            //
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
              ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            textBox_message.BackColor = BackColor;
            //
            SelectMessageTrain.InfoMessage += ProcessingMessage;
            //получаем справку по поезду
            using (SelectMessageTrain trainselect = new SelectMessageTrain())
            {
                trainselect.FindMessage(id);
            }
        }

        private void ProcessingMessage(string message)
        {
            InfoMessageTrain infotrain = new InfoMessageTrain(MessageUpdate);
            Invoke(infotrain, new object[] { message});
        }

        private void MessageUpdate(string textmessage)
        {
            if (!string.IsNullOrEmpty(textmessage))
                textBox_message.Text = textmessage;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowMessageTrain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SelectMessageTrain.InfoMessage -= ProcessingMessage;
        }

        private void ShowMessageTrain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, _strokethickness), new Rectangle(0, 0, Width, Height));
        }

        private void ShowMessageTrain_MouseDown(object sender, MouseEventArgs e)
        {
            _x = PointToScreen(e.Location).X;
            _y = PointToScreen(e.Location).Y;
        }

        private void ShowMessageTrain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                Location = new Point(Location.X + (PointToScreen(e.Location).X - _x), Location.Y + (PointToScreen(e.Location).Y - _y));
        }
    }
}
