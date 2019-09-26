using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficTrain
{
    class CheckElemet
    {
        private List<RouteSignal> _signals = new List<RouteSignal>();
        /// <summary>
        /// перечень сигналов с соседней станции
        /// </summary>
        public List<RouteSignal> Signals
        {
            get { return _signals; }
            set { _signals = value; }
        }
        /// <summary>
        /// комбинация импульсов
        /// </summary>
        public string Impuls { get; set; }

        public CheckElemet()
        {
            Impuls = string.Empty;
        }
    }
}
