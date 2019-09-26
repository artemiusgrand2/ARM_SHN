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
    public partial class CommandsToPerform : Form
    {
        #region Переменные и свойства
        int _x = 0;
        int _y = 0;
        #endregion

        public CommandsToPerform(List<SettingsCommand> command)
        {
            InitializeComponent();
            //
            if (command.Count >= 1 && command.Count <= 10)
                CountAddRow(command.Count - 1);
            //
            if (command.Count > 10)
                CountAddRow(9);
            FullTable(command);
            //
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
           ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            //
            button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void CountAddRow(int count)
        {
            int deltaheight = count * tablecommand.ColumnHeadersHeight;
            tablecommand.Size = new Size(tablecommand.Size.Width, tablecommand.Size.Height + deltaheight);
            Size = new Size(Size.Width, Size.Height + deltaheight);
            button_ok.Location = new Point(button_ok.Location.X, button_ok.Location.Y + deltaheight);
            button_cancel.Location = new Point(button_cancel.Location.X, button_cancel.Location.Y + deltaheight);
        }

        private void FullTable(List<SettingsCommand> command)
        {
            foreach (SettingsCommand com in command)
            {
                object[] row = new object[] {tablecommand.Rows.Count +1, com.Command,FindNameStation(com.StationNumber.ToString()), com.NameCommand };
                tablecommand.Rows.Add(row);
            }
        }

        public static string FindNameStation(string stationnumber)
        {
            foreach (KeyValuePair<string, string> val in LoadProject.NamesStations)
            {
                if (val.Value == stationnumber)
                    return val.Key;
            }
            //
            return stationnumber;
        }

        private void CommandsToPerform_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, Height));
        }

        private void button_ok_Click(object sender, EventArgs e)
        {

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {

        }

        private void CommandsToPerform_MouseDown(object sender, MouseEventArgs e)
        {
            _x = PointToScreen(e.Location).X;
            _y = PointToScreen(e.Location).Y;
        }

        private void CommandsToPerform_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                Location = new Point(Location.X + (PointToScreen(e.Location).X - _x), Location.Y + (PointToScreen(e.Location).Y - _y));
        }

        private void tablecommand_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, tablecommand.Height));
        }
    }
}
