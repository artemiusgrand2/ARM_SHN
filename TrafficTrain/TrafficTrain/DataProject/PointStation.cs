using System;

namespace TrafficTrain.DataProject
{
    [Serializable]
    /// <summary>
    /// описание точки выхода станции
    /// </summary>
    public class PointStation
    {
        /// <summary>
        /// Название станции
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Сетевой код станции
        /// </summary>
        public int Station { get; set; }
    }
}
