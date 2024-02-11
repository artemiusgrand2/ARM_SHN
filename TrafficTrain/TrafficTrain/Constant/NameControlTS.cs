using System;
using System.Collections.Generic;
using System.Text;
using SCADA.Common.Enums;

namespace ARM_SHN.Constant
{
    /// <summary>
    /// возможные условные обозначения контролей
    /// </summary>
    public  struct NameControlTS
    {
        //Виды состояния кнопки станции
     
        public const string not_station = "нет связи со станцией";
        public const string fire = "пожар";
        //Виды состояния элемента перегонной стрелки
        //Виды состояния элемента сигнал
        public const string passage = "проезд";
        public const string signal = "сигнал";
        public const string invitational = "пригласительный";
        /// <summary>
        /// Замыкание поездное
        /// </summary>
        public const string locking = "замыкание";
        /// <summary>
        /// Замыкание маневровое
        /// </summary>
        public const string lockingM = "замыканием";
        /// <summary>
        /// Замыкание аврийное
        /// </summary>
        public const string lockingY = "замыканиежелт";
        public const string indication = "индикация";

        //Контроль для шильд
        public const string controlWhite = "контрбелый";
        public const string controlRed = "контркрасн";
        public const string controlYellow = "контржелт";
        public const string controlRedF = "контркраснмиг";
        public const string controlYellowF = "контржелтмиг";

        public const string installation = "установка";
        public const string cutting = "разделка";
        public const string shunting = "маневровый";
        //Виды состояния элемента главный путь
        public const string auto_run = "авто действие";
        public const string electrification = "электро";
        public const string pass = "пасс";
        //Виды состояния элемента переезд
        public const string closing = "закрытие";
        public const string closing_button = "закрытие кнопкой";
        //Виды состояния общие для всех
        public const string occupation = "занятие";
        public const string fault = "неисправность";
        public const string accident = "авария";
        public const string accident_dga = "авариядга";
        public const string weight_on = "нагрузка";
        public const string weight_off = "безнагрузка";
        //Для искусственного импульса
        public const string impuls_activ = "а";
        public const string impuls_pasiv = "п";

        /// <summary>
        /// проверяем состояние с уже имеющимися
        /// </summary>
        /// <param name="namestate">названия состояния из таблицы</param>
        /// <returns></returns>
        public static Viewmode GetStateTS(string name, string namestate, int stationnumber, log4net.ILog log)
        {
            if (namestate != null && namestate.Length > 0)
            {
                switch (namestate.ToLower())
                {
                    case accident:
                        return Viewmode.accident;
                    case indication:
                        return Viewmode.indication;
                    case cutting:
                        return Viewmode.cutting;
                    case shunting:
                        return Viewmode.shunting;
                    case auto_run:
                        return Viewmode.auto_run;
                    case closing:
                        return Viewmode.closing;
                    case lockingM:
                        return Viewmode.lockingM;
                    case lockingY:
                        return Viewmode.lockingY;
                    case closing_button:
                        return Viewmode.closing_button;
                    case electrification:
                        return Viewmode.electrification;
                    case pass:
                        return Viewmode.pass;
                    case fault:
                        return Viewmode.fault;
                    case fire:
                        return Viewmode.fire;
                    case installation:
                        return Viewmode.installation;
                    case invitational:
                        return Viewmode.invitational;
                    case locking:
                        return Viewmode.locking;
                    case not_station:
                        return Viewmode.not_station;
                    case occupation:
                        return Viewmode.occupation;
                    case passage:
                        return Viewmode.passage;
                    case signal:
                        return Viewmode.signal;
                    case controlWhite:
                        return Viewmode.controlWhite;
                    case controlRed:
                        return Viewmode.controlRed;
                    case controlYellow:
                        return Viewmode.controlYellow;
                    case controlRedF:
                        return Viewmode.controlRedF;
                    case controlYellowF:
                        return Viewmode.controlYellowF;
                    case impuls_activ:
                        return Viewmode.impuls_activ;
                    case impuls_pasiv:
                        return Viewmode.impuls_pasiv;
                    default:
                        log.Info(string.Format("Неправильное состояния {0} для описания объекта {1} на станции {2}", namestate, name, stationnumber));
                        break;
                }
            }
            return Viewmode.none;
        }
    }

}
