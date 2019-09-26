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
    public partial class ShowTrainStation : Form
    {
        #region Variable
        int _x = 0;
        int _y = 0;
        /// <summary>
        /// толщина рамки
        /// </summary>
        int _strokethickness = 4;
        #endregion

        public ShowTrainStation(int Stationnumber)
        {
            InitializeComponent();
            Start(Stationnumber);
        }

        private void Start(int Stationnumber)
        {
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
       ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            //
            trains_station_table.Location = new Point(_strokethickness, _strokethickness);
            trains_station_table.Size = new Size(Width - _strokethickness * 2, trains_station_table.Size.Height);
            FullTrains(Stationnumber);
        }

        private void FullTrains(int StationNumber)
        {
            try
            {
                List<t_train> trains = new List<t_train>();
                foreach (t_train train in MainWindow.TrainsDraw)
                {
                    //if (MainWindow.FilterMapTrain)
                    //{
                        if (train.StationNumber == StationNumber && (train.EventNumber == 1 || train.EventNumber == 2) && train.Views)
                        {
                            if (MainWindow.FilterMapTrain)
                            {
                                if (!train.Filter)
                                    trains.Add(train);
                            }
                            else trains.Add(train);
                        }
                    //}
                    //else
                    //{

                    //}
                }
                //сортируем по времени
                trains.Sort((x, y) =>
                {
                    if (x.TimeEvent > y.TimeEvent) return 1;
                    if (x.TimeEvent < y.TimeEvent) return -1;
                    return 0;
                });
                //
                foreach (t_train train in trains)
                    trains_station_table.Rows.Add(new object[] { train.TrainNumberString, train.TimeEvent, string.Format("Прибыл на путь {0}", train.TrackName) });
            }
            catch(Exception error) 
            {
            }
        }


        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowTrainStation_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown, _strokethickness), new Rectangle(0, 0, Width, Height));
        }
    }
}
