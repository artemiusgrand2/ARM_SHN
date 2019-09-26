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
    public partial class EnterTrainNumber : Form
    {
        #region Variable
        string _prefix = string.Empty;
        /// <summary>
        /// префикс
        /// </summary>
        public string Prefix { get { return _prefix; } set { _prefix = value; } }
        string _sufix = string.Empty;
        /// <summary>
        /// суффикс
        /// </summary>
        public string Sufix { get { return _sufix; } set { _sufix = value; } }
        string _numbertrain = string.Empty;
        /// <summary>
        /// номер поезда
        /// </summary>
        public string NumberTrain { get { return _numbertrain; } set { _numbertrain = value; } }
        string _stationname;
        /// <summary>
        /// номер соседней станции 
        /// </summary>
        public string NameStation { get { return _stationname; } }
        string _stationnameend;
        /// <summary>
        /// номер станции назначения
        /// </summary>
        public string NameStationEnd { get { return _stationnameend; } }
        string _remarka = string.Empty;
        /// <summary>
        /// Пояснения
        /// </summary>
        public string Remarka { get { return _remarka; } set { _remarka = value; } }
        string _n = string.Empty;
        string _number = string.Empty;
        int _x = 0;
        int _y = 0;
        #endregion

        public EnterTrainNumber(bool remarka, string message)
        {
            InitializeComponent();
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
              ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);

            if (remarka)
            {
                label_remarka.Visible = true;
                textBox_remarka.Visible = true;
                label_namestation.Text = "Номер станции назначения";
                comboBox_namestation.Visible = false;
                textBox_namestationend.Visible = true;
                Width += 10;
                button_ok.Location = new Point(button_ok.Location.X+10, button_ok.Location.Y);
                button_cancel.Location = new Point(button_cancel.Location.X+10, button_cancel.Location.Y);
            }
            //
            textBox_remarka.Text = message;
            Start();
        }

        private void Start()
        {
            Opacity = 1;
            Cursor.Position = new Point(Location.X + Width / 2, Location.Y + Height / 2);
            foreach (KeyValuePair<string, string> val in LoadProject.NamesStations)
                comboBox_namestation.Items.Add(val.Key);
            //
            if (comboBox_namestation.Items.Count > 0)
            {
                comboBox_namestation.Text = comboBox_namestation.Items[comboBox_namestation.Items.Count - 1].ToString();
                _stationname = comboBox_namestation.Items[comboBox_namestation.Items.Count - 1].ToString();
            }
        }

        private void EnterTrainNumber_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, Height));
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_numbertrain_TextChanged(object sender, EventArgs e)
        {
            if (textBox_numbertrain.Focused)
            {
                int numberprefix = textBox_numbertrain.Text.IndexOf(Prefix);
                string buffer = string.Empty;

                if (numberprefix != -1)
                {
                   buffer = textBox_numbertrain.Text.Remove(numberprefix, Prefix.Length);
                   int numbersufix = buffer.IndexOf(Sufix);
                   if (numbersufix != -1)
                   {
                       buffer = buffer.Remove(numbersufix, Sufix.Length);
                       if (Formating(buffer)&& buffer.Length <=4)
                       {
                           _numbertrain = buffer;
                           textBox_numbertrain.Text = string.Format("{0}{1}{2}", _prefix, _numbertrain, _sufix);
                       }
                       else
                           textBox_numbertrain.Text = string.Format("{0}{1}{2}", _prefix, _numbertrain, _sufix);
                   }
                   else
                       textBox_numbertrain.Text = string.Format("{0}{1}{2}", _prefix, _numbertrain, _sufix);
                }
                else
                    textBox_numbertrain.Text = string.Format("{0}{1}{2}", _prefix, _numbertrain, _sufix);
                //
                textBox_numbertrain.SelectionStart = _prefix.Length + _numbertrain.Length;
            }
        }

        private void textBox_N_TextChanged(object sender, EventArgs e)
        {
            if (Formating(textBox_N.Text))
            {
                int indexN = Sufix.LastIndexOf(checkBox_N.Text);
                if (indexN != -1 && indexN != Sufix.LastIndexOf(checkBox_NBP.Text))
                {
                    Sufix = Sufix.Remove(indexN + 1, _n.Length);
                    //
                    _n = textBox_N.Text;
                    Sufix = Sufix.Insert(indexN + 1, _n);
                    textBox_numbertrain.Text = string.Format("{0}{1}{2}", _prefix, _numbertrain, _sufix);
                }
            }
            else
                textBox_N.Text = _n;
        }

        private void checkBox_DR_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_DR.Checked, checkBox_DR.Text, ref _sufix);
        }

        private void checkBox_N_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_N.Checked)
                textBox_N.Enabled = true;
            else
            {
                textBox_N.Text = string.Empty;
                textBox_N.Enabled = false;    
            }
            //
            UpdateNumber(checkBox_N.Checked, checkBox_N.Text,ref _sufix);
        }

        private void checkBox_Y_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_Y.Checked, checkBox_Y.Text, ref _sufix);
        }

        private void checkBox_T_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_T.Checked, checkBox_T.Text, ref _sufix);
        }

        private void checkBox_E_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_E.Checked, checkBox_E.Text, ref _sufix);
        }

        private void checkBox_BM_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_BM.Checked, checkBox_BM.Text, ref _sufix);
        }

        private void checkBox_M_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_M.Checked, checkBox_M.Text, ref _sufix);
        }

        private void checkBox_D_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_D.Checked, checkBox_D.Text, ref _sufix);
        }

        private void checkBox_O_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_O.Checked, checkBox_O.Text,ref _prefix);
        }

        private void checkBox_PB_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_PB.Checked, checkBox_PB.Text, ref _sufix);
        }

        private void checkBox_PD_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_PD.Checked, checkBox_PD.Text, ref _sufix);
        }

        private void checkBox_SP_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_SP.Checked, checkBox_SP.Text, ref _sufix);
        }

        private void checkBox_NBP_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNumber(checkBox_NBP.Checked, checkBox_NBP.Text,ref _prefix);
        }

        private void UpdateNumber(bool check, string textinsert,ref string textall)
        {
            if (check)
            {
                textall += textinsert;
                textBox_numbertrain.Text = string.Format("{0}{1}{2}", _prefix, _numbertrain, _sufix);
            }
            else
            {
                if (textall.IndexOf(textinsert) != -1)
                {
                    textall = textall.Remove(textall.IndexOf(textinsert), textinsert.Length);
                    textBox_numbertrain.Text = string.Format("{0}{1}{2}", _prefix, _numbertrain, _sufix);
                }
            }
        }

        private bool Formating(string text)
        {
            int buffer = 0;
            if(int.TryParse(text, out buffer) || string.IsNullOrEmpty(text))
                return true;
            return false;
        }


        private void comboBox_namestation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_namestation.SelectedIndex != -1)
                _stationname = comboBox_namestation.Items[comboBox_namestation.SelectedIndex].ToString();
        }

        private void textBox_numbertrain_Click(object sender, EventArgs e)
        {
            textBox_numbertrain.SelectionStart = _prefix.Length + _numbertrain.Length;
        }


        private void EnterTrainNumber_MouseDown(object sender, MouseEventArgs e)
        {
            _x = PointToScreen(e.Location).X;
            _y = PointToScreen(e.Location).Y;
        }

        private void EnterTrainNumber_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                Location = new Point(Location.X + (PointToScreen(e.Location).X - _x), Location.Y + (PointToScreen(e.Location).Y - _y));
            //
        }

        private void textBox_remarka_TextChanged(object sender, EventArgs e)
        {
            _remarka = textBox_remarka.Text;
        }

        private void textBox_namestationend_TextChanged(object sender, EventArgs e)
        {
            int buffer = 0;
            if (int.TryParse(textBox_namestationend.Text, out buffer))
            {
                if (int.Parse(textBox_namestationend.Text) > 0)
                {
                    _stationnameend = textBox_namestationend.Text;
                }
                else
                {
                    MessageBox.Show("Номер станции должен быть положительным числом !!!");
                }
            }
            else
            {
                MessageBox.Show("Номер станции должен быть целым положительным числом !!!");
            }
        }

    }
}
