using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using sdm.diagnostic_section_model;
using sdm.diagnostic_section_model.client_impulses;
using Move;

namespace TrafficTrain
{
    public partial class DiagnosticForm : Form
    {
        public DiagnosticForm()
        {
            InitializeComponent();
            //
            Start();
        }

        private void Start()
        {
            InfoServerImpuls();
            InfoServerTrain();
             //сообщаем есть ли подключение к импульс серверу 
            ImpulsesClient.ConnectDisconnectionServer += ConnectCloseServer;
            //сообщаем есть ли подключение к серверу поездов
            TrainClient.ConnectDisconnect += ConnectDisconnectNumbertrain;
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
         ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
        }

        private void InfoServerImpuls()
        {
            if (ImpulsesClient.Connect)
            {
                label_imp.Text = "Есть связь с импульс сервером";
                panel_impuls.BackColor = Color.LightGreen;
            }
            else
            {
                label_imp.Text = "Нет связи с импульс сервером";
                panel_impuls.BackColor = Color.Red;
            }
        }

        private void InfoServerTrain()
        {
            if (TrainClient.Connect)
            {
                label_train.Text = "Есть связь с сервером поездов";
                panel_train.BackColor = Color.LightGreen;
            }
            else
            {
                label_train.Text = "Нет связи с сервером поездов";
                panel_train.BackColor = Color.Red;
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            //сообщаем есть ли подключение к импульс серверу 
            ImpulsesClient.ConnectDisconnectionServer -= ConnectCloseServer;
            //сообщаем есть ли подключение к серверу поездов
            TrainClient.ConnectDisconnect -= ConnectDisconnectNumbertrain;
            Close();
        }

        private void DiagnosticForm_Paint(object sender, PaintEventArgs e)
        {
           // e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width, Height));
        }

        /// <summary>
        /// Реагируем на подключение к серверу импульсов
        /// </summary>
        private void ConnectCloseServer()
        {
            Invoke(new Action(() => ServerClose()));
        }

        private void ServerClose()
        {
            InfoServerImpuls();
        }

        /// <summary>
        /// Реагируем на подключение к серверу номеров поездов
        /// </summary>
        private void ConnectDisconnectNumbertrain()
        {
            Invoke(new Action(() => ServerCloseNumberTrain()));
        }

        /// <summary>
        /// реагируем на отключение от сервера номеров поездов
        /// </summary>
        private void ServerCloseNumberTrain()
        {
            InfoServerTrain();
        }
    }
}
