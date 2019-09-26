using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrafficTrain
{
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// точка вставки форма перегона
        /// </summary>
        public double StartStrage_X { get; set; }
        /// <summary>
        /// точка вставки форма перегона
        /// </summary>
        public double StartStrage_Y { get; set; }
        /// <summary>
        /// ширина форма перегона
        /// </summary>
        public double StartStrage_Widht { get; set; }
        /// <summary>
        ///  высота форма перегона
        /// </summary>
        public double StartStrage_Height { get; set; }
        /// <summary>
        /// Включено ли автоматическое обновление стилей
        /// </summary>
        public bool IsUpdateStyle { get; set; }
    }
}
