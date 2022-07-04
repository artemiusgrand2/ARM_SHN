using System;
using System.Configuration;
using System.Timers;

using TrafficTrain.Delegate;
using SCADA.Common.ImpulsClient;

namespace TrafficTrain.WorkWindow
{
    public delegate void NewTaktEvent();

   public  class Connections
    {
        #region Variable

        bool _start = false;
        /// <summary>
        /// переменная показывающая какой сейчас такт
        /// </summary>
        public static bool Taktupdate { get; set; }
        /// <summary>
        /// такты фазы времени мигания
        /// </summary>
        public static event NewTaktEvent NewTart;
        /// <summary>
        /// секунды времени
        /// </summary>
        public static event NewTaktEvent NewSecond;
        static ImpulsesClientTCP  _client;
        /// <summary>
        /// класс для работы с сервером импульсов
        /// </summary>
        public static ImpulsesClientTCP ClientImpulses { get { return _client; } }
        /// <summary>
        /// тактовый таймер для мигания различных элементов
        /// </summary>
        Timer Takt = new Timer(800);
        /// <summary>
        /// тактовый таймер для времени
        /// </summary>
        Timer TaktTime = new Timer(1000);
        /// <summary>
        /// тактовый таймер для обновления цветовой палитры
        /// </summary>
        Timer UpdateScreenColorTime = null;
        /// <summary>
        /// Запущено ли автоматическое изменение стилей
        /// </summary>
        public bool IsStartUpdateStyle
        {
            get
            {
                return UpdateScreenColorTime.Enabled;
            }
        }
        /// <summary>
        /// инфо о ходе загрузки приложения
        /// </summary>
        public static event Info LoadInfo;

        //int index_color = 1;



        #endregion

        public Connections()
        {
            //Настраиваем таймеры
            Takt.Elapsed += Takt_Elapsed;
            TaktTime.Elapsed += TaktTime_Elapsed;
            int buffer;
            int.TryParse(App.Configuration["IntervalUpdatePalitra"], out buffer);
            if (buffer > 0)
                UpdateScreenColorTime = new Timer(buffer * 60000);
            else
                UpdateScreenColorTime = new Timer(60000);
            UpdateScreenColorTime.Elapsed += UpdateScreenTime_Elapsed;
            //сервер импульсов
            if (LoadInfo != null)
                LoadInfo("Подключение к импульс серверу");
            //подключение к импульс серверу
            var config = ServerConfiguration.FromFile(App.Configuration["cfgpath"]);
            _client = new ImpulsesClientTCP(config.Stations, App.Configuration["constring"], App.Configuration["tablespath"], false, LoadProject.TsServiceList);
            if (LoadInfo != null)
                LoadInfo("Подключение к серверу номеров поездов");
        }

        /// <summary>
        /// генерируем такты мигания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Takt_Elapsed(object sender, ElapsedEventArgs e)
        {
            Taktupdate = !Taktupdate;
            if (NewTart != null)
                NewTart();
        }

        /// <summary>
        /// генерируем такты секунд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaktTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (NewSecond != null)
                NewSecond();
            //
            FinishLoad();
        }

        /// <summary>
        /// генерируем такты секунд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateScreenTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            int index_color = GetIndexStyle();
            int index = 0;
            try
            {
                foreach (string el in LoadProject.CollectionStyle.Keys)
                {
                    if (index == index_color)
                    {
                        LoadProject.ColorConfiguration = LoadProject.CollectionStyle[el];
                        LoadProject.CurrentNameStyle = el;
                        if (SettingsWindow.WindowColor != null)
                            SettingsWindow.WindowColor.UpdatePalitra();
                        else
                            CommandColor.SetAllColor();
                        break;
                    }
                    index++;
                }
            }
            catch { }
        }

        private int GetIndexStyle()
        {
            int index = 0;
            foreach (string el in LoadProject.CollectionStyle.Keys)
            {
                if (LoadProject.CurrentNameStyle == el)
                    break;
                index++;
            }
            //
            if ((index + 1) >= LoadProject.CollectionStyle.Keys.Count)
                index = 0;
            else index++;
            //
            return index;
        }

        public void Start()
        {
            //Старт связи с cервером импульсов
            _client.Start();
            //запускаем обработку мигцания
            // Takt.Start();
            //запускаем обработку часов
            TaktTime.Start();
        }

        public void Stop()
        {
            if (_client != null)
                _client.Stop();
            if (Takt != null)
                Takt.Stop();
            if (TaktTime != null)
                TaktTime.Stop();
            if (UpdateScreenColorTime != null)
                UpdateScreenColorTime.Stop();
        }

        public void StartStyle()
        {
            if (UpdateScreenColorTime != null)
                UpdateScreenColorTime.Start();
        }

        public void StopStyle()
        {
            if (UpdateScreenColorTime != null)
                UpdateScreenColorTime.Stop();
        }

        public void StartTs()
        {
            _client.Start();
        }

        public void StopTs()
        {
            _client.Stop();
        }

        public static void Close()
        {
            if (LoadInfo != null)
                LoadInfo("End");
        }

        private void FinishLoad()
        {
            if (!_start)
            {
                _start = true;
                if (LoadInfo != null)
                    LoadInfo("End");
            }
        }
    }
}
