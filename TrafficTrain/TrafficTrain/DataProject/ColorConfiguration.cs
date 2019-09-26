using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficTrain.DataGrafik
{
    /// <summary>
    /// цветовая натройка экрана
    /// </summary>
    [Serializable]
    public class Screen
    {
        /// <summary>
        /// цвет фона экрана
        /// </summary>
        public string Fon { get; set; }
        /// <summary>
        /// цвет управляющей стрелки
        /// </summary>
        public string ArrowCommand { get; set; }

        public Screen()
        {
            Fon = string.Empty;
            ArrowCommand = string.Empty;
        }
    }

    /// <summary>
    /// разбивка цвета в формате RGB
    /// </summary>
    [Serializable]
    public class ColorN
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B{ get; set; }
    }

    /// <summary>
    /// цветовая натройка названия станции
    /// </summary>
    [Serializable]
    public class StationName
    {
        /// <summary>
        /// цвет текста при наличии поездов
        /// </summary>
        public string Train { get; set; }
        /// <summary>
        /// цвет текста для показа номера поезда
        /// </summary>
        public string Track { get; set; }

        public StationName()
        {
            Train = string.Empty;
            Track = string.Empty;
        }
    }


    /// <summary>
    /// цветовая натройка области станции
    /// </summary>
    [Serializable]
    public class AreaStation
    {
        /// <summary>
        /// цвет рамки
        /// </summary>
        public string Stroke { get; set; }
        /// <summary>
        /// цвет фона
        /// </summary>
        public string Fon { get; set; }

        public AreaStation()
        {
            Stroke = string.Empty;
            Fon = string.Empty;
        }
    }


    /// <summary>
    /// цветовая натройка области перегона
    /// </summary>
    [Serializable]
    public class AreaStrage
    {
        /// <summary>
        /// цвет рамки при нормальной работе
        /// </summary>
        public string NormalStroke { get; set; }
        /// <summary>
        /// цвет рамки при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }

        /// <summary>
        /// цвет текста при нормальной работе
        /// </summary>
        public string NormalText { get; set; }
        /// <summary>
        /// цвет текста при каком - либо несоответствии
        /// </summary>
        public string NotNormalText { get; set; }

        /// <summary>
        /// цвет фона при занятом перегоне
        /// </summary>
        public string ActiveFon { get; set; }
        /// <summary>
        /// цвет фона при свободном перегоне
        /// </summary>
        public string PasiveFon { get; set; }
        /// <summary>
        /// цвет фона при потери контроля перегона
        /// </summary>
        public string NotControlFon { get; set; }

        public AreaStrage()
        {
            NormalStroke = string.Empty;
            NotControlStroke = string.Empty;
            NormalText = string.Empty;
            NotNormalText = string.Empty;
            ActiveFon = string.Empty;
            PasiveFon = string.Empty;
            NotControlFon = string.Empty;
        }
    }

    /// <summary>
    /// цветовая натройка пути перегона
    /// </summary>
    [Serializable]
    public class TrackStrage
    {
        /// <summary>
        /// при занятом пути перегона
        /// </summary>
        public string Active { get; set; }
        /// <summary>
        /// при свободном пути перегона
        /// </summary>
        public string Pasive { get; set; }
        /// <summary>
        /// при потери контроля
        /// </summary>
        public string NotControl { get; set; }
        /// <summary>
        /// при замкнутом пути перегона
        /// </summary>
        public string Locking { get; set; }

        public TrackStrage()
        {
            Active = string.Empty;
            Pasive = string.Empty;
            NotControl = string.Empty;
            Locking = string.Empty;
        }
    }

    /// <summary>
    /// цветовая натройка станционного пути
    /// </summary>
    [Serializable]
    public class StationTrack
    {
        /// <summary>
        /// при рамки при автодействии
        /// </summary>
        public string AutoRunStroke { get; set; }
        /// <summary>
        /// при рамки если путь не электрофицирован
        /// </summary>
        public string DiselStroke { get; set; }
        /// <summary>
        ///  при рамки если путь электрофицирован
        /// </summary>
        public string ElectroStroke { get; set; }
        /// <summary>
        ///  рамка при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }
        /// <summary>
        /// при фона если путь замкнут
        /// </summary>
        public string LockingFon { get; set; }
        /// <summary>
        /// при фона если путь занят
        /// </summary>
        public string ActiveFon { get; set; }
        /// <summary>
        /// при фона если путь свободен
        /// </summary>
        public string PasiveFon { get; set; }
        /// <summary>
        /// при фона если путь не контролируется
        /// </summary>
        public string NotControlFon { get; set; }
        /// <summary>
        /// при фона если путь огражден
        /// </summary>
        public string FencingFon { get; set; }
        /// <summary>
        /// при текста для названии пути
        /// </summary>
        public string TrackText { get; set; }
        /// <summary>
        /// при текста для номера поезда
        /// </summary>
        public string TrainText { get; set; }
        /// <summary>
        /// при текста для номера поезда с планом
        /// </summary>
        public string TrainPlanText { get; set; }
        /// <summary>
        /// при текста для векторов
        /// </summary>
        public string VectorText { get; set; }

        public StationTrack()
        {
            AutoRunStroke = string.Empty;
            DiselStroke = string.Empty;
            ElectroStroke = string.Empty;
            NotControlStroke = string.Empty;
            LockingFon = string.Empty;
            ActiveFon = string.Empty;
            PasiveFon = string.Empty;
            NotControlFon = string.Empty;
            FencingFon = string.Empty;
            TrackText = string.Empty;
            TrainText = string.Empty;
            TrainPlanText = string.Empty;
            VectorText = string.Empty;
        }
    }



    /// <summary>
    /// цветовая натройка сигнала (маршрута)
    /// </summary>
    [Serializable]
    public class RouteSignal
    {
        /// <summary>
        /// рамки по умолчанию
        /// </summary>
        public string DefultStroke { get; set; }
        /// <summary>
        /// рамки при получении маршрута первый
        /// </summary>
        public string ReceivedOneStroke { get; set; }
        /// <summary>
        /// рамки при плучении маршрута второй
        /// </summary>
        public string ReceivedTyStroke { get; set; }
        /// <summary>
        /// рамки при проверке маршрута 
        /// </summary>
        public string CheckRouteStroke { get; set; }
        /// <summary>
        /// рамки при ожидании установки маршрута первый
        /// </summary>
        public string WaitInstallOneStroke { get; set; }
        /// <summary>
        /// рамки при ожидании установки маршрута второй
        /// </summary>
        public string WaitInstallTyStroke { get; set; }
        /// <summary>
        /// рамка  установка маршрута первый
        /// </summary>
        public string InstallOneStroke { get; set; }
        /// <summary>
        /// рамка установка маршрута второй
        /// </summary>
        public string InstallTyStroke { get; set; }
        /// <summary>
        /// рамка при несиправности
        /// </summary>
        public string FaultStroke { get; set; }
        /// <summary>
        /// рамка при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }
       
        //-------------------

        /// <summary>
        /// фон - занят
        /// </summary>
        public string ActiceFon { get; set; }
        /// <summary>
        /// фон - проезд
        /// </summary>
        public string PasiveFon { get; set; }
        /// <summary>
        /// фон - замкнут
        /// </summary>
        public string LockingFon { get; set; }
        /// <summary>
        /// фон - пригласительный сигнал первый
        /// </summary>
        public string InvitationalOneFon { get; set; }
        /// <summary>
        /// фон - пригласительный сигнал второй
        /// </summary>
        public string InvitationalTyFon { get; set; }
        /// <summary>
        /// фон - поездной сигнал
        /// </summary>
        public string SignalFon { get; set; }
        /// <summary>
        /// фон - маневровый сигнал
        /// </summary>
        public string ShuntingFon { get; set; }
        /// <summary>
        /// фон - ограждение
        /// </summary>
        public string FencingFon { get; set; }
        /// <summary>
        /// фон - проезд
        /// </summary>
        public string PassageFon { get; set; }
        /// <summary>
        /// фон при потери контроля
        /// </summary>
        public string NotControlFon { get; set; }

        public RouteSignal()
        {
            DefultStroke = string.Empty;
            ReceivedOneStroke = string.Empty;
            ReceivedTyStroke = string.Empty;
            CheckRouteStroke = string.Empty;
            WaitInstallOneStroke = string.Empty;
            WaitInstallTyStroke = string.Empty;
            InstallOneStroke = string.Empty;
            InstallTyStroke = string.Empty;
            FaultStroke = string.Empty;
            NotControlStroke = string.Empty;
            ActiceFon = string.Empty;
            PasiveFon = string.Empty;
            LockingFon = string.Empty;
            InvitationalOneFon = string.Empty;
            InvitationalTyFon = string.Empty;
            SignalFon = string.Empty;
            ShuntingFon = string.Empty;
            FencingFon = string.Empty;
            PassageFon = string.Empty;
            NotControlFon = string.Empty;
        }
    }

    /// <summary>
    /// цветовая натройка сигнала (поездного и маневрового светофора)
    /// </summary>
    [Serializable]
    public class Signal
    {
        /// <summary>
        /// рамки по умолчанию
        /// </summary>
        public string DefultStroke { get; set; }
        /// <summary>
        /// рамка при несиправности
        /// </summary>
        public string FaultStroke { get; set; }
        /// <summary>
        /// рамка при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }
        /// <summary>
        /// рамка при установке маршрута (открытие сигнала)
        /// </summary>
        public string InstallStroke { get; set; }

        //-------------------

        /// <summary>
        /// фон - пригласительный сигнал первый
        /// </summary>
        public string InvitationalOneFon { get; set; }
        /// <summary>
        /// фон - пригласительный сигнал второй
        /// </summary>
        public string InvitationalTyFon { get; set; }
        /// <summary>
        /// фон - поездной сигнал
        /// </summary>
        public string SignalFon { get; set; }
        /// <summary>
        /// фон - маневровый сигнал
        /// </summary>
        public string ShuntingFon { get; set; }
        /// <summary>
        /// фон при потери контроля
        /// </summary>
        public string NotControlFon { get; set; }
        /// <summary>
        /// цвет закрытого сигнала
        /// </summary>
        public string CloseSignalFon { get; set; }
          /// <summary>
        /// цвет головки по умолчанию
        /// </summary>
        public string DefultFon { get; set; }

        public Signal()
        {
            DefultStroke = string.Empty;
            FaultStroke = string.Empty;
            NotControlStroke = string.Empty;
            InstallStroke = string.Empty;
            InvitationalOneFon = string.Empty;
            InvitationalTyFon = string.Empty;
            SignalFon = string.Empty;
            ShuntingFon = string.Empty;
            NotControlFon = string.Empty;
            CloseSignalFon = string.Empty;
            DefultFon = string.Empty;
        }
    }


    /// <summary>
    /// цветовая натройка переезда
    /// </summary>
    [Serializable]
    public class Move
    {
        /// <summary>
        /// рамка - неисправность
        /// </summary>
        public string FaultStroke { get; set; }
        /// <summary>
        /// рамка - авария
        /// </summary>
        public string AccidentStroke { get; set; }
        /// <summary>
        ///  рамка - при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }
        /// <summary>
        /// рамка - по умолчанию
        /// </summary>
        public string DefultStroke { get; set; }

        //-----------------

        /// <summary>
        /// фон - автозакрытие
        /// </summary>
        public string AutoClosedFon { get; set; }
        /// <summary>
        /// фон - закрытие кнопкой
        /// </summary>
        public string ButtonClosedFon { get; set; }
        /// <summary>
        ///  фон -  при потери контроля
        /// </summary>
        public string NotControlFon { get; set; }
        /// <summary>
        /// фон - по умолчанию
        /// </summary>
        public string DefultFon { get; set; }

        public Move()
        {
            FaultStroke = string.Empty;
            AccidentStroke = string.Empty;
            NotControlStroke = string.Empty;
            DefultStroke = string.Empty;
            AutoClosedFon = string.Empty;
            ButtonClosedFon = string.Empty;
            NotControlFon = string.Empty;
            DefultFon = string.Empty;
        }
    }

    /// <summary>
    /// цветовая натройка контрольного объекта (КГУ и КТСМ и другие)
    /// </summary>
    [Serializable]
    public class ControlObject
    {
        /// <summary>
        /// рамка - неисправность
        /// </summary>
        public string FaultStroke { get; set; }
        /// <summary>
        /// рамка - авария
        /// </summary>
        public string AccidentStroke { get; set; }
        /// <summary>
        ///  рамка - при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }
        /// <summary>
        /// рамка - по умолчанию
        /// </summary>
        public string DefultStroke { get; set; }

        //-----------------

        /// <summary>
        /// фон - если элемент сработал
        /// </summary>
        public string PlayFon { get; set; }
        /// <summary>
        ///  фон -  при потери контроля
        /// </summary>
        public string NotControlFon { get; set; }
        /// <summary>
        /// фон - по умолчанию
        /// </summary>
        public string DefultFon { get; set; }

        public ControlObject()
        {
            FaultStroke = string.Empty;
            AccidentStroke = string.Empty;
            NotControlStroke = string.Empty;
            DefultStroke = string.Empty;
            PlayFon = string.Empty;
            NotControlFon = string.Empty;
            DefultFon = string.Empty;
        }
    }

    /// <summary>
    /// цветовая натройка кнопки станции
    /// </summary>
    [Serializable]
    public class ButtonStation
    {
        /// <summary>
        /// рамка - неисправность
        /// </summary>
        public string FaultStroke { get; set; }
        /// <summary>
        /// рамка - авария
        /// </summary>
        public string AccidentStroke { get; set; }
        /// <summary>
        ///  рамка - при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }
        /// <summary>
        /// рамка - по умолчанию
        /// </summary>
        public string DefultStroke { get; set; }

        //-----------------

        /// <summary>
        /// фон - резервный контроль
        /// </summary>
        public string ReserveControlFon { get; set; }
        /// <summary>
        /// фон - автономный контроль
        /// </summary>
        public string AutonomousСontrolFon { get; set; }
        /// <summary>
        /// фон - сезонный контроль
        /// </summary>
        public string SesonControlFon { get; set; }
        /// <summary>
        /// фон - диспетчерский контроль
        /// </summary>
        public string DispatcherControlFon { get; set; }
        /// <summary>
        /// фон - станция не на  диспетчерском контроле
        /// </summary>
        public string NotDispatcherControlFon { get; set; }
        /// <summary>
        /// фон - автопилот контроль
        /// </summary>
        public string AutoDispatcherControlFon { get; set; }
        /// <summary>
        ///  фон -  при потери контроля
        /// </summary>
        public string NotControlFon { get; set; }
        /// <summary>
        /// Пожар
        /// </summary>
        public string FireControlFon { get; set; }
        /// <summary>
        /// Цвет фона по умолчанию
        /// </summary>
        public string DefultFon { get; set; }

        public ButtonStation()
        {
            FaultStroke = string.Empty;
            AccidentStroke = string.Empty;
            NotControlStroke = string.Empty;
            DefultStroke = string.Empty;

            ReserveControlFon = string.Empty;
            AutonomousСontrolFon = string.Empty;
            SesonControlFon = string.Empty;
            DispatcherControlFon = string.Empty;
            NotDispatcherControlFon = string.Empty;
            AutoDispatcherControlFon = string.Empty;
            NotControlFon = string.Empty;
            FireControlFon = string.Empty;
            DefultFon = string.Empty;
        }
    }

    /// <summary>
    /// цветовая натройка стрелок направлений
    /// </summary>
    [Serializable]
    public class Arrow
    {
        /// <summary>
        ///  рамка - при потери контроля
        /// </summary>
        public string NotControlStroke { get; set; }
        /// <summary>
        /// рамка - по умолчанию
        /// </summary>
        public string DefultStroke { get; set; }

        //-----------------

        /// <summary>
        /// фон - блока занятия, если перегон занят
        /// </summary>
        public string ActiveStrageFon { get; set; }
        /// <summary>
        /// фон - блока занятия, если перегон свободен
        /// </summary>
        public string PasiveStrageFon { get; set; }
        /// <summary>
        /// фон - нет конроля перегона или несоответствие
        /// </summary>
        public string NotControlStrageFon { get; set; }
        /// <summary>
        /// фон - отправление
        /// </summary>
        public string DepartureDirectonFon { get; set; }
        /// <summary>
        /// фон - ожидание отправления
        /// </summary>
        public string WaitingDepartureDirectonFon { get; set; }
        /// <summary>
        /// фон - разрешение отправления
        /// </summary>
        public string OKDepartureDirectonFon { get; set; }

        public Arrow()
        {
            NotControlStroke = string.Empty;
            DefultStroke = string.Empty;
            ActiveStrageFon = string.Empty;
            PasiveStrageFon = string.Empty;
            NotControlStrageFon = string.Empty;
            DepartureDirectonFon = string.Empty;
            WaitingDepartureDirectonFon = string.Empty;
            OKDepartureDirectonFon = string.Empty;
        }
    }


    /// <summary>
    /// цветовая натройка автивной линии
    /// </summary>
    [Serializable]
    public class ActiveLine
    {
        /// <summary>
        /// рамка - занята
        /// </summary>
        public string ActiveStroke { get; set; }
        /// <summary>
        /// рамка - свободна
        /// </summary>
        public string PasiveStroke { get; set; }
        /// <summary>
        ///  рамка - замкнута
        /// </summary>
        public string LocingStroke { get; set; }
        /// <summary>
        /// рамка - ограждена
        /// </summary>
        public string FencingStroke { get; set; }
        /// <summary>
        /// рамка - разделка первый
        /// </summary>
        public string СuttingOneStroke { get; set; }
        /// <summary>
        /// рамка - разделка второй
        /// </summary>
        public string СuttingTyStroke { get; set; }
        /// <summary>
        /// рамка - проезд
        /// </summary>
        public string PassegeStroke { get; set; }
        /// <summary>
        /// рамка - нет контроля
        /// </summary>
        public string NotControlStroke { get; set; }

        public ActiveLine()
        {
            ActiveStroke = string.Empty;
            PasiveStroke = string.Empty;
            LocingStroke = string.Empty;
            FencingStroke = string.Empty;
            СuttingOneStroke = string.Empty;
            СuttingTyStroke = string.Empty;
            PassegeStroke = string.Empty;
            NotControlStroke = string.Empty;
        }
    }


    /// <summary>
    /// цветовая натройка шильды
    /// </summary>
    [Serializable]
    public class HelpElement
    {
        /// <summary>
        /// фон - по умолчанию
        /// </summary>
        public string DefultFon { get; set; }
        /// <summary>
        /// фон - при работе ДГА с нагрузкой
        /// </summary>
        public string OnWeightFon { get; set; }
        /// <summary>
        /// фон - при работе ДГА без нагрузки
        /// </summary>
        public string OffWeightFon { get; set; }
        /// <summary>
        /// фон - авария
        /// </summary>
        public string AccidentFon { get; set; }
        /// <summary>
        /// фон - нет контроля
        /// </summary>
        public string NotControlFon { get; set; }
        /// <summary>
        /// рамка - авария
        /// </summary>
        public string AccidentDGAStroke { get; set; }
        /// <summary>
        /// рамка - неисправность
        /// </summary>
        public string FaultStroke { get; set; }
        /// <summary>
        /// рамка - по умолчанию
        /// </summary>
        public string DefultStroke { get; set; }
        /// <summary>
        /// рамка - нет контроля
        /// </summary>
        public string NotControlStroke { get; set; }
        /// <summary>
        /// ттекст
        /// </summary>
        public string Text { get; set; }

        public HelpElement()
        {
            DefultFon = string.Empty;
            OnWeightFon = string.Empty;
            OffWeightFon = string.Empty;
            AccidentFon = string.Empty;
            NotControlFon = string.Empty;
            AccidentDGAStroke = string.Empty;
            FaultStroke = string.Empty;
            DefultStroke = string.Empty;
            NotControlStroke = string.Empty;
            Text = string.Empty;
        }
    }


    /// <summary>
    /// цветовая настройка  справочного текста и часов
    /// </summary>
    [Serializable]
    public class HelpTextAndTime
    {
        /// <summary>
        /// фон - по умолчанию у часов
        /// </summary>
        public string TimeFon { get; set; }
        /// <summary>
        /// Текст - часы
        /// </summary>
        public string TextTime { get; set; }
        /// <summary>
        /// Рамка - часы
        /// </summary>
        public string StrokeTime { get; set; }
        /// <summary>
        /// Текст - справочный текст
        /// </summary>
        public string TextHelp { get; set; }
        /// <summary>
        /// Текст - справки
        /// </summary>
        public string TextMessage { get; set; }

        public HelpTextAndTime()
        {
            TimeFon = string.Empty;
            TextTime = string.Empty;
            StrokeTime = string.Empty;
            TextHelp = string.Empty;
            TextMessage = string.Empty;
        }
    }

    /// <summary>
    /// управляющие элементы
    /// </summary>
    [Serializable]
    public class ManagmentElement
    {
        /// <summary>
        /// рамка - по умолчанию 
        /// </summary>
        public string StrokeDefult { get; set; }
        /// <summary>
        /// текст - справочного элемента
        /// </summary>
        public string TextHelpMessage { get; set; }
        /// <summary>
        /// текст -  кнопки переключения
        /// </summary>
        public string TextSwitchButton { get; set; }
        /// <summary>
        /// текст -  журналов
        /// </summary>
        public string TextJournal { get; set; }
        /// <summary>
        /// фон нет сообщений
        /// </summary>
        public string NotMessageFon { get; set; }
        /// <summary>
        /// фон есть сообщений
        /// </summary>
        public string OkMessageFon { get; set; }
        /// <summary>
        /// фон переключатель включен
        /// </summary>
        public string OnSwitchFon { get; set; }
        /// <summary>
        /// фон переключатель выключен
        /// </summary>
        public string OffSwitchFon { get; set; }

        /// <summary>
        /// справочная строка фон
        /// </summary>
        public string HelpStringFon { get; set; }
        /// <summary>
        /// справочная строка строка
        /// </summary>
        public string HelpStringStroke { get; set; }

        public ManagmentElement()
        {
            StrokeDefult = string.Empty;
            TextHelpMessage = string.Empty;
            TextJournal = string.Empty;
            TextSwitchButton = string.Empty;
            NotMessageFon = string.Empty;
            OkMessageFon = string.Empty;
            OnSwitchFon = string.Empty;
            OffSwitchFon = string.Empty;
            HelpStringFon = string.Empty;
            HelpStringStroke = string.Empty;
        }
    }


    /// <summary>
    /// Информационная таблица по поездам
    /// </summary>
    [Serializable]
    public class TrainTable
    {
        /// <summary>
        /// рамка - по умолчанию 
        /// </summary>
        public string StrokeDefult { get; set; }
        /// <summary>
        /// текст - данных
        /// </summary>
        public string TextDefult { get; set; }
        /// <summary>
        /// текст - заголовка
        /// </summary>
        public string TextHeader { get; set; }
        /// <summary>
        /// текст поезда с планом
        /// </summary>
        public string TextTrainPlan { get; set; }
        /// <summary>
        /// фон - внутрення  не связанная справка
        /// </summary>
        public string NotFixedReferenceInsideFon { get; set; }
        /// <summary>
        /// фон - внешняя  не связанная справка
        /// </summary>
        public string NotFixedReferenceOutsideFon { get; set; }
        /// <summary>
        /// фон - поезд без справки
        /// </summary>
        public string TrainWithoutReferenceFon { get; set; }
        /// <summary>
        /// фон - поезд с справки
        /// </summary>
        public string TrainWithReferenceFon { get; set; }
        /// <summary>
        /// фон выделен
        /// </summary>
        public string AllocatedFon { get; set; }
        /// <summary>
        /// фон заголовка
        /// </summary>
        public string HeaderColumn { get; set; }

        public TrainTable()
        {
            StrokeDefult = string.Empty;
            TextDefult = string.Empty;
            TextHeader = string.Empty;
            TextTrainPlan = string.Empty;
            NotFixedReferenceInsideFon = string.Empty;
            NotFixedReferenceOutsideFon = string.Empty;
            TrainWithoutReferenceFon = string.Empty;
            TrainWithReferenceFon = string.Empty;
            AllocatedFon = string.Empty;
            HeaderColumn = string.Empty;
        }
    }


    /// <summary>
    /// Информационная таблица по командам автопилота 
    /// </summary>
    [Serializable]
    public class AutoPilotTable
    {
        /// <summary>
        /// рамка - по умолчанию 
        /// </summary>
        public string StrokeDefult { get; set; }
        /// <summary>
        /// текст - данных
        /// </summary>
        public string TextDefult { get; set; }
        /// <summary>
        /// текст - заголовка
        /// </summary>
        public string TextHeader { get; set; }
        /// <summary>
        /// фон - команда получена без ошибок
        /// </summary>
        public string CommandReceivedFon { get; set; }
        /// <summary>
        /// фон - команда получена с ошибками
        /// </summary>
        public string CommandErrorFon { get; set; }
        /// <summary>
        /// фон - команда проходит проверку
        /// </summary>
        public string CommandCheckFon { get; set; }
        /// <summary>
        /// фон - команда отправлена
        /// </summary>
        public string CommandSendFon { get; set; }
        /// <summary>
        /// фон - команда выполнена
        /// </summary>
        public string CommandExecutedFon { get; set; }
        /// фон заголовка
        /// </summary>
        public string HeaderColumn { get; set; }

        public AutoPilotTable()
        {
            StrokeDefult = string.Empty;
            TextDefult = string.Empty;
            TextHeader = string.Empty;
            CommandReceivedFon = string.Empty;
            CommandErrorFon = string.Empty;
            CommandCheckFon = string.Empty;
            CommandSendFon = string.Empty;
            CommandExecutedFon = string.Empty;
            HeaderColumn = string.Empty;
        }
    }

    /// <summary>
    /// Общее для таблиц
    /// </summary>
    [Serializable]
    public class OthersTable
    {
        /// <summary>
        /// сетка
        /// </summary>
        public string Grid { get; set; }
        /// <summary>
        /// текст - если выделен
        /// </summary>
        public string IsSelectText { get; set; }
        /// <summary>
        /// фон - если выделен
        /// </summary>
        public string IsSelectFon { get; set; }

        public OthersTable()
        {
            Grid = string.Empty;
            IsSelectText = string.Empty;
            IsSelectFon = string.Empty;
        }
    }

    /// <summary>
    /// контекстное меню
    /// </summary>
    [Serializable]
    public class ContexMenu
    {
        /// <summary>
        /// текст - по умолчанию
        /// </summary>
        public string Defult { get; set; }
        /// <summary>
        /// текст- команды ТУ
        /// </summary>
        public string CommandTU { get; set; }
        /// <summary>
        /// текст - работа с ГИДом
        /// </summary>
        public string CommandGID { get; set; }
        /// <summary>
        /// текст - планирование
        /// </summary>
        public string Planning { get; set; }

        public ContexMenu()
        {
            Defult = string.Empty;
            CommandTU = string.Empty;
            CommandGID = string.Empty;
            Planning = string.Empty;
        }
    }

    /// <summary>
    /// значение цвета
    /// </summary>
    [Serializable]
    public class ValueColor
    {
        /// <summary>
        /// Название цвета
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение цвета
        /// </summary>
        public ColorN Value { get; set; }
    }

    /// <summary>
    /// описание цветового проекта
    /// </summary>
    [Serializable]
    public class ColorConfiguration
    {

        private List<ValueColor> _colorvalues = new List<ValueColor>();
        /// <summary>
        /// коллекция значений цветов
        /// </summary>
        public List<ValueColor> ColorValues
        {
            get
            {
                return _colorvalues;
            }
            set
            {
                _colorvalues = value;
            }
        }

        private List<string> _colornames = new List<string>();
        /// <summary>
        /// коллекция названий цветов
        /// </summary>
        public List<string> ColorNames
        {
            get
            {
                return _colornames;
            }
            set
            {
                _colornames = value;
            }
        }
        /// <summary>
        /// Цвет экрана
        /// </summary>
        public Screen Screen { get; set; }
        /// <summary>
        /// Название станции
        /// </summary>
        public StationName NameStation { get; set; }
        /// <summary>
        /// Рамка станции (область станции)
        /// </summary>
        public AreaStation AreaStation { get; set; }
        /// <summary>
        /// область прегона для вывода номеров поездов
        /// </summary>
        public AreaStrage AreaStrage { get; set; }
        /// <summary>
        /// путь перегона
        /// </summary>
        public TrackStrage TrackStrage { get; set; }
        /// <summary>
        /// станционный путь
        /// </summary>
        public StationTrack Track { get; set; }
        /// <summary>
        /// маршрут
        /// </summary>
        public RouteSignal RouteSignal { get; set; }
        /// <summary>
        /// переезд
        /// </summary>
        public Move Move { get; set; }
        /// <summary>
        /// Контрольный объект
        /// </summary>
        public ControlObject ControlObject { get; set; }
        /// <summary>
        /// кнопка станции
        /// </summary>
        public ButtonStation ButtonStation { get; set; }
        /// <summary>
        /// блок направлений
        /// </summary>
        public Arrow Direction { get; set; }
        /// <summary>
        /// активная линия
        /// </summary>
        public ActiveLine ActiveLine { get; set; }
        /// <summary>
        /// справочный элемент (шильда)
        /// </summary>
        public HelpElement HelpElement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ManagmentElement ManagmentElement { get; set; }
        /// <summary>
        /// справочный текст и часы
        /// </summary>
        public HelpTextAndTime HelpTextAndTime { get; set; }
        /// <summary>
        ///таблица поездов
        /// </summary>
        public TrainTable TrainTable { get; set; }
        /// <summary>
        /// таблица автопилота
        /// </summary>
        public AutoPilotTable AutoPilotTable { get; set; }
        /// <summary>
        /// другие таблицы
        /// </summary>
        public OthersTable CommonTable { get; set; }
        /// <summary>
        /// контекстное меню
        /// </summary>
        public ContexMenu ContexMenu { get; set; }
        /// <summary>
        /// светофор
        /// </summary>
        public Signal Signal { get; set; }
        /// <summary>
        /// название цветовой расскраски
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// нахождение
        /// </summary>
        public string File { get; set; }
    }
}
