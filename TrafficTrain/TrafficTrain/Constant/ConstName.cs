using System;

namespace TrafficTrain.Constant
{
    /// <summary>
    /// зарезервированные слова
    /// </summary>
    public struct ConstName
    {
        public const string assignment_command = "A";
        public const string passage_route = "P";
        public const string check_route = "C";
        public const string move = "ПЕРЕГОН";
        public const string path = "ПУТЬ";
        /// <summary>
        /// обозначение блок участка
        /// </summary>
        public const string block_point = "Б";
        /// <summary>
        /// обозначение пути
        /// </summary>
        public const string new_track = "П";
        /// <summary>
        /// обозначение сигнала
        /// </summary>
        public const string signal = "С";
        public const string message_train = "Справка по поезду";
        public const string yes_route_setting = "Задать маршрут";
        public const string no_route_setting = "Отменить маршрут";
        public const string enter_departure = "Создать отправление";
        public const string enter_arrival = "Создать прибытие";
        public const string command_gid_path_arrival = "Задать путь прибытия";
        public const string update_gid_path_arrival = "Переезд с пути на путь";
        public const string join_train = "Соединить поезда";
        public const string join_train_and_mess = "Соединить справку с поездом";
        public const string enter_number_train = "Ввести номер поезда";
        public const string show_station_train = "Показать поезда на станции";
        public const string show_view_station = "Детальный вид станции";
        public const string reception_vector = "Начало обработки по приему";
        public const string edit_vector = "Корректировка обработки";
        public const string close_processing_train = "Окончание обработки";
        public const string delete_processing_train = "Удалить обработку";
        public const string departure_vector = "Начало обработки по отправлению";
        public const string transit_vector = "Начало обработки по транзиту";
        public const string delete_train = "Скрыть поезд";
        public const string show_train = "Показать поезд";
        public const string delete_message = "Скрыть справку";
        public const string show_message = "Показать справку";
        public const string delete_marker = "DEL";
        public const string standart_view = "Вернуться к первоначальному виду";
        public const string fullscreen_view = "Полноэкранный режим";
        public const string show_other_window = "Показать в отдельном окне";
        public const string fullscreen_view_station = "Растянуть станцию";
        public const string show_commandTU = "Показывать команды ТУ";
        public const string collapse_view = "Свернуть";
        public const string paste_train = "Вставить поезд";
        public const string copy_train = "Копировать поезд";
        public const string close_programm = "Закрыть";
        public const string exit = "Выход";
        public const string unknown_number_train = "X";
        public const string uslown_simvol = "*";
        public const string prefix_station = "TE";
        public const string admin = "admin";
        public const string user = "user";
        public const string verctorCreate = "ПРМ";
        public const string verctorRemove = "ОТПР";
        public const string verctorTrans = "ТРН";
        //
        public const string connect_server_impuls = "Есть связь с импульс сервером";
        public const string disconnect_server_impuls = "Нет связи с импульс сервером";
        public const string connect_server_train = "Есть связь с сервером поездов";
        public const string disconnect_server_train = "Нет связи с сервером поездов";
        public const string server_train = "servertrain";
        public const string server_impuls = "serverimpuls";
        //
        public const string sound_play = "Включить звук";
        public const string sound_cancel = "Отключить звук";
        public const string message = "Журнал сообщений";
        //
        public const string menu_color = "Изменение цвета";
        public const string menu_save = "Сохранить вид";
        //
        public const string menu_on_auto_control = "Включить автопилот";
        public const string menu_off_auto_control = "Выключить автопилот";
        /// <summary>
        /// удаление комманд из таблицы автопилота
        /// </summary>
        public const string delete_command = "Удалить";
    }
}
