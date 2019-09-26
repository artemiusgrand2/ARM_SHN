using System;

namespace TrafficTrain.DataProject
{
   [Serializable]
    /// <summary>
    /// описание точки выхода перегона
    /// </summary>
    public class PointMove
    {
        /// <summary>
        /// Название перегона
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Сетевой код станции слева
        /// </summary>
        public int StationLeft { get; set; }
        /// <summary>
        /// Сетевой код станции справа
        /// </summary>
        public int StationRight { get; set; }
    }
}
