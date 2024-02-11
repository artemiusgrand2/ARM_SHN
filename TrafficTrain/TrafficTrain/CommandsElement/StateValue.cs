using System;
using System.Collections.Generic;
using SCADA.Common.LogicalParse;
using SCADA.Common.Enums;
using ARM_SHN.Enums;

namespace ARM_SHN.CommandsElement
{
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
                    LoadProject.Log.Error($"{headerMessage} {error.Message}");
                }
            }
        }
    }
}
