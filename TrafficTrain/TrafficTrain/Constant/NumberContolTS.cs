using System.Collections.Generic;

namespace TrafficTrain.Constant
{

    /// <summary>
    /// возможные условные обозначения видов активных элементов ввиде цифр
    /// </summary>
    public struct NumberContolTS
    {
        public static string button_station = "1";
        public static string signal = "2";
        public static string big_path = "3";
        public static string activ_line = "4";
        public static string activ_element = "5";
        public static string move = "6";
        public static string auto_supervisory = "7";
        public static string disconnectors = "8";
        public static string analogCell = "9";
        public static string service_impuls = "И";

        public static IList<string> Views = new List<string>()
        {
            button_station, signal, big_path, activ_line, activ_element, move, auto_supervisory, disconnectors, service_impuls, analogCell
        };
    }
}
