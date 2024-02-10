using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;

namespace TrafficTrain
{
    public class Diagnostic
    {
        public static List<string> DiagnosticControl(StateElement element)
        {
            var result = new List<string>();
            if (element.Messages != null)
            {
                if (element.state != StatesControl.nocontrol)
                {
                    if (element.Messages.TryGetValue(element.state, out var elementMesages))
                        result.Add(LoadProject.CreateMessages(elementMesages, DateTime.Now, DateTime.MinValue));
                    ////
                    //if (element.stateActiv != StatesControl.nocontrol)
                    //{
                    //    if (element.state == StatesControl.activ)
                    //    {
                    //        if (element.Messages.ContainsKey(StatesControl.pasiv))
                    //            result.Add(LoadProject.CreateMessages(element.Messages[StatesControl.pasiv], element.LastUpdate, DateTime.Now));
                    //    }
                    //    else
                    //    {
                    //        if (element.Messages.ContainsKey(StatesControl.activ))
                    //            result.Add(LoadProject.CreateMessages(element.Messages[StatesControl.activ], element.LastUpdate, DateTime.Now));
                    //    }
                    //}
                    ////
                    //element.LastUpdate = DateTime.Now;
                    //element.stateActiv = element.state;
                }
            }
            //
            return result;
        }
    }
}
