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
    public partial class ChoicePathArrival : Form
    {
        #region Variable
        /// <summary>
        /// индекс выбраного поезда
        /// </summary>
        public int SelectNumber { get; set; }
        int _x = 0;
        int _y = 0;
        /// <summary>
        /// толщина рамки
        /// </summary>
        int _strokethickness = 4;
        #endregion

        public ChoicePathArrival(List<Move.t_train> trains)
        {
            InitializeComponent();
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
              ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            //
            if (trains.Count >= 1 && trains.Count <= 10)
                CountAddRow(trains.Count - 1);
            else
                CountAddRow(9);
            //   
            tablecommand.Location = new Point(_strokethickness, _strokethickness);
            //
            Opacity = 1;
            int numbertrain = 1;
            //заполнение данными
            foreach (Move.t_train train in trains)
            {
                tablecommand.Rows.Add(new object[] {numbertrain, train.TrackName, train.DObjectName, train.TimeEvent, train.TrainNumberString });
                numbertrain++;
            }
            //
            Cursor.Position = new Point(Location.X + Width / 2, Location.Y + Height / 2);
            SelectNumber = -1;
        }

        private void CountAddRow(int count) 
        {
            int deltaheight = count * tablecommand.ColumnHeadersHeight;
            tablecommand.Size = new Size(Width- _strokethickness*2, tablecommand.Size.Height + deltaheight);
            Size = new Size(Size.Width, Size.Height + deltaheight);
            button_ok.Location = new Point(button_ok.Location.X, button_ok.Location.Y + deltaheight);
            button_cancel.Location = new Point(button_cancel.Location.X, button_cancel.Location.Y + deltaheight);
        }

        private void tablecommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SelectNumber = -1;
                Close();
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (tablecommand.SelectedRows.Count == 1)
                SelectNumber = tablecommand.SelectedRows[tablecommand.SelectedRows.Count-1].Index;
            else SelectNumber = -1;
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            SelectNumber = -1;
            Close();
        }

        private void ChoicePathArrival_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, _strokethickness), new Rectangle(0, 0, Width, Height));
        }

        private void ChoicePathArrival_MouseDown(object sender, MouseEventArgs e)
        {
            _x = PointToScreen(e.Location).X;
            _y = PointToScreen(e.Location).Y;
        }

        private void ChoicePathArrival_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                Location = new Point(Location.X + (PointToScreen(e.Location).X - _x), Location.Y + (PointToScreen(e.Location).Y - _y));
        }

    }
}
