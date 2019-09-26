using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using log4net;

namespace TrafficTrain
{
    public delegate void ClearCommand();
    public delegate void StandartView();

    class MainContextMenuBig
    {
        #region Переменные
        /// <summary>
        /// перечень комманд выбранных для постоения меню
        /// </summary>
        List<SettingsCommand> _selectcom = new List<SettingsCommand>();
        /// <summary>
        /// очищаем коллекцию комманд
        /// </summary>
        public static event ClearCommand ClearCommands;
        ///// <summary>
        ///// перечень команд идущих на выполнение на сервер импульсов
        ///// </summary>
        //List<SettingsCommand> _runcommand_serverimp = new List<SettingsCommand>();
        ///// <summary>
        ///// перечень команд идущих на выполнение на ГИД
        ///// </summary>
        //List<SettingsCommandGID> _runcommand_gid = new List<SettingsCommandGID>();
        /// логирование
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(MainContextMenuBig));
        /// <summary>
        /// событие для переход к стандартному виду
        /// </summary>
        public event StandartView StartView;
        #endregion

        public MainContextMenuBig()
        {
            //Конфигурируем логер
            log4net.Config.XmlConfigurator.Configure();
        }

        public ContextMenu GetContextMenu()
        {
            //
            ContextMenu result = new ContextMenu();
            result.Background = Brushes.LightGray;
            result.BorderThickness = new Thickness(2);
            result.BorderBrush = Brushes.Brown;
            //возврат к исходному виду
            if (Commands.CommandsCollection.Count == 0)
            {
                //Меню возврат к исходному виду
                MenuItem itemуes = new MenuItem();
                itemуes.Header = ConstName.standart_view;
                itemуes.Click += StandartViewCommnadClick;
                result.Items.Add(itemуes);
            }
            //
            if (Commands.CommandsCollection.Count == 1)
            {
                //если выбраны команды на сезонное и отмену сезонного управления
                if (Commands.CommandsCollection[0].StartPath == string.Empty && Commands.CommandsCollection[0].EndPath == string.Empty)
                {
                    foreach (SettingsCommand com in Commands.CommandsCollection[0].CollectionCommand)
                    {
                        MenuItem item = new MenuItem();
                        item.Header = com.NameCommand;
                        item.Click += SeasonalControlClick;
                        result.Items.Add(item);
                        _selectcom.Add(new SettingsCommand() {  NameCommand = com.NameCommand, Command = com.Command, StationNumber = com.StationNumber, View = com.View});
                    }
                }
                //если выбран только один путь на установку или изменение номера поезда
                if (Commands.CommandsCollection[0].PathSelect != null && Commands.CommandsCollection[0].PathSelect.NumberTrain!=null)
                {
                    //Меню задать отправка команды гид
                    MenuItem itemуes = new MenuItem();
                    itemуes.Header = ConstName.enter_number_train;
                    itemуes.Click += GidCommnadClick;
                    result.Items.Add(itemуes);
                }
                //
                //если выбран только один путь и на нем выбран поезд
                if (Commands.CommandsCollection[0].PathSelect != null && Commands.CommandsCollection[0].PathSelect.NumberTrain != null)
                {
                    //Меню задать отправка команды гид
                    MenuItem itemуes = new MenuItem();
                    itemуes.Header = ConstName.delete_train;
                    itemуes.Click += GidCommnadClick;
                    result.Items.Add(itemуes);
                }
            }
            //
            if (Commands.CommandsCollection.Count == 2)
            {
                if (Commands.CommandsCollection[0].NameStation != null)
                {
                    //Меню задать отправка команды гид
                    MenuItem itemуes = new MenuItem();
                    itemуes.Header = ConstName.command_gid_path_arrival;
                    itemуes.Click += GidCommnadClick;
                    result.Items.Add(itemуes);
                }
                else
                {
                    //Меню задать маршрут
                    MenuItem itemуes = new MenuItem();
                    itemуes.Header = ConstName.yes_route_setting;
                    itemуes.Click += RouteControlClick;
                    result.Items.Add(itemуes);
                    //Меню отменить маршрут
                    MenuItem itemno = new MenuItem();
                    itemno.Header = ConstName.no_route_setting;
                    itemno.Click += RouteControlClick;
                    result.Items.Add(itemno);
                    //Меню на разрешение на отпраку на перегон или ее отмену
                    foreach (SettingsStep step in Commands.CommandsCollection)
                    {
                        foreach (SettingsCommand com in step.CollectionCommand)
                        {
                            switch (com.View)
                            {
                                case ViewNameSostNumberTu.yes_route_setting:
                                    _selectcom.Add(new SettingsCommand() { Command = com.Command, View = com.View, StationNumber = com.StationNumber, NameCommand = com.NameCommand });
                                    break;
                                case ViewNameSostNumberTu.no_route_setting:
                                    _selectcom.Add(new SettingsCommand() { Command = com.Command, View = com.View, StationNumber = com.StationNumber, NameCommand = com.NameCommand });
                                    break;
                                case ViewNameSostNumberTu.no_departure:
                                    {
                                        MenuItem item = new MenuItem();
                                        item.Header = com.NameCommand;
                                        item.Click += DepartureControlClick;
                                        result.Items.Add(item);
                                        _selectcom.Add(new SettingsCommand() { Command = com.Command, View = com.View, StationNumber = com.StationNumber, NameCommand = com.NameCommand });
                                    }
                                    break;
                                case ViewNameSostNumberTu.yes_departure:
                                    {
                                        MenuItem item = new MenuItem();
                                        item.Header = com.NameCommand;
                                        item.Click += DepartureControlClick;
                                        result.Items.Add(item);
                                        _selectcom.Add(new SettingsCommand() { Command = com.Command, View = com.View, StationNumber = com.StationNumber, NameCommand = com.NameCommand });
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            //если выбрана серия комманд для построения маршрута
            if (Commands.CommandsCollection.Count > 2)
            {
                //Меню задать маршрут
                MenuItem itemуes = new MenuItem();
                itemуes.Header = ConstName.yes_route_setting;
                itemуes.Click += RouteControlClick;
                result.Items.Add(itemуes);
                //Меню отменить маршрут
                MenuItem itemno = new MenuItem();
                itemno.Header = ConstName.no_route_setting;
                itemno.Click += RouteControlClick;
                result.Items.Add(itemno);
                //
                foreach (SettingsStep step in Commands.CommandsCollection)
                {
                    foreach (SettingsCommand com in step.CollectionCommand)
                    {
                        switch (com.View)
                        {
                            case ViewNameSostNumberTu.yes_route_setting:
                                _selectcom.Add(new SettingsCommand() {Command = com.Command, View = com.View, StationNumber = com.StationNumber, NameCommand = com.NameCommand });
                                break;
                            case ViewNameSostNumberTu.no_route_setting:
                                _selectcom.Add(new SettingsCommand() { Command = com.Command, View = com.View, StationNumber = com.StationNumber, NameCommand = com.NameCommand });
                                break;
                        }
                    }
                }
            }
            //
            if (result.Items.Count > 0)
                return result;
            else return null;
        }
        /// <summary>
        /// отпраляем команду на импульс сервер
        /// </summary>
        private void SendTU(Object stateInfo)
        {
            lock (this)
            {
                List<SettingsCommand> _runcommand_serverimp = stateInfo as List<SettingsCommand>;
                if (_runcommand_serverimp != null)
                {
                    foreach (SettingsCommand com in _runcommand_serverimp)
                        log.Info(MainWindow.ClientImpulses.SendImpulse(com.Command, com.StationNumber, sdm.diagnostic_section_model.client_impulses.ImpulseState.ActiveState));
                }
            }
        }
        /// <summary>
        /// отпраляем команду на обработку Гиду
        /// </summary>
        private void SendGID(Object stateInfo)
        {
            lock (this)
            {
                GidCommand commnadgid = new GidCommand();
                //
                List<SettingsCommandGID> _runcommand_gid = stateInfo as List<SettingsCommandGID>;
                if (_runcommand_gid != null)
                {
                    foreach (SettingsCommandGID com in _runcommand_gid)
                        log.Info(commnadgid.SendCommandGid(com.Command, com.IdTrain, com.NumberStation, com.NamePath, com.NumberTrain, com.TimeEvent, com.NameBlock, com.TypeEvent, com.Prefix, com.Sufix));
                }
            }
        }
        /// <summary>
        /// выполняем команды на сезонное управление и отмену сезонного управления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SeasonalControlClick(object sender, RoutedEventArgs args)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                List<SettingsCommand> _runcommand_serverimp = new List<SettingsCommand>();
                //
                foreach (SettingsCommand com in _selectcom)
                {
                    if (com.NameCommand == item.Header.ToString())
                    {
                        if (ClearCommands != null)
                            ClearCommands();
                        //
                        _runcommand_serverimp.Add(new SettingsCommand() { Command = com.Command, StationNumber = com.StationNumber, NameCommand = com.NameCommand, View = com.View });
                        //
                        ThreadPool.QueueUserWorkItem(new WaitCallback(SendTU), _runcommand_serverimp);
                    }
                }
            }
        }
        /// <summary>
        /// выполняем команды на разрешение на отправление на перегон или отмену
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DepartureControlClick(object sender, RoutedEventArgs args)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                List<SettingsCommand> _runcommand_serverimp = new List<SettingsCommand>();
                //
                foreach (SettingsCommand com in _selectcom)
                {
                    if (com.NameCommand == item.Header.ToString())
                    {
                        if (ClearCommands != null)
                            ClearCommands();
                        //
                        _runcommand_serverimp.Add(new SettingsCommand() { Command = com.Command, StationNumber = com.StationNumber, NameCommand = com.NameCommand, View = com.View });
                        //
                        ThreadPool.QueueUserWorkItem(new WaitCallback(SendTU), _runcommand_serverimp);
                    }
                }
            }
        }
        /// <summary>
        /// выполняем команды на установку и отмену маршрута
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void RouteControlClick(object sender, RoutedEventArgs args)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                List<SettingsCommand> _runcommand_serverimp = new List<SettingsCommand>();
                //
                if (ConstName.yes_route_setting== item.Header.ToString())
                {
                    if (ClearCommands != null)
                        ClearCommands();
                    //
                    foreach (SettingsCommand com in _selectcom)
                    {
                        if(ViewNameSostNumberTu.yes_route_setting == com.View)
                            _runcommand_serverimp.Add(new SettingsCommand() { Command = com.Command, StationNumber = com.StationNumber, NameCommand = com.NameCommand, View = com.View });
                    } 
                    //
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SendTU), _runcommand_serverimp);
                }
                //
                if (ConstName.no_route_setting == item.Header.ToString())
                {
                    if (ClearCommands != null)
                        ClearCommands();
                    //
                    foreach (SettingsCommand com in _selectcom)
                    {
                        if (ViewNameSostNumberTu.no_route_setting == com.View)
                            _runcommand_serverimp.Add(new SettingsCommand() { Command = com.Command, StationNumber = com.StationNumber, NameCommand = com.NameCommand, View = com.View });
                    }
                    //
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SendTU), _runcommand_serverimp);
                }
            }
        }

        /// <summary>
        /// выполняем команду на отправку команды Гиду
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GidCommnadClick(object sender, RoutedEventArgs args)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                List<SettingsCommandGID> _runcommand_gid = new List<SettingsCommandGID>();
                switch (item.Header.ToString())
                {
                    case ConstName.command_gid_path_arrival:
                        {
                            try
                            {
                                string namepath = Commands.CommandsCollection[1].PathSelect.NamePath;
                                List<Move.t_train> trains = Commands.CommandsCollection[0].NameStation.Trains;
                                //
                                if (ClearCommands != null)
                                    ClearCommands();
                                //
                                ChoicePathArrival window = new ChoicePathArrival(trains);
                                window.ShowDialog();
                                if (window.SelectNumber != -1)
                                {
                                    _runcommand_gid.Add(new SettingsCommandGID() { IdTrain = byte.Parse(trains[window.SelectNumber].TrainIndex),
                                        NumberStation = trains[window.SelectNumber].StationNumber.ToString(), NamePath = namepath, Command = 3, TypeEvent =(byte)trains[window.SelectNumber].EventNumber});
                                }
                                ThreadPool.QueueUserWorkItem(new WaitCallback(SendGID), _runcommand_gid);
                            }
                            catch(Exception error)
                            {
                                log.Error(error.Message, error);
                            }
                        }
                        break;
                    case ConstName.enter_number_train:
                        try
                        {
                            string namepath = Commands.CommandsCollection[0].PathSelect.NamePath;
                            Move.t_train train = Commands.CommandsCollection[0].PathSelect.NumberTrain;
                            //
                            if (ClearCommands != null)
                                ClearCommands();
                            //
                            _runcommand_gid.Add(new SettingsCommandGID()
                            {
                                IdTrain = byte.Parse(train.TrainIndex),
                                Command = 15,
                                Prefix = string.Empty
                            });
                            //
                            ThreadPool.QueueUserWorkItem(new WaitCallback(SendGID), _runcommand_gid);
                        }
                        catch (Exception error)
                        {
                            log.Error(error.Message, error);
                        }
                        break;
                    case ConstName.delete_train:
                             try
                        {
                            string namepath = Commands.CommandsCollection[0].PathSelect.NamePath;
                            Move.t_train train = Commands.CommandsCollection[0].PathSelect.NumberTrain;
                            //
                            if (ClearCommands != null)
                                ClearCommands();
                            //
                            EnterTrainNumber window = new EnterTrainNumber();
                            if (window.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                _runcommand_gid.Add(new SettingsCommandGID()
                                    {
                                        IdTrain = byte.Parse(train.TrainIndex),
                                        NumberTrain = window.NumberTrain,
                                        Prefix = window.Prefix,
                                        Sufix = window.Sufix,
                                        Command = 11,
                                        NumberStation = LoadProject.NamesStations[window.NameStation]
                                    });
                                //
                                ThreadPool.QueueUserWorkItem(new WaitCallback(SendGID),_runcommand_gid);
                            }
                        }
                        catch (Exception error)
                        {
                            log.Error(error.Message, error);
                        }
                        break;
                }
            }
        }

        private void StandartViewCommnadClick(object sender, RoutedEventArgs args)
        {
            if (StartView != null)
                StartView();
        }
    }
}
