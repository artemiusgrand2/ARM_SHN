using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TrafficTrain.Impulsesver.Client;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;
using TrafficTrain.Enums;
using SCADA.Common.SaveElement;
using log4net;

namespace TrafficTrain
{
    /// <summary>
    /// Детальный вид станции
    /// </summary>
    public partial class DetailViewStation : UserControl
    {
        #region Variable
        /// <summary>
        /// загрузка проекта по станции
        /// </summary>
        LoadProject _load = null;
        /// <summary>
        /// логирование
        /// </summary>
        readonly ILog log = LogManager.GetLogger(typeof(ViewStations));
        /// <summary>
        /// текущая выбранная станция
        /// </summary>
        public int CurrentStation { get; set; }
        /// <summary>
        /// настройки окна
        /// </summary>
        SettingsWindow settingswindow = null;

        #endregion

        public DetailViewStation(double X, double Y, double height, double width,  CommandButton infoarea)
        {
            InitializeComponent();
            //
            settingswindow = new SettingsWindow(this, DrawCanvas, ViewWindow.detailview);
            Margin = new Thickness(X, Y, 0, 0);
            Width = width;
            Height = height;
            Background = SettingsWindow.m_colorfon;
        }

        public void UpdateStation(StrageProject grafic)
        {
            CurrentStation = grafic.CurrentStation;
            settingswindow.Dispose();
            StartStationUpdate(grafic);
            settingswindow.SizeStation();
        }

        private void StartStationUpdate(StrageProject grafic)
        {
            //_load = new LoadProject();
            ////_load.UpdateStation(DrawCanvas, grafic);
            //settingswindow.UpdateDann(_load.ContentHelp, null, null);
        }

    }
}
