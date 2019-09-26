using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TrafficTrain.DataGrafik;
using TrafficTrain.WorkWindow;
using TrafficTrain.Delegate;

namespace TrafficTrain
{
    class LoadColorControl
    {
        #region Variable
        /// <summary>
        /// Событие для изменения цветового набора проекта
        /// </summary>
        public static event NewColor NewColor;
        private static Dictionary<int, Brush> _collection_color = new Dictionary<int, Brush>();
        #endregion

        public static void UpdateColor()
        {
            if (NewColor != null)
                NewColor();
        }
        public static void AnalisLoad()
        {
            if (LoadProject.ColorConfiguration == null)
                LoadProject.ColorConfiguration = new DataGrafik.ColorConfiguration();
            //Коллекция номеров поездов
                //экран
                if (LoadProject.ColorConfiguration.Screen == null)
                    LoadProject.ColorConfiguration.Screen = new DataGrafik.Screen();
                //название станции
                if (LoadProject.ColorConfiguration.NameStation == null)
                    LoadProject.ColorConfiguration.NameStation = new DataGrafik.StationName();
                //область станции
                if (LoadProject.ColorConfiguration.AreaStation == null)
                    LoadProject.ColorConfiguration.AreaStation = new DataGrafik.AreaStation();
                //область перегона
                if (LoadProject.ColorConfiguration.AreaStrage == null)
                    LoadProject.ColorConfiguration.AreaStrage = new DataGrafik.AreaStrage();
                //путь перегона
                if (LoadProject.ColorConfiguration.TrackStrage == null)
                    LoadProject.ColorConfiguration.TrackStrage = new DataGrafik.TrackStrage();
                //станционный путь
                if (LoadProject.ColorConfiguration.Track == null)
                    LoadProject.ColorConfiguration.Track = new DataGrafik.StationTrack();
                //переезд
                if (LoadProject.ColorConfiguration.Move == null)
                    LoadProject.ColorConfiguration.Move = new DataGrafik.Move();
                //контрольный объект
                if (LoadProject.ColorConfiguration.ControlObject == null)
                    LoadProject.ColorConfiguration.ControlObject = new DataGrafik.ControlObject();
                //кнопка станции
                if (LoadProject.ColorConfiguration.ButtonStation == null)
                    LoadProject.ColorConfiguration.ButtonStation = new DataGrafik.ButtonStation();
                //активная линия
                if (LoadProject.ColorConfiguration.ActiveLine == null)
                    LoadProject.ColorConfiguration.ActiveLine = new DataGrafik.ActiveLine();
                //блок направления
                if (LoadProject.ColorConfiguration.Direction == null)
                    LoadProject.ColorConfiguration.Direction = new DataGrafik.Arrow();
                //сигнал (маршрут)
                if (LoadProject.ColorConfiguration.RouteSignal == null)
                    LoadProject.ColorConfiguration.RouteSignal = new DataGrafik.RouteSignal();
                //шильда
                if (LoadProject.ColorConfiguration.HelpElement == null)
                    LoadProject.ColorConfiguration.HelpElement = new DataGrafik.HelpElement();
                //справочный текст и время
                if (LoadProject.ColorConfiguration.HelpTextAndTime == null)
                    LoadProject.ColorConfiguration.HelpTextAndTime = new DataGrafik.HelpTextAndTime();
                //контекстное меню
                if (LoadProject.ColorConfiguration.ContexMenu == null)
                    LoadProject.ColorConfiguration.ContexMenu = new DataGrafik.ContexMenu();
                //справочные таблицы
                if (LoadProject.ColorConfiguration.CommonTable == null)
                    LoadProject.ColorConfiguration.CommonTable = new DataGrafik.OthersTable();
                //таблица автопилота
                if (LoadProject.ColorConfiguration.AutoPilotTable == null)
                    LoadProject.ColorConfiguration.AutoPilotTable = new DataGrafik.AutoPilotTable();
                //таблица информация по поездам
                if (LoadProject.ColorConfiguration.TrainTable == null)
                    LoadProject.ColorConfiguration.TrainTable = new DataGrafik.TrainTable();
                //управляющие элементы
                if (LoadProject.ColorConfiguration.ManagmentElement == null)
                    LoadProject.ColorConfiguration.ManagmentElement = new DataGrafik.ManagmentElement();
                //управляющие элементы
                if (LoadProject.ColorConfiguration.Signal == null)
                    LoadProject.ColorConfiguration.Signal = new DataGrafik.Signal();
        }

        public void CreatePanel(ComboBox ComBoxColorNames, WrapPanel MainPanel, WindowColor window)
        {
            try
            {
                AnalisLoad();
                ControlColorStatus status1, status2, status3, status4, status5, status6, status7, status8,
                                   status9, status10, status11, status12, status13, status14, status15, status16;
                //
                ComBoxColorNames.ItemsSource = LoadProject.ColorConfiguration.ColorNames;

                status1 = new ControlColorStatus("Номер поезда", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.NameStation.Train, window, NameStation._color_train, EnumColor.NameStationTrain) });
                status2 = new ControlColorStatus("По умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.NameStation.Track, window, NameStation._color_track, EnumColor.NameStationTrack) });
                ControlColorElement namestation = new ControlColorElement("Название станции", new List<ControlColorStatus>() { status1, status2 });
                MainPanel.Children.Add(namestation);
                //Рамка станции
                status1 = new ControlColorStatus("Фон", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStation.Fon, window, RamkaStation._colorfill, EnumColor.AreaStationFon) });
                status2 = new ControlColorStatus("Граница", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStation.Stroke, window, RamkaStation._colorstroke, EnumColor.AreaStationStroke) });
                ControlColorElement ramkastation = new ControlColorElement("Рамка станции", new List<ControlColorStatus>() { status1, status2 });
                MainPanel.Children.Add(ramkastation);
                //Область перегона
                status1 = new ControlColorStatus("Свободный перегон", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStrage.PasiveFon, window, NumberTrainRamka._color_pasiv, EnumColor.AreaStragePasiveFon) });
                status2 = new ControlColorStatus("Занятый перегон", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStrage.ActiveFon, window, NumberTrainRamka._coloractiv, EnumColor.AreaStrageActiveFon) });
                status3 = new ControlColorStatus("Неконтролируемый перегон", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStrage.NotControlFon, window, NumberTrainRamka._colornotcontrol, EnumColor.AreaStrageNotControlFon) });
                status4 = new ControlColorStatus("Граница норм", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStrage.NormalStroke, window, NumberTrainRamka._color_ramka, EnumColor.AreaStrageNormalStroke) });
                status5 = new ControlColorStatus("Граница неконтрол", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStrage.NotControlStroke, window, NumberTrainRamka._colornotcontrolstroke, EnumColor.AreaStrageNotControlStroke) });
                status6 = new ControlColorStatus("Текст норм", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStrage.NormalText, window, NumberTrainRamka._color_text_defult, EnumColor.AreaStrageNormalText) });
                status7 = new ControlColorStatus("Текст не норм", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AreaStrage.NotNormalText, window, NumberTrainRamka._color_text_notnormal, EnumColor.AreaStrageNotNormalText) });
                ControlColorElement areasrtage = new ControlColorElement("Область перегона", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7 });
                MainPanel.Children.Add(areasrtage);
                //путь перегона
                status1 = new ControlColorStatus("Свободный ", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrackStrage.Pasive, window, BlockSection._colorpassiv, EnumColor.TrackStragePasive) });
                status2 = new ControlColorStatus("Занятый", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrackStrage.Active, window, BlockSection._coloractiv, EnumColor.TrackStrageActive) });
                status3 = new ControlColorStatus("Неконтролируемый", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrackStrage.NotControl, window, BlockSection._colornotcontrolstroke, EnumColor.TrackStrageNotControl) });
                ControlColorElement track_strage = new ControlColorElement("Путь перегона", new List<ControlColorStatus>() { status1, status2, status3 });
                MainPanel.Children.Add(track_strage);
                //станционный путь
                status1 = new ControlColorStatus("Свободный ", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.PasiveFon, window, StationPath._colorpassiv, EnumColor.TrackPasiveFon) });
                status2 = new ControlColorStatus("Занятый", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.ActiveFon, window, StationPath._coloractiv, EnumColor.TrackActiveFon) });
                status3 = new ControlColorStatus("Неконтролируемый", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.NotControlFon, window, StationPath._colornotcontrol, EnumColor.TrackNotControlFon) });
                status4 = new ControlColorStatus("Огражден ", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.FencingFon, window, StationPath._colorfencing, EnumColor.TrackFencingFon) });
                status5 = new ControlColorStatus("Замкнут", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.LockingFon, window, StationPath._color_loking, EnumColor.TrackLockingFon) });
                status6 = new ControlColorStatus("Электрофицирован", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.ElectroStroke, window, StationPath._colorelectric_traction, EnumColor.TrackElectroStroke) });
                status7 = new ControlColorStatus("Автономный", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.DiselStroke, window, StationPath.m_colordiesel_traction, EnumColor.TrackDiselStroke) });
                status8 = new ControlColorStatus("Граница неконтрол", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.NotControlStroke, window, StationPath._colornotcontrolstroke, EnumColor.TrackNotControlStroke) });
                status9 = new ControlColorStatus("Текст - путь", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.TrackText, window, StationPath._color_path, EnumColor.TrackTrackText) });
                status10 = new ControlColorStatus("Текст - поезд", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.TrainText, window, StationPath._color_train, EnumColor.TrackTrainText) });
                status11 = new ControlColorStatus("Текст - вектор", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.VectorText, window, StationPath._color_vertor_train, EnumColor.TrackVectorText) });
                status12 = new ControlColorStatus("Текст - поезд (план)", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Track.TrainPlanText, window, StationPath._color_train_plan, EnumColor.TrackTrainPlanText) });
                ControlColorElement track_station = new ControlColorElement("Станционный путь", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8, status9, status10, status11, status12 });
                MainPanel.Children.Add(track_station);
                //переезд
                status1 = new ControlColorStatus("Рамка неисправность", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.FaultStroke, window, Moves._color_faultmove, EnumColor.MoveFaultStroke) });
                status2 = new ControlColorStatus("Рамка авария", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.AccidentStroke, window, Moves._color_accident, EnumColor.MoveAccidentStroke) });
                status3 = new ControlColorStatus("Рамка нет контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.NotControlStroke, window, Moves._colornotcontrolstroke, EnumColor.MoveNotControlStroke) });
                status4 = new ControlColorStatus("Рамка переезд открыт", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.DefultStroke, window, Moves._color_moveopen, EnumColor.MoveDefultStroke) });
                status5 = new ControlColorStatus("Фон переезд открыт", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.DefultFon, window, Moves._color_fon_defult, EnumColor.MoveDefultFon) });
                status6 = new ControlColorStatus("Фон нет контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.NotControlFon, window, Moves._colornotcontrol, EnumColor.MoveNotControlFon) });
                status7 = new ControlColorStatus("Фон закрытие кнопкой", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.ButtonClosedFon, window, Moves._color_closingmove_button, EnumColor.MoveButtonClosedFon) });
                status8 = new ControlColorStatus("Фон авто закрытие", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Move.AutoClosedFon, window, Moves._color_closingmove_auto, EnumColor.MoveAutoClosedFon) });
                ControlColorElement move = new ControlColorElement("Переезд", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8 });
                MainPanel.Children.Add(move);
                //контрольный объект (ктсм и кгу и другие)
                status1 = new ControlColorStatus("Рамка неисправность", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ControlObject.FaultStroke, window, ControlObject._color_fault, EnumColor.ControlObjectFaultStroke) });
                status2 = new ControlColorStatus("Рамка нет контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ControlObject.NotControlStroke, window, ControlObject._colornotcontrolstroke, EnumColor.ControlObjectNotControlStroke) });
                status3 = new ControlColorStatus("Рамка по умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ControlObject.DefultStroke, window, ControlObject._color_normal, EnumColor.ControlObjectDefultStroke) });
                status4 = new ControlColorStatus("Фон устройство сработало", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ControlObject.PlayFon, window, ControlObject._color_break, EnumColor.ControlObjectPlayFon) });
                status5 = new ControlColorStatus("Фон нет контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ControlObject.NotControlFon, window, ControlObject._colornotcontrol, EnumColor.ControlObjectNotControlFon) });
                status6 = new ControlColorStatus("Фон по умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ControlObject.DefultFon, window, ControlObject._color_fon_defult, EnumColor.ControlObjectDefultFon) });
                ControlColorElement control_object = new ControlColorElement("Контрольный объект", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6 });
                MainPanel.Children.Add(control_object);
                //кпонка станции
                status1 = new ControlColorStatus("Резервный контроль", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.ReserveControlFon, window, ButtonStation._color_reserve_control, EnumColor.ButtonStationReserveControlFon) });
                status2 = new ControlColorStatus("Сезонный контроль", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.SesonControlFon, window, ButtonStation._color_sesoncontol, EnumColor.ButtonStationSesonControlFon) });
                status3 = new ControlColorStatus("Диспетчерский контроль", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.DispatcherControlFon, window, ButtonStation._color_dispatcher, EnumColor.ButtonStationDispatcherControlFon) });
                status4 = new ControlColorStatus("Автопилот", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.AutoDispatcherControlFon, window, ButtonStation._color_auto_dispatcher, EnumColor.ButtonStationAutoDispatcherControlFon) });
                status5 = new ControlColorStatus("Автономная станция", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.AutonomousСontrolFon, window, ButtonStation._color_autonomous_control, EnumColor.ButtonStationAutonomousСontrolFon) });
                status6 = new ControlColorStatus("Станция не входящая в участок", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.NotDispatcherControlFon, window, ButtonStation._color_not_dispatcher, EnumColor.ButtonStationNotDispatcherControlFon) });
                status7 = new ControlColorStatus("Потеря контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.NotControlFon, window, ButtonStation._color_notlink, EnumColor.ButtonStationNotControlFon), 
                                                                                           new ControlColor(LoadProject.ColorConfiguration.ButtonStation.DefultFon, window, ButtonStation._color_defult,EnumColor.ButtonStationDefultFon) });
                status8 = new ControlColorStatus("Пожар", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.FireControlFon, window, ButtonStation._color_fire, EnumColor.ButtonStationFireControlFon) });
                status9 = new ControlColorStatus("Неисправность", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.FaultStroke, window, ButtonStation._color_fault, EnumColor.ButtonStationFaultStroke) });
                status10 = new ControlColorStatus("Авария", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.AccidentStroke, window, ButtonStation._color_accident, EnumColor.ButtonStationAccidentStroke) });
                status11 = new ControlColorStatus("Потеря контроля для рамки", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.NotControlStroke, window, ButtonStation._colornotcontrolstroke, EnumColor.ButtonStationNotControlStroke) });
                status12 = new ControlColorStatus("Рамка по умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ButtonStation.DefultStroke, window, ButtonStation._color_stroke_normal, EnumColor.ButtonStationDefultStroke) });
                ControlColorElement button_station = new ControlColorElement("Кнопка станции", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8, status9, status10, status11, status12 });
                MainPanel.Children.Add(button_station);
                //активная линия 
                status1 = new ControlColorStatus("Занят", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ActiveLine.ActiveStroke, window, LineHelp._color_active, EnumColor.ActiveLineActiveStroke) });
                status2 = new ControlColorStatus("Свободен", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ActiveLine.PasiveStroke, window, LineHelp._color_pasive, EnumColor.ActiveLinePasiveStroke) });
                status3 = new ControlColorStatus("Нет контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ActiveLine.NotControlStroke, window, LineHelp._colornotcontrol, EnumColor.ActiveLineNotControlStroke) });
                status4 = new ControlColorStatus("Замкнут", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ActiveLine.LocingStroke, window, LineHelp._colorlocking, EnumColor.ActiveLineLocingStroke) });
                status5 = new ControlColorStatus("Огражден", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ActiveLine.FencingStroke, window, LineHelp._colorfencing, EnumColor.ActiveLineFencingStroke) });
                status6 = new ControlColorStatus("Проезд", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ActiveLine.PassegeStroke, window, LineHelp._color_passage, EnumColor.ActiveLinePassegeStroke) });
                status7 = new ControlColorStatus("Искуственная разделка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ActiveLine.СuttingOneStroke, window, LineHelp._color_cutting_one, EnumColor.ActiveLineСuttingOneStroke), 
                                                                                                 new ControlColor(LoadProject.ColorConfiguration.ActiveLine.СuttingTyStroke, window, LineHelp._color_cutting_ty, EnumColor.ActiveLineСuttingTyStroke) });
                ControlColorElement active_line = new ControlColorElement("Активная линия", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7 });
                MainPanel.Children.Add(active_line);
                //блок направления
                //status1 = new ControlColorStatus("Перегон занят", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.ActiveStrageFon, window, CenterDirection._color_occupation, EnumColor.DirectionActiveStrageFon) });
                status2 = new ControlColorStatus("Перегон свободен", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.PasiveStrageFon, window, Direction._color_pasive, EnumColor.DirectionPasiveStrageFon) });
                status3 = new ControlColorStatus("Перегон неконтролируется", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.NotControlStrageFon, window, Direction._colornotcontrol, EnumColor.DirectionNotControlStrageFon) });
                status4 = new ControlColorStatus("Отправление", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.DepartureDirectonFon, window, Direction._color_departure, EnumColor.DirectionDepartureDirectonFon) });
                status5 = new ControlColorStatus("Ожидание отправления", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.WaitingDepartureDirectonFon, window, Direction._color_wait_departure, EnumColor.DirectionWaitingDepartureDirectonFon) });
                status6 = new ControlColorStatus("Разрешение отправления", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.OKDepartureDirectonFon, window, Direction._color_ok_departure, EnumColor.DirectionOKDepartureDirectonFon) });
                status7 = new ControlColorStatus("Рамка - потеря контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.NotControlStroke, window, Direction._colornotcontrolstroke, EnumColor.DirectionNotControlStroke) });
                status8 = new ControlColorStatus("Рамка по умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Direction.DefultStroke, window, Direction._color_ramka, EnumColor.DirectionDefultStroke) });
                ControlColorElement direction = new ControlColorElement("Блок направления", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8 });
                MainPanel.Children.Add(direction);
                //сигнал (маршрут)
                status1 = new ControlColorStatus("Рамка по умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.DefultStroke, window, RouteSignal._color_ramka_defult, EnumColor.RouteSignalDefultStroke) });
                status2 = new ControlColorStatus("Получение маршрута", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.ReceivedOneStroke, window, RouteSignal._color_command_received_one, EnumColor.RouteSignalReceivedOneStroke),
                                                                                              new ControlColor(LoadProject.ColorConfiguration.RouteSignal.ReceivedTyStroke, window, RouteSignal._color_command_received_ty, EnumColor.RouteSignalReceivedTyStroke)});
                status3 = new ControlColorStatus("Проверка маршрута", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.CheckRouteStroke, window, RouteSignal._color_check_route, EnumColor.RouteSignalCheckRouteStroke) });
                status4 = new ControlColorStatus("Ожидание установки маршрута", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.WaitInstallOneStroke, window, RouteSignal._color_wait_install_one, EnumColor.RouteSignalWaitInstallOneStroke),
                                                                                                      new ControlColor(LoadProject.ColorConfiguration.RouteSignal.WaitInstallTyStroke, window, RouteSignal._color_wait_install_ty, EnumColor.RouteSignalWaitInstallTyStroke)});
                status5 = new ControlColorStatus("Неисправность", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.FaultStroke, window, RouteSignal._color_fault, EnumColor.RouteSignalFaultStroke) });
                status6 = new ControlColorStatus("Рамка потеря контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.NotControlStroke, window, RouteSignal._colornotcontrolstroke, EnumColor.RouteSignalNotControlStroke) });
                status7 = new ControlColorStatus("Занятие", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.ActiceFon, window, RouteSignal._color_active, EnumColor.RouteSignalActiceFon) });
                status8 = new ControlColorStatus("Свободность", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.PasiveFon, window, RouteSignal._color_pasive, EnumColor.RouteSignalPasiveFon) });
                status9 = new ControlColorStatus("Потеря контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.NotControlFon, window, RouteSignal._colornotcontrol, EnumColor.RouteSignalNotControlFon) });
                status10 = new ControlColorStatus("Замыкание", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.LockingFon, window, RouteSignal._color_locing, EnumColor.RouteSignalLockingFon) });
                status11 = new ControlColorStatus("Ограждение", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.FencingFon, window, RouteSignal._color_fencing, EnumColor.RouteSignalFencingFon) });
                status12 = new ControlColorStatus("Проезд", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.PassageFon, window, RouteSignal._color_passage, EnumColor.RouteSignalPassageFon) });
                status13 = new ControlColorStatus("Пригласительный", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.InvitationalOneFon, window, RouteSignal._color_invitational_one, EnumColor.RouteSignalInvitationalOneFon),
                                                                                            new ControlColor(LoadProject.ColorConfiguration.RouteSignal.InvitationalTyFon, window, RouteSignal._color_invitational_ty, EnumColor.RouteSignalInvitationalTyFon)});
                status14 = new ControlColorStatus("Установка маршрута", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.InstallOneStroke, window, RouteSignal._color_install_route_one, EnumColor.RouteSignalInstallOneStroke),
                                                                                            new ControlColor(LoadProject.ColorConfiguration.RouteSignal.InstallTyStroke, window, RouteSignal._color_install_route_ty, EnumColor.RouteSignalInstallTyStroke)});
                status15 = new ControlColorStatus("Маневровый", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.ShuntingFon, window, RouteSignal._color_shunting, EnumColor.RouteSignalShuntingFon) });
                status16 = new ControlColorStatus("Поездной", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.RouteSignal.SignalFon, window, RouteSignal._color_open, EnumColor.RouteSignalSignalFon) });
                ControlColorElement signal_route = new ControlColorElement("Сигнал (маршрут)", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8,
                                                                                                 status9, status10, status11, status12 , status13, status14, status15, status16});
                MainPanel.Children.Add(signal_route);
                //шильда
                status1 = new ControlColorStatus("Контроль Красного", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.AccidentFon, window, HelpElement._colorRed, EnumColor.HelpElementAccidentFon) });
                status2 = new ControlColorStatus("Контроль Желтого", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.AccidentDGAStroke, window, HelpElement._colorYellow, EnumColor.HelpElementAccidentDGAStroke) });
                status3 = new ControlColorStatus("Потеря контроля - фон", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.NotControlFon, window, HelpElement._colornotcontrol, EnumColor.HelpElementNotControlFon) });
                status4 = new ControlColorStatus("Потеря контроля - рамка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.NotControlStroke, window, HelpElement._colornotcontrolstroke, EnumColor.HelpElementNotControlStroke) });
                status5 = new ControlColorStatus("По умолчанию - фон", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.DefultFon, window, HelpElement._color_fon_defult, EnumColor.HelpElementDefultFon) });
                status6 = new ControlColorStatus("По умолчанию - рамка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.DefultStroke, window, HelpElement._color_stroke_defult, EnumColor.HelpElementDefultStroke) });
                status7 = new ControlColorStatus("Контроль белого", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.OnWeightFon, window, HelpElement._colorWhite, EnumColor.HelpElementOnWeightFon) });
                status8 = new ControlColorStatus("Текст", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpElement.Text, window, HelpElement._colortext, EnumColor.HelpElementText) });
                ControlColorElement help_element = new ControlColorElement("Шильда", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8 });
                MainPanel.Children.Add(help_element);
                //справочный текст и время
                status1 = new ControlColorStatus("Фон часов", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpTextAndTime.TimeFon, window, TimeElement._color_fon, EnumColor.HelpTextAndTimeTimeFon) });
                status2 = new ControlColorStatus("Рамка часов", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpTextAndTime.StrokeTime, window, TimeElement._color_stroke, EnumColor.HelpTextAndTimeStrokeTime) });
                status3 = new ControlColorStatus("Текст часов", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpTextAndTime.TextTime, window, TimeElement._color_text, EnumColor.HelpTextAndTimeTextTime) });
                status4 = new ControlColorStatus("Справочный элемент", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpTextAndTime.TextHelp, window, TextHelp._color_text, EnumColor.HelpTextAndTimeTextHelp) });
                status5 = new ControlColorStatus("Текст справки", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.HelpTextAndTime.TextMessage, window, SettingsWindow.m_color_text_message, EnumColor.HelpTextAndTimeTextMessage) });
                ControlColorElement help_text_time = new ControlColorElement("Справочный текст и время", new List<ControlColorStatus>() { status1, status2, status3, status4, status5 });
                MainPanel.Children.Add(help_text_time);
                //Общее для таблиц
                status1 = new ControlColorStatus("Сетка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.CommonTable.Grid, window, ColorCommonTable.Grid, EnumColor.CommonTableGrid) });
                status2 = new ControlColorStatus("Фон при выделении", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.CommonTable.IsSelectFon, window, ColorCommonTable.IsSelectFon, EnumColor.CommonTableIsSelectFon) });
                status3 = new ControlColorStatus("Текст при выделении", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.CommonTable.IsSelectText, window, ColorCommonTable.IsSelectText, EnumColor.CommonTableIsSelectText) });
                ControlColorElement otherstable = new ControlColorElement("Общее для таблиц", new List<ControlColorStatus>() { status1, status2, status3 });
                MainPanel.Children.Add(otherstable);
                //таблица автопилота
                status1 = new ControlColorStatus("Текст данных", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.TextDefult, window, ColorStateTableAutoPilot.Text, EnumColor.AutoPilotTableTextDefult) });
                status2 = new ControlColorStatus("Текст заголовка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.TextHeader, window, ColorStateTableAutoPilot.TextHeader, EnumColor.AutoPilotTableTextHeader) });
                status3 = new ControlColorStatus("Рамка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.StrokeDefult, window, ColorStateTableAutoPilot.Stroke, EnumColor.AutoPilotTableStrokeDefult) });
                status4 = new ControlColorStatus("Команда получена", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.CommandReceivedFon, window, ColorStateTableAutoPilot.ColorReceived, EnumColor.AutoPilotTableCommandReceivedFon) });
                status5 = new ControlColorStatus("Команда проверяется", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.CommandCheckFon, window, ColorStateTableAutoPilot.ColorCheck, EnumColor.AutoPilotTableCommandCheckFon) });
                status6 = new ControlColorStatus("Команда отправлена", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.CommandSendFon, window, ColorStateTableAutoPilot.ColorSend, EnumColor.AutoPilotTableCommandSendFon) });
                status7 = new ControlColorStatus("Команда выполнена", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.CommandExecutedFon, window, ColorStateTableAutoPilot.ColorExecuted, EnumColor.AutoPilotTableCommandExecutedFon) });
                status8 = new ControlColorStatus("Ошибка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.CommandErrorFon, window, ColorStateTableAutoPilot.ColorError, EnumColor.AutoPilotTableCommandErrorFon) });
                status9 = new ControlColorStatus("Заголовок", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.AutoPilotTable.HeaderColumn, window, ColorStateTableAutoPilot.ColumnHeader, EnumColor.AutoPilotTableHeaderColumn) });
                ControlColorElement table_autopilot = new ControlColorElement("Таблица автопилота", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8, status9 });
                MainPanel.Children.Add(table_autopilot);
                //таблица поездов
                status1 = new ControlColorStatus("Текст данных", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.TextDefult, window, ColorStateTableTrain.Text, EnumColor.TrainTableTextDefult) });
                status2 = new ControlColorStatus("Текст заголовка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.TextHeader, window, ColorStateTableTrain.TextHeader, EnumColor.TrainTableTextHeader) });
                status3 = new ControlColorStatus("Текст поезд план", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.TextTrainPlan, window, ColorStateTableTrain.TextPlan, EnumColor.TrainTableTextTrainPlan) });
                status4 = new ControlColorStatus("Рамка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.StrokeDefult, window, ColorStateTableTrain.StrokeDefult, EnumColor.TrainTableStrokeDefult) });
                status5 = new ControlColorStatus("Справка внутренняя", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.NotFixedReferenceInsideFon, window, ColorStateTableTrain.NotFixedReferenceInsideFon, EnumColor.TrainTableNotFixedReferenceInsideFon) });
                status6 = new ControlColorStatus("Справка внешняя", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.NotFixedReferenceOutsideFon, window, ColorStateTableTrain.NotFixedReferenceOutsideFon, EnumColor.TrainTableNotFixedReferenceOutsideFon) });
                status7 = new ControlColorStatus("Поезд без справки", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.TrainWithoutReferenceFon, window, ColorStateTableTrain.TrainWithoutReferenceFon, EnumColor.TrainTableTrainWithoutReferenceFon) });
                status8 = new ControlColorStatus("Поезд со справкой", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.TrainWithReferenceFon, window, ColorStateTableTrain.TrainWithReferenceFon, EnumColor.TrainTableTrainWithReferenceFon) });
                status9 = new ControlColorStatus("Заголовок", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.TrainTable.HeaderColumn, window, ColorStateTableTrain.ColumnHeader, EnumColor.TrainTableHeaderColumn) });
                ControlColorElement table_train = new ControlColorElement("Таблица поездов", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8, status9 });
                MainPanel.Children.Add(table_train);
                //управляющие элементы
                status1 = new ControlColorStatus("Текст справки", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.TextHelpMessage, window, CommandButton._color_text_help, EnumColor.ManagmentElementTextHelpMessage) });
                status2 = new ControlColorStatus("Текст переключарелей", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.TextSwitchButton, window, CommandButton._color_text_switch_button, EnumColor.ManagmentElementTextSwitchButton) });
                status3 = new ControlColorStatus("Текст журналов", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.TextJournal, window, CommandButton._color_text_journal, EnumColor.ManagmentElementTextJournal) });
                status4 = new ControlColorStatus("Рамка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.StrokeDefult, window, CommandButton._color_stroke, EnumColor.ManagmentElementStrokeDefult) });
                status5 = new ControlColorStatus("Есть сообщения", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.OkMessageFon, window, CommandButton._color_yes_message, EnumColor.ManagmentElementOkMessageFon) });
                status6 = new ControlColorStatus("Нет сообщения", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.NotMessageFon, window, CommandButton._color_no_message, EnumColor.ManagmentElementNotMessageFon) });
                status7 = new ControlColorStatus("Переключатель включен", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.OnSwitchFon, window, CommandButton._coloractiv, EnumColor.ManagmentElementOnSwitchFon) });
                status8 = new ControlColorStatus("Переключатель выключен", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.OffSwitchFon, window, CommandButton._colorpasiv, EnumColor.ManagmentElementOffSwitchFon) });
                status9 = new ControlColorStatus("Справочная строка фон", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.HelpStringFon, window, CommandButton._color_helpstring_fon, EnumColor.ManagmentElementHelpStringFon) });
                status10 = new ControlColorStatus("Справочная строка рамка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.ManagmentElement.HelpStringStroke, window, CommandButton._color_helpstring_stroke, EnumColor.ManagmentElementHelpStringStroke) });
                ControlColorElement managment = new ControlColorElement("Управляющие элементы", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8, status9, status10 });
                MainPanel.Children.Add(managment);
                //Светофор
                status1 = new ControlColorStatus("Рамка по умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.DefultStroke, window, LightTrain.m_color_stroke, EnumColor.SignalDefultStroke) });
                status2 = new ControlColorStatus("Установка", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.InstallStroke, window, LightTrain.m_color_install, EnumColor.SignalInstallStroke) });
                status3 = new ControlColorStatus("Рамка - нет контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.NotControlStroke, window, LightTrain.m_color_stroke_notcontrol, EnumColor.SignalNotControlStroke) });
                status4 = new ControlColorStatus("Поездной сигнал", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.SignalFon, window, LightTrain.m_color_open, EnumColor.SignalSignalFon) });
                status5 = new ControlColorStatus("Маневровый", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.ShuntingFon, window, LightTrain.m_color_shunting, EnumColor.SignalShuntingFon) });
                status6 = new ControlColorStatus("Пригласительный", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.InvitationalOneFon, window, LightTrain.m_color_invitation_one, EnumColor.SignalInvitationalOneFon), 
                                                                                           new ControlColor(LoadProject.ColorConfiguration.Signal.InvitationalTyFon, window, LightTrain.m_color_invitation_ty, EnumColor.SignalInvitationalTyFon)});
                status7 = new ControlColorStatus("Фон по умолчанию", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.DefultFon, window, LightTrain.m_color_defult, EnumColor.SignalDefultFon) });
                status8 = new ControlColorStatus("Запрещающий сигнал", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.CloseSignalFon, window, LightTrain.m_color_close, EnumColor.SignalCloseSignalFon) });
                status9 = new ControlColorStatus("Потеря контроля", new List<ControlColor>() { new ControlColor(LoadProject.ColorConfiguration.Signal.NotControlFon, window, LightTrain.m_color_fon_notcontrol, EnumColor.SignalNotControlFon) });
                ControlColorElement signal = new ControlColorElement("Светофор", new List<ControlColorStatus>() { status1, status2, status3, status4, status5, status6, status7, status8, status9, });
                MainPanel.Children.Add(signal);
            }
            catch { }
        }
    }
}
