using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalParse;
using sdm.diagnostic_section_model;
using sdm.diagnostic_section_model.client_impulses;

namespace TrafficTrain
{
    class SetImpuls
    {
        /// <summary>
        /// получаем значение состояния
        /// </summary>
        /// <param name="Station">номер станции</param>
        /// <param name="inNot"></param>
        /// <returns></returns>
        static public InfixNotation.infix_states SetValueImpuls(int Station, ref InfixNotation inNot)
        {
            try
            {
                foreach (string name in inNot._impulsesNames)
                {
                    switch (MainWindow.ClientImpulses.data.Stations[Station].TS.GetState(name))
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
                return inNot.Compute();
            }
            catch(Exception error)
            {
                return InfixNotation.infix_states.UncontrolledState;
            }
        }
    }
}
