using System;
using System.Collections.Generic;
using SCADA.Common.LogicalParse;
using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using TrafficTrain.Enums;

namespace TrafficTrain
{

    /// <summary>
    /// класс описывающий сообщения
    /// </summary>
    class MessageInfo
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
        public  Dictionary<string, List<StateValue>> NamesValue { get; set; }

        public StationTableTs()
        {
            NamesValue = new Dictionary<string, List<StateValue>>();
        }
    }

    /// <summary>
    /// класс описывающий весь набор служебных импульсов тс для станции 
    /// </summary>
    class StationTableServiceTs
    {
        /// <summary>
        /// перечень объектов с описание состояниев
        /// </summary>
        public Dictionary<string, Dictionary<Viewmode,string>> NamesValue { get; set; }

        public StationTableServiceTs()
        {
            NamesValue = new Dictionary<string, Dictionary<Viewmode, string>>();
        }
    }

    /// <summary>
    /// класс состояние - формула для ТС и ТУ
    /// </summary>
    class StateValue
    {
        private Viewmode m_viewTs = Viewmode.none;
        /// <summary>
        /// вид контроля для TC
        /// </summary>
        public Viewmode ViewTS
        {
            get
            {
                return m_viewTs;
            }
        }

        private ViewmodeCommand m_viewTu = ViewmodeCommand.none;
        /// <summary>
        /// вид контроля для TУ
        /// </summary>
        public ViewmodeCommand ViewTU
        {
            get
            {
                return m_viewTu;
            }
        }

        string m_formula = string.Empty;
        /// <summary>
        /// формула контроля
        /// </summary>
        public string Formula
        {
            get
            {
                return m_formula;
            }
        }

        private IDictionary<StatesControl, string> m_messages = null;
        /// <summary>
        /// диагностические сообщения
        /// </summary>
        public IDictionary<StatesControl, string> Messages
        {
            get
            {
                return m_messages;
            }
            set
            {
                m_messages = value;
            }
        }


        public StateValue(string formula, Viewmode viewTs, ViewmodeCommand viewTu)
        {
            m_formula = formula;
            m_viewTs = viewTs;
            m_viewTu = viewTu;
            m_messages = new Dictionary<StatesControl, string>();
        }

        public void CheckFormula(string headerMessage)
        {
            if (m_viewTs != Viewmode.none)
            {
                try
                {
                    var check = new InfixNotation(m_formula);
                }
                catch (Exception error)
                {
                    LoadProject.Log.Error(string.Format("{0} {1}", headerMessage, error.Message));
                }
            }
        }
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
