using System;
using System.Collections.Generic;

namespace ARM_SHN.CommandsElement
{

    /// <summary>
    /// класс описывающий весь набор импульсов тс для станции 
    /// </summary>
    class StationTableTs
    {
        /// <summary>
        /// перечень объектов с описание состояниев
        /// </summary>
        public Dictionary<string, List<StateValue>> NamesValue { get; set; }

        public StationTableTs()
        {
            NamesValue = new Dictionary<string, List<StateValue>>();
        }
    }
}
