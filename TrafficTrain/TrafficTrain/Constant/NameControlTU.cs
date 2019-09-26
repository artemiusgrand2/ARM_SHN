using TrafficTrain.Enums;

namespace TrafficTrain.Constant
{
    /// <summary>
    /// возможные состояния элементов описания автодействия
    /// </summary>
    public struct NameControlAnalis
    {
        const string install = "Установка";
        const string notsignal = "безсигнала";
        const string cancel = "Отмена";
        const string route = "Маршрут";
        const string advert = "Объявление";
        const string passage = "Проезд";
        const string check = "Проверка";
        const string check_next_station = "Проверка сосед станция";
        const string direction = "Направление";
        const string resolution = "Разрешение отправления";

        public static ViewmodeCommand GetStateAnalisTU(string name, string namestate, int stationnumber, log4net.ILog log)
        {
            if (namestate != null && namestate.Length > 0)
            {
                switch (namestate.Trim(new char[] { ' ' }))
                {
                    case advert:
                        return ViewmodeCommand.advert;
                    case check_next_station:
                        return ViewmodeCommand.check_next_station;
                    case cancel:
                        return ViewmodeCommand.cancel_route;
                    case direction:
                        return ViewmodeCommand.direction;
                    case resolution:
                        return ViewmodeCommand.resolution;
                    case check:
                        return ViewmodeCommand.check;
                    case install:
                        return ViewmodeCommand.install_route;
                    case notsignal:
                        return ViewmodeCommand.notsignal;
                    case passage:
                        return ViewmodeCommand.passage;
                    case route:
                        return ViewmodeCommand.route;
                    default:
                        log.Info(string.Format("Неправильное состояния {0} для описания объекта {1} автопилота на станции {2}", namestate, name, stationnumber));
                        break;
                }
            }
            return ViewmodeCommand.none;
        }
    }
}
