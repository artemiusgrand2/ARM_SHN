using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Move;
using sdm.diagnostic_section_model.client_impulses;

namespace TrafficTrain
{
    public delegate void UpdateInfo();

    public partial class JournalMessageDiagnostik : Form
    {
         /// <summary>
        /// толщина рамки
        /// </summary>
        int _strokethickness = 4;
        /// <summary>
        /// произошли изменения в коллекции собщений
        /// </summary>
        public static event UpdateInfo UpdateInfo;

        public JournalMessageDiagnostik()
        {
            InitializeComponent();
            //
            Start();
        }

        private void Start()
        {
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
       ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            //
            tablemessagediagnostic.Location = new Point(_strokethickness, _strokethickness);
            tablemessagediagnostic.Size = new Size(Width - _strokethickness * 2, tablemessagediagnostic.Size.Height);
            Fulltable();
            //сообщаем есть ли новые сообщения по диагностике
            MainWindow.NewDiagnostic += Info;
        }

        private void JournalMessageDiagnostic_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, Height));
        }

        private void UpdateDiagnosticCollection()
        {
            tablemessagediagnostic.Rows.Clear();
            Fulltable();
        }

        private void Fulltable()
        {
            foreach (SoundMessage message in LoadProject.JournalDiagnosticMessage)
            {
                tablemessagediagnostic.Rows.Add(new object[] { message.Time, message.Message });
                if (!message.OpenMessage)
                {
                    tablemessagediagnostic.Rows[tablemessagediagnostic.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }


        private void deletemessage()
        {
            foreach (DataGridViewRow row in tablemessagediagnostic.SelectedRows)
            {
                tablemessagediagnostic.Rows.Remove(row);
                //
                int index_row = FindMessageDiagnostic((DateTime)row.Cells[0].Value);
                if (index_row != -1)
                {
                    lock (LoadProject.JournalDiagnosticMessage)
                    {
                       LoadProject.JournalDiagnosticMessage.RemoveAt(index_row);
                    }
                }
                 
            }
            //
        }

        private int FindMessageDiagnostic(DateTime time)
        {
            if (time != null)
            {
                int index = 0;
                foreach (SoundMessage message in LoadProject.JournalDiagnosticMessage)
                {
                    if (time == message.Time)
                        return index;
                    index++;
                }
            }
            return -1;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            foreach (SoundMessage message in LoadProject.JournalDiagnosticMessage)
            {
                if (!message.OpenMessage)
                    message.OpenMessage = true;
            }
            //
            if (UpdateInfo != null)
                UpdateInfo();
            //
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tablemessagesound_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (tablemessagediagnostic.SelectedRows.Count > 0)
                {
                    deletemessage();
                    //
                    if (UpdateInfo != null)
                        UpdateInfo();
                }
            }
        }

        private void JournalMessageSound_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainWindow.NewDiagnostic -= Info;
        }

   
        private void Info()
        {
            Diagnoctic();
        }

        private void Diagnoctic()
        {
            UpdateDiagnosticCollection();
        }

    }
}
