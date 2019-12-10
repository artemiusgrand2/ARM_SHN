﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Configuration;
using System.Windows.Media;
using System.Windows;
using System.Threading;

using TrafficTrain.Enums;
using TrafficTrain.Constant;
using TrafficTrain.Interface;
using TrafficTrain.WorkWindow;
using TrafficTrain.Delegate;
using TrafficTrain.DataProject;
using TrafficTrain.DataGrafik;
using TrafficTrain.DataServer;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.Strage.SaveElement;
using SCADA.Common.LogicalParse;
using SCADA.Common.Constant;
using log4net;

namespace TrafficTrain
{

    /// <summary>
    /// загружаем данные из конфирурации на диске 
    /// </summary>
     class LoadProject
    {
        #region Переменные и свойства

         private IList<CommandButton> filters_number_train = new List<CommandButton>();

         public IList<CommandButton> FiltersNumberTrain
         {
             get
             {
                 return filters_number_train;
             }
         }
        /// <summary>
        /// область вывода справочной информации
        /// </summary>
        public static  CommandButton ContentHelp {get;set;}
        /// <summary>
        /// набор ТС импульсов по каждой станции
        /// </summary>
        public static Dictionary<int, StationTableTs> TsList = new Dictionary<int, StationTableTs>();
        /// <summary>
        /// набор ТС служебных импульсов по каждой станции
        /// </summary>
        public static Dictionary<int, StationTableServiceTs> TsServiceList = new Dictionary<int, StationTableServiceTs>();
        /// <summary>
        /// набор ТУ импульсов по каждой станции
        /// </summary>
        public static Dictionary<int, StationTableTu> TuList = new Dictionary<int, StationTableTu>();
        /// <summary>
        /// журнал звуковых сообщений
        /// </summary>
        public static List<MessageInfo> JournalSoundMessage = new List<MessageInfo>();
        /// <summary>
        /// журнал сообщений диагностики
        /// </summary>
        public static List<MessageInfo> JournalDiagnosticMessage = new List<MessageInfo>();
        /// <summary>
        /// инфо о ходе загрузки
        /// </summary>
        public static event Info LoadInfo;

        static readonly ILog m_log = LogManager.GetLogger(typeof(LoadProject));
        /// <summary>
        /// логирование
        /// </summary>
        public static ILog Log
        {
            get
            {
                return m_log;
            }
        }
        /// <summary>
        /// проект графики текущий вид
        /// </summary>
        public static SCADA.Common.SaveElement.StrageProject ProejctGrafic;

        private static int m_code_first = 0;
         /// <summary>
         /// код станции главного вида
         /// </summary>
        public static int CodeFirst
        {
            get
            {
                return m_code_first;
            }
        }
        /// <summary>
        /// номер текущей станции
        /// </summary>
        public static int CurrentStation { get; set; }
        /// <summary>
        /// проект детальной графики по станциям
        /// </summary>
        public static Dictionary<int, SCADA.Common.SaveElement.StrageProject> ProejctViewStations = new Dictionary<int, SCADA.Common.SaveElement.StrageProject>();

        private static Dictionary<int, IList<UIElement>> m_elementView = new Dictionary<int, IList<UIElement>>();
        /// <summary>
        /// коллеция графический объектов по станциям
        /// </summary>
        public static Dictionary<int, IList<UIElement>> ElementsView
        {
            get
            {
                return m_elementView;
            }
        }

        private static IList<IIndicationEl> m_indications = new List<IIndicationEl>();
        /// <summary>
        /// Элементы с индикацией
        /// </summary>
        public static IList<IIndicationEl> Indications
        {
            get
            {
                return m_indications;
            }
        }
        /// <summary>
        /// проект цвета текущий (полный)
        /// </summary>
        public static ColorConfiguration ColorConfiguration { get; set; }
        /// <summary>
        /// текущее название стиля
        /// </summary>
        public static string CurrentNameStyle {get;set;}
        /// <summary>
        /// проект стилей цвета
        /// </summary>
        public static Dictionary<string, ColorConfiguration> CollectionStyle = new Dictionary<string, ColorConfiguration>();


        private static TextBox m_areaMessage = null;

        /// <summary>
        /// Область нахождения справки
        /// </summary>
        public static TextBox AreaMessage
        {
            get
            {
                return m_areaMessage;
            }
        }

        private DetailViewStation m_detailStation = null;

        /// <summary>
        /// Область детального вида
        /// </summary>
        public DetailViewStation DetailStation
        {
            get
            {
                return m_detailStation;
            }
        }

        private static uint m_returnTime = 600;
        /// <summary>
        /// Максимальный интервал ожидания активности пользователя в секундах
        /// </summary>
        public static uint ReturnTime
        {
            get
            {
                return m_returnTime;
            }
        }

        private static IList<string> m_messages = new List<string>();
        /// <summary>
        /// максимальное количество диагностических сообщений
        /// </summary>
        private static uint m_countMessages = 100;

        private static string m_DirDiagnostics = @".\Messages";

        private static string m_FileDiagnostics;

        private static bool isWriteDiagnostics = false;

        private static string patternLog = @"^\s*([0-9]+)\s*.\s*([0-9]+)\s*.\s*([0-9]+)\s* \s*([0-9]+)\s*:\s*([0-9]+)\s*:\s*([0-9]+).\s*(.+)$";


        private static IList<AnalogCell> m_analogGrafic = new List<AnalogCell>();
        /// <summary>
        /// перечень графических объектов для вывода аналоговых данных
        /// </summary>
        public static IList<AnalogCell> AnalogGrafic
        {
            get
            {
                return m_analogGrafic;
            }
        }

        private static Object thisLock = new Object();
        /// <summary>
        /// список повторяющихся сообщений по названиям объектов
        /// </summary>
        private static IDictionary<int, IDictionary<string, IDictionary<Viewmode, IList<string>>>> m_messagesRepeat = new Dictionary<int, IDictionary<string, IDictionary<Viewmode, IList<string>>>>();

        private IDictionary<string, FieldType> m_nameTypesFiled = new Dictionary<string, FieldType>() { { "float", FieldType.floatType }, { "int", FieldType.intType } };

        #endregion


        public static void AddMessages(IList<string> messages)
        {
            if (m_areaMessage != null)
            {
                var update = false;
                foreach (var message in messages)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        m_messages.Add(message);
                        //
                        if (m_messages.Count > m_countMessages)
                            m_messages.RemoveAt(0);
                        //записываем сообщение
                        if (isWriteDiagnostics)
                            lock (thisLock)
                            {
                                //(new AddMessage(() =>
                                //{
                                    File.AppendAllText(m_FileDiagnostics, message + Environment.NewLine, Encoding.UTF8);
                              //  })).BeginInvoke(null, null);
                            }
                        update = true;
                    }
                }
                //
                if (update)
                {
                    var result = new StringBuilder();
                    foreach (var message in m_messages)
                        result.Append(message);
                    //
                    m_areaMessage.Text = result.ToString(0, (result.Length > 0) ? result.Length - 1 : result.Length);
                    m_areaMessage.ScrollToEnd();
                }
            }
        }

        public static string CreateMessages(string message, DateTime start, DateTime end)
        {
            var startStr = GetDateTimeStr(start);
            var endStr =(end != DateTime.MinValue) ? GetDateTimeStr(end) :string.Empty;
            //
            return string.Format("{0}{1}. {2}{3}", startStr, (!string.IsNullOrEmpty(endStr)) ? string.Format(" - {0}", endStr) : endStr, message, Environment.NewLine);
        }

        public static string GetDateTimeStr(DateTime dateTime)
        {
            return string.Format("{0:D2}.{1:D2}.{2:D4} {3:D2}:{4:D2}:{5:D2}", dateTime.Day, dateTime.Month, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        private string GetListStringNamesFieldType()
        {
            var result = string.Empty;
            var index =0;
            foreach (var type in m_nameTypesFiled)
            {
                result += (index != m_nameTypesFiled.Count - 1) ? (type.Key + ", ") : type.Key;
                index++;
            }
            //
            return result;
        }

        /// <summary>
        /// загружаем файл графики  проекта 
        /// </summary>
        /// <param name="projectmodel">путь к файлу</param>
        private SCADA.Common.SaveElement.StrageProject LoadGrafickProject(string filename, string namedesing)
        {
            try
            {
                if (!string.IsNullOrEmpty(filename))
                {
                    if (new FileInfo(filename).Exists)
                    {
                        SCADA.Common.SaveElement.StrageProject Project = new SCADA.Common.SaveElement.StrageProject();
                        using (var reader = new XmlTextReader(filename))
                        {
                            var deserializer = new XmlSerializer(typeof(SCADA.Common.SaveElement.StrageProject));
                            Project = (SCADA.Common.SaveElement.StrageProject)deserializer.Deserialize(reader);
                            //находим коэффициент масштаба
                            if (Project.Scroll <= 0)
                                Project.Scroll = 1;
                            //
                            reader.Close();
                            m_log.Info(string.Format("{0} успешно загружена !!!", namedesing));
                            return Project;
                        }
                    }
                    else
                    {
                        m_log.Error(string.Format("Файла графики по адресу {0} - не существует", filename));
                        return null;
                    }
                }
                else
                {
                    m_log.Error(string.Format("Введите в файле конфигурации путь к проекту {0}", namedesing));
                    return null;
                }
            }
            catch (Exception error)
            {
                m_log.Error(string.Format("Ошибка {0}, в файле {1} !!!", error.Message, namedesing));
                return null;
            }
        }

        /// <summary>
        /// загружаем расскраску
        /// </summary>
        private static ColorConfiguration LoadColor(string colorfile, string namestyle)
        {
            //проверяем информацию по импульсам телесигнализации
            try
            {
                ColorConfiguration Proejct = new ColorConfiguration();
                using (var reader = new XmlTextReader(colorfile))
                {
                    var deserializer = new XmlSerializer(typeof(ColorConfiguration));
                    Proejct = (ColorConfiguration)deserializer.Deserialize(reader);
                    Proejct.Name = namestyle;
                    Proejct.File = colorfile;
                    reader.Close();
                    m_log.Info(string.Format("Cтиль {0} успешно загружен!!!", namestyle));
                    return Proejct;
                }
            }
            catch (Exception error)
            {
                m_log.Error(string.Format("Ошибка в файле стилей {0}", colorfile));
                m_log.Error(error.Message);
                return null;
            }
        }

        /// <summary>
        /// закружаем коллекцию стилей
        /// </summary>
        /// <returns></returns>
        private static void LoadStyles()
        {
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["file_styles"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["file_styles"]).Exists)
                    {
                        List<string> spisokstyle = GetStrLineFileRead(ConfigurationManager.AppSettings["file_styles"], Encoding.UTF8);
                        foreach (string st in spisokstyle)
                        {
                            try
                            {
                                string[] file = st.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                if (file.Length == 2)
                                {
                                    if (new FileInfo(file[1]).Exists)
                                    {
                                        ColorConfiguration proejct = LoadColor(file[1], file[0]);
                                       try
                                       {
                                           if(proejct!=null)
                                           {
                                               proejct.Name = file[0];
                                               proejct.File = file[1];
                                               //
                                               CollectionStyle.Add(file[0], proejct);
                                               //
                                               if (ColorConfiguration == null)
                                               {
                                                   ColorConfiguration = proejct;
                                                   CurrentNameStyle = file[0];
                                               }
                                           }
                                       }
                                       catch
                                       {
                                           m_log.Error(string.Format("Стиль c названием {0} уже существует !!!", file[0], file[1]));
                                       }
                                        
                                    }
                                    else
                                        m_log.Error(string.Format("Файла с cтиля c названем {0} по адресу {1} - не существует", file[0], file[1]));
                                }
                            }
                            catch (Exception error)
                            {
                                m_log.Error(error.Message);
                            }
                        }
                        //
                        m_log.Info("Стили загружены");
                    }
                    else
                        m_log.Error(string.Format("Файла с перечнем путей с цветовым коллекциям по адресу {0} - не существует", ConfigurationManager.AppSettings["file_styles"]));
                }
                else
                    m_log.Error(string.Format("Введите в файле конфигурации путь к перечню файлов стилей , обозначенного {0}", "<<file_styles>>"));
            }
            catch (Exception error)
            {
                m_log.Error(error.Message);
            }
        }

        private static Point NewScalePosition(Point center, double scale,Point pointold)
        {
            return new Point((pointold.X * scale + center.X),(pointold.Y * scale + center.Y));
        }

        private static void SetSettingsBeforeSave(BaseSave el, Point center, double scale)
        {
            foreach (Figure fig in el.Figures)
            {
                fig.StartPoint = NewScalePosition(center, scale, fig.StartPoint);
                //
                foreach (Segment seg in fig.Segments)
                {
                    seg.Point = NewScalePosition(center, scale, seg.Point);
                    seg.RadiusX *= scale;
                    seg.RadiusY *= scale;
                }
            }
        }

        /// <summary>
        /// анализируем объекты перед сохранением
        /// </summary>
        public static SCADA.Common.SaveElement.StrageProject SaveAnalis(Point center, double scale, double widthtrainramka, double heightrainramka)
        {
            //находим  коэффициент масштаба
            try
            {
                if (ProejctGrafic.Scroll > 0)
                    ProejctGrafic.Scroll *= scale;
                else ProejctGrafic.Scroll = 1;

                ProejctGrafic.WightTrainInfo = widthtrainramka;
                ProejctGrafic.HeightTrainInfo = heightrainramka;
            }
            catch(Exception error) 
            {
                m_log.Error(error.Message, error);
            }
            //анализируем сигнал

            foreach (BaseSave el in ProejctGrafic.GraficObjects)
            {
                try
                {
                    foreach (Figure fig in el.Figures)
                    {
                        fig.StartPoint = NewScalePosition(center, scale, fig.StartPoint);
                        //
                        foreach (Segment seg in fig.Segments)
                        {
                            seg.Point = NewScalePosition(center, scale, seg.Point);
                            seg.RadiusX *= scale;
                            seg.RadiusY *= scale;
                        }
                    }
                    switch (el.ViewElement)
                    {
                        case ViewElement.chiefroad:
                            {
                                RoadStation bigpath = el as RoadStation;
                                bigpath.TextSize *= scale;
                                bigpath.Xinsert = bigpath.Xinsert * scale + center.X;
                                bigpath.Yinsert = bigpath.Yinsert * scale + center.Y;
                            }
                            break;
                        case ViewElement.time:
                            {
                                TimeSave time = el as TimeSave;
                                time.FontSize *= scale;
                                time.Height *= scale;
                                time.Width *= scale;
                                time.Left = time.Left * scale + center.X;
                                time.Top = time.Top * scale + center.Y;
                            }
                            break;
                        case ViewElement.buttoncommand:
                            {
                                ButtonCommandSave commandbutton = el as ButtonCommandSave;
                                commandbutton.TextSize *= scale;
                                commandbutton.Xinsert = commandbutton.Xinsert * scale + center.X;
                                commandbutton.Yinsert = commandbutton.Yinsert * scale + center.Y;
                            }
                            break;
                        case ViewElement.namestation:
                            {
                                NameStationSave namestation = el as NameStationSave;
                                namestation.FontSize *= scale;
                                namestation.Height *= scale;
                                namestation.Width *= scale;
                                namestation.Left = namestation.Left * scale + center.X;
                                namestation.Top = namestation.Top * scale + center.Y;
                            }
                            break;
                        case ViewElement.help_element:
                            {
                                TextHelpSave helptext = el as TextHelpSave;
                                helptext.FontSize *= scale;
                                helptext.Height *= scale;
                                helptext.Width *= scale;
                                helptext.Left = helptext.Left * scale + center.X;
                                helptext.Top = helptext.Top * scale + center.Y;
                            }
                            break;
                    }
                }
                catch (Exception error)
                {
                    m_log.Error(error.Message, error);
                }

            }
            //
            return ProejctGrafic;
        }

        private string Trim(List<char> trim, List<char> massiv)
        {
            for(int i = 0;i < massiv.Count;i++)
            {
                foreach (char tr in trim)
                {
                    if (massiv[i] == tr)
                    {
                        massiv.RemoveAt(i);
                        i--;
                    }
                }
            }

            string answer = string.Empty;
            foreach (char ch in massiv)
            {
                answer += ch.ToString();
            }

            return answer ;
        }


        private static List<string> GetStrLineFileRead(string filename, Encoding coding)
        {
            List<string> file_ts = new List<string>();
            try
            {
                using (StreamReader strReader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), coding))
                {
                    string str;
                    while ((str = strReader.ReadLine()) != null)
                        file_ts.Add(str);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return file_ts;
            }
            //
            return file_ts;
        }

        /// <summary>
        /// загрузка таблиц ТУ  и ТС
        /// </summary>
        private void LoadImpulsInformation()
        {
            //проверяем информацию по импульсам телесигнализации
            try
            {

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["filestationTS"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["filestationTS"]).Exists)
                    {

                        List<string> spisokstation = GetStrLineFileRead(ConfigurationManager.AppSettings["filestationTS"], Encoding.UTF8);
                        foreach (string st in spisokstation)
                        {
                            try
                            {
                                int index =1;
                                string[] files = st.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                int buffer = 0;
                                if (files.Length == 2 && int.TryParse(files[0], out buffer))
                                {
                                    int current_numberstation = int.Parse(files[0]);
                                    if (new FileInfo(files[1]).Exists)
                                    {
                                        List<string> fileTsinfo = GetStrLineFileRead(files[1], Encoding.GetEncoding(1251));
                                        TsList.Add(current_numberstation, new StationTableTs());
                                        //
                                        foreach (string row in fileTsinfo)
                                        {
                                            try
                                            {
                                                //не смотрим комментарии
                                                if (row.IndexOf("#") == -1)
                                                {
                                                    string[] cells = row.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                    if (cells.Length >= 4 )
                                                    {
                                                        string[] views = cells[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                        string[] names = cells[1].Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                                                        var messageActiv =(cells.Length > 4) ? cells[4].Trim() : string.Empty; 
                                                        var messagePasiv =(cells.Length > 5) ? cells[5].Trim() : string.Empty;
                                                        var messageNotControl = (cells.Length > 6) ? cells[6].Trim() : string.Empty;
                                                       // var format = (cells.Length > 6) ? cells[5].Trim() : string.Empty;
                                                        names = RemoveIsEmpty(names.ToList());
                                                        if (names.Length == 1 && views.Length == 1)
                                                        {
                                                            names = names[names.Length - 1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                            foreach (string name in names)
                                                                RepetitionElement(cells[0].Trim(), name.Trim(), cells[2].Trim(), cells[3].Trim(), current_numberstation, messageActiv, messagePasiv, messageNotControl);
                                                        }
                                                        else
                                                        {
                                                            if (views.Length == names.Length && views.Length > 1)
                                                            {
                                                                for (int i = 0; i < views.Length; i++)
                                                                {
                                                                    string[] peregen = names[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                                    foreach (string name in peregen)
                                                                    {
                                                                        RepetitionElement(views[i].Trim(), name.Trim(), cells[2].Trim(), cells[3].Trim(), current_numberstation, messageActiv, messagePasiv, messageNotControl);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                names = cells[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                                if (views.Length == names.Length && views.Length > 1)
                                                                {
                                                                    for (int i = 0; i < views.Length; i++)
                                                                    {
                                                                        RepetitionElement(views[i].Trim(), names[i].Trim(), cells[2].Trim(), cells[3].Trim(), current_numberstation, messageActiv, messagePasiv, messageNotControl);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (views.Length > 1 && names.Length == 1)
                                                                    {
                                                                        foreach (string view in views)
                                                                            RepetitionElement(view.Trim(), cells[1].Trim(), cells[2].Trim(), cells[3].Trim(), current_numberstation, messageActiv, messagePasiv, messageNotControl);
                                                                    }
                                                                    else if (views.Length == 1 && names.Length > 1)
                                                                    {
                                                                        foreach (string name in names)
                                                                            RepetitionElement(cells[0].Trim(), name.Trim(), cells[2].Trim(), cells[3].Trim(), current_numberstation, messageActiv, messagePasiv, messageNotControl);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception error) { m_log.Error(error.Message, error); }
                                            index++;
                                        }
                                    }
                                    else
                                        m_log.Error(string.Format("Файла с перечнем импульсов ТС станции {0} по адресу {1} - не существует", current_numberstation.ToString(), files[1]));
                                }
                            }
                            catch (Exception error)
                            {
                                m_log.Error(error.Message);
                            }
                        }
                        //
                        m_log.Info("Таблицы импульсов ТС загружены");
                    }
                    else
                        m_log.Error(string.Format("Файла с перечнем импульсов ТС по адресу {0} - не существует", ConfigurationManager.AppSettings["filestationTS"]));
                }
                else
                    m_log.Error(string.Format("Введите в файле конфигурации путь к описанию импульсов ТС, обозначенного {0}", "<<filestationTS>>"));
            }
            catch(Exception error)
            {
                m_log.Error(error.Message);
            }
            //проверяем информацию по импульсам телеуправления
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["filestationTU"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["filestationTU"]).Exists)
                    {
                        List<string> spisokstation = GetStrLineFileRead(ConfigurationManager.AppSettings["filestationTU"], Encoding.UTF8);
                        foreach (string st in spisokstation)
                        {
                            try
                            {
                                string[] files = st.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                int buffer = 0;
                                if (files.Length == 2 && int.TryParse(files[0], out buffer))
                                {
                                    int current_numberstation = int.Parse(files[0]);
                                    if (new FileInfo(files[1]).Exists)
                                    {
                                        List<string> fileTuinfo = GetStrLineFileRead(files[1], Encoding.GetEncoding(1251));
                                        TuList.Add(current_numberstation, new StationTableTu());
                                        //
                                        foreach (string row in fileTuinfo)
                                        {
                                            try
                                            {
                                                string[] cells = row.Split(new string[] { ";" }, StringSplitOptions.None);
                                                if (cells.Length >= 7)
                                                    FillingTableTu(cells[0], cells[1], string.Format("{0}-{1}", cells[2], cells[3]), string.Format("{0}-{1}", cells[4], cells[5]), cells[6], current_numberstation);
                                            }
                                            catch (Exception error) { m_log.Error(error.Message ,error); }
                                        }
                                    }
                                    else
                                        m_log.Error(string.Format("Файла с перечнем импульсов ТУ станции {0} по адресу {1} - не существует", current_numberstation.ToString(), files[1]));
                                }
                            }
                            catch (Exception error)
                            {
                                m_log.Error(error.Message);
                            }
                        }
                        //
                        m_log.Info("Таблицы импульсов ТУ загружены");
                    }
                    else
                        m_log.Error(string.Format("Файла с перечнем импульсов ТУ по адресу {0} - не существует", ConfigurationManager.AppSettings["filestationTU"]));
                }
                else
                    m_log.Error(string.Format("Введите в файле конфигурации путь к описанию импульсов ТУ, обозначенного {0}", "<<filestationTU>>"));
            }
            catch (Exception error)
            {
                m_log.Error(error.Message);
            }
        }

        private string [] RemoveIsEmpty(List<string> massiv)
        {
            //
            for (int i = 0; i < massiv.Count; i++)
            {
                massiv[i] = massiv[i].Trim(new char[] { ' ', ',' });
                if (string.IsNullOrEmpty(massiv[i]))
                {
                    massiv.RemoveAt(i);
                    i--;
                }
            }
            //
            return massiv.ToArray();
        }

        /// <summary>
        /// загрузка детальные виды станций
        /// </summary>
        private void LoadViewStations()
        {
            //проверяем информацию по импульсам телесигнализации
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["view_stations"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["view_stations"]).Exists)
                    {
                        List<string> spisokstation = GetStrLineFileRead(ConfigurationManager.AppSettings["view_stations"], Encoding.GetEncoding(1251));
                        foreach (string st in spisokstation)
                        {
                            try
                            {
                                string[] files = st.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                int buffer = 0;
                                if (files.Length == 2 && int.TryParse(files[0], out buffer))
                                {
                                    int current_numberstation = int.Parse(files[0]);
                                    if (new FileInfo(files[1]).Exists)
                                    {
                                        SCADA.Common.SaveElement.StrageProject project = LoadGrafickProject(files[1], string.Format("Графика станции {0}", current_numberstation));
                                        project.CurrentStation = current_numberstation;
                                        if (project != null)
                                        {
                                            if (!ProejctViewStations.ContainsKey(current_numberstation))
                                                ProejctViewStations.Add(current_numberstation, project);
                                        }

                                    }
                                    else
                                        m_log.Error(string.Format("Файла с детальной графикой станции {0} по адресу {1} - не существует", current_numberstation.ToString(), files[1]));
                                }
                            }
                            catch (Exception error)
                            {
                                m_log.Error(error.Message);
                            }
                        }
                        //
                        m_log.Info("Графики станций загружены");
                    }
                    else
                        m_log.Error(string.Format("Файла с перечнем файлов детальных график станции по адресу {0} - не существует", ConfigurationManager.AppSettings["view_stations"]));
                }
                else
                    m_log.Error(string.Format("Введите в файле конфигурации путь к описанию детальных график станций, обозначенного {0}", "<<view_stations>>"));
            }
            catch (Exception error)
            {
                m_log.Error(error.Message);
            }
        }
        

       /// <summary>
        /// заполнение таблиц ту по каждой станции
       /// </summary>
       /// <param name="numbersost">виды управляющих комманд</param>
       /// <param name="namecommand">название комманды</param>
       /// <param name="startpath">путь задания команды начало</param>
        /// <param name="endpath">путь задания команды окончание</param>
       /// <param name="tu">управляющий импульс</param>
       /// <param name="number_station">номер станции</param>
        private void FillingTableTu(string numbersost, string namecommand, string startpath, string endpath, string tu, int number_station)
        {
            if (TuList.Count > 0)
            {
                switch (numbersost)
                {
                    case ViewNameSostNumberTu.seasonal_management:
                        TuList[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu =tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.yes_departure:
                        TuList[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu = tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.yes_route_setting:
                        TuList[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu = tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.no_departure:
                        TuList[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu = tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.no_route_setting:
                        TuList[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, ViewCommand = numbersost, Tu = tu,});
                        break;
                }
            }
        }
     
        /// <summary>
        /// анализируем данные проекта станции
        /// </summary>
        /// <param name="name">название элемента</param>
        /// <returns></returns>
        private void RepetitionElement(string viewObject, string name, string state, string folmula, int station_number, string messageActiv, string messagePasiv, string messageNotControl)
        {
            if (TsList.ContainsKey(station_number))
            {
                if (NumberContolTS.Views.Contains(viewObject))
                {
                    string sost_name = string.Format("{0}-{1}", viewObject, name);
                    if (!TsList[station_number].NamesValue.ContainsKey(sost_name))
                        TsList[station_number].NamesValue.Add(sost_name, new List<StateValue>());
                    //
                    Viewmode viewTS = Viewmode.none;
                    ViewmodeCommand viewTU = ViewmodeCommand.none;
                    if(viewObject != NumberContolTS.auto_supervisory )
                      viewTS = TrafficTrain.Constant.NameControlTS.GetStateTS(name, state, station_number, m_log);
                    if (viewTS == Viewmode.none && viewObject == NumberContolTS.auto_supervisory)
                        viewTU = NameControlAnalis.GetStateAnalisTU(name, state, station_number, m_log);
                    if (viewTS != Viewmode.none || viewTU != ViewmodeCommand.none)
                    {
                        var newState = new StateValue(folmula, viewTS, viewTU);
                        newState.CheckFormula(string.Format("Объект {0}, станция {1}", name, station_number));
                        if (!string.IsNullOrEmpty(messageActiv))
                            newState.Messages.Add(StatesControl.activ, messageActiv);
                        //
                        if (!string.IsNullOrEmpty(messagePasiv))
                            newState.Messages.Add(StatesControl.pasiv, messagePasiv);
                        if (!string.IsNullOrEmpty(messageNotControl))
                            newState.Messages.Add(StatesControl.nocontrol, messageNotControl);
                        //
                        var findState = TsList[station_number].NamesValue[sost_name].Where(x => x.ViewTS == newState.ViewTS).FirstOrDefault();
                        if (findState == null)
                            TsList[station_number].NamesValue[sost_name].Add(newState);
                        else
                            m_log.Info(string.Format("Для объекта {0} на станции {1} тип контроля - {2} повторяется, текущее значение для контроля- {3}", name, station_number, state, findState.Formula));
                    }
                }
                else
                {
                    m_log.Info(string.Format("Неправильный вид объекта {0} на станции {1}", viewObject, station_number));
                }
            }
        }

        private void SetSettingsObject(IGraficElement element, BaseSave baseSave, Canvas DrawCanvas, IList<UIElement> elements, Visibility visible)
        {
            if (((UIElement)element).Visibility != Visibility.Collapsed)
                ((UIElement)element).Visibility = visible;
            DrawCanvas.Children.Add((UIElement)element);
            elements.Add((UIElement)element);
            element.Notes = baseSave.Notes;
            Canvas.SetZIndex((UIElement)element, baseSave.ZIndex);
            element.ZIntex = baseSave.ZIndex;
            if (!baseSave.IsVisible)
                ((UIElement)element).Visibility = Visibility.Collapsed;
            if (element is IText)
            {
                if (((UIElement)element).Visibility != Visibility.Collapsed)
                    (element as IText).Text.Visibility = visible;
                else
                    (element as IText).Text.Visibility = Visibility.Collapsed;
                DrawCanvas.Children.Add((element as IText).Text);
                elements.Add((element as IText).Text);
               // (element as IText).Text.FontFamily = new FontFamily("Consolas");
                Canvas.SetZIndex((UIElement)(element as IText).Text, baseSave.ZIndex + 1);
            }
            var indicEl = element as IIndicationEl;
            var selectEl = element as ISelectElement;
            if (selectEl != null)
            {
                selectEl.StationControl = baseSave.StationNumber;
                selectEl.StationTransition = baseSave.StationNumberRight;
            }
            //если элемент поддекживает индикацию
            if (indicEl != null && indicEl.Impulses.Count > 0)
            {
                m_indications.Add(element as IIndicationEl);
            }
        }


        private int SortElement(BaseSave x, BaseSave y)
        {

            //if (IndexSort(x) && !IndexSort(y))
            //    return -1;
            //else return 1;
            if (x.ViewElement < y.ViewElement)
                return -1;
            else return 1;
        }

        private bool IndexSort(BaseSave x)
        {
            if (x.ViewElement == ViewElement.namestation || x.ViewElement == ViewElement.buttonstation || x.ViewElement == ViewElement.arrowmove)
            {
                return true;     
            }
            else return false;
        }

        private void GetNameElement(string name, out string textShow, out string nameKey)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var namesKey = name.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                textShow = namesKey[0].Trim();
                nameKey = (namesKey.Length > 1) ? namesKey[1].Trim() : textShow;
            }
            else
            {
                textShow = string.Empty;
                nameKey = string.Empty;
            }
        }

        private bool GetStationAnalog(ref int stationCode)
        {
            foreach (var station in DataServer.Core.Stations)
            {
                foreach (var table in station.Value)
                {
                    var stationStr = string.Format("{0}{1}", station.Key, table.Key);
                    uint stationParse;
                    if (uint.TryParse(stationStr, out stationParse))
                    {
                        if (stationCode == stationParse)
                        {
                            stationCode = station.Key;
                            return true;
                        }
                    }
                }
            }
            //
            return false;
        }

        private bool GetIdItemTable(int stationSource, string data, ref int tableId, ref string item, string nameObject, ref FieldType type, ref int stationResult)
        {
            if (GetStationAnalog(ref stationSource))
            {
                stationResult = stationSource;
                if (DataServer.Core.Stations.ContainsKey(stationSource))
                {
                    var cells = data.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (cells.Length >= 2)
                    {
                        if (int.TryParse(cells[0], out tableId))
                        {
                            if (DataServer.Core.Stations[stationSource].ContainsKey(tableId))
                            {
                                type = GetFieldType(stationSource, tableId);
                                item = data.Replace(cells[0] + ".", string.Empty).Trim();
                                if (DataServer.Core.Stations[stationSource][tableId].TryItem(item))
                                {
                                    return true;
                                }
                                else
                                {
                                    m_log.Error(string.Format("Станция {0} графический объект - {1}, в таблице с tableId = {2} item = {3} не существует", stationSource, nameObject, tableId, item));
                                }
                            }
                            else
                            {
                                m_log.Error(string.Format("Станция {0} графический объект - {1}, таблицы с tableId = {2} не существует", stationSource, nameObject, tableId));
                            }
                        }
                        else
                        {
                            m_log.Error(string.Format("Станция {0} графический объект - {1}, tableId {2} в строке {3} имеет неверный формат", stationSource, nameObject, cells[0], data));
                        }
                    }
                    else
                    {
                        m_log.Error(string.Format("Станция {0} графический объект - {1}, строка {2} имеет неверный формат", stationSource, nameObject, data));
                    }
                }
                else
                {
                    m_log.Error(string.Format("Станции {0} для аналоговых данных не существует, графический объект - {1}", stationSource, nameObject));
                }
            }
            else
            {
                m_log.Error(string.Format("Код станции {0} для аналоговых данных имеет неверный формат, графический объект - {1}", stationSource, nameObject));
            }
            //
            return false;
        }

        /// <summary>
        /// Отрисовываем загруженную графику
        /// </summary>
        /// <param name="Project">занруженная графика</param>
        private IList<UIElement> DrawGrafick(SCADA.Common.SaveElement.StrageProject Project, Canvas DrawCanvas, Visibility visible, Window win)
        {
            var result = new List<UIElement>();
            if (Project != null)
            {
                Project.GraficObjects.Sort(SortElement);
                foreach (BaseSave el in Project.GraficObjects)
                    {
                        try
                        {
                            string textShow,nameKey ;
                            GetNameElement(el.Name, out textShow, out nameKey);
                            //
                            switch (el.ViewElement)
                            {
                                case ViewElement.line:
                                {
                                    LineHelpSave line = el as LineHelpSave;
                                    LineHelp newelement = new LineHelp(GetPathGeometry(el.Figures), line.WeightStroke * SystemParameters.CaretWidth, line.NameColor,
                                                                 nameKey, FullImpulsesElement((int)el.StationNumber, NumberContolTS.activ_line, string.Format("{0}-{1}", NumberContolTS.activ_line, nameKey)), el.IsVisible);
                                    foreach (double step in line.StrokeDashArray)
                                        newelement.StrokeDashArray.Add(step);
                                    SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    if (newelement.StationControl > 0)
                                        newelement.Analis();
                                }
                                    break;
                                case ViewElement.texthelp:
                                    {
                                        TextHelpSave text = el as TextHelpSave;
                                        TextHelp newelement = new TextHelp(GetPathGeometry(el.Figures), text.Left * System.Windows.SystemParameters.CaretWidth,
                                                                        text.Top * System.Windows.SystemParameters.CaretWidth, text.FontSize * System.Windows.SystemParameters.CaretWidth, text.Text);
                                        //
                                        newelement.Text.RenderTransform = new RotateTransform(text.Angle);
                                        newelement.Text.MaxWidth = text.Width * System.Windows.SystemParameters.CaretWidth;
                                        newelement.Text.MaxHeight = text.Height * System.Windows.SystemParameters.CaretWidth;
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.buttonstation:
                                    {
                                        ButtonStation newelement = new ButtonStation(GetPathGeometry(el.Figures), nameKey,
                                                                                      FullImpulsesElement((int)el.StationNumber, NumberContolTS.button_station,
                                                                                      string.Format("{0}-{1}", NumberContolTS.button_station, nameKey)));
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.signal:
                                    {
                                        RouteSignal newelement = new RouteSignal(GetPathGeometry(el.Figures), nameKey,
                                                                                FullImpulsesElement((int)el.StationNumber, NumberContolTS.signal,
                                                                                string.Format("{0}-{1}", NumberContolTS.signal, nameKey)));
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.buttoncommand:
                                    {
                                        ButtonCommandSave command = el as ButtonCommandSave;
                                        CommandButton newelement = new CommandButton(GetPathGeometry(el.Figures), nameKey, command.HelpText,
                                                                                          command.Xinsert * System.Windows.SystemParameters.CaretWidth,
                                                                                          command.Yinsert * System.Windows.SystemParameters.CaretWidth,
                                                                                          command.TextSize * System.Windows.SystemParameters.CaretWidth, command.Angle, command.ViewCommand, command.ViewPanel);
                                       
                                        newelement.Text.FontWeight = FontWeights.Bold;
                                        //выводи объекта на панель
                                        if (command.ViewCommand == ViewCommand.content_help)
                                            command.ZIndex = int.MaxValue - 1;
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible); 
                                        //
                                        switch (command.ViewCommand)
                                        {
                                            case ViewCommand.content_help:
                                                if (ContentHelp == null)
                                                    ContentHelp = newelement;
                                                break;
                                        }
                                        //
                                        if (command.ViewCommand == ViewCommand.viewtrain)
                                            filters_number_train.Add(newelement);
                                    }
                                    break;
                                case ViewElement.ramka:
                                    {
                                        RamkaStation newelement = new RamkaStation(GetPathGeometry(el.Figures));
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.disconnectors:
                                    {
                                        Disconnectors newelement = new Disconnectors(GetPathGeometry(el.Figures), nameKey, FullImpulsesElement((int)el.StationNumber, NumberContolTS.disconnectors,
                                                                                   string.Format("{0}-{1}", NumberContolTS.disconnectors, nameKey)), el.TypeDisconnector);
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.chiefroad:
                                    {
                                        RoadStation track = el as RoadStation;
                                        switch (track.View)
                                        {
                                            case ViewTrack.analogCell:
                                                {
                                                if(el.StationNumber == 14 &&  nameKey== "ЩП.Uh")
                                                {

                                                }
                                                    var rows = FullImpulsesElement((int)el.StationNumber, NumberContolTS.analogCell, string.Format("{0}-{1}", NumberContolTS.analogCell, nameKey));
                                                    switch (rows.Count)
                                                    {
                                                        case 0:
                                                            {
                                                                m_log.Error(string.Format("Стыковка аналоговая янейка '{0}' на станции {1} не описана в проекте.", nameKey, el.StationNumber));
                                                            }
                                                            break;
                                                        case 1:
                                                            {
                                                                var format = string.Empty;
                                                                var factor = string.Empty;
                                                                int tableId = 0;
                                                                string item = string.Empty;
                                                                var type = FieldType.UintType;
                                                                var find = false;
                                                                var station = el.StationNumber;
                                                                //
                                                                foreach (var row in rows)
                                                                {
                                                                    find = GetIdItemTable(el.StationNumber,  row.Value.Impuls, ref tableId, ref item, nameKey, ref type, ref station);
                                                                    format = (row.Value.Messages.ContainsKey(StatesControl.activ)) ? row.Value.Messages[StatesControl.activ] : string.Empty;
                                                                    factor = (row.Value.Messages.ContainsKey(StatesControl.pasiv)) ? row.Value.Messages[StatesControl.pasiv] : string.Empty;
                                                                    if (row.Value.Messages.ContainsKey(StatesControl.nocontrol))
                                                                    {
                                                                        if (m_nameTypesFiled.ContainsKey(row.Value.Messages[StatesControl.nocontrol].ToLower()))
                                                                        {
                                                                            type = m_nameTypesFiled[row.Value.Messages[StatesControl.nocontrol].ToLower()];
                                                                        }
                                                                        else
                                                                        {
                                                                            m_log.Error(string.Format("Неверный тип данных {0} для  аналоговая янейка '{1}' на станции {2} (возможные типы - {3})", row.Value.Messages[StatesControl.nocontrol], nameKey, el.StationNumber, GetListStringNamesFieldType()));
                                                                        }
                                                                    }
                                                                    break;
                                                                }
                                                                //
                                                                var newelement = new AnalogCell(station, GetPathGeometry(el.Figures), nameKey,
                                                                track.Xinsert * System.Windows.SystemParameters.CaretWidth,
                                                                track.Yinsert * System.Windows.SystemParameters.CaretWidth,
                                                                track.TextSize * System.Windows.SystemParameters.CaretWidth, track.Angle, tableId, item, format, factor, type);
                                                                newelement.Text.FontWeight = FontWeights.Bold;
                                                                newelement.IsFind = find;
                                                                m_analogGrafic.Add(newelement);
                                                                //
                                                                SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                                            }
                                                            break;
                                                        default:
                                                            {
                                                                m_log.Error(string.Format("Стыковка аналоговая янейка '{0}' на станции {1} несколько раз описана в проекте.", nameKey, el.StationNumber));
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                            case ViewTrack.helpelement:
                                                {
                                                    var newelement = new HelpElement(GetPathGeometry(el.Figures), textShow, nameKey,
                                             track.Xinsert * System.Windows.SystemParameters.CaretWidth,
                                             track.Yinsert * System.Windows.SystemParameters.CaretWidth,
                                             track.TextSize * System.Windows.SystemParameters.CaretWidth, track.Angle,
                                             FullImpulsesElement((int)el.StationNumber, NumberContolTS.activ_element, string.Format("{0}-{1}", NumberContolTS.activ_element, nameKey)));
                                                    newelement.Text.FontWeight = FontWeights.Bold;
                                                    //
                                                    SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                                }
                                                break;
                                            case ViewTrack.track:
                                                {
                                                    var newelement = new StationPath(GetPathGeometry(el.Figures), textShow, nameKey,
                                                                                     track.Xinsert * System.Windows.SystemParameters.CaretWidth,
                                                                                     track.Yinsert * System.Windows.SystemParameters.CaretWidth,
                                                                                     track.TextSize * System.Windows.SystemParameters.CaretWidth, track.Angle,
                                                                                     AnalisServiceImpulses(FullImpulsesElement((int)el.StationNumber, NumberContolTS.big_path,
                                                                                     string.Format("{0}-{1}", NumberContolTS.big_path, nameKey))));
                                                    newelement.Text.FontWeight = FontWeights.Bold;
                                                    //проверяем есть ли электрофикация пути
                                                    if (newelement.Impulses.ContainsKey(Viewmode.electrification))
                                                        newelement.ViewTraction = ViewTraction.electric_traction;
                                                    //проверяем есть ли на пути пассжирская платформа
                                                    if (newelement.Impulses.ContainsKey(Viewmode.pass))
                                                        newelement.IsPlatform = true;
                                                    SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case ViewElement.namestation:
                                    {
                                        NameStationSave namestation = el as NameStationSave;
                                        NameStation newelement = new NameStation(GetPathGeometry(el.Figures), nameKey,
                                                                                     namestation.Left * System.Windows.SystemParameters.CaretWidth,
                                                                                     namestation.Top * System.Windows.SystemParameters.CaretWidth,
                                                                                     namestation.FontSize * System.Windows.SystemParameters.CaretWidth);
                                        //
                                        newelement.Text.RenderTransform = new RotateTransform(namestation.Angle);
                                        newelement.Text.MaxWidth = namestation.Width * System.Windows.SystemParameters.CaretWidth;
                                        newelement.Text.MaxHeight = namestation.Height * System.Windows.SystemParameters.CaretWidth;
                                        //выводи объекта на панель
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.numbertrain:
                                    {
                                        NumberTrainSave train = el as NumberTrainSave;
                                        NumberTrainRamka newelement;
                                        newelement = new NumberTrainRamka(GetPathGeometry(el.Figures), string.Empty, string.Empty, train.Name, train.RotateText);
                                        //выводи объект на панель
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.lightShunting:
                                    {
                                        LightShunting newelement = new LightShunting(GetPathGeometry(el.Figures), nameKey,
                                                                          FullImpulsesElement((int)el.StationNumber, NumberContolTS.signal, string.Format("{0}-{1}", NumberContolTS.signal, nameKey)));
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible);
                                    }
                                    break;
                                case ViewElement.time:
                                    {
                                        TimeSave time = el as TimeSave;
                                        TimeElement newelement = new TimeElement(GetPathGeometry(el.Figures),
                                                                             time.Left * System.Windows.SystemParameters.CaretWidth,
                                                                             time.Top * System.Windows.SystemParameters.CaretWidth,
                                                                             time.FontSize * System.Windows.SystemParameters.CaretWidth, win);
                                        //
                                        newelement.Text.RenderTransform = new RotateTransform(time.Angle);
                                        newelement.Text.MaxWidth = time.Width * System.Windows.SystemParameters.CaretWidth;
                                        newelement.Text.MaxHeight = time.Height * System.Windows.SystemParameters.CaretWidth;
                                        //
                                        //выводи объекта на панель
                                        SetSettingsObject(newelement, el, DrawCanvas, result, visible); 
                                    }
                                    break;
                                case ViewElement.area:
                                    {
                                        AreaSave area = el as AreaSave;
                                        CreateArea(area, area.View, DrawCanvas, result, visible);
                                    }
                                    break;
                            }
                        }
                        catch (Exception error) { m_log.Error(error.Message); }
                }
            }
            //
            return result;
        }

        private FieldType GetFieldType(int stationCode, int tableId)
        {
            foreach (var station in RW.KTC.ORPO.Berezina.Configuration.ServerConfiguration.Instance.Stations)
            {
                if (station.Code == stationCode)
                {
                    foreach (var table in station.Tables)
                    {
                        if (table.Id == tableId)
                        {
                            try
                            {
                                string value;
                                if (TryField("FieldType", table, out value))
                                {
                                    if (m_nameTypesFiled.ContainsKey(value.ToLower()))
                                    {
                                        return m_nameTypesFiled[value.ToLower()];
                                    }
                                }
                            }
                            catch { }
                            break;
                        }
                    }
                    break;
                }
            }
            //
            return FieldType.UintType;
        }


        private bool TryField(string key, RW.KTC.ORPO.Berezina.Configuration.Records.TableRecord table, out string value)
        {
            value = string.Empty;
            foreach (var setting in table.Settings)
            {
                if (setting.Key == key)
                {
                    value = setting.Value;
                    return true;
                }
            }
            //
            return false;
        }

        /// <summary>
        /// создание коллекции служебных импульсов
        /// </summary>
        private void FullServiceImpuls()
        {
            foreach (KeyValuePair<int, StationTableTs> station in TsList)
            {
                foreach (KeyValuePair<string, List<StateValue>> names in station.Value.NamesValue)
                {
                    string[] split_name = names.Key.Split(new char[] { '-' });
                    if (split_name.Length == 2 && split_name[0] == NumberContolTS.service_impuls)
                    {
                        if (names.Value.Count == 2)
                        {
                            if (CheckServiceImpuls(names.Value))
                            {
                                if (!TsServiceList.ContainsKey(station.Key))
                                    TsServiceList.Add(station.Key, new StationTableServiceTs());
                                //
                                if (!TsServiceList[station.Key].NamesValue.ContainsKey(split_name[1]))
                                {
                                    if (Connections.ClientImpulses.data.Stations.ContainsKey(station.Key))
                                    {
                                        if (Connections.ClientImpulses.data.Stations[station.Key].TS.AddTSImpuls(split_name[1]))
                                        {
                                            TsServiceList[station.Key].NamesValue.Add(split_name[1], new Dictionary<Viewmode, string>());
                                            foreach (StateValue state in names.Value)
                                                TsServiceList[station.Key].NamesValue[split_name[1]].Add(state.ViewTS, state.Formula);
                                        }
                                        else
                                        {
                                            m_log.Info(string.Format("Служебный импульс {0} станции {1} описан уже существует в проекте импульсов ТС", split_name[1], station.Key));
                                        }
                                    }
                                    else
                                    {
                                        m_log.Info(string.Format("Cтанция {0} не загружена в список станций для подключения к серверу импульсов", station.Key));
                                    }
                                }
                            }
                            else
                            {
                                m_log.Info(string.Format("У служебного импульса {0} станции {1} неверные состояния (одно должно быть типа {2}, другое типа {3})",
                                                        split_name[1], station.Key, TrafficTrain.Constant.NameControlTS.impuls_activ, TrafficTrain.Constant.NameControlTS.impuls_pasiv));
                            }
                        }
                        else
                        {
                            m_log.Info(string.Format("У служебного импульса {0} станции {1} неверное количество состояний", split_name[1], station.Key));
                        }
                    }
                }
            }
        }

        private bool CheckServiceImpuls(List<StateValue> impulses)
        {
            Viewmode buffer = Viewmode.none;
            switch (impulses[0].ViewTS)
            {
                case Viewmode.impuls_activ:
                    buffer = Viewmode.impuls_activ;
                    break;
                case Viewmode.impuls_pasiv:
                    buffer = Viewmode.impuls_pasiv;
                    break;
            }
            //
            if (buffer != Viewmode.none)
            {
                switch (buffer)
                {
                    case Viewmode.impuls_activ:
                        if (impulses[1].ViewTS == Viewmode.impuls_pasiv)
                            return true;
                        break;
                    case Viewmode.impuls_pasiv:
                        if (impulses[1].ViewTS == Viewmode.impuls_activ)
                            return true;
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// находим блок участки относящиеся к светофору
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="nameblock"></param>
        /// <returns></returns>
        private List<BlockSection> GetBlockSectionLight(List<BlockSection> blocks, List<string> nameblock)
        {
            List<BlockSection> answer = new List<BlockSection>();
            foreach (string name in nameblock)
            {
                foreach (BlockSection block in blocks)
                {
                    if (block.NameBlock == name)
                        answer.Add(block);
                }
            }
            return answer;
        }

        private Direction GetDirection(int StationNumberLeft, int StationNumberRight, string NamePath, Canvas Draw, ViewDirection view)
        {
            try
            {
                foreach (UIElement el in Draw.Children)
                {
                    if (el is Direction)
                    {
                        Direction direction = el as Direction;
                        if (direction.StationNumberLeft == StationNumberLeft && direction.StationTransition == StationNumberRight && direction.NameTrack == NamePath && view == direction.ViewDirection)
                        {
                            return direction;
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static SCADA.Common.SaveElement.StrageProject GetGrafika(int station)
        {
            if (LoadProject.ProejctViewStations.ContainsKey(station))
                return LoadProject.ProejctViewStations[station];
            else return null;
        }

        /// <summary>
        /// заполняем массив состояний ТС для каждого активного элемента
        /// </summary>
        private Dictionary<Viewmode, StateElement> FullImpulsesElement(int station_number, string index, string name_element)
        {
            if (NumberContolTS.Views.Contains(index))
                return GetImpulses(name_element, station_number);
            else
                return new Dictionary<Viewmode, StateElement>();
        }

        private Dictionary<Viewmode, StateElement> AnalisServiceImpulses(Dictionary<Viewmode, StateElement> impulses)
        {
            if (!(impulses.ContainsKey(Viewmode.head_left) && impulses.ContainsKey(Viewmode.head_right)))
            {
                if (!impulses.ContainsKey(Viewmode.head_left) && !impulses.ContainsKey(Viewmode.head_right))
                {
                    impulses.Add(Viewmode.head_left, new StateElement() { Name = Viewmode.head_left, ViewControlDraw = ViewElementDraw.none });
                    impulses.Add(Viewmode.head_right, new StateElement() { Name = Viewmode.head_right, ViewControlDraw = ViewElementDraw.none });
                }
                else
                {
                    if (!impulses.ContainsKey(Viewmode.head_left))
                        impulses.Add(Viewmode.head_left, new StateElement() { Name = Viewmode.head_left, ViewControlDraw = ViewElementDraw.none });
                    else impulses.Add(Viewmode.head_right, new StateElement() { Name = Viewmode.head_right, ViewControlDraw = ViewElementDraw.none });
                }
            }
            //
            return impulses;
        }

        private Dictionary<Viewmode, StateElement> GetImpulses(string name_element, int station_number)
        {
            Dictionary<Viewmode, StateElement> impulses = new Dictionary<Viewmode, StateElement>();
            try
            {
                var isFind = false;
                if (TsList.ContainsKey(station_number))
                {
                    if (TsList[station_number].NamesValue.ContainsKey(name_element))
                    {
                        isFind = true;
                        foreach (StateValue value in TsList[station_number].NamesValue[name_element])
                        {
                            if (!impulses.ContainsKey(value.ViewTS))
                            {
                                var isAddMessage = false;
                                if (!m_messagesRepeat.ContainsKey(station_number))
                                    m_messagesRepeat.Add(station_number, new Dictionary<string, IDictionary<Viewmode, IList<string>>>());
                                if (!m_messagesRepeat[station_number].ContainsKey(name_element))
                                    m_messagesRepeat[station_number].Add(name_element, new Dictionary<Viewmode, IList<string>>());
                                if (!m_messagesRepeat[station_number][name_element].ContainsKey(value.ViewTS) && value.Messages != null)
                                {
                                    m_messagesRepeat[station_number][name_element].Add(value.ViewTS, value.Messages.Values.ToList());
                                    isAddMessage = true;
                                }
                                //
                                impulses.Add(value.ViewTS, new StateElement((isAddMessage) ? value.Messages : null) { Name = value.ViewTS, Impuls = value.Formula });
                            }
                        }
                    }
                }
                //
                if (!isFind && station_number != CommonConstant.defultStationNumber)
                    m_log.Info(string.Format("Нет в проекте импульсов элемента со станции {0} и именем {1}", station_number, name_element));
                //
                return impulses;
            }
            catch
            {
                return impulses;
            }
        }

        public static void UpdateStation(int newStation, string name)
        {
            if (m_elementView.ContainsKey(newStation))
            {
                foreach (var element in m_elementView[CurrentStation])
                {
                    var line = element as LineHelp;
                    if (line != null && !line.IsVisible && line.Visibility == Visibility.Visible)
                        line.Visibility = Visibility.Collapsed;
                    else
                    {
                        if (element.Visibility != Visibility.Collapsed)
                            element.Visibility = Visibility.Hidden;
                    }
                }
                //
                foreach (var element in m_elementView[newStation])
                {
                    if (element.Visibility != Visibility.Collapsed)
                        element.Visibility = Visibility.Visible;
                    else
                    {
                        var line = element as LineHelp;
                        if (line != null)
                        {
                            if (!string.IsNullOrEmpty(name) && line.NameLine == name && !line.IsVisible)
                            {
                                element.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
                //
                CurrentStation = newStation;
                ProejctGrafic = ProejctViewStations[newStation];
                if (m_areaMessage != null && m_areaMessage.Visibility == Visibility.Visible)
                    m_areaMessage.ScrollToEnd();
                //if (ContentHelp != null)
                //{
                //    ContentHelp.Visibility = Visibility.Hidden;
                //    ContentHelp.Text.Visibility = Visibility.Hidden;
                //}
            }
        }

        /// <summary>
        /// Загружаем и отображаем данные файлов конфигурации
        /// </summary>
        /// <param name="Palitra">палитра для отрисовки</param>
        public void Load(Canvas Palitra, Window win)
        {
            if (LoadInfo != null)
                LoadInfo("Загрузка таблиц ТС и ТУ");
            LoadImpulsInformation();
            //загружаем детальную графику станций
            LoadViewStations();
            //заполняем коллекцию служебных импульсов
            FullServiceImpuls();
            //загружаем аналоговые данные
            if (LoadInfo != null)
                LoadInfo("Загрузка аналоговых данных");
            Core.Load(ConfigurationManager.AppSettings["filestationAnalog"]);
            //отрисовываем графику
            if (LoadInfo != null)
                LoadInfo("Загрузка графики");
            //
            var index =0;
       
            foreach (var view in ProejctViewStations)
            {
                ProejctGrafic = view.Value;
                m_elementView.Add(view.Key, DrawGrafick(view.Value, Palitra, (index == 0) ? Visibility.Visible : Visibility.Hidden, win));
                if (index == 0)
                {
                    CurrentStation = view.Key;
                    m_code_first = view.Key;
                }
                index++;
            }
            //
            foreach (var view in ProejctViewStations)
            {
                ProejctGrafic = view.Value;
                break;
            }
            //
            uint buffer;
            if (uint.TryParse(ConfigurationManager.AppSettings["max_count_message"], out buffer))
                m_countMessages = buffer;
            //
            if (uint.TryParse(ConfigurationManager.AppSettings["ReturnTime"], out buffer))
                m_returnTime = buffer;
            //
            CreateDirDiagnostics(ConfigurationManager.AppSettings["Diagnostics"].ToString(), m_DirDiagnostics);
            LoadDiagnostics();
            m_log.Info("-----------------------------------------------------------------------Загрузка завершена !!!!-------------------------------------------------------------------");
        }

        private static void CreateDirDiagnostics(string path, string pathDefult)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    if (!string.IsNullOrEmpty(pathDefult))
                        CreateDirDiagnostics(pathDefult, string.Empty);
                    else
                    {
                        m_log.Error(string.Format("Нельзя создать католог {0} для диагностики", path));
                        return;
                    }
                }
            }
            //
            m_FileDiagnostics = string.Format("{0}{1}", (new DirectoryInfo(path)).FullName, string.Format(@"\Журнал_событий_{0:D4}_{1:D2}_{2:D2}_{3:D2}_{4:D2}_{5:D2}.log", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            isWriteDiagnostics = true;
            m_DirDiagnostics = (new DirectoryInfo(path)).FullName;
        }


        private static void LoadDiagnostics()
        {
            if (isWriteDiagnostics)
            {
                if (Directory.Exists(m_DirDiagnostics))
                {
                    var directory = new DirectoryInfo(m_DirDiagnostics);
                    var messages = new List<string>();
                    foreach (var file in directory.GetFiles("Журнал_событий_*_*_*_*_*_*.log", SearchOption.TopDirectoryOnly).OrderByDescending(x=>x.CreationTime))
                    {
                        var text = File.ReadLines(file.FullName, Encoding.UTF8).ToList();
                        //
                        if (text != null)
                        {
                            text.Reverse();
                            foreach (var row in text)
                            {
                                  if (Regex.Match(row, patternLog).Success)
                                  {
                                      if (messages.Count < m_countMessages)
                                      {
                                          messages.Add(row + Environment.NewLine);
                                      }
                                      else
                                      {
                                          goto metka;
                                      }
                                  }
                            }
                        }
                    }
                    //
                metka:
                    if (messages.Count > 0)
                    {
                        messages.Reverse();
                        AddMessages(messages);
                    }
                }
            }
        }

        public static void LoadColorAll()
        {
            if (LoadInfo != null)
                LoadInfo("Загрузка проекта цвета");
            LoadStyles();
            CommandColor.SetAllColor();
        }
  
        private PathGeometry GetPathGeometry(List<Figure> Figures)
        {
            PathGeometry geo = new PathGeometry();
            foreach (Figure figure in Figures)
            {
                PathSegmentCollection segment = new PathSegmentCollection();
                foreach (Segment seg in figure.Segments)
                {
                    if (seg.RadiusX != 0 && seg.RadiusY != 0)
                        segment.Add(new ArcSegment()
                        {
                            Point = new Point(seg.Point.X * System.Windows.SystemParameters.CaretWidth, seg.Point.Y * System.Windows.SystemParameters.CaretWidth),
                            Size = new Size(seg.RadiusX * System.Windows.SystemParameters.CaretWidth, seg.RadiusY * System.Windows.SystemParameters.CaretWidth)
                        });
                    else
                        segment.Add(new LineSegment()
                        {
                            Point = new Point(seg.Point.X * System.Windows.SystemParameters.CaretWidth, seg.Point.Y * System.Windows.SystemParameters.CaretWidth)
                        });
                }
                //
                geo.Figures.Add(new PathFigure()
                {
                    StartPoint = new Point(figure.StartPoint.X * System.Windows.SystemParameters.CaretWidth, figure.StartPoint.Y * System.Windows.SystemParameters.CaretWidth),
                    Segments = segment
                });

            }
            return geo;
        }

        private void CreateArea(AreaSave area, ViewArea View, Canvas drawCanvas, IList<UIElement> elements, Visibility visible)
        {
            foreach (Figure figure in area.Figures)
            {
                if (figure.Segments.Count == 4)
                {
                    double x = figure.StartPoint.X * System.Windows.SystemParameters.CaretWidth;
                    double y = figure.StartPoint.Y * System.Windows.SystemParameters.CaretWidth;
                    double width = Math.Abs((figure.Segments[0].Point.X - figure.StartPoint.X) * System.Windows.SystemParameters.CaretWidth);
                    double height = Math.Abs((figure.Segments[1].Point.Y - figure.StartPoint.Y) * System.Windows.SystemParameters.CaretWidth);
                    switch (View)
                    {
                        case ViewArea.area_picture:
                            {
                                AreaPicture areaPicture = new AreaPicture(GetPathGeometry(area.Figures), area.Path, area.Angle) { Nodes = area.Notes, StationTransition = area.StationNumberRight };
                                //выводи объекта на панель
                                areaPicture.Visibility = visible;
                                drawCanvas.Children.Add(areaPicture);
                                elements.Add(areaPicture);
                            }
                            break;
                         case ViewArea.area_message:
                            m_log.Info(string.Format("Область справки подгружена"));
                            if (m_areaMessage == null)
                            {
                                m_areaMessage = new TextBox()
                                {
                                    Background = Brushes.Transparent,
                                    IsReadOnly = true,
                                    FontWeight = FontWeights.Bold,
                                    FontSize = 14,
                                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                                    Name = "textblock_message",
                                    TextAlignment = System.Windows.TextAlignment.Left,
                                    TextWrapping = System.Windows.TextWrapping.Wrap,
                                    Focusable = false,
                                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                    Margin = new Thickness(x, y, 0,0),
                                    Width = width,
                                    Height = height
                                };
                                m_areaMessage.Visibility = visible;
                                drawCanvas.Children.Add(m_areaMessage);
                                elements.Add(m_areaMessage);
                            }
                            break;
                         //case ViewArea.area_station:
                         //   {
                         //       if (m_detailStation == null)
                         //       {
                         //           m_detailStation = new DetailViewStation(x, y, width, height, ContentHelp);
                         //           m_detailStation.
                         //           drawCanvas.Children.Add(m_detailStation);
                         //       }
                         //   }
                         //   break;
                    }
                    break;
                }
            }
        }

        private List<Figure> GetFigures(List<Point> points)
        {
            List<Figure> figures = new List<Figure>();
            if (points != null && points.Count > 0)
            {
                figures.Add(new Figure());
                //
                for (int i = 0; i < points.Count; i++)
                {
                    if (i == 0)
                        figures[figures.Count-1].StartPoint = new Point(points[i].X, points[i].Y);
                    else
                        figures[figures.Count - 1].Segments.Add(new Segment() { Point = new Point(points[i].X, points[i].Y) });
                }
            }
            //
            return figures;
        }

        private void CreateCollectioAbsolute(LinePeregonSave peregon, List<SettingsSegmentBlock> collection, double start, double end, double Lenght)
        {
            double lenght = 0;
            double locationstart = start;
            try
            {
                foreach (Figure f in peregon.Figures)
                {
                    for (int i = 0; i < f.Segments.Count; i++)
                    {
                        if (i == 0)
                        {
                            double L =  Math.Sqrt(Math.Pow(f.Segments[i].Point.X - f.StartPoint.X, 2) + Math.Pow(f.Segments[i].Point.Y - f.StartPoint.Y, 2));
                            lenght +=L;
                            if (start < end)
                            {
                                double locationend = start + (lenght / Lenght) * (end - start);
                                collection.Add(new SettingsSegmentBlock() { StartPoint = new Point(f.StartPoint.X, f.StartPoint.Y), EndPoint = new Point(f.Segments[i].Point.X, f.Segments[i].Point.Y), Lenght= L, StartKilomentr = locationstart, EndKilomentr = locationend});
                                locationstart = locationend;
                            }
                            else
                            {
                                double locationend = start - (lenght / Lenght) * (start - end);
                                collection.Add(new SettingsSegmentBlock() { StartPoint = new Point(f.StartPoint.X, f.StartPoint.Y), EndPoint = new Point(f.Segments[i].Point.X, f.Segments[i].Point.Y), Lenght = L, StartKilomentr = locationstart, EndKilomentr = locationend });
                                locationstart = locationend;
                            }

                        }
                        if (i > 0)
                        {
                            double L = Math.Sqrt(Math.Pow(f.Segments[i].Point.X - f.Segments[i - 1].Point.X, 2) + Math.Pow(f.Segments[i].Point.Y - f.Segments[i - 1].Point.Y, 2));
                            lenght += L;
                            if (start < end)
                            {
                                double locationend = start + (lenght / Lenght) * (end - start);
                                collection.Add(new SettingsSegmentBlock() { StartPoint = new Point(f.Segments[i - 1].Point.X, f.Segments[i - 1].Point.Y), EndPoint = new Point(f.Segments[i].Point.X, f.Segments[i].Point.Y), Lenght = L, StartKilomentr = locationstart, EndKilomentr = locationend });
                                locationstart = locationend;
                            }
                            else
                            {
                                double locationend = start - (lenght / Lenght) * (start - end);
                                collection.Add(new SettingsSegmentBlock() { StartPoint = new Point(f.Segments[i - 1].Point.X, f.Segments[i - 1].Point.Y), EndPoint = new Point(f.Segments[i].Point.X, f.Segments[i].Point.Y), Lenght = L, StartKilomentr = locationstart, EndKilomentr = locationend });
                                locationstart = locationend;
                            }
                        }
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// определяем длину перегона
        /// </summary>
        /// <param name="peregon"></param>
        /// <returns></returns>
        private double LenghtPeregon(LinePeregonSave peregon)
        {
            double lenght = 0;
            try
            {
                foreach (Figure f in peregon.Figures)
                {
                    for (int i = 0; i < f.Segments.Count; i++)
                    {
                        if (i == 0)
                            lenght += Math.Sqrt(Math.Pow(f.Segments[i].Point.X - f.StartPoint.X, 2) + Math.Pow(f.Segments[i].Point.Y - f.StartPoint.Y, 2));
                        if (i > 0)
                            lenght += Math.Sqrt(Math.Pow(f.Segments[i].Point.X - f.Segments[i - 1].Point.X, 2) + Math.Pow(f.Segments[i].Point.Y - f.Segments[i - 1].Point.Y, 2));
                    }
                }
            }
            catch { return lenght; }
            return lenght;
        }
    }
}
