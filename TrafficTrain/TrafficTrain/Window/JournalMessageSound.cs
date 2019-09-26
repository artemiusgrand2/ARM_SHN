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
    public partial class JournalMessageSound : Form
    {
        /// <summary>
        /// толщина рамки
        /// </summary>
        int _strokethickness = 4;
        /// <summary>
        /// произошли изменения в коллекции собщений
        /// </summary>
        public static event UpdateInfo UpdateInfo;

        public JournalMessageSound()
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
            tablemessagesound.Location = new Point(_strokethickness, _strokethickness);
            tablemessagesound.Size = new Size(Width - _strokethickness * 2, tablemessagesound.Size.Height);
            Fulltable();
            LightElementControl.UpdateCollectionSound += UpdateSoundCollection;
        }

        private void JournalMessageSound_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, Height));
        }

        private void UpdateSoundCollection()
        {
            tablemessagesound.Rows.Clear();
            Fulltable();
        }

        private void Fulltable()
        {
            foreach (SoundMessage message in LoadProject.JournalSoundMessage)
            {
                tablemessagesound.Rows.Add(new object[] { message.Time, message.Message });
                if (!message.OpenMessage)
                {
                    tablemessagesound.Rows[tablemessagesound.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }


        private void deletemessage()
        {
            foreach (DataGridViewRow row in tablemessagesound.SelectedRows)
            {
                tablemessagesound.Rows.Remove(row);
                //
                int index_row = FindMessageSound((DateTime)row.Cells[0].Value);
                if (index_row != -1)
                {
                    lock (LoadProject.JournalSoundMessage)
                    {
                        LoadProject.JournalSoundMessage.RemoveAt(index_row);
                    }
                }
                 
            }
            //
        }

        private int FindMessageSound(DateTime time)
        {
            if (time != null)
            {
                int index = 0;
                foreach (SoundMessage message in LoadProject.JournalSoundMessage)
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
            foreach (SoundMessage message in LoadProject.JournalSoundMessage)
            {
                if (!message.OpenMessage)
                    message.OpenMessage = true;
            }
            //
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
                if (tablemessagesound.SelectedRows.Count > 0)
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
            LightElementControl.UpdateCollectionSound -= UpdateSoundCollection;
        }

    }
}
