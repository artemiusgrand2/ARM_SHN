using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Move;

namespace TrafficTrain
{
    public partial class MatchTrainsForm : Form
    {
        #region Variable
        /// <summary>
        /// Идентификатор поезда слева
        /// </summary>
        public int TrainIdnLeft { get; set; }
        /// <summary>
        /// Идентификатор поезда справа
        /// </summary>
        public int TrainIdnRight { get; set; }
        #endregion
        public MatchTrainsForm(List<t_train> trains_left, List<t_train> trains_right, string info_left, string info_right)
        {
            InitializeComponent();
            label_left.Text = info_left;
            label_right.Text = info_right;
            Start(trains_left, trains_right);
        }

        private void Start(List<t_train> trains_left, List<t_train> trains_right)
        {
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
       ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            FullTrains(trains_left, trains_right);
        }

        private void FullTrains(List<t_train> trains_left, List<t_train> trains_right)
        {
            try
            {
                foreach (t_train train in trains_left)
                    trains_table_left.Rows.Add(new object[] {false, train.TrainNumberString, train.TimeEvent,  GetInfoTrains(train), train.TrainIndex});
                //
                foreach (t_train train in trains_right)
                    trains_table_right.Rows.Add(new object[] { false, train.TrainNumberString, train.TimeEvent, GetInfoTrains(train), train.TrainIndex});
                //
                if (trains_table_left.Rows.Count == 1 && trains_table_right.Rows.Count == 1)
                {
                    trains_table_left.Rows[0].Cells[0].Value = true;
                    trains_table_right.Rows[0].Cells[0].Value = true;
                    TrainIdnLeft = trains_left[trains_left.Count - 1].TrainIndex;
                    TrainIdnRight = trains_right[trains_right.Count - 1].TrainIndex;
                }
            }
            catch (Exception error)
            {
            }
        }

        private string GetInfoTrains(t_train train)
        {
            //если поезд поехал на перегон
            if (train.EventNumber == 3)
                return string.Format("Отправление с {0} пути", train.TrackName);
            //если прибыл на станцию
            if (train.EventNumber == 1 || train.EventNumber == 2)
                return string.Format("Прибытие на {0} путь", train.TrackName);
            //
            return string.Empty;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {

        }

        private void trains_table_left_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int buffer = 0;
            if (int.TryParse(trains_table_left.Rows[trains_table_left.SelectedCells[0].RowIndex].Cells[4].Value.ToString(), out buffer))
            {
                TrainIdnLeft = int.Parse(trains_table_left.Rows[trains_table_left.SelectedCells[0].RowIndex].Cells[4].Value.ToString());
            }
            //
            SelectOne(trains_table_left);
        }

        private void trains_table_right_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int buffer = 0;
            if (int.TryParse(trains_table_right.Rows[trains_table_right.SelectedCells[0].RowIndex].Cells[4].Value.ToString(), out buffer))
            {
                TrainIdnRight = int.Parse(trains_table_right.Rows[trains_table_right.SelectedCells[0].RowIndex].Cells[4].Value.ToString());
            }
            //
            SelectOne(trains_table_right);
        }

        private void SelectOne(DataGridView table)
        {
            foreach (DataGridViewRow row in table.Rows)
            {
                if ((bool)row.Cells[0].Value)
                {
                    row.Cells[0].Value = false;
                }
            }
        }

    }
}
