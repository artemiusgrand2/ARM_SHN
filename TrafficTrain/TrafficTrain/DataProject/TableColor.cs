using System.Windows.Media;

namespace TrafficTrain
{
    /// <summary>
    /// цветовая расцветка таблицы автопилота
    /// </summary>
    struct ColorStateTableAutoPilot
    {
        /// <summary>
        /// цвет состояния получения команды
        /// </summary>
        public static Brush ColorReceived = Brushes.LightBlue;
        /// <summary>
        /// цвет ожидания отправки команды (проверка)
        /// </summary>
        public static Brush ColorCheck = Brushes.Yellow;
        /// <summary>
        /// цвет ожидания выполнения  команды
        /// </summary>
        public static Brush ColorSend = Brushes.LightGreen;
        /// <summary>
        /// цвет выполнения команды
        /// </summary>
        public static Brush ColorExecuted = Brushes.LightGray;
        /// <summary>
        /// цвет ошибки
        /// </summary>
        public static Brush ColorError = Brushes.Red;
        /// <summary>
        /// цвет текста таблицы по умолчанию
        /// </summary>
        public static Brush Text = Brushes.Black;
        /// <summary>
        /// цвет текста заголовка
        /// </summary>
        public static Brush TextHeader = Brushes.Black;
        /// <summary>
        /// цвет рамки таблицы
        /// </summary>
        public static Brush Stroke = Brushes.Brown;
        /// <summary>
        /// цвет фона заголовка таблицы
        /// </summary>
        public static Brush ColumnHeader = Brushes.Red;
    }

    /// <summary>
    /// цветовая расскраска таблицы поездов
    /// </summary>
    struct ColorStateTableTrain
    {
        /// <summary>
        /// рамка - по умолчанию 
        /// </summary>
        public static Brush StrokeDefult = Brushes.Brown;
        /// <summary>
        /// текст - по умолчанию
        /// </summary>
        public static Brush Text = Brushes.Black;
        /// <summary>
        /// цвет текста поезда с планом
        /// </summary>
        public static Brush TextPlan = Brushes.Blue;
        /// <summary>
        /// цвет текста заголовка
        /// </summary>
        public static Brush TextHeader = Brushes.Black;
        /// <summary>
        /// фон - внутрення  не связанная справка
        /// </summary>
        public static Brush NotFixedReferenceInsideFon = Brushes.Red;
        /// <summary>
        /// фон - внешняя  не связанная справка
        /// </summary>
        public static Brush NotFixedReferenceOutsideFon = Brushes.OrangeRed;
        /// <summary>
        /// фон - поезд без справки
        /// </summary>
        public static Brush TrainWithoutReferenceFon = Brushes.Yellow;
        /// <summary>
        /// фон - поезд со справкой
        /// </summary>
        public static Brush TrainWithReferenceFon = Brushes.LightGreen;
        /// <summary>
        /// цвет фона заголовка таблицы
        /// </summary>
        public static Brush ColumnHeader = Brushes.Brown;
    }

    /// <summary>
    /// общее для таблиц
    /// </summary>
    struct ColorCommonTable
    {
        /// <summary>
        /// сетка
        /// </summary>
        public static Brush Grid = Brushes.Brown;
        /// <summary>
        /// текст - при выделении
        /// </summary>
        public static Brush IsSelectText = Brushes.Yellow;
        /// <summary>
        /// фон - при выделении
        /// </summary>
        public static Brush IsSelectFon = Brushes.Silver;
    }
}

