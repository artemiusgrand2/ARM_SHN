using System;
using System.Collections.Generic;
using System.Configuration;
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
        public static Configuration LoadConfiguration { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //if (CheckPhysicalAddress())
            //{
                string nameproject = "ARMGraficDNC";
                if (e.Args.Length == 2)
                {
                    if (File.Exists(e.Args[0]))
                    {
                        LoadConfiguration = ConfigurationManager.OpenExeConfiguration(e.Args[0]);
                        nameproject = e.Args[1];
                    }
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
