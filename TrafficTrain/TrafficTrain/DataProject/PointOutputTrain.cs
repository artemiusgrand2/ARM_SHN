using System;
using System.Collections.Generic;

namespace TrafficTrain.DataProject
{
     [Serializable]
    /// <summary>
    /// точки выхода поездов с участка
    /// </summary>
    public class PointOutputTrain
    {
        private List<PointStation> _pointstation = new List<PointStation>();
        private List<PointMove> _pointmove = new List<PointMove>();

        /// <summary>
        /// Список точек выхода со станции
        /// </summary>
        public List<PointStation> PointsStation
        {
            get
            {
                return _pointstation;
            }
            set
            {
                _pointstation = value;
            }
        }

        /// <summary>
        /// Список точек выхода с перегона
        /// </summary>
        public List<PointMove> PointsMove
        {
            get
            {
                return _pointmove;
            }
            set
            {
                _pointmove = value;
            }
        }
    }
}
