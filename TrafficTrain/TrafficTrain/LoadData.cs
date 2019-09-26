using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Configuration;
using System.Windows.Media;
using System.Windows;
using TrafficTrain.DataGrafik;
using log4net;
using Move;

namespace TrafficTrain
{
    /// <summary>
    /// делегат сообщющий о ходе загрузки
    /// </summary>
    /// <param name="info">инфо загрузки</param>
    public delegate void Info(string info);
    /// <summary>
    /// делегат о справке по поезду 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="row"></param>
    public delegate void InfoMessageTrain(string info, TableRow row, Move.Even table);
    /// <summary>
    /// делегат для изменения цвета главной формы
    /// </summary>
    /// <param name="fon"></param>
    /// <param name="arrowcommand"></param>
    public delegate void NewColor();
    /// <summary>
    /// возможные условные обозначения контролей
    /// </summary>
    public struct ViewNameSost
    {
        //Виды состояния кнопки станции
        public const string seasonal_management = "Сезонное управление";
        public const string start_seasonal_management = "Передача на сезонное управление";
        public const string reserve_control = "Резервное управление";
        public const string supervisory_control = "Диспетчерское управление";
        public const string not_station = "Нет связи со станцией";
        public const string fire = "Пожар";
        public const string autonomous_control = "Автономное управление";
        //Виды состояния элемента перегонной стрелки
        public const string departure = "Отправление";
        public const string resolution_of_origin = "Разрешение отправления";
        public const string waiting_for_departure = "Ожидание отправления";
        //Виды состояния элемента сигнал
        public const string passage = "Проезд";
        public const string signal = "Сигнал";
        public const string invitational = "Пригласительный";
        public const string locking = "Замыкание";
        public const string installation = "Установка";
        public const string shunting = "Маневровый";
        //Виды состояния элемента главный путь
        public const string auto_run = "Авто действие";
        public const string electrification = "Электрификация";
        public const string fencing = "Ограждение";
        //Виды состояния элемента переезд
        public const string closing = "Закрытие";
        public const string closing_button = "Закрытие кнопкой";
        //Виды состояния элемента КГУ
        public const string play_KGU = "Обрыв контура";
        //Виды состояния элемента КТСМ
        public const string play_KTCM = "Срабатывание КТСМ";
        //Виды состояния общие для всех
        public const string occupation = "Занятие";
        public const string fault = "Неисправность";
        public const string accident = "Авария";
    }

    /// <summary>
    /// возможные условные обозначения видов активных элементов ввиде цифр
    /// </summary>
    public struct ViewNameSostNumberTS
    {
        public const string button_station = "1";
      //  public const string peregon_arrow = "2";
        public const string signal = "2";
        public const string big_path = "3";
      //  public const string peregon_path = "5";
     //  public const string move = "6";
     //   public const string ktcm = "7";
     //   public const string kgu = "8";
    }

    /// <summary>
    /// зарезервированные слова
    /// </summary>
    public struct ConstName
    {
        public const string move = "ПЕРЕГОН";
        public const string path = "ПУТЬ";
        public const string yes_route_setting = "Задать маршрут";
        public const string no_route_setting = "Отменить маршрут";
        public const string command_gid_path_arrival = "Задать путь прибытия";
        public const string update_gid_path_arrival = "Изменить путь прибытия";
        public const string enter_number_train = "Ввести номер поезда";
        public const string create_new_train = "Сформировать поезд";
        public const string delete_train = "Удалить поезд";
        public const string standart_view = "Вернуться к первоначальному виду";
        public const string exit = "Выход";
        public const string unknown_number_train = "XXXX";
        public const string prefix_station = "TE";
        public const string admin = "admin";
        public const string user = "user";
    }

    /// <summary>
    /// виды прибытий и отправлений
    /// </summary>
    public struct ViewTrainArrivalDeparture
    {
        public const string arrival_normal = "0";
        public const string not_arrival_normal = "1";
        public const string departure_normal = "2";
        public const string not_departure_normal = "3";
    }

    /// <summary>
    /// возможные условные обозначения видов элеметов управления
    /// </summary>
    public struct ViewNameSostNumberTu
    {
        /// <summary>
        /// сезонное управление
        /// </summary>
        public const string seasonal_management = "1";
        /// <summary>
        /// разрешение отправления на перегон
        /// </summary>
        public const string yes_departure = "2";
        /// <summary>
        /// отменить отправления на перегон
        /// </summary>
        public const string no_departure = "3";
        /// <summary>
        /// установка маршрута
        /// </summary>
        public const string yes_route_setting = "4";
        /// <summary>
        /// отмена маршрута
        /// </summary>
        public const string no_route_setting = "5";
    }

    /// <summary>
    /// загружаем данные из конфирурации на диске 
    /// </summary>
    class LoadProject
    {

        #region Переменные и свойства
        /// <summary>
        /// набор ТС импульсов по каждой станции
        /// </summary>
        public static Dictionary<int, StationTableTs> TS_list = new Dictionary<int, StationTableTs>();
        /// <summary>
        /// набор ТУ импульсов по каждой станции
        /// </summary>
        public static Dictionary<int, StationTableTu> TU_list = new Dictionary<int, StationTableTu>();
        /// <summary>
        /// инфо о ходе загрузки
        /// </summary>
        public static event Info LoadInfo;
        /// <summary>
        /// логирование
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(LoadProject));
        /// <summary>
        /// проект графики
        /// </summary>
        TrafficTrain.DataGrafik.StrageProject ProejctGrafic;
        /// <summary>
        /// проект перегонов
        /// </summary>
        Move.Plot ProejctMove;
        /// <summary>
        /// проект цвета
        /// </summary>
        public static TrafficTrain.DataGrafik.ColorSaveProejct ColorProject;
        /// <summary>
        /// Событие для изменения цвета главного окна
        /// </summary>
        public static event NewColor NewColor;
        
        /// <summary>
        /// коллекция названий станций и их сетевых имен
        /// </summary>
        public static Dictionary<string, string> NamesStations = new Dictionary<string, string>();
        /// <summary>
        /// коллекция светофор работающих на перегоне
        /// </summary>
        public static List<LightElementControl> LigthCollection = new List<LightElementControl>();
        #endregion

        /// <summary>
        /// конструктор по умолчанию
        /// </summary>
        public LoadProject()
        {
            //Конфигурируем логер
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// загружаем файл графики  проекта 
        /// </summary>
        /// <param name="projectmodel">путь к файлу</param>
        private TrafficTrain.DataGrafik.StrageProject  LoadGrafickProject()
        {
            try
            {
                if (ConfigurationManager.AppSettings["grafick_project"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["grafick_project"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["grafick_project"]).Exists)
                    {
                        TrafficTrain.DataGrafik.StrageProject Project = new DataGrafik.StrageProject();
                        using (var reader = new XmlTextReader(ConfigurationManager.AppSettings["grafick_project"]))
                        {
                            var deserializer = new XmlSerializer(typeof(TrafficTrain.DataGrafik.StrageProject));
                            Project = (TrafficTrain.DataGrafik.StrageProject)deserializer.Deserialize(reader);
                            reader.Close();
                            log.Info("Графика успешно загружена !!!");
                            return Project;
                        }
                    }
                    else
                    {
                        log.Error(string.Format("Файла графики по адресу {0} - не существует", ConfigurationManager.AppSettings["grafick_project"]));
                        return null;
                    }
                }
                else
                {
                    log.Error(string.Format("Введите в файле конфигурации путь к проекту графики участка, обозначенного {0}", "<<grafick_project>>"));
                    return null;
                }
            }
            catch (Exception error)
            {
                log.Error(string.Format("Ошибка {0}, в файле графики !!!",error.Message));
                return null;
            }
        }

        /// <summary>
        /// загружаем расскраску
        /// </summary>
        private static TrafficTrain.DataGrafik.ColorSaveProejct LoadColor()
        {
            //проверяем информацию по импульсам телесигнализации
            try
            {
                if (ConfigurationManager.AppSettings["file_color_configuration"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["file_color_configuration"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["file_color_configuration"]).Exists)
                    {
                        TrafficTrain.DataGrafik.ColorSaveProejct Proejct = new ColorSaveProejct();
                        using (var reader = new XmlTextReader(ConfigurationManager.AppSettings["file_color_configuration"]))
                        {
                            var deserializer = new XmlSerializer(typeof(TrafficTrain.DataGrafik.ColorSaveProejct));
                            Proejct = (TrafficTrain.DataGrafik.ColorSaveProejct)deserializer.Deserialize(reader);
                            reader.Close();
                            log.Info("Цветовая расскраска успешно загружена !!!");
                            return Proejct;
                        }
                    }
                    else
                    {
                        log.Error(string.Format("Файла с описанием цветовой расскраски {0} - не существует", ConfigurationManager.AppSettings["file_color_configuration"]));
                        return null;
                    }
                }
                else
                {
                    log.Error(string.Format("Введите в файле конфигурации путь к описанию цветовой расскраски, обозначенного {0}", "<<file_color_configuration>>"));
                    return null;
                }
            }
            catch (Exception error)
            {
                log.Error(error.Message);
                return null;
            }
        }

        /// <summary>
        /// загрузка таблиц ТУ  и ТС
        /// </summary>
        private void LoadImpulsInformation()
        {
            //проверяем информацию по импульсам телесигнализации
            try
            {
                if (ConfigurationManager.AppSettings["filestationTS"]!=null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["filestationTS"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["filestationTS"]).Exists)
                    {
                        string[] spisokstation = File.ReadAllLines(ConfigurationManager.AppSettings["filestationTS"]);
                        foreach (string st in spisokstation)
                        {
                            string[] files = st.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            int buffer = 0;
                            if (files.Length == 2 && int.TryParse(files[0], out buffer))
                            {
                                int current_numberstation = int.Parse(files[0]);
                                if (new FileInfo(files[1]).Exists)
                                {
                                    string[] fileTsinfo = File.ReadAllLines(files[1], Encoding.GetEncoding(1251));
                                    TS_list.Add(current_numberstation, new StationTableTs());
                                    //
                                    foreach (string row in fileTsinfo)
                                    {
                                        string[] cells = row.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                        if (cells.Length == 4)
                                            RepetitionElementTS(string.Format("{0}-{1}", cells[0], cells[1]), cells[2], cells[3], current_numberstation);
                                    }
                                }
                                else
                                    log.Error(string.Format("Файла с перечнем импульсов ТС станции {0} по адресу {1} - не существует", current_numberstation.ToString(), files[1]));
                            }
                        }
                        //
                        log.Info("Таблицы импульсов ТС загружены");
                    }
                    else
                        log.Error(string.Format("Файла с перечнем импульсов ТС по адресу {0} - не существует", ConfigurationManager.AppSettings["filestationTS"]));
                }
                else
                    log.Error(string.Format("Введите в файле конфигурации путь к описанию импульсов ТС, обозначенного {0}", "<<filestationTS>>"));
            }
            catch(Exception error)
            {
                log.Error(error.Message);
            }
            //проверяем информацию по импульсам телеуправления
            try
            {
                if (ConfigurationManager.AppSettings["filestationTU"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["filestationTU"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["filestationTU"]).Exists)
                    {
                        string[] spisokstation = File.ReadAllLines(ConfigurationManager.AppSettings["filestationTU"]);
                        foreach (string st in spisokstation)
                        {
                            string[] files = st.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                            int buffer = 0;
                            if (files.Length == 2 && int.TryParse(files[0], out buffer))
                            {
                                int current_numberstation = int.Parse(files[0]);
                                if (new FileInfo(files[1]).Exists)
                                {
                                    string[] fileTuinfo = File.ReadAllLines(files[1], Encoding.GetEncoding(1251));
                                    TU_list.Add(current_numberstation, new StationTableTu());
                                    //
                                    foreach (string row in fileTuinfo)
                                    {
                                        string[] cells = row.Split(new string[] { ";" }, StringSplitOptions.None);
                                        if (cells.Length == 7)
                                            FillingTableTu(cells[0], cells[1], string.Format("{0}-{1}", cells[2], cells[3]), string.Format("{0}-{1}", cells[4], cells[5]), cells[6], current_numberstation);
                                    }
                                }
                                else
                                    log.Error(string.Format("Файла с перечнем импульсов ТУ станции {0} по адресу {1} - не существует",current_numberstation.ToString(), files[1]));
                            }
                        }
                        //
                        log.Info("Таблицы импульсов ТУ загружены");
                    }
                    else
                        log.Error(string.Format("Файла с перечнем импульсов ТУ по адресу {0} - не существует", ConfigurationManager.AppSettings["filestationTU"]));
                }
                else
                    log.Error(string.Format("Введите в файле конфигурации путь к описанию импульсов ТУ, обозначенного {0}", "<<filestationTU>>"));
            }
            catch (Exception error)
            {
                log.Error(error.Message);
            }
        }

        /// <summary>
        /// загружаем информацию по перегонам
        /// </summary>
        private Plot LoadStrageInfo()
        {
            try
            {
                if (ConfigurationManager.AppSettings["move_settings"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["move_settings"]))
                {
                    if (new FileInfo(ConfigurationManager.AppSettings["move_settings"]).Exists)
                    {
                        Move.Plot Project = new Move.Plot();
                        using (var reader = new XmlTextReader(ConfigurationManager.AppSettings["move_settings"]))
                        {
                            var deserializer = new XmlSerializer(typeof(Move.Plot));
                            Project = (Move.Plot)deserializer.Deserialize(reader);
                            reader.Close();
                            foreach (Move.StrageProject move in Project.Moves)
                            {
                                string nameleft = move.NameLeftBlock;
                                string nameright = move.NameRightBlock;
                                LoadMoveBlock block = new LoadMoveBlock();
                                move.Blockplots =  block.CreateBlock(move.Lightsmoves, move.Infostrage.Start, move.Infostrage.End, ref nameleft, ref nameright);
                                move.NameLeftBlock = nameleft;
                                move.NameRightBlock = nameright;
                            }
                            log.Info("Информация по перегонам загружена и обработана!!!");
                            return Project;
                        }
                    }
                    else
                    {
                        log.Error(string.Format("Файла описания перегонов участка {0} - не существует", ConfigurationManager.AppSettings["move_settings"]));
                        return null;
                    }
                }
                else
                {
                    log.Error(string.Format("Введите в файле конфигурации путь к описанию перегонов, обозначенного {0}", "<<move_settings>>"));
                    return null;
                }
            }
            catch (Exception error)
            {
                log.Error(error.Message);
                return null;
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
            if (TU_list.Count > 0)
            {
                switch (numbersost)
                {
                    case ViewNameSostNumberTu.seasonal_management:
                        TU_list[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu =tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.yes_departure:
                        TU_list[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu = tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.yes_route_setting:
                        TU_list[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu = tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.no_departure:
                        TU_list[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, Tu = tu, ViewCommand = numbersost });
                        break;
                    case ViewNameSostNumberTu.no_route_setting:
                        TU_list[number_station].NamesValue.Add(new StateValueTu() { NameTu = namecommand, StartPath = startpath, EndPath = endpath, ViewCommand = numbersost, Tu = tu,});
                        break;
                }
            }
        }
     
        /// <summary>
        /// проверяем на повтор элемент в таблице ТС
        /// </summary>
        /// <param name="NameElement">название элемента</param>
        /// <returns></returns>
        private void RepetitionElementTS(string NameElement, string state, string folmula, int number_station)
        {
            if (TS_list.Count > 0)
            {
                bool repetition = false;
                //
                foreach (KeyValuePair<string, List<StateValueTs>> value in TS_list[number_station].NamesValue)
                {
                    if (NameElement == value.Key)
                    {
                        if (GetState(state) != Move.Viewmode.none)
                            value.Value.Add(new StateValueTs() { View = GetState(state), Formula = folmula });
                        repetition = true;
                        break;
                    }
                }
                //
                if (!repetition)
                {
                    TS_list[number_station].NamesValue.Add(NameElement, new List<StateValueTs>());
                    if (GetState(state) != Move.Viewmode.none)
                        TS_list[number_station].NamesValue[NameElement].Add(new StateValueTs() { View = GetState(state), Formula = folmula });
                }
            }
        }
        

        /// <summary>
        /// проверяем состояние с уже имеющимися
        /// </summary>
        /// <param name="Namestate">названия состояния из таблицы</param>
        /// <returns></returns>
        private Move.Viewmode GetState(string Namestate)
        {
            if (Namestate != null && Namestate.Length > 0)
            {
                switch (Namestate.Trim(new char[]{' '}))
                {
                    case ViewNameSost.accident:
                        return Viewmode.accident;
                    case ViewNameSost.shunting:
                        return Viewmode.shunting;
                    case ViewNameSost.auto_run:
                        return Viewmode.auto_run;
                    case ViewNameSost.closing:
                        return Viewmode.closing;
                    case ViewNameSost.closing_button:
                        return Viewmode.closing_button;
                    case ViewNameSost.departure:
                        return Viewmode.departure;
                    case ViewNameSost.electrification:
                        return Viewmode.electrification;
                    case ViewNameSost.fault:
                        return Viewmode.fault;
                    case ViewNameSost.fencing:
                        return Viewmode.fencing;
                    case ViewNameSost.fire:
                        return Viewmode.fire;
                    case ViewNameSost.installation:
                        return Viewmode.installation;
                    case ViewNameSost.invitational:
                        return Viewmode.invitational;
                    case ViewNameSost.locking:
                        return Viewmode.locking;
                    case ViewNameSost.not_station:
                        return Viewmode.not_station;
                    case ViewNameSost.occupation:
                        return Viewmode.occupation;
                    case ViewNameSost.passage:
                        return Viewmode.passage;
                    case ViewNameSost.play_KGU:
                        return Viewmode.play_KGU;
                    case ViewNameSost.play_KTCM:
                        return Viewmode.play_KTCM;
                    case ViewNameSost.reserve_control:
                        return Viewmode.reserve_control;
                    case ViewNameSost.resolution_of_origin:
                        return Viewmode.resolution_of_origin;
                    case ViewNameSost.seasonal_management:
                        return Viewmode.seasonal_management;
                    case ViewNameSost.signal:
                        return Viewmode.signal;
                    case ViewNameSost.start_seasonal_management:
                        return Viewmode.start_seasonal_management;
                    case ViewNameSost.supervisory_control:
                        return Viewmode.supervisory_control;
                    case ViewNameSost.waiting_for_departure:
                        return Viewmode.waiting_for_departure;
                    case ViewNameSost.autonomous_control:
                        return Viewmode.autonomous_control;
                    default:
                        log.Info(string.Format("Нет такого состояния {0}, проверьте правильность написания!!!",Namestate));
                        break;
                }
            }
            return Move.Viewmode.none;
        }

        /// <summary>
        /// находим нужный перегон из описаловки перегонов детальной
        /// </summary>
        /// <param name="StationNumber">номер станции слева</param>
        /// <param name="StationNumberRight">номер станции справа</param>
        /// <param name="NameMove">название пути перегона</param>
        /// <param name="NameBlock">название блок участка</param>
        /// <returns></returns>
        public Move.StrageProject FindPeregon(int StationNumber, int StationNumberRight , string NameMove,  string NameBlock)
        {
            try
            {
                foreach (Move.StrageProject strage in ProejctMove.Moves)
                {
                    if (strage.Infostrage.Stationnumberleft == StationNumber && strage.Infostrage.Stationnumberright == StationNumberRight && strage.NameMove == NameMove)
                    {
                        foreach (Move.BlockPlotProject _block in strage.Blockplots)
                        {
                            if (_block.NameBlock == NameBlock)
                                return strage;
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


        /// <summary>
        /// Отрисовываем загруженную графику
        /// </summary>
        /// <param name="Project">занруженная графика</param>
        private void DrawGrafick(TrafficTrain.DataGrafik.StrageProject Project, Plot Move, ref Canvas DrawCanvas)
        {
            if (Project != null)
            {
                //рисуем элемент сигнал
                try
                {
                    foreach (TrafficTrain.DataGrafik.AdjacencyPathSave _signal in Project.Adjacencypaths)
                    {
                        //
                        Signal newsignal = new Signal((int)_signal.StationNumber, GetPathGeometry(_signal.Figures), _signal.Name);
                        ImpulsTSElement activ = newsignal as ImpulsTSElement;
                        FullImpulsesElement(newsignal.StationNumber, ViewNameSostNumberTS.signal, string.Format("{0}-{1}", ViewNameSostNumberTS.signal, newsignal.NameSignal), activ);
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(newsignal);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем кнопку станции
                try
                {
                    foreach (TrafficTrain.DataGrafik.ButtonStationSave _buttomst in Project.ButtonsStation)
                    {
                        //
                        ButtonStation newbuttonst = new ButtonStation((int)_buttomst.StationNumber, GetPathGeometry(_buttomst.Figures), _buttomst.Name);
                        ImpulsTSElement activ = newbuttonst as ImpulsTSElement;
                        FullImpulsesElement(newbuttonst.StationNumber, ViewNameSostNumberTS.button_station, string.Format("{0}-{1}", ViewNameSostNumberTS.button_station, newbuttonst.NameButton), activ);
                        //проверяем находится ли станция на автономном управлении
                        foreach (StateElement imp in newbuttonst.Impulses)
                        {
                            if (imp.Name == Viewmode.autonomous_control && imp.Impuls.ToUpper() == "ДА")
                            {
                                newbuttonst.AutonomousControl = true; break;
                            }

                        }
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(newbuttonst);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем главные пути
                try
                {
                    foreach (TrafficTrain.DataGrafik.RoadStation _road in Project.HighRoads)
                    {
                        //
                        StationPath newpath = new StationPath((int)_road.StationNumber, GetPathGeometry(_road.Figures), _road.Name, _road.Xinsert, _road.Yinsert, _road.TextSize);
                        newpath.Text.FontWeight = FontWeights.Bold;
                        newpath.RotateText = _road.Angle;
                        newpath.Text.RenderTransform = new RotateTransform(_road.Angle);
                        ImpulsTSElement activ = newpath as ImpulsTSElement;
                        FullImpulsesElement(newpath.StationNumber, ViewNameSostNumberTS.big_path, string.Format("{0}-{1}", ViewNameSostNumberTS.big_path, newpath.NamePath), activ);
                        //проверяем есть ли электрофикация пуи
                        foreach (StateElement imp in newpath.Impulses)
                        {
                            if (imp.Name == Viewmode.electrification && imp.Impuls.ToUpper() == "ДА")
                            {
                                newpath.ViewTraction = ViewTraction.electric_traction; break;
                            }
                        }
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(newpath);
                        DrawCanvas.Children.Add(newpath.Text);
                    }
                }
                catch (Exception error) {log.Error(error.Message); }
                //рисуем рамку станции
                try
                {
                    foreach (TrafficTrain.DataGrafik.RamkaStationSave _ramka in Project.RamkaStations)
                    {
                        RamkaStation ramkastation = new RamkaStation(GetPathGeometry(_ramka.Figures));
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(ramkastation);
                        Canvas.SetZIndex(ramkastation, -1);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем вспомагательную линию
                try
                {
                    foreach (TrafficTrain.DataGrafik.LineHelpSave _helpline in Project.LineHelps)
                    {
                        LineHelp help = new LineHelp(GetPathGeometry(_helpline.Figures), _helpline.WeightStroke, new SolidColorBrush(Color.FromRgb(_helpline.R, _helpline.G ,_helpline.B)));
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(help);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем переезды
                try
                {
                    foreach (TrafficTrain.DataGrafik.MoveSave _move in Project.Moves)
                    {
                        //
                        Moves newmove = new Moves((int)_move.StationNumber, GetPathGeometry(_move.Figures), _move.Name);
                        newmove.Impulses = GetImpulsesMove(newmove.StationNumber, newmove.NameMove);
                        //ImpulsTSElement activ = newmove as ImpulsTSElement;
                        //FullImpulsesElement(newmove.StationNumber, ViewNameSostNumberTS.move, string.Format("{0}-{1}", ViewNameSostNumberTS.move, newmove.NameMove), activ);
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(newmove);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем КГУ
                try
                {
                    foreach (TrafficTrain.DataGrafik.KGUSave _kgu in Project.KGUs)
                    {
                        //
                        KGU newkgu = new KGU((int)_kgu.StationNumber, GetPathGeometry(_kgu.Figures), _kgu.Name);
                        newkgu.Impulses = GetImpulsesKGU(newkgu.StationNumber, newkgu.NameKGU);
                        //ImpulsTSElement activ = newkgu as ImpulsTSElement;
                        //FullImpulsesElement(newkgu.StationNumber, ViewNameSostNumberTS.kgu, string.Format("{0}-{1}", ViewNameSostNumberTS.kgu, newkgu.NameKGU), activ);
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(newkgu);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем КТСМ
                try
                {
                    foreach (TrafficTrain.DataGrafik.KTCMSave _ktcm in Project.KTCMs)
                    {
                        //
                        KTCM newktcm = new KTCM((int)_ktcm.StationNumber, GetPathGeometry(_ktcm.Figures), _ktcm.Name);
                        newktcm.Impulses = GetImpulsesKTCM(newktcm.StationNumber, newktcm.NameKTCM);
                        //ImpulsTSElement activ = newktcm as ImpulsTSElement;
                        //FullImpulsesElement(newktcm.StationNumber, ViewNameSostNumberTS.ktcm, string.Format("{0}-{1}", ViewNameSostNumberTS.ktcm, newktcm.NameKTCM), activ);
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(newktcm);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем элемент название станции
                try
                {
                    foreach (TrafficTrain.DataGrafik.NameStationSave _namestation in Project.NameStations)
                    {
                        //
                        NameStation newnamestation = new NameStation((int)_namestation.StationNumber, GetPathGeometry(_namestation.Figures), _namestation.Name, _namestation.Left, _namestation.Top, _namestation.FontSize);
                        //
                        newnamestation.Text.RenderTransform = new RotateTransform(_namestation.Angle);
                        newnamestation.Text.MaxWidth = _namestation.Width;
                        newnamestation.Text.MaxHeight = _namestation.Height;
                        //
                        try
                        {
                            NamesStations.Add(newnamestation.NameSt, newnamestation.StationNumber.ToString());
                        }
                        catch (Exception error) { log.Error(error.Message); }
                        //
                        //выводи объекта на панель
                        DrawCanvas.Children.Add(newnamestation);
                        DrawCanvas.Children.Add(newnamestation.Text);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем элемент перегонная стрелка
                try
                {
                    foreach (TrafficTrain.DataGrafik.ArrowMoveSave _arrow in Project.ArrowMoves)
                    {
                        ArrowMove new_arrowmove = new ArrowMove((int)_arrow.StationNumber, (int)_arrow.StationNumberRight, GetPathGeometry(_arrow.Figures), _arrow.Graniza);
                        //
                        //if (new_arrowmove.Graniza.Split(new char[] { '-' }).Length == 2)
                        //{
                        //    ImpulsTSElement activ = new_arrowmove as ImpulsTSElement;
                         //   FullImpulsesElement(new_arrowmove.StationNumber, ViewNameSostNumberTS.peregon_arrow, string.Format("{0}-{1}", ViewNameSostNumberTS.peregon_arrow, new_arrowmove.Graniza.Split(new char[] { '-' })[0]), activ);
                        //}
                        //выводи объекты на панель
                        DrawCanvas.Children.Add(new_arrowmove.LeftArrow);
                        DrawCanvas.Children.Add(new_arrowmove.Center);
                        DrawCanvas.Children.Add(new_arrowmove.RightArrow);
                        DrawCanvas.Children.Add(new_arrowmove);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
                //рисуем область номеров поездов
                try
                {
                    foreach (TrafficTrain.DataGrafik.NumberTrainSave _train in Project.NumberTrains)
                    {
                        Move.StrageProject strage = GetPeregon((int)_train.StationNumber, (int)_train.StationNumberRight, _train.Name);
                        NumberTrainRamka new_train;
                        if (strage != null)
                            new_train = new NumberTrainRamka((int)_train.StationNumber, (int)_train.StationNumberRight, GetPathGeometry(_train.Figures), strage.NameLeftBlock, strage.NameRightBlock, strage.NameMove, _train.RotateText);
                        else
                            new_train = new NumberTrainRamka((int)_train.StationNumber, (int)_train.StationNumberRight, GetPathGeometry(_train.Figures), string.Empty, string.Empty, string.Empty, _train.RotateText);
                        //выводи объект на панель
                        DrawCanvas.Children.Add(new_train);
                        DrawCanvas.Children.Add(new_train.Text);
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
              //рисуем блок участки
                try
                {
                    foreach (TrafficTrain.DataGrafik.LinePeregonSave _block in Project.Peregon)
                    {
                        int index = 1;
                        List<BlockSection> blocks = new List<BlockSection>();
                        if (_block.StationNumber == 160002 )
                        {
                        }
                        try
                        {
                            foreach (SettingBlock block in AnalisLinePeregon(_block))
                            {
                                if (index % 2 == 0)
                                {
                                    BlockSection newblock = new BlockSection(block.StationNumber, block.StationNumberRight, GetPathGeometry(block.Figure), block.Name, block.NameMove ,  Brushes.Black);
                                    if (block.Points.Count >= 3)
                                        blocks.Insert(0, newblock);
                                    else blocks.Add(newblock);
                                }
                                else
                                {
                                    BlockSection newblock = new BlockSection(block.StationNumber, block.StationNumberRight, GetPathGeometry(block.Figure), block.Name, block.NameMove, Brushes.Red);
                                    if (block.Points.Count >= 3)
                                        blocks.Insert(0, newblock);
                                    else blocks.Add(newblock);
                                }
                                index++;
                            }
                        }
                        catch { }
                        //
                        if (blocks.Count > 0)
                        {
                            foreach (BlockSection newblock in blocks)
                                DrawCanvas.Children.Add(newblock);
                        }
                        else
                        {
                            BlockSection newblock = new BlockSection((int)_block.StationNumber, (int)_block.StationNumberRight, GetPathGeometry(_block.Figures), _block.Name, _block.Name, BlockSection._colornotcontrolstroke);
                            DrawCanvas.Children.Add(newblock);
                        }
                    }
                }
                catch (Exception error) { log.Error(error.Message); }
            }
        }

        private Move.StrageProject GetPeregon(int StationNumberLeft, int StationNumberRight, string NamePath)
        {
            try
            {
                foreach (Move.StrageProject row in ProejctMove.Moves)
                {
                    if (row.Infostrage.Stationnumberleft == StationNumberLeft && row.Infostrage.Stationnumberright == StationNumberRight && row.NameMove == NamePath)
                    {
                        return row;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// поиск импульсов ТУ переездов
        /// </summary>
        /// <param name="stationnumber">номер станции контроля переезда</param>
        /// <param name="namemove">название переезда</param>
        /// <returns></returns>
        private List<StateElement> GetImpulsesMove(int stationnumber, string namemove)
        {
            List<StateElement> impulses = new List<StateElement>();
            if (ProejctMove != null)
            {
                foreach (Move.StrageProject rowstation in ProejctMove.Moves)
                {
                    if (rowstation.Infostrage.Stationnumberleft == stationnumber || rowstation.Infostrage.Stationnumberright == stationnumber)
                    {
                        foreach (Move.MoveProject move in rowstation.MoveElements)
                        {
                            if (move.NameMove == namemove)
                            {
                                foreach (Move.StateElement imp in move.Impulses)
                                    impulses.Add(new StateElement() { Name = imp.Name, Impuls = imp.Impuls });
                            }
                        }
                    }
                }
            }
            return impulses;
        }


        /// <summary>
        /// поиск импульсов ТУ КГУ
        /// </summary>
        /// <param name="stationnumber">номер станции контроля кгу</param>
        /// <param name="namekgu">название кгу</param>
        /// <returns></returns>
        private List<StateElement> GetImpulsesKGU(int stationnumber, string namekgu)
        {
            List<StateElement> impulses = new List<StateElement>();
            if (ProejctMove != null)
            {
                foreach (Move.StrageProject rowstation in ProejctMove.Moves)
                {
                    if (rowstation.Infostrage.Stationnumberleft == stationnumber || rowstation.Infostrage.Stationnumberright == stationnumber)
                    {
                        foreach (Move.KGU kgu in rowstation.KGUelements)
                        {
                            if (kgu.NameKGU == namekgu)
                            {
                                foreach (Move.StateElement imp in kgu.Impulses)
                                    impulses.Add(new StateElement() { Name = imp.Name, Impuls = imp.Impuls });
                            }
                        }
                    }
                }
            }
            return impulses;
        }

        /// <summary>
        /// поиск импульсов ТУ КТСМ
        /// </summary>
        /// <param name="stationnumber">номер станции контроля КТСМ</param>
        /// <param name="namektcm">название КТСМ</param>
        /// <returns></returns>
        private List<StateElement> GetImpulsesKTCM(int stationnumber, string namektcm)
        {
            List<StateElement> impulses = new List<StateElement>();
            if (ProejctMove != null)
            {
                foreach (Move.StrageProject rowstation in ProejctMove.Moves)
                {
                    if (rowstation.Infostrage.Stationnumberleft == stationnumber || rowstation.Infostrage.Stationnumberright == stationnumber)
                    {
                        foreach (Move.KTCM ktcm in rowstation.KTCMelements)
                        {
                            if (ktcm.NameKTCM == namektcm)
                            {
                                foreach (Move.StateElement imp in ktcm.Impulses)
                                    impulses.Add(new StateElement() { Name = imp.Name, Impuls = imp.Impuls });
                            }
                        }
                    }
                }
            }
            return impulses;
        }

        /// <summary>
        /// поиск импульсов ТУ КТСМ
        /// </summary>
        /// <param name="stationnumber">номер станции контроля КТСМ</param>
        /// <param name="namektcm">название КТСМ</param>
        /// <returns></returns>
        private List<StateElement> GetBlockDirection(int stationnumbereft,int stationnumberight, string namepath)
        {
            List<StateElement> impulses = new List<StateElement>();
            if (ProejctMove != null)
            {
                foreach (Move.StrageProject rowstation in ProejctMove.Moves)
                {
                    if (rowstation.Infostrage.Stationnumberleft == stationnumbereft && rowstation.Infostrage.Stationnumberright == stationnumberight && rowstation.NameMove == namepath)
                    {
                       
                    }
                }
            }
            return impulses;
        }

        /// <summary>
        /// заполняем массив состояний ТС для каждого активного элемента
        /// </summary>
        private void FullImpulsesElement(int station_number, string index, string name_element,  ImpulsTSElement activ)
        {
            switch (index)
            {
                case ViewNameSostNumberTS.big_path:
                    activ.Impulses = GetImpulses(name_element, station_number);
                    break;
                //case ViewNameSostNumberTS.move:
                //    activ.Impulses = GetImpulses(name_element, station_number);
                //    break;
                //case ViewNameSostNumberTS.kgu:
                //    activ.Impulses = GetImpulses(name_element, station_number);
                   // break;
                //case ViewNameSostNumberTS.ktcm:
                //    activ.Impulses = GetImpulses(name_element, station_number);
                 //   break;
                case ViewNameSostNumberTS.button_station:
                    activ.Impulses = GetImpulses(name_element, station_number);
                    break;
                case ViewNameSostNumberTS.signal:
                    activ.Impulses = GetImpulses(name_element, station_number);
                    break;
                //case ViewNameSostNumberTS.peregon_arrow:
                //    activ.Impulses = GetImpulses(name_element, station_number);
                //    ArrowMove arrow = activ as ArrowMove;
                //    if (arrow != null)
                //        arrow.ImpulsesRight = GetImpulses(string.Format("{0}-{1}", ViewNameSostNumberTS.peregon_arrow, arrow.Graniza.Split(new char[] { '-' })[1]), arrow.StationNumberRight);
                //    break;
            }
        }

        private List<StateElement> GetImpulses(string name_element, int station_number)
        {
            List<StateElement> impulses = new List<StateElement>();
            try
            {
                foreach (StateValueTs value in TS_list[station_number].NamesValue[name_element])
                    impulses.Add(new StateElement() { Name = value.View, Impuls = value.Formula });
                return impulses;
            }
            catch
            { 
                return impulses; 
            } 
        }

        /// <summary>
        /// Загружаем и отображаем данные файлов конфигурации
        /// </summary>
        /// <param name="Palitra">палитра для отрисовки</param>
        public void Load(ref Canvas Palitra)
        {
            //
            if (LoadInfo != null)
                LoadInfo("Загрузка таблиц ТС и ТУ станций участка");
            LoadImpulsInformation();
            //получаем проект по перегонам
            if (LoadInfo != null)
                LoadInfo("Загрузка информации по перегонам");
            ProejctMove = LoadStrageInfo();
            //получаем проект и отрисовываем его рисуем 
            if (LoadInfo != null)
                LoadInfo("Загрузка графики участка");
            ProejctGrafic = LoadGrafickProject();
            DrawGrafick(ProejctGrafic, ProejctMove, ref Palitra);
        }


        public static void LoadColorAll()
        {
            if (LoadInfo != null)
                LoadInfo("Загрузка проекта цвета");
            ColorProject = LoadColor();
            AnalisColor();
        }
        /// <summary>
        /// анализируем цветовую палитру
        /// </summary>
         private static void AnalisColor()
        {
            ProverkaColor();
        }

         private static void ProverkaColor()
         {
             if (ColorProject == null)
                 ColorProject = new ColorSaveProejct();
             //основные цвета
             if (ColorProject.ColorFon == null)
                 ColorProject.ColorFon = new ColorN() { R = ((SolidColorBrush)MainWindow._colorfon).Color.R, G = ((SolidColorBrush)MainWindow._colorfon).Color.G, B = ((SolidColorBrush)MainWindow._colorfon).Color.B };
             else MainWindow._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorFon.R, ColorProject.ColorFon.G, ColorProject.ColorFon.B));
             if (ColorProject.ColorArrowCommand == null)
                 ColorProject.ColorArrowCommand = new ColorN() { R = ((SolidColorBrush)MainWindow._colorselect).Color.R, G = ((SolidColorBrush)MainWindow._colorselect).Color.G, B = ((SolidColorBrush)MainWindow._colorselect).Color.B };
             else MainWindow._colorselect = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrowCommand.R, ColorProject.ColorArrowCommand.G, ColorProject.ColorArrowCommand.B));

             ////Цветовой набор для главного пути
             if (ColorProject.ColorPath == null)
             {
                 ColorProject.ColorPath = new ColorPath();
                 ColorProject.ColorPath.ColorActiv = new ColorN() { R = ((SolidColorBrush)StationPath._coloractiv).Color.R, G = ((SolidColorBrush)StationPath._coloractiv).Color.G, B = ((SolidColorBrush)StationPath._coloractiv).Color.B };
                 ColorProject.ColorPath.ColorAuto = new ColorN() { R = ((SolidColorBrush)StationPath._colorauto_action).Color.R, G = ((SolidColorBrush)StationPath._colorauto_action).Color.G, B = ((SolidColorBrush)StationPath._colorauto_action).Color.B };
                 ColorProject.ColorPath.ColorDieselTraction = new ColorN() { R = ((SolidColorBrush)StationPath._colordiesel_traction).Color.R, G = ((SolidColorBrush)StationPath._colordiesel_traction).Color.G, B = ((SolidColorBrush)StationPath._colordiesel_traction).Color.B };
                 ColorProject.ColorPath.ColorElectricTraction = new ColorN() { R = ((SolidColorBrush)StationPath._colorelectric_traction).Color.R, G = ((SolidColorBrush)StationPath._colorelectric_traction).Color.G, B = ((SolidColorBrush)StationPath._colorelectric_traction).Color.B };
                 ColorProject.ColorPath.ColorFencing = new ColorN() { R = ((SolidColorBrush)StationPath._colorfencing).Color.R, G = ((SolidColorBrush)StationPath._colorfencing).Color.G, B = ((SolidColorBrush)StationPath._colorfencing).Color.B };
                 ColorProject.ColorPath.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)StationPath._colornotcontrol).Color.R, G = ((SolidColorBrush)StationPath._colornotcontrol).Color.G, B = ((SolidColorBrush)StationPath._colornotcontrol).Color.B };
                 ColorProject.ColorPath.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)StationPath._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)StationPath._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)StationPath._colornotcontrolstroke).Color.B };
                 ColorProject.ColorPath.ColorPasiv = new ColorN() { R = ((SolidColorBrush)StationPath._colorpassiv).Color.R, G = ((SolidColorBrush)StationPath._colorpassiv).Color.G, B = ((SolidColorBrush)StationPath._colorpassiv).Color.B };
                 ColorProject.ColorPath.ColorPathName = new ColorN() { R = ((SolidColorBrush)StationPath._color_path).Color.R, G = ((SolidColorBrush)StationPath._color_path).Color.G, B = ((SolidColorBrush)StationPath._color_path).Color.B };
                 ColorProject.ColorPath.ColorTrain = new ColorN() { R = ((SolidColorBrush)StationPath._color_train).Color.R, G = ((SolidColorBrush)StationPath._color_train).Color.G, B = ((SolidColorBrush)StationPath._color_train).Color.B };
                 ColorProject.ColorPath.ColorTrainVertor = new ColorN() { R = ((SolidColorBrush)StationPath._color_vertor_train).Color.R, G = ((SolidColorBrush)StationPath._color_vertor_train).Color.G, B = ((SolidColorBrush)StationPath._color_vertor_train).Color.B };
             }
             else
             {
                 if(ColorProject.ColorPath.ColorActiv == null)
                     ColorProject.ColorPath.ColorActiv = new ColorN() { R = ((SolidColorBrush)StationPath._coloractiv).Color.R, G = ((SolidColorBrush)StationPath._coloractiv).Color.G, B = ((SolidColorBrush)StationPath._coloractiv).Color.B };
                 else StationPath._coloractiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorActiv.R, ColorProject.ColorPath.ColorActiv.G, ColorProject.ColorPath.ColorActiv.B));
                 //
                 if (ColorProject.ColorPath.ColorAuto == null)
                     ColorProject.ColorPath.ColorAuto = new ColorN() { R = ((SolidColorBrush)StationPath._colorauto_action).Color.R, G = ((SolidColorBrush)StationPath._colorauto_action).Color.G, B = ((SolidColorBrush)StationPath._colorauto_action).Color.B };
                 else StationPath._colorauto_action = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorAuto.R, ColorProject.ColorPath.ColorAuto.G, ColorProject.ColorPath.ColorAuto.B));
                 //
                 if (ColorProject.ColorPath.ColorDieselTraction == null)
                     ColorProject.ColorPath.ColorDieselTraction = new ColorN() { R = ((SolidColorBrush)StationPath._colordiesel_traction).Color.R, G = ((SolidColorBrush)StationPath._colordiesel_traction).Color.G, B = ((SolidColorBrush)StationPath._colordiesel_traction).Color.B };
                 else StationPath._colordiesel_traction = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorDieselTraction.R, ColorProject.ColorPath.ColorDieselTraction.G, ColorProject.ColorPath.ColorDieselTraction.B));
                 //
                 if (ColorProject.ColorPath.ColorElectricTraction == null)
                     ColorProject.ColorPath.ColorElectricTraction = new ColorN() { R = ((SolidColorBrush)StationPath._colorelectric_traction).Color.R, G = ((SolidColorBrush)StationPath._colorelectric_traction).Color.G, B = ((SolidColorBrush)StationPath._colorelectric_traction).Color.B };
                 else StationPath._colorelectric_traction = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorElectricTraction.R, ColorProject.ColorPath.ColorElectricTraction.G, ColorProject.ColorPath.ColorElectricTraction.B));
                 //
                 if (ColorProject.ColorPath.ColorFencing == null)
                     ColorProject.ColorPath.ColorFencing = new ColorN() { R = ((SolidColorBrush)StationPath._colorfencing).Color.R, G = ((SolidColorBrush)StationPath._colorfencing).Color.G, B = ((SolidColorBrush)StationPath._colorfencing).Color.B };
                 else StationPath._colorfencing = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorFencing.R, ColorProject.ColorPath.ColorFencing.G, ColorProject.ColorPath.ColorFencing.B));
                 //
                 if (ColorProject.ColorPath.ColorFillNotControl == null)
                     ColorProject.ColorPath.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)StationPath._colornotcontrol).Color.R, G = ((SolidColorBrush)StationPath._colornotcontrol).Color.G, B = ((SolidColorBrush)StationPath._colornotcontrol).Color.B };
                 else StationPath._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorFillNotControl.R, ColorProject.ColorPath.ColorFillNotControl.G, ColorProject.ColorPath.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorPath.ColorNotControlStroke == null)
                     ColorProject.ColorPath.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)StationPath._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)StationPath._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)StationPath._colornotcontrolstroke).Color.B };
                 else StationPath._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorNotControlStroke.R, ColorProject.ColorPath.ColorNotControlStroke.G, ColorProject.ColorPath.ColorNotControlStroke.B));
                 //
                 if (ColorProject.ColorPath.ColorPasiv == null)
                     ColorProject.ColorPath.ColorPasiv = new ColorN() { R = ((SolidColorBrush)StationPath._colorpassiv).Color.R, G = ((SolidColorBrush)StationPath._colorpassiv).Color.G, B = ((SolidColorBrush)StationPath._colorpassiv).Color.B };
                 else StationPath._colorpassiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorPasiv.R, ColorProject.ColorPath.ColorPasiv.G, ColorProject.ColorPath.ColorPasiv.B));
                 //
                 if (ColorProject.ColorPath.ColorPathName == null)
                     ColorProject.ColorPath.ColorPathName = new ColorN() { R = ((SolidColorBrush)StationPath._color_path).Color.R, G = ((SolidColorBrush)StationPath._color_path).Color.G, B = ((SolidColorBrush)StationPath._color_path).Color.B };
                 else StationPath._color_path = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorPathName.R, ColorProject.ColorPath.ColorPathName.G, ColorProject.ColorPath.ColorPathName.B));
                 //
                 if (ColorProject.ColorPath.ColorTrain == null)
                     ColorProject.ColorPath.ColorTrain = new ColorN() { R = ((SolidColorBrush)StationPath._color_train).Color.R, G = ((SolidColorBrush)StationPath._color_train).Color.G, B = ((SolidColorBrush)StationPath._color_train).Color.B };
                 else StationPath._color_train = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorTrain.R, ColorProject.ColorPath.ColorTrain.G, ColorProject.ColorPath.ColorTrain.B));
                 //
                 if (ColorProject.ColorPath.ColorTrainVertor == null)
                     ColorProject.ColorPath.ColorTrainVertor = new ColorN() { R = ((SolidColorBrush)StationPath._color_vertor_train).Color.R, G = ((SolidColorBrush)StationPath._color_vertor_train).Color.G, B = ((SolidColorBrush)StationPath._color_vertor_train).Color.B };
                 else StationPath._color_vertor_train = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorTrainVertor.R, ColorProject.ColorPath.ColorTrainVertor.G, ColorProject.ColorPath.ColorTrainVertor.B));
                 //
             }


             ////Цветовой набор для сигнала
             if (ColorProject.ColorSignal == null)
             {
                 ColorProject.ColorSignal = new ColorSignal();
                 ColorProject.ColorSignal.ColorBusy = new ColorN() { R = ((SolidColorBrush)Signal._color_busy).Color.R, G = ((SolidColorBrush)Signal._color_busy).Color.G, B = ((SolidColorBrush)Signal._color_busy).Color.B };
                 ColorProject.ColorSignal.ColorClosed = new ColorN() { R = ((SolidColorBrush)Signal._color_closed).Color.R, G = ((SolidColorBrush)Signal._color_closed).Color.G, B = ((SolidColorBrush)Signal._color_closed).Color.B };
                 ColorProject.ColorSignal.ColorFault = new ColorN() { R = ((SolidColorBrush)Signal._color_fault).Color.R, G = ((SolidColorBrush)Signal._color_fault).Color.G, B = ((SolidColorBrush)Signal._color_fault).Color.B };
                 ColorProject.ColorSignal.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)Signal._colornotcontrol).Color.R, G = ((SolidColorBrush)Signal._colornotcontrol).Color.G, B = ((SolidColorBrush)Signal._colornotcontrol).Color.B };
                 ColorProject.ColorSignal.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)Signal._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)Signal._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)Signal._colornotcontrolstroke).Color.B };
                 ColorProject.ColorSignal.ColorInstall = new ColorN() { R = ((SolidColorBrush)Signal._color_install).Color.R, G = ((SolidColorBrush)Signal._color_install).Color.G, B = ((SolidColorBrush)Signal._color_install).Color.B };
                 ColorProject.ColorSignal.ColorFree = new ColorN() { R = ((SolidColorBrush)Signal._colorfree).Color.R, G = ((SolidColorBrush)Signal._colorfree).Color.G, B = ((SolidColorBrush)Signal._colorfree).Color.B };
                 ColorProject.ColorSignal.ColorOpen = new ColorN() { R = ((SolidColorBrush)Signal._color_open).Color.R, G = ((SolidColorBrush)Signal._color_open).Color.G, B = ((SolidColorBrush)Signal._color_open).Color.B };
                 ColorProject.ColorSignal.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)Signal._color_ramka_defult).Color.R, G = ((SolidColorBrush)Signal._color_ramka_defult).Color.G, B = ((SolidColorBrush)Signal._color_ramka_defult).Color.B };
                 ColorProject.ColorSignal.ColorShunting = new ColorN() { R = ((SolidColorBrush)Signal._color_shunting).Color.R, G = ((SolidColorBrush)Signal._color_shunting).Color.G, B = ((SolidColorBrush)Signal._color_shunting).Color.B };
                 ColorProject.ColorSignal.ColorInvitationalOne = new ColorN() { R = ((SolidColorBrush)Signal._color_invitational_one).Color.R, G = ((SolidColorBrush)Signal._color_invitational_one).Color.G, B = ((SolidColorBrush)Signal._color_invitational_one).Color.B };
                 ColorProject.ColorSignal.ColorInvitationalTy = new ColorN() { R = ((SolidColorBrush)Signal._color_invitational_ty).Color.R, G = ((SolidColorBrush)Signal._color_invitational_ty).Color.G, B = ((SolidColorBrush)Signal._color_invitational_ty).Color.B };
                 ColorProject.ColorSignal.ColorInstallRoute = new ColorN() { R = ((SolidColorBrush)Signal._color_install_route).Color.R, G = ((SolidColorBrush)Signal._color_install_route).Color.G, B = ((SolidColorBrush)Signal._color_install_route).Color.B };
             }
             else
             {
                 if (ColorProject.ColorSignal.ColorBusy == null)
                     ColorProject.ColorSignal.ColorBusy = new ColorN() { R = ((SolidColorBrush)Signal._color_busy).Color.R, G = ((SolidColorBrush)Signal._color_busy).Color.G, B = ((SolidColorBrush)Signal._color_busy).Color.B };
                 else Signal._color_busy = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorBusy.R, ColorProject.ColorSignal.ColorBusy.G, ColorProject.ColorSignal.ColorBusy.B));
                 //
                 if (ColorProject.ColorSignal.ColorClosed == null)
                     ColorProject.ColorSignal.ColorClosed = new ColorN() { R = ((SolidColorBrush)Signal._color_closed).Color.R, G = ((SolidColorBrush)Signal._color_closed).Color.G, B = ((SolidColorBrush)Signal._color_closed).Color.B };
                 else Signal._color_closed = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorClosed.R, ColorProject.ColorSignal.ColorClosed.G, ColorProject.ColorSignal.ColorClosed.B));
                 //
                 if (ColorProject.ColorSignal.ColorFault == null)
                     ColorProject.ColorSignal.ColorFault = new ColorN() { R = ((SolidColorBrush)Signal._color_fault).Color.R, G = ((SolidColorBrush)Signal._color_fault).Color.G, B = ((SolidColorBrush)Signal._color_fault).Color.B };
                 else Signal._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorFault.R, ColorProject.ColorSignal.ColorFault.G, ColorProject.ColorSignal.ColorFault.B));
                 //
                 if (ColorProject.ColorSignal.ColorFillNotControl == null)
                     ColorProject.ColorSignal.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)Signal._colornotcontrol).Color.R, G = ((SolidColorBrush)Signal._colornotcontrol).Color.G, B = ((SolidColorBrush)Signal._colornotcontrol).Color.B };
                 else Signal._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorFillNotControl.R, ColorProject.ColorSignal.ColorFillNotControl.G, ColorProject.ColorSignal.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorSignal.ColorFree == null)
                     ColorProject.ColorSignal.ColorFree = new ColorN() { R = ((SolidColorBrush)Signal._colorfree).Color.R, G = ((SolidColorBrush)Signal._colorfree).Color.G, B = ((SolidColorBrush)Signal._colorfree).Color.B };
                 else Signal._colorfree = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorFree.R, ColorProject.ColorSignal.ColorFree.G, ColorProject.ColorSignal.ColorFree.B));
                 //
                 if (ColorProject.ColorSignal.ColorInstall == null)
                     ColorProject.ColorSignal.ColorInstall = new ColorN() { R = ((SolidColorBrush)Signal._color_install).Color.R, G = ((SolidColorBrush)Signal._color_install).Color.G, B = ((SolidColorBrush)Signal._color_install).Color.B };
                 else Signal._color_install = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInstall.R, ColorProject.ColorSignal.ColorInstall.G, ColorProject.ColorSignal.ColorInstall.B));
                 //
                 if (ColorProject.ColorSignal.ColorInvitationalOne == null)
                     ColorProject.ColorSignal.ColorInvitationalOne = new ColorN() { R = ((SolidColorBrush)Signal._color_invitational_one).Color.R, G = ((SolidColorBrush)Signal._color_invitational_one).Color.G, B = ((SolidColorBrush)Signal._color_invitational_one).Color.B };
                 else Signal._color_invitational_one = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInvitationalOne.R, ColorProject.ColorSignal.ColorInvitationalOne.G, ColorProject.ColorSignal.ColorInvitationalOne.B));
                 //
                 if (ColorProject.ColorSignal.ColorInvitationalTy == null)
                     ColorProject.ColorSignal.ColorInvitationalTy = new ColorN() { R = ((SolidColorBrush)Signal._color_invitational_ty).Color.R, G = ((SolidColorBrush)Signal._color_invitational_ty).Color.G, B = ((SolidColorBrush)Signal._color_invitational_ty).Color.B };
                 else Signal._color_invitational_ty = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInvitationalTy.R, ColorProject.ColorSignal.ColorInvitationalTy.G, ColorProject.ColorSignal.ColorInvitationalTy.B));
                 //
                 if (ColorProject.ColorSignal.ColorNotControlStroke == null)
                     ColorProject.ColorSignal.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)Signal._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)Signal._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)Signal._colornotcontrolstroke).Color.B };
                 else Signal._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorNotControlStroke.R, ColorProject.ColorSignal.ColorNotControlStroke.G, ColorProject.ColorSignal.ColorNotControlStroke.B));
                 //
                 if (ColorProject.ColorSignal.ColorOpen == null)
                     ColorProject.ColorSignal.ColorOpen = new ColorN() { R = ((SolidColorBrush)Signal._color_open).Color.R, G = ((SolidColorBrush)Signal._color_open).Color.G, B = ((SolidColorBrush)Signal._color_open).Color.B };
                 else Signal._color_open = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorOpen.R, ColorProject.ColorSignal.ColorOpen.G, ColorProject.ColorSignal.ColorOpen.B));
                 //
                 if (ColorProject.ColorSignal.ColorRamkaDefult == null)
                     ColorProject.ColorSignal.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)Signal._color_ramka_defult).Color.R, G = ((SolidColorBrush)Signal._color_ramka_defult).Color.G, B = ((SolidColorBrush)Signal._color_ramka_defult).Color.B };
                 else Signal._color_ramka_defult = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorRamkaDefult.R, ColorProject.ColorSignal.ColorRamkaDefult.G, ColorProject.ColorSignal.ColorRamkaDefult.B));
                 //
                 if (ColorProject.ColorSignal.ColorShunting == null)
                     ColorProject.ColorSignal.ColorShunting = new ColorN() { R = ((SolidColorBrush)Signal._color_shunting).Color.R, G = ((SolidColorBrush)Signal._color_shunting).Color.G, B = ((SolidColorBrush)Signal._color_shunting).Color.B };
                 else Signal._color_shunting = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorShunting.R, ColorProject.ColorSignal.ColorShunting.G, ColorProject.ColorSignal.ColorShunting.B));
                 //
                 if (ColorProject.ColorSignal.ColorInstallRoute == null)
                     ColorProject.ColorSignal.ColorInstallRoute = new ColorN() { R = ((SolidColorBrush)Signal._color_install_route).Color.R, G = ((SolidColorBrush)Signal._color_install_route).Color.G, B = ((SolidColorBrush)Signal._color_install_route).Color.B };
                 else Signal._color_install_route = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInstallRoute.R, ColorProject.ColorSignal.ColorInstallRoute.G, ColorProject.ColorSignal.ColorInstallRoute.B));
                 //
             }


             ////Цветовой набор для кнопки станции
             if (ColorProject.ColorButtonStation == null)
             {
                 ColorProject.ColorButtonStation = new ColorButtonStation();
                 ColorProject.ColorButtonStation.ColorAccident = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_accident).Color.R, G = ((SolidColorBrush)ButtonStation._color_accident).Color.G, B = ((SolidColorBrush)ButtonStation._color_accident).Color.B };
                 ColorProject.ColorButtonStation.ColorAutonomousControl = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_autonomous_control).Color.R, G = ((SolidColorBrush)ButtonStation._color_autonomous_control).Color.G, B = ((SolidColorBrush)ButtonStation._color_autonomous_control).Color.B };
                 ColorProject.ColorButtonStation.ColorDispatcher = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_dispatcher).Color.R, G = ((SolidColorBrush)ButtonStation._color_dispatcher).Color.G, B = ((SolidColorBrush)ButtonStation._color_dispatcher).Color.B };
                 ColorProject.ColorButtonStation.ColorFault = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_fault).Color.R, G = ((SolidColorBrush)ButtonStation._color_fault).Color.G, B = ((SolidColorBrush)ButtonStation._color_fault).Color.B };
                 ColorProject.ColorButtonStation.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)ButtonStation._colornotcontrol).Color.R, G = ((SolidColorBrush)ButtonStation._colornotcontrol).Color.G, B = ((SolidColorBrush)ButtonStation._colornotcontrol).Color.B };
                 ColorProject.ColorButtonStation.ColorFire = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_fire).Color.R, G = ((SolidColorBrush)ButtonStation._color_fire).Color.G, B = ((SolidColorBrush)ButtonStation._color_fire).Color.B };
                 ColorProject.ColorButtonStation.ColorFon = new ColorN() { R = ((SolidColorBrush)Signal._colorfree).Color.R, G = ((SolidColorBrush)Signal._colorfree).Color.G, B = ((SolidColorBrush)Signal._colorfree).Color.B };
                 ColorProject.ColorButtonStation.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)ButtonStation._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)ButtonStation._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)ButtonStation._colornotcontrolstroke).Color.B };
                 ColorProject.ColorButtonStation.ColorNotLink = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_notlink).Color.R, G = ((SolidColorBrush)ButtonStation._color_notlink).Color.G, B = ((SolidColorBrush)ButtonStation._color_notlink).Color.B };
                 ColorProject.ColorButtonStation.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_normal).Color.R, G = ((SolidColorBrush)ButtonStation._color_normal).Color.G, B = ((SolidColorBrush)ButtonStation._color_normal).Color.B };
                 ColorProject.ColorButtonStation.ColorReserveControl = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_reserve_control).Color.R, G = ((SolidColorBrush)ButtonStation._color_reserve_control).Color.G, B = ((SolidColorBrush)ButtonStation._color_reserve_control).Color.B };
                 ColorProject.ColorButtonStation.ColorSesonContol = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_sesoncontol).Color.R, G = ((SolidColorBrush)ButtonStation._color_sesoncontol).Color.G, B = ((SolidColorBrush)ButtonStation._color_sesoncontol).Color.B };
             }
             else
             {
                 if (ColorProject.ColorButtonStation.ColorAccident == null)
                     ColorProject.ColorButtonStation.ColorAccident = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_accident).Color.R, G = ((SolidColorBrush)ButtonStation._color_accident).Color.G, B = ((SolidColorBrush)ButtonStation._color_accident).Color.B };
                 else ButtonStation._color_accident = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorAccident.R, ColorProject.ColorButtonStation.ColorAccident.G, ColorProject.ColorButtonStation.ColorAccident.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorAutonomousControl == null)
                     ColorProject.ColorButtonStation.ColorAutonomousControl = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_autonomous_control).Color.R, G = ((SolidColorBrush)ButtonStation._color_autonomous_control).Color.G, B = ((SolidColorBrush)ButtonStation._color_autonomous_control).Color.B };
                 else ButtonStation._color_autonomous_control = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorAutonomousControl.R, ColorProject.ColorButtonStation.ColorAutonomousControl.G, ColorProject.ColorButtonStation.ColorAutonomousControl.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorDispatcher == null)
                     ColorProject.ColorButtonStation.ColorDispatcher = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_dispatcher).Color.R, G = ((SolidColorBrush)ButtonStation._color_dispatcher).Color.G, B = ((SolidColorBrush)ButtonStation._color_dispatcher).Color.B };
                 else ButtonStation._color_dispatcher = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorDispatcher.R, ColorProject.ColorButtonStation.ColorDispatcher.G, ColorProject.ColorButtonStation.ColorDispatcher.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorFault == null)
                     ColorProject.ColorButtonStation.ColorFault = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_fault).Color.R, G = ((SolidColorBrush)ButtonStation._color_fault).Color.G, B = ((SolidColorBrush)ButtonStation._color_fault).Color.B };
                 else ButtonStation._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFault.R, ColorProject.ColorButtonStation.ColorFault.G, ColorProject.ColorButtonStation.ColorFault.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorFillNotControl == null)
                     ColorProject.ColorButtonStation.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)ButtonStation._colornotcontrol).Color.R, G = ((SolidColorBrush)ButtonStation._colornotcontrol).Color.G, B = ((SolidColorBrush)ButtonStation._colornotcontrol).Color.B };
                 else ButtonStation._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFillNotControl.R, ColorProject.ColorButtonStation.ColorFillNotControl.G, ColorProject.ColorButtonStation.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorFire == null)
                     ColorProject.ColorButtonStation.ColorFire = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_fire).Color.R, G = ((SolidColorBrush)ButtonStation._color_fire).Color.G, B = ((SolidColorBrush)ButtonStation._color_fire).Color.B };
                 else ButtonStation._color_fire = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFire.R, ColorProject.ColorButtonStation.ColorFire.G, ColorProject.ColorButtonStation.ColorFire.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorFon == null)
                     ColorProject.ColorButtonStation.ColorFon = new ColorN() { R = ((SolidColorBrush)Signal._colorfree).Color.R, G = ((SolidColorBrush)Signal._colorfree).Color.G, B = ((SolidColorBrush)Signal._colorfree).Color.B };
                 else ButtonStation._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFon.R, ColorProject.ColorButtonStation.ColorFon.G, ColorProject.ColorButtonStation.ColorFon.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorNotControlStroke == null)
                     ColorProject.ColorButtonStation.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)ButtonStation._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)ButtonStation._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)ButtonStation._colornotcontrolstroke).Color.B };
                 else ButtonStation._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorNotControlStroke.R, ColorProject.ColorButtonStation.ColorNotControlStroke.G, ColorProject.ColorButtonStation.ColorNotControlStroke.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorNotLink == null)
                     ColorProject.ColorButtonStation.ColorNotLink = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_notlink).Color.R, G = ((SolidColorBrush)ButtonStation._color_notlink).Color.G, B = ((SolidColorBrush)ButtonStation._color_notlink).Color.B };
                 else ButtonStation._color_notlink = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorNotLink.R, ColorProject.ColorButtonStation.ColorNotLink.G, ColorProject.ColorButtonStation.ColorNotLink.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorRamkaDefult == null)
                     ColorProject.ColorButtonStation.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_normal).Color.R, G = ((SolidColorBrush)ButtonStation._color_normal).Color.G, B = ((SolidColorBrush)ButtonStation._color_normal).Color.B };
                 else ButtonStation._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorRamkaDefult.R, ColorProject.ColorButtonStation.ColorRamkaDefult.G, ColorProject.ColorButtonStation.ColorRamkaDefult.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorReserveControl == null)
                     ColorProject.ColorButtonStation.ColorReserveControl = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_reserve_control).Color.R, G = ((SolidColorBrush)ButtonStation._color_reserve_control).Color.G, B = ((SolidColorBrush)ButtonStation._color_reserve_control).Color.B };
                 else ButtonStation._color_reserve_control = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorReserveControl.R, ColorProject.ColorButtonStation.ColorReserveControl.G, ColorProject.ColorButtonStation.ColorReserveControl.B));
                 //
                 if (ColorProject.ColorButtonStation.ColorSesonContol == null)
                     ColorProject.ColorButtonStation.ColorSesonContol = new ColorN() { R = ((SolidColorBrush)ButtonStation._color_sesoncontol).Color.R, G = ((SolidColorBrush)ButtonStation._color_sesoncontol).Color.G, B = ((SolidColorBrush)ButtonStation._color_sesoncontol).Color.B };
                 else ButtonStation._color_sesoncontol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorSesonContol.R, ColorProject.ColorButtonStation.ColorSesonContol.G, ColorProject.ColorButtonStation.ColorSesonContol.B));
                 //

             }


             ////Цветовой набор для переезда
             if (ColorProject.ColorMove == null)
             {
                 ColorProject.ColorMove = new ColorMove();
                 ColorProject.ColorMove.ColorAccident = new ColorN() { R = ((SolidColorBrush)Moves._color_accident).Color.R, G = ((SolidColorBrush)Moves._color_accident).Color.G, B = ((SolidColorBrush)Moves._color_accident).Color.B };
                 ColorProject.ColorMove.ColorClose = new ColorN() { R = ((SolidColorBrush)Moves._color_closingmove).Color.R, G = ((SolidColorBrush)Moves._color_closingmove).Color.G, B = ((SolidColorBrush)Moves._color_closingmove).Color.B };
                 ColorProject.ColorMove.ColorFault = new ColorN() { R = ((SolidColorBrush)Moves._color_faultmove).Color.R, G = ((SolidColorBrush)Moves._color_faultmove).Color.G, B = ((SolidColorBrush)Moves._color_faultmove).Color.B };
                 ColorProject.ColorMove.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)Moves._colornotcontrol).Color.R, G = ((SolidColorBrush)Moves._colornotcontrol).Color.G, B = ((SolidColorBrush)Moves._colornotcontrol).Color.B };
                 ColorProject.ColorMove.ColorFon = new ColorN() { R = ((SolidColorBrush)Moves._colorfon).Color.R, G = ((SolidColorBrush)Moves._colorfon).Color.G, B = ((SolidColorBrush)Moves._colorfon).Color.B };
                 ColorProject.ColorMove.ColorMoveOpen = new ColorN() { R = ((SolidColorBrush)Moves._color_moveopen).Color.R, G = ((SolidColorBrush)Moves._color_moveopen).Color.G, B = ((SolidColorBrush)Moves._color_moveopen).Color.B };
                 ColorProject.ColorMove.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)Moves._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)Moves._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)Moves._colornotcontrolstroke).Color.B };
             }
             else
             {
                 if (ColorProject.ColorMove.ColorAccident == null)
                     ColorProject.ColorMove.ColorAccident = new ColorN() { R = ((SolidColorBrush)Moves._color_accident).Color.R, G = ((SolidColorBrush)Moves._color_accident).Color.G, B = ((SolidColorBrush)Moves._color_accident).Color.B };
                 else Moves._color_accident = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorAccident.R, ColorProject.ColorMove.ColorAccident.G, ColorProject.ColorMove.ColorAccident.B));
                 //
                 if (ColorProject.ColorMove.ColorClose == null)
                     ColorProject.ColorMove.ColorClose = new ColorN() { R = ((SolidColorBrush)Moves._color_closingmove).Color.R, G = ((SolidColorBrush)Moves._color_closingmove).Color.G, B = ((SolidColorBrush)Moves._color_closingmove).Color.B };
                 else Moves._color_closingmove = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorClose.R, ColorProject.ColorMove.ColorClose.G, ColorProject.ColorMove.ColorClose.B));
                 //
                 if (ColorProject.ColorMove.ColorFault == null)
                     ColorProject.ColorMove.ColorFault = new ColorN() { R = ((SolidColorBrush)Moves._color_faultmove).Color.R, G = ((SolidColorBrush)Moves._color_faultmove).Color.G, B = ((SolidColorBrush)Moves._color_faultmove).Color.B };
                 else Moves._color_faultmove = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorFault.R, ColorProject.ColorMove.ColorFault.G, ColorProject.ColorMove.ColorFault.B));
                 //
                 if (ColorProject.ColorMove.ColorFillNotControl == null)
                     ColorProject.ColorMove.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)Moves._colornotcontrol).Color.R, G = ((SolidColorBrush)Moves._colornotcontrol).Color.G, B = ((SolidColorBrush)Moves._colornotcontrol).Color.B };
                 else Moves._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorFillNotControl.R, ColorProject.ColorMove.ColorFillNotControl.G, ColorProject.ColorMove.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorMove.ColorFon == null)
                     ColorProject.ColorMove.ColorFon = new ColorN() { R = ((SolidColorBrush)Moves._colorfon).Color.R, G = ((SolidColorBrush)Moves._colorfon).Color.G, B = ((SolidColorBrush)Moves._colorfon).Color.B };
                 else Moves._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorFon.R, ColorProject.ColorMove.ColorFon.G, ColorProject.ColorMove.ColorFon.B));
                 //
                 if (ColorProject.ColorMove.ColorMoveOpen == null)
                     ColorProject.ColorMove.ColorMoveOpen = new ColorN() { R = ((SolidColorBrush)Moves._color_moveopen).Color.R, G = ((SolidColorBrush)Moves._color_moveopen).Color.G, B = ((SolidColorBrush)Moves._color_moveopen).Color.B };
                 else Moves._color_moveopen = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorMoveOpen.R, ColorProject.ColorMove.ColorMoveOpen.G, ColorProject.ColorMove.ColorMoveOpen.B));
                 //
                 if (ColorProject.ColorMove.ColorNotControlStroke == null)
                     ColorProject.ColorMove.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)Moves._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)Moves._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)Moves._colornotcontrolstroke).Color.B };
                 else Moves._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorNotControlStroke.R, ColorProject.ColorMove.ColorNotControlStroke.G, ColorProject.ColorMove.ColorNotControlStroke.B));
                 //
             }

             ////Цветовой набор для КТСМ
             if (ColorProject.ColorKTCM == null)
             {
                 ColorProject.ColorKTCM = new ColorKTCM();
                 ColorProject.ColorKTCM.ColorAccident = new ColorN() { R = ((SolidColorBrush)KTCM._color_break).Color.R, G = ((SolidColorBrush)KTCM._color_break).Color.G, B = ((SolidColorBrush)KTCM._color_break).Color.B };
                 ColorProject.ColorKTCM.ColorFault = new ColorN() { R = ((SolidColorBrush)KTCM._color_fault).Color.R, G = ((SolidColorBrush)KTCM._color_fault).Color.G, B = ((SolidColorBrush)KTCM._color_fault).Color.B };
                 ColorProject.ColorKTCM.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)KTCM._colornotcontrol).Color.R, G = ((SolidColorBrush)KTCM._colornotcontrol).Color.G, B = ((SolidColorBrush)KTCM._colornotcontrol).Color.B };
                 ColorProject.ColorKTCM.ColorFon = new ColorN() { R = ((SolidColorBrush)KTCM._colorfon).Color.R, G = ((SolidColorBrush)KTCM._colorfon).Color.G, B = ((SolidColorBrush)KTCM._colorfon).Color.B };
                 ColorProject.ColorKTCM.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)KTCM._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)KTCM._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)KTCM._colornotcontrolstroke).Color.B };
                 ColorProject.ColorKTCM.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)KTCM._color_normal).Color.R, G = ((SolidColorBrush)KTCM._color_normal).Color.G, B = ((SolidColorBrush)KTCM._color_normal).Color.B };
             }
             else
             {
                 if (ColorProject.ColorKTCM.ColorAccident == null)
                     ColorProject.ColorKTCM.ColorAccident = new ColorN() { R = ((SolidColorBrush)KTCM._color_break).Color.R, G = ((SolidColorBrush)KTCM._color_break).Color.G, B = ((SolidColorBrush)KTCM._color_break).Color.B };
                 else KTCM._color_break = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorAccident.R, ColorProject.ColorKTCM.ColorAccident.G, ColorProject.ColorKTCM.ColorAccident.B));
                 //
                 if (ColorProject.ColorKTCM.ColorFault == null)
                     ColorProject.ColorKTCM.ColorFault = new ColorN() { R = ((SolidColorBrush)KTCM._color_fault).Color.R, G = ((SolidColorBrush)KTCM._color_fault).Color.G, B = ((SolidColorBrush)KTCM._color_fault).Color.B };
                 else KTCM._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorFault.R, ColorProject.ColorKTCM.ColorFault.G, ColorProject.ColorKTCM.ColorFault.B));
                 //
                 if (ColorProject.ColorKTCM.ColorFillNotControl == null)
                     ColorProject.ColorKTCM.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)KTCM._colornotcontrol).Color.R, G = ((SolidColorBrush)KTCM._colornotcontrol).Color.G, B = ((SolidColorBrush)KTCM._colornotcontrol).Color.B };
                 else KTCM._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorFillNotControl.R, ColorProject.ColorKTCM.ColorFillNotControl.G, ColorProject.ColorKTCM.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorKTCM.ColorFon == null)
                     ColorProject.ColorKTCM.ColorFon = new ColorN() { R = ((SolidColorBrush)KTCM._colorfon).Color.R, G = ((SolidColorBrush)KTCM._colorfon).Color.G, B = ((SolidColorBrush)KTCM._colorfon).Color.B };
                 else KTCM._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorFon.R, ColorProject.ColorKTCM.ColorFon.G, ColorProject.ColorKTCM.ColorFon.B));
                 //
                 if (ColorProject.ColorKTCM.ColorNotControlStroke == null)
                     ColorProject.ColorKTCM.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)KTCM._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)KTCM._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)KTCM._colornotcontrolstroke).Color.B };
                 else KTCM._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorNotControlStroke.R, ColorProject.ColorKTCM.ColorNotControlStroke.G, ColorProject.ColorKTCM.ColorNotControlStroke.B));
                 //
                 if (ColorProject.ColorKTCM.ColorRamkaDefult == null)
                     ColorProject.ColorKTCM.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)KTCM._color_normal).Color.R, G = ((SolidColorBrush)KTCM._color_normal).Color.G, B = ((SolidColorBrush)KTCM._color_normal).Color.B };
                 else KTCM._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorRamkaDefult.R, ColorProject.ColorKTCM.ColorRamkaDefult.G, ColorProject.ColorKTCM.ColorRamkaDefult.B));
                 //
             }

             ////Цветовой набор для КГУ
             if (ColorProject.ColorKGU == null)
             {
                 ColorProject.ColorKGU = new ColorKGU();
                 ColorProject.ColorKGU.ColorAccident = new ColorN() { R = ((SolidColorBrush)KGU._color_break).Color.R, G = ((SolidColorBrush)KGU._color_break).Color.G, B = ((SolidColorBrush)KGU._color_break).Color.B };
                 ColorProject.ColorKGU.ColorFault = new ColorN() { R = ((SolidColorBrush)KGU._color_fault).Color.R, G = ((SolidColorBrush)KGU._color_fault).Color.G, B = ((SolidColorBrush)KGU._color_fault).Color.B };
                 ColorProject.ColorKGU.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)KGU._colornotcontrol).Color.R, G = ((SolidColorBrush)KGU._colornotcontrol).Color.G, B = ((SolidColorBrush)KGU._colornotcontrol).Color.B };
                 ColorProject.ColorKGU.ColorFon = new ColorN() { R = ((SolidColorBrush)KGU._colorfon).Color.R, G = ((SolidColorBrush)KGU._colorfon).Color.G, B = ((SolidColorBrush)KGU._colorfon).Color.B };
                 ColorProject.ColorKGU.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)KGU._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)KGU._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)KGU._colornotcontrolstroke).Color.B };
                 ColorProject.ColorKGU.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)KGU._color_normal).Color.R, G = ((SolidColorBrush)KGU._color_normal).Color.G, B = ((SolidColorBrush)KGU._color_normal).Color.B };
             }
             else
             {
                 if (ColorProject.ColorKGU.ColorAccident == null)
                     ColorProject.ColorKGU.ColorAccident = new ColorN() { R = ((SolidColorBrush)KGU._color_break).Color.R, G = ((SolidColorBrush)KGU._color_break).Color.G, B = ((SolidColorBrush)KGU._color_break).Color.B };
                 else KTCM._color_break = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorAccident.R, ColorProject.ColorKGU.ColorAccident.G, ColorProject.ColorKGU.ColorAccident.B));
                 //
                 if (ColorProject.ColorKGU.ColorFault == null)
                     ColorProject.ColorKGU.ColorFault = new ColorN() { R = ((SolidColorBrush)KGU._color_fault).Color.R, G = ((SolidColorBrush)KGU._color_fault).Color.G, B = ((SolidColorBrush)KGU._color_fault).Color.B };
                 else KTCM._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorFault.R, ColorProject.ColorKGU.ColorFault.G, ColorProject.ColorKGU.ColorFault.B));
                 //
                 if (ColorProject.ColorKGU.ColorFillNotControl == null)
                     ColorProject.ColorKGU.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)KGU._colornotcontrol).Color.R, G = ((SolidColorBrush)KGU._colornotcontrol).Color.G, B = ((SolidColorBrush)KGU._colornotcontrol).Color.B };
                 else KTCM._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorFillNotControl.R, ColorProject.ColorKGU.ColorFillNotControl.G, ColorProject.ColorKGU.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorKGU.ColorFon == null)
                     ColorProject.ColorKGU.ColorFon = new ColorN() { R = ((SolidColorBrush)KGU._colorfon).Color.R, G = ((SolidColorBrush)KGU._colorfon).Color.G, B = ((SolidColorBrush)KGU._colorfon).Color.B };
                 else KTCM._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorFon.R, ColorProject.ColorKGU.ColorFon.G, ColorProject.ColorKGU.ColorFon.B));
                 //
                 if (ColorProject.ColorKGU.ColorNotControlStroke == null)
                     ColorProject.ColorKGU.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)KGU._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)KGU._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)KGU._colornotcontrolstroke).Color.B };
                 else KTCM._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorNotControlStroke.R, ColorProject.ColorKGU.ColorNotControlStroke.G, ColorProject.ColorKGU.ColorNotControlStroke.B));
                 //
                 if (ColorProject.ColorKGU.ColorRamkaDefult == null)
                     ColorProject.ColorKGU.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)KGU._color_normal).Color.R, G = ((SolidColorBrush)KGU._color_normal).Color.G, B = ((SolidColorBrush)KGU._color_normal).Color.B };
                 else KTCM._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorRamkaDefult.R, ColorProject.ColorKGU.ColorRamkaDefult.G, ColorProject.ColorKGU.ColorRamkaDefult.B));
                 //
             }
   
             ////Цветовой набор для элемента номера поездов
             if (ColorProject.ColorNumberTrain == null)
             {
                 ColorProject.ColorNumberTrain = new ColorNumberTrain();
                 ColorProject.ColorNumberTrain.ColorActiv = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._coloractiv).Color.R, G = ((SolidColorBrush)NumberTrainRamka._coloractiv).Color.G, B = ((SolidColorBrush)NumberTrainRamka._coloractiv).Color.B };
                 ColorProject.ColorNumberTrain.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._colornotcontrol).Color.R, G = ((SolidColorBrush)NumberTrainRamka._colornotcontrol).Color.G, B = ((SolidColorBrush)NumberTrainRamka._colornotcontrol).Color.B };
                 ColorProject.ColorNumberTrain.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)NumberTrainRamka._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)NumberTrainRamka._colornotcontrolstroke).Color.B };
                 ColorProject.ColorNumberTrain.ColorPasiv = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._color_pasiv).Color.R, G = ((SolidColorBrush)NumberTrainRamka._color_pasiv).Color.G, B = ((SolidColorBrush)NumberTrainRamka._color_pasiv).Color.B };
                 ColorProject.ColorNumberTrain.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._color_ramka).Color.R, G = ((SolidColorBrush)NumberTrainRamka._color_ramka).Color.G, B = ((SolidColorBrush)NumberTrainRamka._color_ramka).Color.B };
                 ColorProject.ColorNumberTrain.ColorTrain = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._color_train).Color.R, G = ((SolidColorBrush)NumberTrainRamka._color_train).Color.G, B = ((SolidColorBrush)NumberTrainRamka._color_train).Color.B };
             }
             else
             {
                 if (ColorProject.ColorNumberTrain.ColorActiv == null)
                     ColorProject.ColorNumberTrain.ColorActiv = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._coloractiv).Color.R, G = ((SolidColorBrush)NumberTrainRamka._coloractiv).Color.G, B = ((SolidColorBrush)NumberTrainRamka._coloractiv).Color.B };
                 else NumberTrainRamka._coloractiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorActiv.R, ColorProject.ColorNumberTrain.ColorActiv.G, ColorProject.ColorNumberTrain.ColorActiv.B));
                 //
                 if (ColorProject.ColorNumberTrain.ColorFillNotControl == null)
                     ColorProject.ColorNumberTrain.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._colornotcontrol).Color.R, G = ((SolidColorBrush)NumberTrainRamka._colornotcontrol).Color.G, B = ((SolidColorBrush)NumberTrainRamka._colornotcontrol).Color.B };
                 else NumberTrainRamka._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorFillNotControl.R, ColorProject.ColorNumberTrain.ColorFillNotControl.G, ColorProject.ColorNumberTrain.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorNumberTrain.ColorNotControlStroke == null)
                     ColorProject.ColorNumberTrain.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)NumberTrainRamka._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)NumberTrainRamka._colornotcontrolstroke).Color.B };
                 else NumberTrainRamka._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorNotControlStroke.R, ColorProject.ColorNumberTrain.ColorNotControlStroke.G, ColorProject.ColorNumberTrain.ColorNotControlStroke.B));
                 //
                 if (ColorProject.ColorNumberTrain.ColorPasiv == null)
                     ColorProject.ColorNumberTrain.ColorPasiv = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._color_pasiv).Color.R, G = ((SolidColorBrush)NumberTrainRamka._color_pasiv).Color.G, B = ((SolidColorBrush)NumberTrainRamka._color_pasiv).Color.B };
                 else NumberTrainRamka._color_pasiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorPasiv.R, ColorProject.ColorNumberTrain.ColorPasiv.G, ColorProject.ColorNumberTrain.ColorPasiv.B));
                 //
                 if (ColorProject.ColorNumberTrain.ColorRamkaDefult == null)
                     ColorProject.ColorNumberTrain.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._color_ramka).Color.R, G = ((SolidColorBrush)NumberTrainRamka._color_ramka).Color.G, B = ((SolidColorBrush)NumberTrainRamka._color_ramka).Color.B };
                 else NumberTrainRamka._color_ramka = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorRamkaDefult.R, ColorProject.ColorNumberTrain.ColorRamkaDefult.G, ColorProject.ColorNumberTrain.ColorRamkaDefult.B));
                 //
                 if (ColorProject.ColorNumberTrain.ColorTrain == null)
                     ColorProject.ColorNumberTrain.ColorTrain = new ColorN() { R = ((SolidColorBrush)NumberTrainRamka._color_train).Color.R, G = ((SolidColorBrush)NumberTrainRamka._color_train).Color.G, B = ((SolidColorBrush)NumberTrainRamka._color_train).Color.B };
                 else NumberTrainRamka._color_train = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorTrain.R, ColorProject.ColorNumberTrain.ColorTrain.G, ColorProject.ColorNumberTrain.ColorTrain.B));
                 //
             }


             ////Цветовой набор для элемента стрелка поворотов перегона
             if (ColorProject.ColorArrow == null)
             {
                 ColorProject.ColorArrow = new ColorArrow();
                 ColorProject.ColorArrow.ColorDeparture = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_departure).Color.R, G = ((SolidColorBrush)ArrowMove._color_departure).Color.G, B = ((SolidColorBrush)ArrowMove._color_departure).Color.B };
                 ColorProject.ColorArrow.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)ArrowMove._colornotcontrol).Color.R, G = ((SolidColorBrush)ArrowMove._colornotcontrol).Color.G, B = ((SolidColorBrush)ArrowMove._colornotcontrol).Color.B };
                 ColorProject.ColorArrow.ColorNormal = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_normal).Color.R, G = ((SolidColorBrush)ArrowMove._color_normal).Color.G, B = ((SolidColorBrush)ArrowMove._color_normal).Color.B };
                 ColorProject.ColorArrow.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)ArrowMove._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)ArrowMove._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)ArrowMove._colornotcontrolstroke).Color.B };
                 ColorProject.ColorArrow.ColorOccupation = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_occupation).Color.R, G = ((SolidColorBrush)ArrowMove._color_occupation).Color.G, B = ((SolidColorBrush)ArrowMove._color_occupation).Color.B };
                 ColorProject.ColorArrow.ColorOkDeparture = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_ok_departure).Color.R, G = ((SolidColorBrush)ArrowMove._color_ok_departure).Color.G, B = ((SolidColorBrush)ArrowMove._color_ok_departure).Color.B };
                 ColorProject.ColorArrow.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_ramka).Color.R, G = ((SolidColorBrush)ArrowMove._color_ramka).Color.G, B = ((SolidColorBrush)ArrowMove._color_ramka).Color.B };
                 ColorProject.ColorArrow.ColorWaitDeparture = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_wait_departure).Color.R, G = ((SolidColorBrush)ArrowMove._color_wait_departure).Color.G, B = ((SolidColorBrush)ArrowMove._color_wait_departure).Color.B };
             }
             else
             {
                 //
                 if (ColorProject.ColorArrow.ColorDeparture == null)
                     ColorProject.ColorArrow.ColorDeparture = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_departure).Color.R, G = ((SolidColorBrush)ArrowMove._color_departure).Color.G, B = ((SolidColorBrush)ArrowMove._color_departure).Color.B };
                 else ArrowMove._color_departure = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorDeparture.R, ColorProject.ColorArrow.ColorDeparture.G, ColorProject.ColorArrow.ColorDeparture.B));
                 //
                 if (ColorProject.ColorArrow.ColorFillNotControl == null)
                     ColorProject.ColorArrow.ColorFillNotControl = new ColorN() { R = ((SolidColorBrush)ArrowMove._colornotcontrol).Color.R, G = ((SolidColorBrush)ArrowMove._colornotcontrol).Color.G, B = ((SolidColorBrush)ArrowMove._colornotcontrol).Color.B };
                 else ArrowMove._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorFillNotControl.R, ColorProject.ColorArrow.ColorFillNotControl.G, ColorProject.ColorArrow.ColorFillNotControl.B));
                 //
                 if (ColorProject.ColorArrow.ColorNormal == null)
                     ColorProject.ColorArrow.ColorNormal = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_normal).Color.R, G = ((SolidColorBrush)ArrowMove._color_normal).Color.G, B = ((SolidColorBrush)ArrowMove._color_normal).Color.B };
                 else ArrowMove._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorNormal.R, ColorProject.ColorArrow.ColorNormal.G, ColorProject.ColorArrow.ColorNormal.B));
                 //
                 if (ColorProject.ColorArrow.ColorNotControlStroke == null)
                     ColorProject.ColorArrow.ColorNotControlStroke = new ColorN() { R = ((SolidColorBrush)ArrowMove._colornotcontrolstroke).Color.R, G = ((SolidColorBrush)ArrowMove._colornotcontrolstroke).Color.G, B = ((SolidColorBrush)ArrowMove._colornotcontrolstroke).Color.B };
                 else ArrowMove._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorNotControlStroke.R, ColorProject.ColorArrow.ColorNotControlStroke.G, ColorProject.ColorArrow.ColorNotControlStroke.B));
                 //
                 if (ColorProject.ColorArrow.ColorOccupation == null)
                     ColorProject.ColorArrow.ColorOccupation = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_occupation).Color.R, G = ((SolidColorBrush)ArrowMove._color_occupation).Color.G, B = ((SolidColorBrush)ArrowMove._color_occupation).Color.B };
                 else ArrowMove._color_occupation = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorOccupation.R, ColorProject.ColorArrow.ColorOccupation.G, ColorProject.ColorArrow.ColorOccupation.B));
                 //
                 if (ColorProject.ColorArrow.ColorOkDeparture == null)
                     ColorProject.ColorArrow.ColorOkDeparture = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_ok_departure).Color.R, G = ((SolidColorBrush)ArrowMove._color_ok_departure).Color.G, B = ((SolidColorBrush)ArrowMove._color_ok_departure).Color.B };
                 else ArrowMove._color_ok_departure = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorOkDeparture.R, ColorProject.ColorArrow.ColorOkDeparture.G, ColorProject.ColorArrow.ColorOkDeparture.B));
                 //
                 if (ColorProject.ColorArrow.ColorRamkaDefult == null)
                     ColorProject.ColorArrow.ColorRamkaDefult = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_ramka).Color.R, G = ((SolidColorBrush)ArrowMove._color_ramka).Color.G, B = ((SolidColorBrush)ArrowMove._color_ramka).Color.B };
                 else ArrowMove._color_ramka = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorRamkaDefult.R, ColorProject.ColorArrow.ColorRamkaDefult.G, ColorProject.ColorArrow.ColorRamkaDefult.B));
                 //
                 if (ColorProject.ColorArrow.ColorWaitDeparture == null)
                     ColorProject.ColorArrow.ColorWaitDeparture = new ColorN() { R = ((SolidColorBrush)ArrowMove._color_wait_departure).Color.R, G = ((SolidColorBrush)ArrowMove._color_wait_departure).Color.G, B = ((SolidColorBrush)ArrowMove._color_wait_departure).Color.B };
                 else ArrowMove._color_wait_departure = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorWaitDeparture.R, ColorProject.ColorArrow.ColorWaitDeparture.G, ColorProject.ColorArrow.ColorWaitDeparture.B));
                 //
             }
         }

        public static void SetColor()
        {
            MainWindow._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorFon.R, ColorProject.ColorFon.G, ColorProject.ColorFon.B));
            MainWindow._colorselect = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrowCommand.R, ColorProject.ColorArrowCommand.G, ColorProject.ColorArrowCommand.B));
            ////Цветовой набор для главного пути
            StationPath._coloractiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorActiv.R, ColorProject.ColorPath.ColorActiv.G, ColorProject.ColorPath.ColorActiv.B));
            StationPath._colorauto_action = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorAuto.R, ColorProject.ColorPath.ColorAuto.G, ColorProject.ColorPath.ColorAuto.B));
            StationPath._colordiesel_traction = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorDieselTraction.R, ColorProject.ColorPath.ColorDieselTraction.G, ColorProject.ColorPath.ColorDieselTraction.B));
            StationPath._colorelectric_traction = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorElectricTraction.R, ColorProject.ColorPath.ColorElectricTraction.G, ColorProject.ColorPath.ColorElectricTraction.B));
            StationPath._colorfencing = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorFencing.R, ColorProject.ColorPath.ColorFencing.G, ColorProject.ColorPath.ColorFencing.B));
            StationPath._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorFillNotControl.R, ColorProject.ColorPath.ColorFillNotControl.G, ColorProject.ColorPath.ColorFillNotControl.B));
            StationPath._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorNotControlStroke.R, ColorProject.ColorPath.ColorNotControlStroke.G, ColorProject.ColorPath.ColorNotControlStroke.B));
            StationPath._colorpassiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorPasiv.R, ColorProject.ColorPath.ColorPasiv.G, ColorProject.ColorPath.ColorPasiv.B));
            StationPath._color_path = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorPathName.R, ColorProject.ColorPath.ColorPathName.G, ColorProject.ColorPath.ColorPathName.B));
            StationPath._color_train = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorTrain.R, ColorProject.ColorPath.ColorTrain.G, ColorProject.ColorPath.ColorTrain.B));
            StationPath._color_vertor_train = new SolidColorBrush(Color.FromRgb(ColorProject.ColorPath.ColorTrainVertor.R, ColorProject.ColorPath.ColorTrainVertor.G, ColorProject.ColorPath.ColorTrainVertor.B));
            ////Цветовой набор для сигнала
            Signal._color_busy = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorBusy.R, ColorProject.ColorSignal.ColorBusy.G, ColorProject.ColorSignal.ColorBusy.B));
            Signal._color_closed = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorClosed.R, ColorProject.ColorSignal.ColorClosed.G, ColorProject.ColorSignal.ColorClosed.B));
            Signal._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorFault.R, ColorProject.ColorSignal.ColorFault.G, ColorProject.ColorSignal.ColorFault.B));
            Signal._color_install = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInstall.R, ColorProject.ColorSignal.ColorInstall.G, ColorProject.ColorSignal.ColorInstall.B));
            Signal._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorFillNotControl.R, ColorProject.ColorSignal.ColorFillNotControl.G, ColorProject.ColorSignal.ColorFillNotControl.B));
            Signal._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorNotControlStroke.R, ColorProject.ColorSignal.ColorNotControlStroke.G, ColorProject.ColorSignal.ColorNotControlStroke.B));
            Signal._color_open = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorOpen.R, ColorProject.ColorSignal.ColorOpen.G, ColorProject.ColorSignal.ColorOpen.B));
            Signal._color_ramka_defult = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorRamkaDefult.R, ColorProject.ColorSignal.ColorRamkaDefult.G, ColorProject.ColorSignal.ColorRamkaDefult.B));
            Signal._color_shunting = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorShunting.R, ColorProject.ColorSignal.ColorShunting.G, ColorProject.ColorSignal.ColorShunting.B));
            Signal._colorfree = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorFree.R, ColorProject.ColorSignal.ColorFree.G, ColorProject.ColorSignal.ColorFree.B));
            Signal._color_invitational_one = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInvitationalOne.R, ColorProject.ColorSignal.ColorInvitationalOne.G, ColorProject.ColorSignal.ColorInvitationalOne.B));
            Signal._color_invitational_ty = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInvitationalTy.R, ColorProject.ColorSignal.ColorInvitationalTy.G, ColorProject.ColorSignal.ColorInvitationalTy.B));
            Signal._color_install_route = new SolidColorBrush(Color.FromRgb(ColorProject.ColorSignal.ColorInstallRoute.R, ColorProject.ColorSignal.ColorInstallRoute.G, ColorProject.ColorSignal.ColorInstallRoute.B));
            ////Цветовой набор кнопки станции
            ButtonStation._color_accident = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorAccident.R, ColorProject.ColorButtonStation.ColorAccident.G, ColorProject.ColorButtonStation.ColorAccident.B));
            ButtonStation._color_autonomous_control = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorAutonomousControl.R, ColorProject.ColorButtonStation.ColorAutonomousControl.G, ColorProject.ColorButtonStation.ColorAutonomousControl.B));
            ButtonStation._color_dispatcher = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorDispatcher.R, ColorProject.ColorButtonStation.ColorDispatcher.G, ColorProject.ColorButtonStation.ColorDispatcher.B));
            ButtonStation._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFault.R, ColorProject.ColorButtonStation.ColorFault.G, ColorProject.ColorButtonStation.ColorFault.B));
            ButtonStation._color_fire = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFire.R, ColorProject.ColorButtonStation.ColorFire.G, ColorProject.ColorButtonStation.ColorFire.B));
            ButtonStation._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorRamkaDefult.R, ColorProject.ColorButtonStation.ColorRamkaDefult.G, ColorProject.ColorButtonStation.ColorRamkaDefult.B));
            ButtonStation._color_notlink = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorNotLink.R, ColorProject.ColorButtonStation.ColorNotLink.G, ColorProject.ColorButtonStation.ColorNotLink.B));
            ButtonStation._color_reserve_control = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorReserveControl.R, ColorProject.ColorButtonStation.ColorReserveControl.G, ColorProject.ColorButtonStation.ColorReserveControl.B));
            ButtonStation._color_sesoncontol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorSesonContol.R, ColorProject.ColorButtonStation.ColorSesonContol.G, ColorProject.ColorButtonStation.ColorSesonContol.B));
            ButtonStation._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFon.R, ColorProject.ColorButtonStation.ColorFon.G, ColorProject.ColorButtonStation.ColorFon.B));
            ButtonStation._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorFillNotControl.R, ColorProject.ColorButtonStation.ColorFillNotControl.G, ColorProject.ColorButtonStation.ColorFillNotControl.B));
            ButtonStation._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorButtonStation.ColorNotControlStroke.R, ColorProject.ColorButtonStation.ColorNotControlStroke.G, ColorProject.ColorButtonStation.ColorNotControlStroke.B));
            ////Цветовой набор переезда
            Moves._color_accident = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorAccident.R, ColorProject.ColorMove.ColorAccident.G, ColorProject.ColorMove.ColorAccident.B));
            Moves._color_closingmove = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorClose.R, ColorProject.ColorMove.ColorClose.G, ColorProject.ColorMove.ColorClose.B));
            Moves._color_faultmove = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorFault.R, ColorProject.ColorMove.ColorFault.G, ColorProject.ColorMove.ColorFault.B));
            Moves._color_moveopen = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorMoveOpen.R, ColorProject.ColorMove.ColorMoveOpen.G, ColorProject.ColorMove.ColorMoveOpen.B));
            Moves._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorFon.R, ColorProject.ColorMove.ColorFon.G, ColorProject.ColorMove.ColorFon.B));
            Moves._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorFillNotControl.R, ColorProject.ColorMove.ColorFillNotControl.G, ColorProject.ColorMove.ColorFillNotControl.B));
            Moves._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorMove.ColorNotControlStroke.R, ColorProject.ColorMove.ColorNotControlStroke.G, ColorProject.ColorMove.ColorNotControlStroke.B));
            //
            ////Цветовой набор КТСМа
            KTCM._color_break = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorAccident.R, ColorProject.ColorKTCM.ColorAccident.G, ColorProject.ColorKTCM.ColorAccident.B));
            KTCM._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorFault.R, ColorProject.ColorKTCM.ColorFault.G, ColorProject.ColorKTCM.ColorFault.B));
            KTCM._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorRamkaDefult.R, ColorProject.ColorKTCM.ColorRamkaDefult.G, ColorProject.ColorKTCM.ColorRamkaDefult.B));
            KTCM._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorFon.R, ColorProject.ColorKTCM.ColorFon.G, ColorProject.ColorKTCM.ColorFon.B));
            KTCM._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorFillNotControl.R, ColorProject.ColorKTCM.ColorFillNotControl.G, ColorProject.ColorKTCM.ColorFillNotControl.B));
            KTCM._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKTCM.ColorNotControlStroke.R, ColorProject.ColorKTCM.ColorNotControlStroke.G, ColorProject.ColorKTCM.ColorNotControlStroke.B));
            ////Цветовой набор КГУ
            KGU._color_break = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorAccident.R, ColorProject.ColorKGU.ColorAccident.G, ColorProject.ColorKGU.ColorAccident.B));
            KGU._color_fault = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorFault.R, ColorProject.ColorKGU.ColorFault.G, ColorProject.ColorKGU.ColorFault.B));
            KGU._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorRamkaDefult.R, ColorProject.ColorKGU.ColorRamkaDefult.G, ColorProject.ColorKGU.ColorRamkaDefult.B));
            KGU._colorfon = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorFon.R, ColorProject.ColorKGU.ColorFon.G, ColorProject.ColorKGU.ColorFon.B));
            KGU._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorFillNotControl.R, ColorProject.ColorKGU.ColorFillNotControl.G, ColorProject.ColorKGU.ColorFillNotControl.B));
            KGU._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorKGU.ColorNotControlStroke.R, ColorProject.ColorKGU.ColorNotControlStroke.G, ColorProject.ColorKGU.ColorNotControlStroke.B));
            ////Цветовой набор элемента стрелка
            ArrowMove._color_departure = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorDeparture.R, ColorProject.ColorArrow.ColorDeparture.G, ColorProject.ColorArrow.ColorDeparture.B));
            ArrowMove._color_normal = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorNormal.R, ColorProject.ColorArrow.ColorNormal.G, ColorProject.ColorArrow.ColorNormal.B));
            ArrowMove._color_ramka = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorRamkaDefult.R, ColorProject.ColorArrow.ColorRamkaDefult.G, ColorProject.ColorArrow.ColorRamkaDefult.B));
            ArrowMove._color_occupation = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorOccupation.R, ColorProject.ColorArrow.ColorOccupation.G, ColorProject.ColorArrow.ColorOccupation.B));
            ArrowMove._color_ok_departure = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorOkDeparture.R, ColorProject.ColorArrow.ColorOkDeparture.G, ColorProject.ColorArrow.ColorOkDeparture.B));
            ArrowMove._color_wait_departure = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorWaitDeparture.R, ColorProject.ColorArrow.ColorWaitDeparture.G, ColorProject.ColorArrow.ColorWaitDeparture.B));
            ArrowMove._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorFillNotControl.R, ColorProject.ColorArrow.ColorFillNotControl.G, ColorProject.ColorArrow.ColorFillNotControl.B));
            ArrowMove._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorArrow.ColorNotControlStroke.R, ColorProject.ColorArrow.ColorNotControlStroke.G, ColorProject.ColorArrow.ColorNotControlStroke.B));
            ////Цветовой набор элемента номера поездов
            NumberTrainRamka._color_ramka = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorRamkaDefult.R, ColorProject.ColorNumberTrain.ColorRamkaDefult.G, ColorProject.ColorNumberTrain.ColorRamkaDefult.B));
            NumberTrainRamka._color_train = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorTrain.R, ColorProject.ColorNumberTrain.ColorTrain.G, ColorProject.ColorNumberTrain.ColorTrain.B));
            NumberTrainRamka._coloractiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorActiv.R, ColorProject.ColorNumberTrain.ColorActiv.G, ColorProject.ColorNumberTrain.ColorActiv.B));
            NumberTrainRamka._color_pasiv = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorPasiv.R, ColorProject.ColorNumberTrain.ColorPasiv.G, ColorProject.ColorNumberTrain.ColorPasiv.B));
            NumberTrainRamka._colornotcontrol = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorFillNotControl.R, ColorProject.ColorNumberTrain.ColorFillNotControl.G, ColorProject.ColorNumberTrain.ColorFillNotControl.B));
            NumberTrainRamka._colornotcontrolstroke = new SolidColorBrush(Color.FromRgb(ColorProject.ColorNumberTrain.ColorNotControlStroke.R, ColorProject.ColorNumberTrain.ColorNotControlStroke.G, ColorProject.ColorNumberTrain.ColorNotControlStroke.B));

            if (NewColor != null)
                NewColor();
        }

        /// <summary>
        /// получаем геометрию объекта из сохраненных сегментов
        /// </summary>
        /// <param name="Figures"></param>
        /// <returns></returns>
        private PathGeometry GetPathGeometry(List<Figure> Figures)
        {
            PathGeometry geo = new PathGeometry();
            foreach (Figure figure in Figures)
            {
                PathSegmentCollection segment = new PathSegmentCollection();
                foreach (Segment seg in figure.Segments)
                {
                    if (seg.RadiusX != 0 && seg.RadiusY != 0)
                        segment.Add(new ArcSegment() { Point = new Point(seg.Point.X, seg.Point.Y), Size = new Size(seg.RadiusX, seg.RadiusY) });
                    else
                        segment.Add(new LineSegment() { Point = new Point(seg.Point.X, seg.Point.Y) });
                }
                //
                geo.Figures.Add(new PathFigure() { StartPoint = new Point(figure.StartPoint.X, figure.StartPoint.Y), Segments = segment });

            }
            return geo;
        }


        private List<SettingBlock> AnalisLinePeregon(LinePeregonSave lineperegon)
        {
            
            List<SettingsSegmentBlock> collectionPoint = new List<SettingsSegmentBlock>();
            //
            List<SettingBlock> answer = new List<SettingBlock>();
            foreach (Move.StrageProject row in ProejctMove.Moves)
            {
                if (row.Infostrage.Stationnumberleft == lineperegon.StationNumber && row.Infostrage.Stationnumberright == lineperegon.StationNumberRight )
                {
                    string[] namestrage = lineperegon.Name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    //если линия перегона составная
                    if (namestrage.Length == 2 && namestrage[0] == row.NameMove)
                    {

                    }
                    //если линия перегона не составная
                    if (namestrage.Length == 1 && namestrage[0] == row.NameMove)
                    {
                        CreateCollectioAbsolute(lineperegon, collectionPoint, row.Infostrage.Start, row.Infostrage.End, LenghtPeregon(lineperegon));
                        foreach (BlockPlotProject block in row.Blockplots)
                        {
                            answer.Add(new SettingBlock() { Name = block.NameBlock, StationNumber = row.Infostrage.Stationnumberleft, StationNumberRight = row.Infostrage.Stationnumberright, NameMove = row.NameMove });
                            //
                            for (int i = 0; i < collectionPoint.Count; i++)
                            {
                                int indexstart =0;
                                if ((block.StartKilometr >= collectionPoint[i].StartKilomentr && block.StartKilometr <= collectionPoint[i].EndKilomentr) || (block.StartKilometr <= collectionPoint[i].StartKilomentr && block.StartKilometr >= collectionPoint[i].EndKilomentr))
                                {
                                    indexstart = i;
                                    Point pointstart = new Point();
                                    double g = (collectionPoint[i].Lenght * (Math.Abs(block.StartKilometr - collectionPoint[i].StartKilomentr) / Math.Abs(collectionPoint[i].EndKilomentr - collectionPoint[i].StartKilomentr)));
                                    if (SelectElements.GetPointIntersection(collectionPoint[i].StartPoint, collectionPoint[i].StartPoint, collectionPoint[i].EndPoint, (collectionPoint[i].Lenght * (Math.Abs(block.StartKilometr - collectionPoint[i].StartKilomentr) / Math.Abs(collectionPoint[i].EndKilomentr - collectionPoint[i].StartKilomentr))), ref pointstart))
                                        answer[answer.Count - 1].Points.Add(pointstart);
                                    //
                                    for (int j = indexstart; j < collectionPoint.Count; j++)
                                    {
                                        if ((block.EndKilometr >= collectionPoint[j].StartKilomentr && block.EndKilometr <= collectionPoint[j].EndKilomentr) || (block.EndKilometr <= collectionPoint[j].StartKilomentr && block.EndKilometr >= collectionPoint[j].EndKilomentr))
                                        {
                                            Point pointend = new Point();
                                            if (SelectElements.GetPointIntersection(collectionPoint[j].StartPoint, collectionPoint[j].StartPoint, collectionPoint[j].EndPoint, (collectionPoint[j].Lenght * (Math.Abs(block.EndKilometr - collectionPoint[j].StartKilomentr) / Math.Abs(collectionPoint[j].EndKilomentr - collectionPoint[j].StartKilomentr))), ref pointend))
                                            {
                                                answer[answer.Count - 1].Points.Add(pointend);
                                                answer[answer.Count - 1].Figure = GetFigures(answer[answer.Count - 1].Points);
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (j == indexstart)
                                                answer[answer.Count - 1].Points.Add(collectionPoint[j].EndPoint);
                                            else
                                            {
                                                answer[answer.Count - 1].Points.Add(collectionPoint[j].StartPoint);
                                                answer[answer.Count - 1].Points.Add(collectionPoint[j].EndPoint);
                                            }
                                          
                                        }
                                    }
                                    break;
                                }
                            }
                            //если не найдены точки то удаляем объект из коллекции
                            if (answer[answer.Count - 1].Points.Count < 2)
                                answer.RemoveAt(answer.Count - 1);
                        }
                    }
                }
            }
            return answer;
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
