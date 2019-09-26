using System;

namespace TrafficTrain.DataGrafik
{
    [Serializable]
    public class ConfigurationFile
    {
        /// <summary>
        /// пользователь
        /// </summary>
        public string Users { get; set; }
        /// <summary>
        /// Нахождение файла конфигурации импульс сервера
        /// </summary>
        public string Cfgpath { get; set; }
        /// <summary>
        /// Нахождение таблиц ТУ и ТС
        /// </summary>
        public string Tablespath { get; set; }
        /// <summary>
        /// Ip - адрес и порт импульс сервера
        /// </summary>
        public string Constring { get; set; }
        /// <summary>
        /// нахождение графики проекта
        /// </summary>
        public string GrafickProject { get; set; }
        /// <summary>
        /// путь к файлу описанию перегонов
        /// </summary>
        public string MoveSettings { get; set; }
        /// <summary>
        /// путь к набору описаний ТС состояний обектов
        /// </summary>
        public string FilestationTS { get; set; }
        /// <summary>
        /// путь к набору ТУ команд
        /// </summary>
        public string FilestationTU { get; set; }
        /// <summary>
        /// интервал времени в миллисекундах через который обновляется информация по номерам поездов
        /// </summary>
        public int TrainIntervalUpdate { get; set; }
        /// <summary>
        /// строка подключения к базе ГИДА
        /// </summary>
        public string ConnectionStringGID { get; set; }
        /// <summary>
        /// Нахождение файла с таблицоми выхода поездов
        /// </summary>
        public string TrainTable { get; set; }
        /// <summary>
        /// Ip - адрес компьютера для получения справок по поездам
        /// </summary>
        public string TrainCertificateIp { get; set; }
        /// <summary>
        /// путь к описанию точек выхода
        /// </summary>
        public string FilePointOutput { get; set; }
        /// <summary>
        ///путь к звуковому файлу
        /// </summary>
        public string FileSound { get; set; }
        /// <summary>
        /// путь к файлу конфигурации стилей расскраски
        /// </summary>
        public string FileStyles { get; set; }
    }
}
