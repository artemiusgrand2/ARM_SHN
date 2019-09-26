using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Move;

namespace TrafficTrain
{


    /// <summary>
    /// класс описывающий таблицу выхода с участка
    /// </summary>
    class TableTrainExit
    {
        /// <summary>
        /// время ожидания в минутах 
        /// </summary>
        public int TimePause { get; set; }
        /// <summary>
        /// номер поезда
        /// </summary>
        public int TrainNumber { get; set; }
        /// <summary>
        /// номер станции текущей
        /// </summary>
        public int NumberStationCurrent { get; set; }
        /// <summary>
        /// номер станции выхода
        /// </summary>
        public int NumberStationExit { get; set; }

        public TableTrainExit()
        {
            TimePause = -1;
            TrainNumber = -1;
            NumberStationExit = -1;
            NumberStationCurrent = -1;
        }
    }

    /// <summary>
    /// класс описывающий звуковое сообщение
    /// </summary>
    class SoundMessage
    {
        /// <summary>
        /// время сообщения
        /// </summary>
        public DateTime Time {get;set;}
        /// <summary>
        /// пояснение
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// открыто ли сообщение
        /// </summary>
        public bool OpenMessage { get; set; }
    }


    /// <summary>
    /// класс описывающий весь набор импульсов тс для станции 
    /// </summary>
    class StationTableTs
    {
        /// <summary>
        /// перечень объектов с описание состояниев
        /// </summary>
        public  Dictionary<string, List<StateValueTs>> NamesValue { get; set; }

        public StationTableTs()
        {
            NamesValue = new Dictionary<string, List<StateValueTs>>();
        }
    }

    /// <summary>
    /// класс состояние - формула для ТС
    /// </summary>
    class StateValueTs
    {
        /// <summary>
        /// вид контроля
        /// </summary>
        public Viewmode View { get; set; }
        /// <summary>
        /// формула контроля
        /// </summary>
        public string Formula { get; set; }
    }

    /// <summary>
    /// класс описывающий весь набор импульсов ту для станции 
    /// </summary>
    class StationTableTu
    {
        /// <summary>
        /// перечень объектов с описание состояниев
        /// </summary>
        public List<StateValueTu> NamesValue { get; set; }

        public StationTableTu()
        {
            NamesValue = new List< StateValueTu>();
        }
    }

    /// <summary>
    /// класс состояние - формула для ТУ
    /// </summary>
    class StateValueTu
    {
        /// <summary>
        /// название упраляющей комманды
        /// </summary>
        public string NameTu { get; set; }
        /// <summary>
        /// путь задания комманды начало (от)
        /// </summary>
        public string StartPath { get; set; }
        /// <summary>
        /// путь задания комманды окончание (до)
        /// </summary>
        public string EndPath { get; set; }
        /// <summary>
        /// управляющая команда
        /// </summary>
        public string Tu { get; set; }
        /// <summary>
        /// вид команды
        /// </summary>
        public string ViewCommand { get; set; }

        public StateValueTu()
        {
            NameTu = string.Empty;
            StartPath = string.Empty;
            EndPath = string.Empty;
            Tu = string.Empty;
            ViewCommand = string.Empty;
        }
    }
}
