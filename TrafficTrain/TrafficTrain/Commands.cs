using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrafficTrain
{
    /// <summary>
    /// класс отвечает за составление комманд, анализ и их выполнение
    /// </summary>
    class Commands
    {
        #region Переменные и свойства
        /// <summary>
        /// писок всевозможных команд для всех шагов
        /// </summary>
        public static List<SettingsStep> CommandsCollection = new List<SettingsStep>();
        /// <summary>
        /// информация о правильности построения команды
        /// </summary>
        public static event Info InfoCommand;
        #endregion

        /// <summary>
        /// анализируем новую точку для команду
        /// </summary>
        /// <returns>можно или нет добавлять команду в списокыы</returns>
        public static bool AnalisCommand(CommandElement element)
        {
            //если выбран элемент название станции
            CommandButton command = element as CommandButton;
            if (command != null)
            {
                switch (command.ViewCommand)
                {
                    case DataGrafik.ViewCommand.sound:
                        CommandsCollection.Add(new SettingsStep() { CommandButton = command });
                        return true;
                    case DataGrafik.ViewCommand.diagnostics:
                        CommandsCollection.Add(new SettingsStep() { CommandButton = command });
                        return true;
                    case DataGrafik.ViewCommand.style:
                        CommandsCollection.Add(new SettingsStep() { CommandButton = command });
                        return true;
                    default:
                        command.Click();
                        return false;
                }
                
            }
            //если выбран элемент название станции
            NameStation namestation = element as NameStation;
            if (namestation != null)
            {
                if (CommandsCollection.Count == 0 /*&& namestation.Trains.Count > 0*/)
                {
                    CommandsCollection.Add(new SettingsStep() { StationNumberCommand = namestation.StationNumber, SelectObejct = namestation });
                    return true;
                }
                //
                if (CommandsCollection.Count == 1)
                {
                    //если соединять нитки поездов между 2-мя станциями
                    if (CommandsCollection[CommandsCollection.Count - 1].SelectObejct != null)
                    {
                        CommandsCollection.Add(new SettingsStep() {  SelectObejct =  namestation });
                        return true;
                    }
                }
            }
            else
            {
                //если выбрана кнопка станции
                ButtonStation buttonstation = element as ButtonStation;
                if (buttonstation != null)
                {
                    if (CommandsCollection.Count == 0)
                    {
                        List<SettingsCommand> _listtu = FindCommandCollection(new string[] { ViewNameSostNumberTu.seasonal_management }, string.Empty, string.Empty, buttonstation.StationNumber);
                        if (_listtu != null)
                        {
                            CommandsCollection.Add(new SettingsStep() { CollectionCommand = _listtu, ButtonStation = buttonstation });
                            return true;
                        }
                    }
                    else
                    {
                        if (InfoCommand != null)
                            InfoCommand("Нельзя выбрать данный элемент !!!");
                    }
                }
                else
                {
                    //если выбран главный путь
                    StationPath stpath = element as StationPath;
                    if (stpath != null)
                    {
                        if (CommandsCollection.Count == 0)
                        {

                            if (FindCommand(string.Format("{0}-{1}", ConstName.path, stpath.NamePath), stpath.StationNumber))
                            {
                                CommandsCollection.Add(new SettingsStep() { StartPath = string.Format("{0}-{1}", ConstName.path, stpath.NamePath), SelectObejct = stpath });
                                return true;
                            }
                            else
                            {
                                CommandsCollection.Add(new SettingsStep() { SelectObejct = stpath });
                                return true;
                            }
                        }
                        else
                        {
                            if (CommandsCollection[CommandsCollection.Count - 1].SelectObejct != null)
                            {
                                if (CommandsCollection.Count == 1)
                                {
                                    //if (CommandsCollection[0].StationNumberCommand == stpath.StationNumber)
                                    //{
                                    //    CommandsCollection.Add(new SettingsStep() { SelectObejct = stpath });
                                    //    return true;
                                    //}
                                    //
                                    CommandsCollection.Add(new SettingsStep() { SelectObejct = stpath });
                                    return true;
                                }
                                //else if (CommandsCollection[0].StationNumberCommand != stpath.StationNumber && CommandsCollection.Count == 1)
                                //    InfoCommand("Нельзя выбрать путь другой станции !!!");
                            }
                            else
                            {
                                //
                                if (CommandsCollection[CommandsCollection.Count - 1].SelectObejct != null && CommandsCollection[CommandsCollection.Count - 1].SelectObejct is NumberTrainRamka)
                                {
                                    if ((CommandsCollection[CommandsCollection.Count - 1].SelectObejct as NumberTrainRamka).StationNumber == stpath.StationNumber)
                                        CommandsCollection[CommandsCollection.Count - 1].StartPath = string.Format("{0}-{1}", ConstName.move, (CommandsCollection[CommandsCollection.Count - 1].SelectObejct as NumberTrainRamka).LeftBorder);
                                    //
                                    if ((CommandsCollection[CommandsCollection.Count - 1].SelectObejct as NumberTrainRamka).StationNumberRight == stpath.StationNumber)
                                        CommandsCollection[CommandsCollection.Count - 1].StartPath = string.Format("{0}-{1}", ConstName.move, (CommandsCollection[CommandsCollection.Count - 1].SelectObejct as NumberTrainRamka).RightBorder);
                                }
                                //
                                string[] viewcommands = new string[] { ViewNameSostNumberTu.yes_route_setting, ViewNameSostNumberTu.no_route_setting };
                                List<SettingsCommand> _listtu = FindCommandCollection(viewcommands, CommandsCollection[CommandsCollection.Count - 1].StartPath, string.Format("{0}-{1}", ConstName.path, stpath.NamePath), stpath.StationNumber);
                                if (_listtu != null && !BackRoute(string.Format("{0}-{1}", ConstName.path, stpath.NamePath), stpath.StationNumber))
                                {
                                    CommandsCollection[CommandsCollection.Count - 1].EndPath = string.Format("{0}-{1}", ConstName.path, stpath.NamePath);
                                    CommandsCollection[CommandsCollection.Count - 1].CollectionCommand = _listtu;
                                    CommandsCollection[CommandsCollection.Count - 1].StationNumberCommand = stpath.StationNumber;
                                    CommandsCollection.Add(new SettingsStep() { StartPath = CommandsCollection[CommandsCollection.Count - 1].EndPath, SelectObejct = stpath });
                                    //
                                    return true;
                                }
                                //изменитьпуть прибытия
                                //if (CommandsCollection.Count == 1 && CommandsCollection[CommandsCollection.Count - 1].PathSelect != null && CommandsCollection[CommandsCollection.Count - 1].PathSelect.NumberTrains.Count > 0)
                                //{
                                //    if (CommandsCollection[CommandsCollection.Count - 1].PathSelect.StationNumber == stpath.StationNumber)
                                //    {
                                //        CommandsCollection.Add(new SettingsStep() { PathSelect = stpath });
                                //        return true;
                                //    }
                                //    else
                                //        InfoCommand("Нельзя выбрать путь другой станции !!!");
                                //}
                                if (CommandsCollection.Count == 1)
                                {
                                    //для команды прибытия
                                    if (CommandsCollection[CommandsCollection.Count - 1].SelectObejct != null && CommandsCollection[CommandsCollection.Count - 1].SelectObejct is NumberTrainRamka
                                        && IsMoveNeighboring(stpath, CommandsCollection[CommandsCollection.Count - 1].SelectObejct as NumberTrainRamka))
                                    {
                                        CommandsCollection.Add(new SettingsStep() {  SelectObejct = stpath });
                                        return true;
                                    }
                                    //если соединять нитки поездов 
                                    if (CommandsCollection[CommandsCollection.Count - 1].SelectObejct != null
                                        && (!(CommandsCollection[CommandsCollection.Count - 1].SelectObejct is StationPath)
                                        || (CommandsCollection[CommandsCollection.Count - 1].SelectObejct is StationPath
                                        && (CommandsCollection[0].SelectObejct as StationPath).NamePath != stpath.NamePath
                                        && (CommandsCollection[0].SelectObejct as StationPath).StationNumber != stpath.StationNumber)))
                                    {
                                        CommandsCollection.Add(new SettingsStep() {  SelectObejct = stpath });
                                        return true;
                                    }
                                }

                                //для построения длинных маршрутов по умолчанию
                                //if (AnalisRoute(stpath))
                                //{
                                //    return true;
                                //}
                            }
                        }

                    }
                    //если выбран перегон
                    NumberTrainRamka numbertrain = element as NumberTrainRamka;
                    if (numbertrain != null)
                    {
                        if (CommandsCollection.Count == 0)
                        {
                            if (FindCommand(string.Format("{0}-{1}", ConstName.move, numbertrain.LeftBorder), numbertrain.StationNumber) || FindCommand(string.Format("{0}-{1}", ConstName.move, numbertrain.RightBorder), numbertrain.StationNumberRight))
                            {
                                CommandsCollection.Add(new SettingsStep() { StartPath = string.Format("{0}-{1}", ConstName.move, string.Empty),SelectObejct = numbertrain });
                                return true;
                            }
                            else
                            {
                                CommandsCollection.Add(new SettingsStep() { SelectObejct = numbertrain });
                                return true;
                            }
                        }
                        else
                        {
                            if (CommandsCollection[CommandsCollection.Count - 1].SelectObejct != null && CommandsCollection[CommandsCollection.Count - 1].SelectObejct is StationPath)
                            {
                                if ((CommandsCollection[CommandsCollection.Count - 1].SelectObejct as StationPath).StationNumber == numbertrain.StationNumber)
                                    CommandsCollection[CommandsCollection.Count - 1].EndPath = string.Format("{0}-{1}", ConstName.move, numbertrain.LeftBorder);
                                //
                                if ((CommandsCollection[CommandsCollection.Count - 1].SelectObejct as StationPath).StationNumber == numbertrain.StationNumberRight)
                                    CommandsCollection[CommandsCollection.Count - 1].EndPath = string.Format("{0}-{1}", ConstName.move, numbertrain.RightBorder);
                                //
                                string[] viewcommands = new string[] { ViewNameSostNumberTu.yes_route_setting, ViewNameSostNumberTu.no_route_setting, ViewNameSostNumberTu.yes_departure, ViewNameSostNumberTu.no_departure };
                                List<SettingsCommand> _listtu = FindCommandCollection(viewcommands, CommandsCollection[CommandsCollection.Count - 1].StartPath, CommandsCollection[CommandsCollection.Count - 1].EndPath, (CommandsCollection[CommandsCollection.Count - 1].SelectObejct as StationPath).StationNumber);
                                if (_listtu != null && !BackRoute(CommandsCollection[CommandsCollection.Count - 1].EndPath, (CommandsCollection[CommandsCollection.Count - 1].SelectObejct as StationPath).StationNumber))
                                {
                                    CommandsCollection[CommandsCollection.Count - 1].CollectionCommand = _listtu;
                                    CommandsCollection[CommandsCollection.Count - 1].StationNumberCommand = (CommandsCollection[CommandsCollection.Count - 1].SelectObejct as StationPath).StationNumber;
                                    CommandsCollection.Add(new SettingsStep() { StartPath = CommandsCollection[CommandsCollection.Count - 1].EndPath, SelectObejct = numbertrain });
                                    return true;
                                }
                            }
                            //
                            if (CommandsCollection.Count == 1)
                            {
                                CommandsCollection.Add(new SettingsStep() { SelectObejct = numbertrain });
                                return true;
                            }
                            //для построения длинных маршрутов по умолчанию
                            //if (AnalisRoute(numbertrain))
                            //{
                            //    return true;
                            //}
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// опредяем есть ли некорректные маршруты
        /// </summary>
        /// <returns></returns>
        private static bool BackRoute(string endroute, int stnumber)
        {
            if (CommandsCollection.Count > 0)
            {
                for (int i = 0; i < CommandsCollection.Count - 1 ; i++)
                {
                    if (endroute == CommandsCollection[i].StartPath && CommandsCollection[i].StationNumberCommand == stnumber )
                    {
                        if (InfoCommand != null)
                            InfoCommand("Нельзя выбрать данный элемент, некорректно задан маршрут !!!");
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsMoveNeighboring(StationPath path, NumberTrainRamka move)
        {
            if (path.StationNumber == move.StationNumber || path.StationNumber == move.StationNumberRight)
                return true;
            else return false;
        }

        /// <summary>
        /// находим нужную команду из списка
        /// </summary>
        /// <param name="Viewcommand">вид команды</param>
        /// <param name="StartPath">начало маршрута</param>
        /// <param name="EndPath">окончание маршрута</param>
        /// <param name="StNumber">номер станции</param>
        /// <returns></returns>
        private static List<SettingsCommand> FindCommandCollection(string [] Viewcommands, string StartPath, string EndPath, int StNumber)
        {
            try
            {
                List<SettingsCommand> _list = new List<SettingsCommand>();
                foreach (StateValueTu element in LoadProject.TU_list[StNumber].NamesValue)
                {
                    foreach (string command in Viewcommands)
                    {
                        switch (element.ViewCommand)
                        {
                            case ViewNameSostNumberTu.seasonal_management:
                                {
                                    if (command == element.ViewCommand)
                                        _list.Add(new SettingsCommand() { View = element.ViewCommand, StationNumber = StNumber, Command = element.Tu, NameCommand = element.NameTu });
                                }
                                break;
                            case ViewNameSostNumberTu.yes_route_setting:
                                {
                                    if (command == element.ViewCommand && StartPath == element.StartPath && EndPath == element.EndPath)
                                        _list.Add(new SettingsCommand() { View = element.ViewCommand, StationNumber = StNumber, Command = element.Tu, NameCommand = element.NameTu });
                                }
                                break;
                            case ViewNameSostNumberTu.no_route_setting:
                                {
                                    if (command == element.ViewCommand && StartPath == element.StartPath && EndPath == element.EndPath)
                                        _list.Add(new SettingsCommand() { View = element.ViewCommand, StationNumber = StNumber, Command = element.Tu, NameCommand = element.NameTu });
                                }
                                break;
                            case ViewNameSostNumberTu.yes_departure:
                                if (command == element.ViewCommand && StartPath.StartsWith(ConstName.path) && element.StartPath.StartsWith(ConstName.path) && EndPath == element.EndPath)
                                    _list.Add(new SettingsCommand() { View = element.ViewCommand, StationNumber = StNumber, Command = element.Tu, NameCommand = element.NameTu });
                                break;
                            case ViewNameSostNumberTu.no_departure:
                                if (command == element.ViewCommand && StartPath.StartsWith(ConstName.path) && element.StartPath.StartsWith(ConstName.path) && EndPath == element.EndPath)
                                    _list.Add(new SettingsCommand() { View = element.ViewCommand, StationNumber = StNumber, Command = element.Tu, NameCommand = element.NameTu });
                                break;
                        }
                    }
                }
                //
                if (_list.Count > 0)
                    return _list;
                else
                {
                    if (InfoCommand != null)
                        InfoCommand("Элемент не имееет управляющей команды !!!");
                    return null;
                }
            }
            catch
            {
                if (InfoCommand != null)
                    InfoCommand("Элемент не имееет управляющей команды !!!");
                return null;
            }
        }


        private static bool FindCommand(string StartPath, int StNumber)
        {
            try
            {
                foreach (StateValueTu element in LoadProject.TU_list[StNumber].NamesValue)
                {
                    if (element.StartPath == StartPath)
                        return true;
                }
                //
                if (InfoCommand != null)
                    InfoCommand("данный элемент не имеет управляющей команды !!!");
                return false;
            }
            catch { return false; }
        }

        //private static bool AnalisRoute(ElementRoute elementriute)
        //{
        //    if (CommandsCollection[CommandsCollection.Count - 1].PathSelect != null)
        //    {
        //        RouteFind routefind = FindStationRoute(CommandsCollection[CommandsCollection.Count - 1].PathSelect, elementriute);
        //        if (routefind != null)
        //        {
        //            for (int i = routefind.StartNode.IndexStation; i <= routefind.EndNode.IndexStation; i++)
        //            {

        //                //если сначало первый узел
        //                if (i == routefind.StartNode.IndexStation)
        //                {
        //                    //если выбранного сегмента маршрута нет в маршрутах по умолчанию
        //                    if (routefind.StartNode.IndexSegment == -1)
        //                    {
        //                        string[] viewcommands = new string[] { ViewNameSostNumberTu.yes_route_setting, ViewNameSostNumberTu.no_route_setting };
        //                      //  List<SettingsCommand> _listtu = FindCommandCollection(viewcommands, GetSegmnet(CommandsCollection[CommandsCollection.Count - 1].PathSelect), CommandsCollection[CommandsCollection.Count - 1].EndPath, CommandsCollection[CommandsCollection.Count - 1].PathSelect.StationNumber);
        //                       // CommandsCollection[CommandsCollection.Count - 1].EndPath = string.Format("{0}-{1}", ConstName.path, stpath.NamePath);
        //                    }
        //                    else
        //                    {

        //                    }
        //                }
        //                //
        //            }
        //            return true;
        //        }
        //    }
        //    else if (CommandsCollection[CommandsCollection.Count - 1].RamkaNumberSelect != null)
        //    {
        //        RouteFind routefind = FindStationRoute(CommandsCollection[CommandsCollection.Count - 1].RamkaNumberSelect, elementriute);
        //        if (routefind != null)
        //        {
        //            return true;
        //        }
        //    }
        //    ////
        //    return false;
        //}

        private static SegmentRoute GetSegmnet(ElementRoute element, string direction)
        {
            if (element is StationPath)
                return new SegmentRoute() { Name = ConstName.path, Value = ((StationPath)element).NamePath };
            else
            {
                if(direction == "L")
                    return new SegmentRoute() { Name = ConstName.move, Value = ((NumberTrainRamka)element).LeftBorder };
                else
                    return new SegmentRoute() { Name = ConstName.move, Value = ((NumberTrainRamka)element).RightBorder };
            }
        }

        /// <summary>
        /// нахождения сегмента маршрута по станции
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="findsegment"></param>
        /// <returns></returns>
        private static int FindSegmentStationRoute(List<SegmentRoute> segments, SegmentRoute findsegment)
        {
            int index = 0;
            //
            foreach (SegmentRoute seg in segments)
            {
                if (seg.Name == findsegment.Name && seg.Value == findsegment.Value)
                    return index;
                index++;
            }
            //
            return -1;
        }

        /// <summary>
        /// ищем в маршрутах по умолчанию маршрут с заданными координатами
        /// </summary>
        /// <param name="elementriute_start">координата начала</param>
        /// <param name="elementriute_end">координата конца</param>
        private static RouteFind FindStationRoute(ElementRoute elementriute_start, ElementRoute elementriute_end)
        {
            foreach (Route rout in LoadProject.Routes)
            {
                SegmentFind index_start, index_end;
                //
                index_start = FindNode(elementriute_start, rout);
                if (index_start.IndexStation == -1)
                    continue;
                index_end = FindNode(elementriute_end, rout);
                if (index_end.IndexStation == -1)
                    continue;
                //
                if (index_start.IndexStation <= index_end.IndexStation)
                    return new RouteFind() { Route = rout, StartNode = index_start, EndNode = index_end };
            }
            //
            return null;
        }

        /// <summary>
        /// находим узлы маршрута
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rout"></param>
        /// <returns></returns>
        private static SegmentFind FindNode(ElementRoute element, Route rout)
        {
            int index = 0;
            //если выбран путь
            if (element is StationPath)
            {
                StationPath path = element as StationPath;
                //
                foreach (SettingsStationRoute node in rout.Stations)
                {
                    if (node.StationNumber == path.StationNumber)
                    {
                        SegmentFind answer= new SegmentFind(){ IndexStation = index};
                        int index_segment = FindSegmentStationRoute(node.Segments, GetSegmnet(element, string.Empty));
                        if (index_segment != -1)
                        {
                            answer.IndexSegment = index_segment;
                            answer.NameSegment = node.Segments[index_segment];
                        }

                        //
                        return answer;
                    }
                    index++;
                }
            }
            //если выбран перегон
            if (element is NumberTrainRamka)
            {
                NumberTrainRamka move = element as NumberTrainRamka;
                //если выбран подход к станции со стороны левой станции
                SegmentFind move_left = new SegmentFind();
                //если выбран подход к станции со стороны правой станции
                SegmentFind move_right = new SegmentFind();
                //
                //проверяем подход к станции слева
                foreach (SettingsStationRoute node in rout.Stations)
                {
                    if (node.StationNumber == move.StationNumber)
                    {
                        if (node.Segments.Count > 0)
                         {
                             int index_segment = FindSegmentStationRoute(node.Segments, GetSegmnet(element, "L"));
                             if (index_segment != -1)
                             {
                                 move_left.IndexStation = index;
                                 move_left.IndexSegment = index_segment;
                                 move_left.NameSegment = node.Segments[index_segment];
                             }
                         }
                    }
                    //проверяем подход к станции справа
                    if (node.StationNumber == move.StationNumberRight)
                    {
                        if (node.Segments.Count > 0)
                        {
                            int index_segment = FindSegmentStationRoute(node.Segments, GetSegmnet(element, "R"));
                            if (index_segment != -1)
                            {
                                move_right.IndexStation = index;
                                move_right.IndexSegment = index_segment;
                                move_right.NameSegment = node.Segments[index_segment];
                            }
                        }
                    }
                    index++;
                }
                //если имеем два подхода к станции (слева и справа)
                if (move_left.IndexStation != -1 && move_right.IndexStation != -1)
                {
                    if (move_left.IndexStation > move_right.IndexStation)
                        return move_left;
                    else return move_right;
                }
                //если имеем один подход к станции (слева)
                if (move_left.IndexStation != -1)
                    return move_left;
                //если имеем один подход к станции (справа)
                if (move_right.IndexStation != -1)
                    return move_right;
            }
            //
            return new SegmentFind();
        }
    }
}
