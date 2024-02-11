using System;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using ARM_SHN.WorkWindow;

namespace ARM_SHN
{
    public partial class StartWpf : Window
    {
        #region Переменные и свойства
        /// <summary>
        /// текущее время ожидания
        /// </summary>
        int current_time = 0;
        /// <summary>
        /// максимальное время ожидания в секундах
        /// </summary>
        int maxinterval = 7;
        /// <summary>
        /// таймер нештатного запуска
        /// </summary>
        System.Timers.Timer _timer_close = new System.Timers.Timer(1000);
        /// <summary>
        /// основное окно программы
        /// </summary>
        Window window;
        /// <summary>
        /// цвет текста
        /// </summary>
        Brush _colortext = new RadialGradientBrush(Color.FromArgb(255, 255, 0, 0), Color.FromArgb(127, 160, 0, 0));
        #endregion

        public StartWpf(Window winone)
        {
            InitializeComponent();
            //
            label_apm.Foreground = _colortext;
            label_apm.FontSize = label_apm.FontSize * System.Windows.SystemParameters.CaretWidth;
            label_progress.FontSize = label_progress.FontSize * System.Windows.SystemParameters.CaretWidth;
            //
            _timer_close.Elapsed += TaktTime_Elapsed;
            _timer_close.Start();
            //
            window = winone;
            LoadProject.LoadInfo += LoadData_LoadInfo;
            Connections.LoadInfo += MainWindow_LoadInfo;
        }


        /// <summary>
        /// генерируем такты секунд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaktTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            current_time++;
            if (current_time >= maxinterval)
            {
                _timer_close.Stop();
                MainWindow_LoadInfo("End");
            }
        }

        /// <summary>
        /// информация о загрузке графики и перегонов
        /// </summary>
        /// <param name="infoload">инфо</param>
        private void LoadData_LoadInfo(string infoload)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    label_progress.Content = infoload;
                }
                catch { }
            }
             ));
        }

        /// <summary>
        /// информация о подключении к серверам
        /// </summary>
        /// <param name="infoload"></param>
        private void MainWindow_LoadInfo(string infoload)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    if (infoload != "End")
                    {
                        label_progress.Content = infoload;
                    }
                    else
                    {
                        _timer_close.Stop();
                        Close();
                    }
                }
                catch { }
            }
         ));
            //активация основного потока
            window.Dispatcher.Invoke(
               new Action(() =>
               {
                   try
                   {
                       if (infoload == "End")
                       {
                           window.Visibility = System.Windows.Visibility.Visible;
                           window.Activate();
                       }
                   }
                   catch { }
               }
               )
               );
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadProject.LoadInfo -= LoadData_LoadInfo;
            Connections.LoadInfo -= MainWindow_LoadInfo;
        }
    }
}
