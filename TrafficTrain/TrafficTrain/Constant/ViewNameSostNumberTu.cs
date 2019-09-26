
namespace TrafficTrain.Constant
{
    /// <summary>
    /// возможные условные обозначения видов элеметов управления
    /// </summary>
    public struct ViewNameSostNumberTu
    {
        /// <summary>
        /// сезонное управление
        /// </summary>
        public const string seasonal_management = "1";
        /// <summary>
        /// разрешение отправления на перегон
        /// </summary>
        public const string yes_departure = "2";
        /// <summary>
        /// отменить отправления на перегон
        /// </summary>
        public const string no_departure = "3";
        /// <summary>
        /// установка маршрута
        /// </summary>
        public const string yes_route_setting = "4";
        /// <summary>
        /// отмена маршрута
        /// </summary>
        public const string no_route_setting = "5";
    }
}
