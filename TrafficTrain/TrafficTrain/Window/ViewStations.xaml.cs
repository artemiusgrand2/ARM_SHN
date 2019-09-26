using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;
using TrafficTrain.Enums;
using SCADA.Common.SaveElement;
using log4net;

namespace TrafficTrain
{
    /// <summary>
    /// Датальный вид станции
    /// </summary>
    public partial class ViewStations : Window, IDisposable
    {

        #region Variable
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

        public ViewStations(StrageProject grafic)
        {
            InitializeComponent();
            StartStation(grafic);
        }

        public void Dispose() {  }

        private void StartStation(StrageProject movesettings)
        {
            border.Background = SettingsWindow.m_colorfon;
            settingswindow = new SettingsWindow(this, DrawCanvas, ViewWindow.otherwindow);
            UpdateStation(movesettings);
        }

        private void StartStationUpdate(StrageProject grafic)
        {
          //  var load = new LoadProject();
          ////  load.UpdateStation(DrawCanvas,  grafic);
          //  settingswindow.UpdateDann(load.ContentHelp, null, null);
          //  //показываем окно
          //  Show();
        }

        public void UpdateStation(StrageProject grafic)
        {
            Activate();
            CurrentStation = grafic.CurrentStation;
            settingswindow.Dispose();
            StartStationUpdate(grafic);
            settingswindow.SizeStation();
        }

    }
}
