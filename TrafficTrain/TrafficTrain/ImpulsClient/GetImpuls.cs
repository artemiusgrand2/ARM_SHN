using System;
using System.Collections.Generic;
using TrafficTrain.Impulsesver.Client;
using TrafficTrain.WorkWindow;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;
using SCADA.Common.LogicalParse;

namespace TrafficTrain
{
    class GetImpuls
    {
        /// <summary>
        /// получаем значение состояния
        /// </summary>
        /// <param name="stationDefault">номер станции</param>
        /// <param name="inNot"></param>
        /// <returns></returns>
        static private InfixNotation.infix_states GetValueImpuls(int stationDefault, string Formula)
        {
            try
            {
                InfixNotation inNot = new InfixNotation(Formula);
                foreach (string name in inNot._impulsesNames)
                {
                    var nameImpuls = name;
                    var station = ParseStationNumber(ref nameImpuls, stationDefault);
                    if (Connections.ClientImpulses.data.Stations.ContainsKey(station))
                    {
                        switch (Connections.ClientImpulses.data.Stations[station].TS.GetState(nameImpuls))
                        {
                            case ImpulseState.ActiveState:
                                {
                                    inNot._impulsesValues[name] = InfixNotation.infix_states.ActiveState;
                                    break;
                                }
                            case ImpulseState.PassiveState:
                                {
                                    inNot._impulsesValues[name] = InfixNotation.infix_states.PassiveState;
                                    break;
                                }
                            default: inNot._impulsesValues[name] = InfixNotation.infix_states.UncontrolledState;
                                break;
                        }
                    }
                }
                return inNot.Compute();
            }
            catch
            {
                return InfixNotation.infix_states.UncontrolledState;
            }
        }

        private static int ParseStationNumber(ref string nameImpuls, int stationDefault)
        {
           var cells = nameImpuls.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
           if (cells.Length > 1)
           {
               int buffer;
               if (int.TryParse(cells[0], out buffer))
               {
                   nameImpuls = nameImpuls.Replace(cells[0] + ".", string.Empty);
                   return buffer;
               }
           }
            //
           return stationDefault;
        }

        /// <summary>
        /// находим состояние элемента индикации
        /// </summary>
        /// <param name="stationnumber">номер станции контроля</param>
        /// <param name="impuls">формула импульсов управляющих</param>
        /// <returns></returns>
        static public StatesControl GetStateControl(int stationnumber, string impuls)
        {
            switch (GetImpuls.GetValueImpuls(stationnumber, impuls))
            {
                case InfixNotation.infix_states.ActiveState:
                    return StatesControl.activ;
                case InfixNotation.infix_states.PassiveState:
                    return StatesControl.pasiv;
                default:
                    return StatesControl.nocontrol;
            }
        }
    }
}
