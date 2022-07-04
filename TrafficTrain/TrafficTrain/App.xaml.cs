using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.Net.NetworkInformation;
using System.Windows;
using System.IO;

namespace TrafficTrain
{
    
    public partial class App : Application
    {
        //запуск одной копии приложения
        System.Threading.Mutex MutexnewProcess;
        //mac адрес карты для сравнения
        private static string m_PhysicalAddress = "001CF0C7FC7F";
        public static bool Close = true;
        public static NameValueCollection Configuration { get; set; } = ConfigurationManager.AppSettings;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //if (CheckPhysicalAddress())
            //{
            var nameproject = "ARMGraficDNC";
            if (e.Args.Length > 0)
            {
                if (File.Exists(e.Args[0]))
                {
                    var fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = e.Args[0];
                    var loadConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    Configuration = new NameValueCollection();
                    for (var i = 0; i < loadConfig.AppSettings.Settings.AllKeys.Length; i++)
                        Configuration.Add(loadConfig.AppSettings.Settings.AllKeys[i], loadConfig.AppSettings.Settings[loadConfig.AppSettings.Settings.AllKeys[i]].Value);
                }
            }
            //
            if(Configuration.Count == 0)
            {
                MessageBox.Show("Не найдена конфигурация", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Close = false;
                Shutdown();
            }
            //Проверяем на открытие второй копии программы
            bool createdNew;
            MutexnewProcess = new System.Threading.Mutex(true, nameproject, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("Программа уже запущена !!!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Close = false;
                Shutdown();
            }
            //}
            //else
            //{
            //    Close = false;
            //    Shutdown();
            //}
        }

        private bool CheckPhysicalAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                var adress =  adapter.GetPhysicalAddress().ToString();
                if (m_PhysicalAddress == adress)
                {
                    return true;
                }
            }
            //
            return false;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
