﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.Net.NetworkInformation;
using System.Windows;
using System.IO;
using RW.KTC.ORPO.Berezina.Common.Helpers;
using System.Linq;

namespace ARM_SHN
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
            //
            if (Configuration.AllKeys.Contains("M"))
            {
                if (!PhysicalAddressHelper.CheckPhysicalAddresses(Configuration.GetValues("M")[0]))
                {
                    MessageBox.Show("Нет лицензии для использования программного обеспечения. Обратитесь к производителю.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close = false;
                    Shutdown();
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
