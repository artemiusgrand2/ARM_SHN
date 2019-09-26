using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficTrain.Impulsesver.Client.requests
{
    unsafe class ImpulsesAnswer
    {
        /// <summary>
        /// Заголовок ответа.
        /// </summary>
        public ImpulsesAnswerHeader Header;

        /// <summary>
        /// Импульсы ТС.
        /// </summary>
        public byte[] TsImpulses;

        /// <summary>
        /// Импульсы ТУ.
        /// </summary>
        public byte[] TuImpulses;
    }
}
