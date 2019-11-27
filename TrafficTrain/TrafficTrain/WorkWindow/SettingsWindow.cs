using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using System.Linq;
using System.IO;
using System.Xml;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using System.Configuration;
using log4net;
using TrafficTrain.Interface;
using TrafficTrain.Enums;
using TrafficTrain.Delegate;
using TrafficTrain.Impulsesver.Client;

using SCADA.Common.Enums;
using SCADA.Common.Strage.SaveElement;

namespace TrafficTrain.WorkWindow
{
    class SettingsWindow
    {
        #region Variable
        /// <summary>
        /// вид окна
        /// </summary>
        ViewWindow _viewwindow;
        /// <summary>
        /// коллекция коэффициентов масштабирования окон детального вида
        /// </summary>
        Dictionary<ViewWindow, double> _collection_k = new Dictionary<ViewWindow, double>() { { ViewWindow.detailview, 1 }, { ViewWindow.otherwindow, 0.95 }, { ViewWindow.mainwindow, 1 } };
        /// <summary>
        /// ширина окна
        /// </summary>
        double _width;
        /// <summary>
        /// высота окна
        /// </summary>
        double _height;
        /// <summary>
        /// группа трансформаций
        /// </summary>
        TransformGroup groupTransform = new TransformGroup();
        /// <summary>
        /// Флаг закрытия программы
        /// </summary>
        public static bool CloseProg { get; set; }
        /// <summary>
        /// цвет фона
        /// </summary>
        public static Brush m_colorfon = new SolidColorBrush(Color.FromArgb(220, 175, 175, 175));
        /// <summary>
        /// цвет текста сообщения
        /// </summary>
        public static Brush m_color_text_message = Brushes.Black;
        /// <summary>
        /// положительное изменение масштаба
        /// </summary>
        public  double ScrollMinusMain = 0.95;
        /// <summary>
        /// отрицательное изменение масштаба
        /// </summary>
        public  double ScrollPlusMain = 1.05;
        /// <summary>
        /// текущая координата курсора по оси X
        /// </summary>
        double m_cursorX;
        /// <summary>
        /// текущая координата курсора по оси Y
        /// </summary>
        double m_cursorY;
        /// <summary>
        /// текущий масштаб
        /// </summary>
        double m_currentscale = 1;
        public double CurrentScale
        {
            get
            {
                return m_currentscale;
            }
        }
        /// <summary>
        /// минимально допустимый масштаб
        /// </summary>
        const double m_scrolminimum = 0.2;
        /// <summary>
        /// максимально допустимый масштаб
        /// </summary>
        const double m_scrolmax = 1 / m_scrolminimum;
        /// <summary>
        /// есть попадание на кнопку станции
        /// </summary>
        bool _hitbuttonstation = false;
        /// <summary>
        /// точка относительного центра координат
        /// </summary>
        Point m_Po = new Point(0, 0);
        public Point Zenter
        {
            get
            {
                return m_Po;
            }
        }
        private ContentControl m_content = null;
        /// <summary>
        /// элемент рисования
        /// </summary>
        private Canvas DrawCanvas = null;

        /// <summary>
        /// область вывода справок
        /// </summary>
        private TextBox m_textMessage = null;
        /// <summary>
        /// форма для отображения детального вида перегона
        /// </summary>
        public static ViewStations WindowStation { get; set; }
        /// <summary>
        /// форма для работы с наборами цветов
        /// </summary>
        public static WindowColor WindowColor { get; set; }
        /// <summary>
        /// элемент вывода справочной информации
        /// </summary>
        CommandButton AreaInfo;
        /// <summary>
        /// логирование
        /// </summary>
        readonly ILog log = LogManager.GetLogger(typeof(SettingsWindow));
        /// <summary>
        /// настройки формы
        /// </summary>
        static Settings _settings = null;
        /// <summary>
        /// настройки формы
        /// </summary>
        public static Settings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
        /// <summary>
        /// нахождение файла справки
        /// </summary>
        public static string HelpFile { get; set; }
        /// <summary>
        /// проигрывать ли звуковые сообщения
        /// </summary>
        public static bool YesPlaySoundMessage { get; set; }

        private DetailViewStation m_detailStation = null;
        /// <summary>
        /// область для детального вида станции
        /// </summary>
        public DetailViewStation DetailViewStation
        {
            get
            {
                return m_detailStation;
            }
        }

        private static DateTime m_lastActive = DateTime.MaxValue;
        /// <summary>
        /// Время последней активности пользователя
        /// </summary>
        public static DateTime LastActive
        {
            get
            {
                return m_lastActive;
            }
            set
            {
                m_lastActive = value;
            }
        }
     
        #endregion

        public SettingsWindow(ContentControl window, Canvas drawcanvas, CommandButton areainfo, TextBox textmessage, DetailViewStation detailStation, ViewWindow viewwindow)
        {
            FirstSettings(window, drawcanvas, areainfo, viewwindow);
            m_textMessage = textmessage;
            m_detailStation = detailStation;
            //настраиваем события
            m_content.MouseWheel += window_MouseWheel;
            m_content.KeyDown += window_KeyDown;
            //если элемент является окном
            if (m_content is Window)
            {
                m_content.Background = m_colorfon;
                (m_content as Window).Activated += window_Activated;
                (m_content as Window).Deactivated += window_Deactivated;
                (m_content as Window).StateChanged += window_StateChanged;
                (m_content as Window).Closing += window_Closing;
                CommandButton.OpenColorDialog += OpenColorDialog;
                if (m_content is MainWindow)
                {
                    StartSettings();
                    SetSettings(drawcanvas);
                    //
                    if (m_textMessage != null)
                    {
                        m_textMessage.Foreground = m_color_text_message;
                        m_textMessage.MouseWheel += textblock_message_MouseWheel;
                    }
                }
                CommandButton.OnOffObject += OnOffEventRun;
            }
            //
            ImpulsesClient.NewData += NewInfomation;
            ImpulsesClient.ConnectDisconnectionServer += ConnectCloseServer;
        }

        public SettingsWindow(ContentControl window, Canvas drawcanvas, ViewWindow viewwindow)
        {
            FirstSettings(window, drawcanvas, null, viewwindow);
            //настраиваем события
            if (window is Window)
            {
                m_content.MouseLeftButtonDown += window_MouseLeftButtonDown;
                m_content.MouseWheel += window_MouseWheel;
            }
            m_content.KeyDown += window_KeyDown;
            m_content.SizeChanged += window_SizeChanged;
            //если элемент является окном
            if (m_content is Window)
                (m_content as Window).Closing += window_Closing;
        }


        private void FirstSettings(ContentControl window, Canvas drawcanvas, CommandButton areainfo, ViewWindow viewwindow)
        {
            _viewwindow = viewwindow;
            m_content = window;
            DrawCanvas = drawcanvas;
            groupTransform.Children.Add(new TranslateTransform());
            DrawCanvas.RenderTransform = groupTransform;
            if (areainfo != null)
                AreaInfo = areainfo;
            //настраиваем основные события
            m_content.MouseMove += window_MouseMove;
            m_content.MouseDown += window_MouseDown;
            m_content.MouseDoubleClick += window_MouseDoubleClick;
            LoadColorControl.NewColor += NewColor;
        }

        ~SettingsWindow()
        {
            LoadColorControl.NewColor -= NewColor;
            CommandButton.OnOffObject -= OnOffEventRun;
            ImpulsesClient.NewData -= NewInfomation;
            ImpulsesClient.ConnectDisconnectionServer -= ConnectCloseServer;
        }

        private void SetSettings(Canvas drawcanvas)
        {
            foreach (UIElement el in drawcanvas.Children)
            {
                CommandButton command = el as CommandButton;
                if (command != null)
                {
                    if (command.ViewCommand == ViewCommand.update_style)
                    {
                        if (_settings != null)
                        {
                            if (_settings.IsUpdateStyle)
                                command.UpdateState();
                        }
                    }
                }
            }
        }


        private void NewInfomation()
        {
            m_content.Dispatcher.Invoke(new Action(() =>
            {
                {
                    var messages = new List<string>();
                    foreach (var el in LoadProject.Indications)
                        messages.AddRange(el.Analis());
                    //
                    if (messages.Count > 0)
                        LoadProject.AddMessages(messages.Distinct().ToList());
                }
            }));
        }

        private void ConnectCloseServer()
        {
            m_content.Dispatcher.Invoke(new Action(() =>
            {
                {
                    foreach (var el in LoadProject.Indications)
                    {
                        el.ServerClose();
                    }
                }
            }));
        }

        public void OnOffEventRun(bool status, ViewCommand view)
        {
            if (m_content is MainWindow)
            {
                Connections connections = (m_content as MainWindow).Connections;
                if (connections != null)
                {
                    switch (view)
                    {
                        case ViewCommand.show_control:
                            {
                                if (status)
                                    connections.StopTs();
                                else connections.StartTs();
                            }
                            break;
                        case ViewCommand.update_style:
                            {
                                if (status)
                                    connections.StartStyle();
                                else connections.StopStyle();
                            }
                            break;
                    }
                }
            }
           
        }

        public void UpdateDann(CommandButton areainfo, TextBox textMessage, DetailViewStation detailStation)
        {
            try
            {
                if (areainfo == null)
                    AreaInfo = MainWindow.AreaInfo;
                else
                    AreaInfo = areainfo;
                m_textMessage = textMessage;
                m_detailStation = detailStation;
            }
            catch { }
        }

        private void ProcessingMessage(string message)
        {
            InfoMessageTrain infotrain = new InfoMessageTrain(MessageUpdate);
            m_content.Dispatcher.Invoke(infotrain, new object[] { message });
        }

        private void MessageUpdate(string textmessage)
        {
            if (!string.IsNullOrEmpty(textmessage))
                m_textMessage.Text = textmessage;
        }

        public void SizeStation()
        {
            SizeScrollStation(_width, _height, true);
        }


        /// <summary>
        /// растягиваем картинку станции по экрану
        /// </summary>
        /// <param name="widthscreen"></param>
        /// <param name="heightscreen"></param>
        private void SizeScrollStation(double widthscreen, double heightscreen, bool scroll)
        {
            double centerscreenX = widthscreen / 2;
            double centerscreenY = heightscreen / 2;
            //
            double minYFigure = double.MaxValue;
            double maxYFigure = double.MinValue;
            double minXFigure = double.MaxValue;
            double maxXFigure = double.MinValue;
            //
            groupTransform.Children.Clear();
            groupTransform.Children.Add(new TranslateTransform());
            foreach (UIElement el in DrawCanvas.Children)
            {
                //графические объекты
                IGraficElement geometry = el as IGraficElement;
                if (geometry != null)
                {
                    SizeMaxMinStation(ref minYFigure, ref maxYFigure, ref minXFigure, ref maxXFigure, geometry.Figure.Figures);
                    continue;
                }
                ////элементы перегоной стрелки
                System.Windows.Shapes.Path figure = el as System.Windows.Shapes.Path;
                if (figure != null)
                {
                    PathGeometry path = (PathGeometry)figure.Data;
                    SizeMaxMinStation(ref minYFigure, ref maxYFigure, ref minXFigure, ref maxXFigure, path.Figures);
                }
            }
            //
            double centerscreenSrationX = (maxXFigure + minXFigure) / 2;
            double centerscreenSrationY = (maxYFigure + minYFigure) / 2;
            //
            NewSize(centerscreenX - centerscreenSrationX, centerscreenY - centerscreenSrationY);
            //
            if (scroll)
            {
                double heightStation = maxYFigure - minYFigure;
                double wightStation = maxXFigure - minXFigure;
                //
                if ((_width / wightStation) > (_height / heightStation))
                    ModelScaleWheel((_height / heightStation) * (_collection_k[_viewwindow]), centerscreenX, centerscreenY);
                else
                    ModelScaleWheel((_width / wightStation) * (_collection_k[_viewwindow]), centerscreenX, centerscreenY);
            }
            m_currentscale = 1;
        }

        private void SizeMaxMinStation(ref double minYFigure, ref double maxYFigure, ref double minXFigure, ref double maxXFigure, PathFigureCollection Figures)
        {
            foreach (PathFigure geo in Figures)
            {
                MinMaxValue(ref  minYFigure, ref  maxYFigure, ref minXFigure, ref  maxXFigure, geo.StartPoint);
                //
                foreach (PathSegment seg in geo.Segments)
                {
                    //сегмент линия
                    LineSegment lin = seg as LineSegment;
                    if (lin != null)
                        MinMaxValue(ref  minYFigure, ref  maxYFigure, ref minXFigure, ref  maxXFigure, lin.Point);
                    //сегмент арка
                    ArcSegment arc = seg as ArcSegment;
                    if (arc != null)
                        MinMaxValue(ref  minYFigure, ref  maxYFigure, ref minXFigure, ref  maxXFigure, arc.Point);
                }
            }
        }

        private void MinMaxValue(ref double minYFigure, ref double maxYFigure, ref double minXFigure, ref double maxXFigure, Point point)
        {
            if (minXFigure > point.X)
                minXFigure = point.X;
            if (minYFigure > point.Y)
                minYFigure = point.Y;
            //
            if (maxXFigure < point.X)
                maxXFigure = point.X;
            if (maxYFigure < point.Y)
                maxYFigure = point.Y;
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _width = e.NewSize.Width;
            _height = e.NewSize.Height;
        }

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (m_content is Window)
            {
                if (Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down)
                {
                    if (m_content.Cursor != Cursors.SizeAll)
                        m_content.Cursor = Cursors.SizeAll;
                    (m_content as Window).DragMove();
                }
            }
        }

        private void textblock_message_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Delta > 0)
                    m_textMessage.FontSize *= 1.1;
                else
                    m_textMessage.FontSize *= 0.9;
            }
        }
        private void NewColor()
        {
            m_content.Background = m_colorfon;
            //обновляем общие цвета для таблиц
            App.Current.Resources["TableGridBrush"] = ColorCommonTable.Grid;
            App.Current.Resources["TableIsSelectFonBrush"] = ColorCommonTable.IsSelectFon;
            App.Current.Resources["TableIsSelectTextBrush"] = ColorCommonTable.IsSelectText;
            //
            if (m_textMessage != null)
                m_textMessage.Foreground = m_color_text_message;
        }


        private void StartSettings()
        {
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["file_settings"]) && new FileInfo(ConfigurationManager.AppSettings["file_settings"]).Exists)
                {
                    var reader = new XmlTextReader(ConfigurationManager.AppSettings["file_settings"]);
                    var deserializer = new XmlSerializer(typeof(Settings));
                    _settings = (Settings)deserializer.Deserialize(reader);
                    reader.Close();
                }
                if (_settings == null)
                    _settings = new Settings();
                //файл справки
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["file_help"]) && (new FileInfo(ConfigurationManager.AppSettings["file_help"]).Exists))
                    HelpFile = ConfigurationManager.AppSettings["file_help"];
                //настраиваем скорость прокрутки
                YesPlaySoundMessage = true;
            }
            catch { }

        }

        private void window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Scroll(e.Delta);
        }

        private void Scroll(int Delta)
        {
            if (Delta > 0)
            {
                if (m_currentscale <= m_scrolmax && Keyboard.IsKeyDown(Key.LeftShift))
                    ModelScaleWheel(ScrollPlusMain, m_cursorX, m_cursorY);
            }
            else
            {
                if (m_currentscale >= m_scrolminimum && Keyboard.IsKeyDown(Key.LeftShift))
                    ModelScaleWheel(ScrollMinusMain, m_cursorX, m_cursorY);
            }
        }

        /// <summary>
        /// масштабируем все объекты
        /// </summary>
        /// <param name="scale_factor"></param>
        private void ModelScaleWheel(double scale_factor, double cursorX, double cursorY)
        {
            if (groupTransform.Children.Count > 0)
            {
                ScaleTransform new_scale_trans = new ScaleTransform(scale_factor, scale_factor, cursorX, cursorY);
                groupTransform.Children.Add(new_scale_trans);
                m_currentscale *= scale_factor;
                //
                ScaleTransform scale = new ScaleTransform(scale_factor, scale_factor, cursorX, cursorY);
                m_Po = scale.Transform(m_Po);
            }
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDownClick(e.GetPosition(DrawCanvas), e.ChangedButton);
        }

        /// <summary>
        /// обрабатываем нажатие мыши
        /// </summary>
        /// <param name="cursor">координаты курсора мыши</param>
        /// <param name="buttonselect">какая клавиша нажата</param>
        private void MouseDownClick(Point cursor, MouseButton buttonselect)
        {
            m_lastActive = DateTime.Now;
            m_cursorX = cursor.X;
            m_cursorY = cursor.Y;
            //
            //если нажата левая клавиша мыши
            if (buttonselect == MouseButton.Left)
            {
                if (LoadProject.ElementsView.ContainsKey(LoadProject.CurrentStation))
                    SelectElements.ClickElement(LoadProject.ElementsView[LoadProject.CurrentStation], cursor, m_content);
            }
        }

        private void OpenColorDialog()
        {
            try
            {
                if (WindowColor != null)
                {
                    SettingsWindow.WindowColor.Show();
                    WindowColor.WindowState = WindowState.Normal;
                    WindowColor.ShowInTaskbar = true;
                }
            }
            catch { }
        }

        private void CloseProgramm()
        {
            if (m_content is Window)
            {
                (m_content as Window).Close();
            }
        }

        private void FullSreenView()
        {
            if (m_content is Window)
            {
                if (m_content is MainWindow)
                {
                    if ((m_content as Window).WindowStyle == System.Windows.WindowStyle.None)
                        (m_content as Window).WindowStyle = System.Windows.WindowStyle.ToolWindow;
                    else
                    {
                        (m_content as Window).WindowStyle = System.Windows.WindowStyle.None;
                        (m_content as Window).WindowState = System.Windows.WindowState.Maximized;
                    }
                }
                //
                if (m_content is ViewStations)
                {
                    if ((m_content as Window).WindowState == System.Windows.WindowState.Normal)
                        (m_content as Window).WindowState = System.Windows.WindowState.Maximized;
                    else (m_content as Window).WindowState = System.Windows.WindowState.Normal;
                    SizeScrollStation(_width, _height, true);
                }
            }
        }

        private void CollapseView()
        {
            if (m_content is Window)
            {
                if (m_content is MainWindow)
                {
                    if (WindowStation != null)
                        WindowStation.Hide();
                    if (WindowColor != null && WindowColor.Visibility == Visibility.Visible)
                        WindowColor.Hide();
                }
                (m_content as Window).WindowState = System.Windows.WindowState.Minimized;
            }
        }

        private void FullSreenStation()
        {
            if(m_content is ViewStations)
                SizeScrollStation(_width, _height, true);
            //вызываем станцию в отдельном окне
            if (m_content is DetailViewStation)
            {
                SCADA.Common.SaveElement.StrageProject GraficProject = LoadProject.GetGrafika((m_content as DetailViewStation).CurrentStation);
                if (GraficProject != null)
                {
                    if (WindowStation == null)
                    {
                        WindowStation = new ViewStations(GraficProject);
                        WindowStation.Show();
                    }
                    else
                    {
                        if (GraficProject.CurrentStation != WindowStation.CurrentStation)
                            WindowStation.UpdateStation(GraficProject);
                    }
                }
            }
        }

        /// <summary>
        /// сохраняем масштаб
        /// </summary>
        /// <param name="Project"></param>
        private void Save()
        {
            try
            {
                SCADA.Common.SaveElement.StrageProject Project = LoadProject.SaveAnalis(m_Po, m_currentscale, 0, 0);
                if (Project != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["grafick_project"] != null && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["grafick_project"]))
                    {
                        using (Stream savestream = new FileStream(System.Configuration.ConfigurationManager.AppSettings["grafick_project"], FileMode.Create))
                        {
                            // Указываем тип того объекта, который сериализуем
                            XmlSerializer xml = new XmlSerializer(typeof(SCADA.Common.SaveElement.StrageProject));
                            // Сериализуем
                            xml.Serialize(savestream, Project);
                            savestream.Close();
                        }
                        //приводим значение к исходному состоянию
                        m_currentscale = 1;
                        m_Po.X = 0; m_Po.Y = 0;
                    }
                }
            }
            catch (Exception error)
            {
                log.Error(error.Message, error);
            }
        }

        /// <summary>
        /// откат к первоначальный координатам
        /// </summary>
        private void StartPosition()
        {
            for (int i = 0; i < groupTransform.Children.Count; i++)
            {
                if (groupTransform.Children[i] is TranslateTransform)
                {
                    (groupTransform.Children[i] as TranslateTransform).X = 0;
                    (groupTransform.Children[i] as TranslateTransform).Y = 0;
                }
                //
                if (groupTransform.Children[i] is ScaleTransform)
                {
                    groupTransform.Children.RemoveAt(i);
                    i--;
                }
            }
            m_currentscale = 1;
            m_Po.X = 0; m_Po.Y = 0;
        }

        private void window_MouseMove(object sender, MouseEventArgs e)
        {
             m_lastActive = DateTime.Now;
            //если класс является главным окном 
            if (m_content is MainWindow)
            {
                if ((m_content as MainWindow).Topmost)
                    (m_content as MainWindow).Topmost = false;
            }
            //
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if(m_content is Window)
                    ModelNewSize(e.GetPosition(DrawCanvas).X - m_cursorX, e.GetPosition(DrawCanvas).Y - m_cursorY);
            }
            else
            {
                if (CheckHitPoint(e.GetPosition(DrawCanvas)) && LoadProject.ElementsView.ContainsKey(LoadProject.CurrentStation) && SelectElements.FindInfoElement(LoadProject.ElementsView[LoadProject.CurrentStation], e.GetPosition(DrawCanvas), AreaInfo))
                {
                    if (m_content.Cursor != Cursors.Hand)
                        m_content.Cursor = Cursors.Hand;
                }
                else
                {
                    if (m_content.Cursor != Cursors.Arrow)
                        m_content.Cursor = Cursors.Arrow;
                }
            }
            //изменяем текущие кординаты
            m_cursorX = e.GetPosition(DrawCanvas).X;
            m_cursorY = e.GetPosition(DrawCanvas).Y;
        }

        private bool CheckHitPoint(Point point)
        {
            if (DetailViewStation != null)
            {
                if (m_content is MainWindow)
                {
                    if (point.X >= DetailViewStation.Margin.Left && point.X <= (DetailViewStation.Margin.Left + DetailViewStation.Width) &&
                        point.Y >= DetailViewStation.Margin.Top && point.Y <= (DetailViewStation.Margin.Top + DetailViewStation.Height))
                    {
                        return false;
                    }
                }
            }
            //
            return true;
        }

        /// <summary>
        /// переносим  все объекты
        /// </summary>
        /// <param name="deltaX">смещение по оси Х</param>
        /// <param name="deltaY">мещение по оси У</param>
        private void ModelNewSize(double deltaX, double deltaY)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (m_content.Cursor != Cursors.SizeAll)
                    m_content.Cursor = Cursors.SizeAll;
                //
                m_Po.X += deltaX;
                m_Po.Y += deltaY;
                NewSize(deltaX, deltaY);
            }
        }

        private void NewSize(double deltaX, double deltaY)
        {
            if (groupTransform.Children.Count > 0 && groupTransform.Children[0] is TranslateTransform)
            {
                (groupTransform.Children[0] as TranslateTransform).X += deltaX;
                (groupTransform.Children[0] as TranslateTransform).Y += deltaY;
            }
        }

        private void window_Activated(object sender, EventArgs e)
        {
            try
            {
                if (WindowStation != null)
                    WindowStation.Topmost = true;
                if (WindowColor != null && WindowColor.Visibility == Visibility.Visible)
                    WindowColor.Topmost = true;
              
            }
            catch (Exception error) { log.Error(error.Message, error); }
        }

        private void window_Deactivated(object sender, EventArgs e)
        {
            try
            {
                if (WindowStation != null)
                    WindowStation.Topmost = false;
                if (WindowColor != null && WindowColor.Visibility == Visibility.Visible)
                    WindowColor.Topmost = false;
            }
            catch (Exception error) { log.Error(error.Message, error);  }
        }

        private void window_StateChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_content != null && (m_content is Window))
                {
                    switch ((m_content as Window).WindowState)
                    {
                        case System.Windows.WindowState.Maximized:
                            {
                                if (WindowStation != null)
                                    WindowStation.Show();
                            }
                            break;
                        case System.Windows.WindowState.Minimized:
                            {
                                if (WindowStation != null)
                                    WindowStation.Hide();
                            }
                            break;
                        case System.Windows.WindowState.Normal:
                            {
                                if (WindowStation != null)
                                    WindowStation.Show();
                            }
                            break;
                    }
                }
            }
            catch (Exception error) { log.Error(error.Message, error); }
        }

        private void window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            m_lastActive = DateTime.Now;
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    {
                        if (m_content is Window)
                        {
                            (m_content as Window).Close();
                        }
                    }
                    break;
            }
        }

        private void SetNotSelectTable(DataGrid table)
        {
            if (table != null  && table.SelectedIndex != -1)
                table.SelectedIndex = -1;
        }
      
        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadColorControl.NewColor -= NewColor;
            //
            if (m_content != null)
            {
                //закрывем окно детального вида
                if (m_content is ViewStations)
                {
                    Dispose();
                    SettingsWindow.WindowStation = null;
                    return;
                }
                //закрываем основное окно
                if (m_content is MainWindow)
                {
                    if (_settings != null)
                        _settings.IsUpdateStyle = (m_content as MainWindow).Connections.IsStartUpdateStyle;
                    //
                    if (App.Close)
                    {
                        TrafficTrain.WorkForm.CloseProgramm closewindow = new WorkForm.CloseProgramm();
                        if (!(bool)closewindow.ShowDialog())
                            e.Cancel = true;
                        else
                        {
                            System.Threading.Thread potokclose = new System.Threading.Thread(CloseForm);
                            potokclose.SetApartmentState(System.Threading.ApartmentState.STA);
                            potokclose.Start();
                            (m_content as MainWindow).CloseAll();
                            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["file_settings"]))
                            {
                                using (Stream savestream = new FileStream(System.Configuration.ConfigurationManager.AppSettings["file_settings"], FileMode.Create))
                                {
                                    XmlSerializer xml = new XmlSerializer(typeof(Settings));
                                    xml.Serialize(savestream, _settings);
                                    savestream.Close();
                                }
                            }
                            Dispose();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (UIElement el in DrawCanvas.Children)
            {
                if (el is IDisposable)
                    (el as IDisposable).Dispose();
            }
            //
            DrawCanvas.Children.Clear();
        }

        private void CloseForm()
        {
            CloseWindowWpf form = new CloseWindowWpf();
            form.ShowDialog();
        }
    }
}
