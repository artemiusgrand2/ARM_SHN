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
            }
            catch { }
        }
    }
}
