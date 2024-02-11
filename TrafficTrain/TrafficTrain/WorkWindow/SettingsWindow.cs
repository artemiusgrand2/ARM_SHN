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
using ARM_SHN.Interface;
using ARM_SHN.Enums;
using ARM_SHN.Delegate;
using ARM_SHN.ElementControl;
using ARM_SHN.CommandsElement;

using SCADA.Common.Enums;
using SCADA.Common.Strage.SaveElement;
using SCADA.Common.ImpulsClient;


namespace ARM_SHN.WorkWindow
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

        public SettingsWindow(ContentControl window, Canvas drawcanvas, CommandButton areainfo, TextBox textmessage, ViewWindow viewwindow)
        {
            FirstSettings(window, drawcanvas, areainfo, viewwindow);
            m_textMessage = textmessage;
            //настраиваем события
            m_content.MouseWheel += window_MouseWheel;
            m_content.KeyDown += window_KeyDown;
            //если элемент является окном
            if (m_content is Window)
            {
                m_content.Background = m_colorfon;
                (m_content as Window).Activated += window_Activated;
                (m_content as Window).Deactivated += window_Deactivated;
                (m_content as Window).Closing += window_Closing;
                CommandButton.OpenColorDialog += OpenColorDialog;
                if (m_content is MainWindow)
                {
                    if (m_textMessage != null)
                    {
                        m_textMessage.Foreground = m_color_text_message;
                        m_textMessage.MouseWheel += textblock_message_MouseWheel;
                    }
                }
            }
            //
            ImpulsesClientTCP.NewData += NewInfomation;
            ImpulsesClientTCP.ConnectDisconnectionServer += ConnectCloseServer;
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
            ImpulsesClientTCP.NewData -= NewInfomation;
            ImpulsesClientTCP.ConnectDisconnectionServer -= ConnectCloseServer;
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
                if (LoadProject.ElementsView.TryGetValue(LoadProject.CurrentStation, out var selectStation))
                    SelectElements.ClickElement(selectStation, cursor, m_content);
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
                if (LoadProject.ElementsView.ContainsKey(LoadProject.CurrentStation) && SelectElements.FindInfoElement(LoadProject.ElementsView[LoadProject.CurrentStation], e.GetPosition(DrawCanvas), AreaInfo))
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
                if (WindowColor != null && WindowColor.Visibility == Visibility.Visible)
                    WindowColor.Topmost = true;
              
            }
            catch (Exception error) { log.Error(error.Message, error); }
        }

        private void window_Deactivated(object sender, EventArgs e)
        {
            try
            {;
                if (WindowColor != null && WindowColor.Visibility == Visibility.Visible)
                    WindowColor.Topmost = false;
            }
            catch (Exception error) { log.Error(error.Message, error);  }
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
      
        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadColorControl.NewColor -= NewColor;
            //
            if (m_content != null)
            {
                //закрываем основное окно
                if (m_content is MainWindow)
                {
                    if (App.Close)
                    {
                        ARM_SHN.WorkForm.CloseProgramm closewindow = new WorkForm.CloseProgramm();
                        if (!(bool)closewindow.ShowDialog())
                            e.Cancel = true;
                        else
                        {
                            (m_content as MainWindow).CloseAll();
                            Dispose();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var el in DrawCanvas.Children)
            {
                if (el is IDisposable)
                    (el as IDisposable).Dispose();
            }
            //
            DrawCanvas.Children.Clear();
        }

    }
}
