//using System;
//using SCADA.Common.Enums;

//namespace TrafficTrain.DataProject
//{
//    /// <summary>
//    /// класс описывающий возможные состояния элементов и комбинации импульсов при которых они возможны
//    /// </summary>
//    public class StateElement
//    {
//        /// <summary>
//        /// режим отрисовки
//        /// </summary>
//        public ViewElementDraw ViewControlDraw { get; set; }

//        private DateTime _lastUpdate = DateTime.Now;
//        /// <summary>
//        /// время последнего изменения состояния
//        /// </summary>
//        public DateTime LastUpdate { get { return _lastUpdate; } set { _lastUpdate = value; } }
//        /// <summary>
//        /// состояние элемента
//        /// </summary>
//        public StatesControl state { get; set; }
//        /// <summary>
//        /// комбинация импульсов
//        /// </summary>
//        public string Impuls { get; set; }
//        /// <summary>
//        /// название режима
//        /// </summary>
//        public Viewmode Name { get; set; }
//        /// <summary>
//        /// Было ли обновлено состояние
//        /// </summary>
//        public bool Update { get; set; }

//        public StateElement()
//        {
//            Impuls = string.Empty;
//        }
//    }
//}
