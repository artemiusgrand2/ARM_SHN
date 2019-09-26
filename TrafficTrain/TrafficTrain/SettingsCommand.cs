using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrafficTrain
{
    /// <summary>
    /// свойтсва комманды ТУ
    /// </summary>
    public class SettingsCommand
    {
        /// <summary>
        /// вид команды
        /// </summary>
        public string View { get; set; }
        /// <summary>
        /// номер команды которой команда принадлежит
        /// </summary>
        public int StationNumber { get; set; }
        /// <summary>
        /// Команда
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// название команды
        /// </summary>
        public string NameCommand { get; set; }
    }

    /// <summary>
    /// свойтсва для постоения команды ГИД
    /// </summary>
    class SettingsCommandGID
    {
        /// <summary>
        /// вид команды
        /// </summary>
        public byte  Command { get; set; }
        /// <summary>
        /// идентификатор поезда
        /// </summary>
        public int IdTrain { get; set; }
        /// <summary>
        /// номер станции
        /// </summary>
        public string NumberStation { get; set; }
        /// <summary>
        /// название пути
        /// </summary>
        public string NamePath { get; set; }
        /// <summary>
        /// номер поезда
        /// </summary>
        public string NumberTrain { get; set; }
        /// <summary>
        /// время
        /// </summary>
        public DateTime TimeEvent { get; set; }
        /// <summary>
        /// название блок участка
        /// </summary>
        public string NameBlock { get; set; }
        /// <summary>
        /// тип сообщения (прибытие/отправление)
        /// </summary>
        public byte TypeEvent { get; set; }
        /// <summary>
        /// перфикс номера поезда
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// суффикс номера поезда
        /// </summary>
        public string Sufix { get; set; }
        /// <summary>
        /// значение под команды
        /// </summary>
        public int Resreve { get; set; }
        /// <summary>
        /// перфикс номера поезда вектора
        /// </summary>
        public string PrefixVector { get; set; }
        /// <summary>
        /// суффикс номера поезда вектора
        /// </summary>
        public string SufixVector { get; set; }

        public SettingsCommandGID()
        {
            NumberTrain = null;
            NamePath = null;
            NumberTrain = null;
            TimeEvent = DateTime.MinValue;
            NameBlock = null;
            Prefix = null;
            Sufix = null;
            PrefixVector = null;
            SufixVector = null;
        }
    }

    /// <summary>
    /// свойтства каждого шага постоения команды
    /// </summary>
    class SettingsStep
    {
        /// <summary>
        /// маршрут начала команды
        /// </summary>
        public string StartPath { get; set; }
        /// <summary>
        /// маршрут окончания команды
        /// </summary>
        public string EndPath { get; set; }
        /// <summary>
        ///выделенный объект 
        /// </summary>
        public TrainsInfo SelectObejct { get; set; }
        ///// <summary>
        /////объект путь 
        ///// </summary>
        //public StationPath  PathSelect { get; set; }
        ///// <summary>
        /////объект перегон
        ///// </summary>
        //public NumberTrainRamka RamkaNumberSelect { get; set; }
        ///// <summary>
        ///// элемент название станции
        ///// </summary>
        //public NameStation NameStation { get; set; }
        /// <summary>
        /// элемент кнопка управления
        /// </summary>
        public CommandButton CommandButton { get; set; }
        /// <summary>
        /// элемент кпонка станции
        /// </summary>
        public ButtonStation ButtonStation { get; set; }
        /// <summary>
        /// Сетевой номер станции которой принадлежат команды телеуправления
        /// </summary>
        public int StationNumberCommand { get; set; }
        List<SettingsCommand> _collectioncommand = new List<SettingsCommand>();
        /// <summary>
        /// коллекция всех команд для каждого шага построения 
        /// </summary>
        public List<SettingsCommand> CollectionCommand
        {
            get
            {
                return _collectioncommand;
            }
            set
            {
                _collectioncommand = value;
            }
        }

        public SettingsStep()
        {
            StartPath = string.Empty;
            EndPath = string.Empty;
        }
    }
}
