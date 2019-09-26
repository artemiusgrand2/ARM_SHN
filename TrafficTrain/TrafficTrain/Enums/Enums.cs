using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrafficTrain.Enums
{
    /// <summary>
    /// различные виды пути в зависимости от тяги
    /// </summary>
    public enum ViewTraction
    {
        //путь с автономной тягой
        diesel_traction = 0,
        //путь с электрической тягой
        electric_traction = 1
    }

    /// <summary>
    /// виды блок участков
    /// </summary>
    public enum BlockView
    {
        control = 0,
        notcontrol = 1
    }

    enum ViewControlObject
    {
        none = 0,
        ktcm = 1,
        kgu = 2
    }

    /// <summary>
    ///  направления 
    /// </summary>
    public enum ViewDirection
    {
        left = 0,
        right = 1
    }

    public enum EvenFilter
    {
        /// <summary>
        /// неизвестна четность поезда
        /// </summary>
        unknown = 0,
        /// <summary>
        /// поезд четный
        /// </summary>
        even,
        /// <summary>
        /// поезд нечетный
        /// </summary>
        odd
    }


    public enum EventVisibleElement
    {
        train =0,
        track,
        electro_track,
        platform,
        traintable
    }
    //public enum ViewTrain
    //{
    //    //неизвестна четность поезда
    //    none =0,
    //    /// <summary>
    //    /// грузовой поезд
    //    /// </summary>
    //    freight,
    //    /// <summary>
    //    /// пассажирский поезд
    //    /// </summary>
    //    passenger,
    //    /// <summary>
    //    /// пригородный
    //    /// </summary>
    //    suburban,
    //    /// <summary>
    //    /// локомотив
    //    /// </summary>
    //    locomotive,
    //    /// <summary>
    //    /// хозяйственный
    //    /// </summary>
    //    economic
    //}

    /// <summary>
    /// виды состояния сигнала
    /// </summary>
    public enum ViewSostSignal
    {
        none = 0,
        invitational = 1,
        signal = 2,
        shunting = 3
    }

    enum ViewWindow
    {
        mainwindow = 0,
        otherwindow = 1,
        detailview = 2
    }

    public enum SizeAligment
    {
        none = 0,
        vectical = 1,
        horizontal = 2
    }

    public enum ViewMessageJournal
    {
        none = 0,
        sound = 1,
        diagnostik = 2
    }

    /// <summary>
    /// виды комманд
    /// </summary>
    public enum ViewCommandTU
    {
        /// <summary>
        /// задание маршрута
        /// </summary>
        install_route = 0,
        /// <summary>
        /// отмена маршрута
        /// </summary>
        cancel_route = 1,
        /// <summary>
        /// задание маршрута без сигнала
        /// </summary>
        notsignal = 2,
        /// <summary>
        /// разрешение отправления
        /// </summary>
        resolution = 3
    }

    /// <summary>
    /// возможные состояния элементов проекта для команд ТУ
    /// </summary>
    public enum ViewmodeCommand
    {
        /// <summary>
        /// нет никаного контроля
        /// </summary>
        none = 0,
        /// <summary>
        /// отмена маршрута
        /// </summary>
        cancel_route,
        /// <summary>
        /// установка маршрута
        /// </summary>
        install_route,
        /// <summary>
        /// установка маршрута без сигнала
        /// </summary>
        notsignal,
        /// <summary>
        /// Траектория маршрута
        /// </summary>
        route,
        /// <summary>
        /// Пояснения по маршруту
        /// </summary>
        advert, 
        /// <summary>
        /// Проверка маршрута на проезд
        /// </summary>
        passage,
        /// <summary>
        /// Проверка маршрута на возможность задания
        /// </summary>
        check,
        /// <summary>
        /// Проверка маршрута на возможность задания со стороны соседней станции на врождебные маршруты
        /// </summary>
        check_next_station,
        /// <summary>
        /// проверка направления
        /// </summary>
        direction,
        /// <summary>
        /// разрешение отправления
        /// </summary>
        resolution
    }

    public enum ViewArrivalDeparture
    {
        not_normal = 0,
        normal
    }

    enum ViewTable
    {
        table_train = 0,
        table_command_auto = 1
    }

    /// <summary>
    /// виды контроля управления станцией
    /// </summary>
    public enum ViewStateStation
    {
        /// <summary>
        /// Нет связи со станцией
        /// </summary>
        not_connect = 0,
        /// <summary>
        /// Сезонное управление
        /// </summary>
        seasonal_management = 1,
        /// <summary>
        /// Резервное управление
        /// </summary>
        reserve_control = 2,
        /// <summary>
        /// Диспетчерское управление
        /// </summary>
        supervisory_control = 4,
        /// <summary>
        /// автономное управление станцией
        /// </summary>
        autonomous_control = 5,
        /// <summary>
        /// режим автопилота
        /// </summary>
        auto_supervisory = 5,
    }

    enum ViewPartRoute
    {
        none = 0,
        block_point = 1,
        track = 2
    }

    public enum ViewTypeCommandAuto
    {
        /// <summary>
        /// открыть маршрут
        /// </summary>
        open_route = 0,
        /// <summary>
        /// закрыть маршрут
        /// </summary>
        close_route = 1
    }

    public enum ViewCheckRoute
    {
        /// <summary>
        /// маршрут не установлен и не устанавливается
        /// </summary>
        none = 0,
        /// <summary>
        /// маршрут установливается
        /// </summary>
        install = 1,
        /// <summary>
        /// маршрут установлен
        /// </summary>
        installed = 2
    }

    public enum StatusInstalled
    {
        none = 0,
        yesPro = 1,
        installed = 2
    }

    public enum FieldType
    {
        UintType = 0,
        intType,
        floatType
    }

}
