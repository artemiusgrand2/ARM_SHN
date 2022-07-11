using System;
using System.Text;
using System.Windows;
using System.IO;
using System.Configuration;
using SCADA.Common.SaveElement;
using TrafficTrain.WorkWindow;
using TrafficTrain.Enums;
using TrafficTrain.Constant;
using TrafficTrain.Interface;
using log4net;

namespace TrafficTrain
{
    public partial class MainWindow : Window
    {
        #region Переменные и свойства
        /// <summary>
        /// Флаг закрытия программы
        /// </summary>
        public static bool CloseProg { get; set; }
        /// <summary>
        /// программа запущена под провами администратора или нет
        /// </summary>
        public static bool Admin { get; set; }
        /// <summary>
        /// логирование
        /// </summary>
        readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        /// <summary>
        /// элемент вывода справочной информации
        /// </summary>
        public static CommandButton AreaInfo { get; set; }
       
        Connections connections = null;
        /// <summary>
        /// соединение с источниками данных
        /// </summary>
        public Connections Connections
        {
            get
            {
                return connections;
            }
        }

        SettingsWindow settingswindow = null;

        /// <summary>
        /// текущая выбранная станция
        /// </summary>
        public int CurrentStation { get; set; }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            if(App.Close)
                Start();
        }

        /// <summary>
        /// старт программы
        /// </summary>
        private void Start()
        {
            //настройки
            StartSettings();
            //подключения
            StartConnection();
        }

        private void StartSettings()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    MessageBox.Show("Произошла ошибка. Подробности в лог файле");
                    log.Error(string.Format("Unhadled exception. {0}", eventArgs.ExceptionObject));
                    Environment.Exit(-1);
                };
                //Конфигурируем логер
                log4net.Config.XmlConfigurator.Configure(new FileInfo(App.Configuration["file_log_configuration"]));
                log.Info("Программа запущена!!!");
                //загрузка цветовой палитры
                LoadProject.LoadColorAll();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        /// <summary>
        /// запуск сервера импульсов и поездов
        /// </summary>
        private void StartConnection()
        {
            try
            {
                System.Threading.Thread potok_headband = new System.Threading.Thread(StartHeadband);
                potok_headband.SetApartmentState(System.Threading.ApartmentState.STA);
                potok_headband.Start();
                //инициализируем подключения
                connections = new Connections();
                if (App.Configuration["users"] != null && !string.IsNullOrEmpty(App.Configuration["users"]))
                {
                    if (App.Configuration["users"] == ConstName.admin)
                        Admin = true;
                }
                //загружаем проект
                var loadproject = new LoadProject();
                loadproject.Load(DrawCanvas, this);
                AreaInfo = LoadProject.ContentHelp;
                settingswindow = new SettingsWindow(this, DrawCanvas, LoadProject.ContentHelp /*loadproject.ContentHelp*/, LoadProject.AreaMessage, loadproject.DetailStation, ViewWindow.mainwindow);
                //проверяем показывать ли меню
                ScroollView(settingswindow);
                if (Admin)
                {
                    SettingsWindow.WindowColor = new WindowColor();
                    SettingsWindow.WindowColor.Show();
                }
                //отрываем подключения
                connections.Start();
                DataServer.Core.Start();
                Connections.NewSecond += SetChiefScreen;
            }
            catch (Exception error)
            {
                log.Error(error.Message, error);
                Connections.Close();
            }
        }

        private void ScroollView(SettingsWindow settingswindow)
        {
            //масштаб при сохранении
            double buffer = 0;
            double scroll_save = 0;
            if (Admin)
            {
                if (!string.IsNullOrEmpty(App.Configuration["scroll_save"]) && double.TryParse(App.Configuration["scroll_save"], out buffer))
                    scroll_save = Math.Abs(double.Parse(App.Configuration["scroll_save"]));
            }
            else
            {
                if (!string.IsNullOrEmpty(App.Configuration["scroll_main"]) && double.TryParse(App.Configuration["scroll_main"], out buffer))
                    scroll_save = Math.Abs(double.Parse(App.Configuration["scroll_main"]));
            }
            //
            if (scroll_save >= 0.5 && scroll_save <= 99.5)
            {
                settingswindow.ScrollMinusMain = (1 - scroll_save / 100);
                settingswindow.ScrollPlusMain = 1 / settingswindow.ScrollMinusMain;
            }
        }

        private void SetChiefScreen()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if ((DateTime.Now - SettingsWindow.LastActive).TotalSeconds > LoadProject.ReturnTime && LoadProject.CurrentStation != LoadProject.CodeFirst)
                {
                    LoadProject.UpdateStation(LoadProject.CodeFirst, string.Empty);
                }
            }));
        }

        /// <summary>
        /// запускаем заставку
        /// </summary>
        private void StartHeadband()
        {
            StartWpf form = new StartWpf(this);
            form.ShowDialog();
        }

        public void CloseAll()
        {
            try
            {
                connections.Stop();
                DataServer.Core.Stop();
                if (SettingsWindow.WindowStation != null)
                    SettingsWindow.WindowStation.Close();
                if (SettingsWindow.WindowColor != null)
                {
                    SettingsWindow.WindowColor.YesClose = true;
                    SettingsWindow.WindowColor.Close();
                }
                //
                log.Info("Программа закрыта");
                CloseProg = true;
            }
            catch (Exception error)
            {
                log.Error(error.Message, error);
                CloseProg = true;
            }
        }

        public  void AddNewMessage(string str)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (combox_exsample.Items.Count >= 35)
                    combox_exsample.Items.RemoveAt(0);
                combox_exsample.Items.Add(str);
            }
                )); ;
        }

        public void UpdateStation(StrageProject grafic)
        {
            //CurrentStation = grafic.CurrentStation;
            //settingswindow.Dispose();
            //var load = new LoadProject();
            //load.Load(DrawCanvas, grafic);
            //settingswindow.UpdateDann(load.ContentHelp, load.AreaMessage, load.DetailStation);
        }

    }
}
