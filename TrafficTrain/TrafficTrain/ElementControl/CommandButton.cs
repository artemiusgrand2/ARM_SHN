using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARM_SHN.Interface;
using ARM_SHN.Enums;
using ARM_SHN.Delegate;
using ARM_SHN.WorkWindow;
using ARM_SHN.EditText;
using SCADA.Common.Enums;

namespace ARM_SHN.ElementControl
{

    public class CommandButton : BaseTextGraficElement, ISelectElement,  IInfoElement, IText
    {
        #region Переменные и свойства

        //////цветовая палитра
        /// <summary>
        /// цвет нормальной работы при диагностики
        /// </summary>
        public static Brush _color_no_message = new LinearGradientBrush(Color.FromArgb(255,144, 180, 144), Color.FromArgb(127,144, 255, 144), 45);
        /// <summary>
        /// цвет не нормальной работы при диагностики
        /// </summary>
        public static Brush _color_yes_message = new LinearGradientBrush(Color.FromArgb(255, 255, 0, 0), Color.FromArgb(127, 160, 0, 0), 45);
        /// <summary>
        /// цвет текста справочного элемента
        /// </summary>
        public static Brush _color_text_help = Brushes.Black;
        /// <summary>
        /// цвет текста кнопок переключения
        /// </summary>
        public static Brush _color_text_switch_button = Brushes.Black;
        /// <summary>
        /// цвет текста журналов
        /// </summary>
        public static Brush _color_text_journal = Brushes.Black;
        /// <summary>
        /// цвет  рамки
        /// </summary>
        public static Brush _color_stroke = Brushes.Black;
        /// <summary>
        /// цвет фона если кнопка не нажата
        /// </summary>
        public static Brush _colorpasiv = new LinearGradientBrush(Color.FromRgb(170, 170, 170), Color.FromRgb(225, 225, 225), 45);
        /// <summary>
        /// цвет фона если кнопка нажата
        /// </summary>
        public static Brush _coloractiv = new LinearGradientBrush(Color.FromRgb(128, 0, 128), Color.FromRgb(128, 0, 200), 45);
        /// <summary>
        /// цвет фона справочной строки
        /// </summary>
        public static Brush _color_helpstring_fon = new LinearGradientBrush(Color.FromRgb(170, 170, 170), Color.FromRgb(225, 225, 225), 45);
        /// <summary>
        /// цвет границы справочной строки
        /// </summary>
        public static Brush _color_helpstring_stroke = new LinearGradientBrush(Color.FromRgb(128, 0, 128), Color.FromRgb(128, 0, 200), 45);
     
        /// <summary>
        /// шестизначный номер станции перехода
        /// </summary>
        public int StationTransition { get; set; }
         /// <summary>
        /// вид управляющей команды
        /// </summary>
        private ViewCommand viewcommand = ViewCommand.none;
        public ViewCommand ViewCommand
        {
            get
            {
                return viewcommand;
            }
            set
            {
                viewcommand = value;
            }
        }
        /// <summary>
        /// вид управляющей панели
        /// </summary>
        private ViewPanel viewpanel = ViewPanel.none;
        public ViewPanel ViewPanel
        {
            get
            {
                return viewpanel;
            }
            set
            {
                viewpanel = value;
            }
        }

        bool downclick = false;
        /// <summary>
        /// нажата ли клавиша
        /// </summary>
        public bool DownClik
        {
            get
            {
                return downclick;
            }
        }
        /// <summary>
        /// показать диалоговое окно цвета
        /// </summary>
        public static event NewColor OpenColorDialog;
        /// <summary>
        /// Заблакированна ли клавиша
        /// </summary>
        private bool m_block = false;

        public string NameUl
        {
            get
            {
                return NameObject;
            }
        }

        public string FileClick { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="geometry">геометрия объекта</param>
        public CommandButton(PathGeometry geometry, string name, string help, double marginX, double marginY, double fontsize, double rotate, ViewCommand viewcommand, ViewPanel viewpanel):
             base(name, ViewElement.buttoncommand, geometry, rotate)
        {
            this.viewcommand = viewcommand;
            this.viewpanel = viewpanel;
            SetText(fontsize, rotate, marginX, marginY);
            //первоначальные координаты
            ViewModel.StrokeThickness = (SystemParameters.CaretWidth * LoadProject.ProejctGrafic.Scroll);
            //
            if (viewcommand == ViewCommand.style && !MainWindow.Admin)
            {
                ViewModel.Visibility = System.Windows.Visibility.Collapsed;
                ViewModel.TextVisibility = System.Windows.Visibility.Collapsed;
            }
            //устанавливаем цвета
            NewColor();
            LoadColorControl.NewColor += NewColor;
        }

        public string InfoElement()
        {
            return Notes;
        }

        public override void Dispose()
        {
            LoadColorControl.NewColor -= NewColor;
        }

        private void SetText(double fontsize, double rotate, double marginX, double marginY)
        {
            ViewModel.Text = string.Empty;
            ViewModel.Foreground = _color_text_help;
            ViewModel.FontSize = fontsize;
            ViewModel.RenderTransform = new RotateTransform(RotateText);
            ViewModel.Margin = new Thickness(marginX, marginY, 0, 0);
        }

        public void UpdateState()
        {
            downclick = !downclick;
            if (downclick)
                ViewModel.Fill = _coloractiv;
            else
                ViewModel.Fill = _colorpasiv;
        }

        public void Click(ContentControl content)
        {
            if (!m_block)
            {
                UpdateState();
                //
                switch (viewcommand)
                {
                    case ViewCommand.exit:
                        {
                            if (content != null)
                            {
                                if (content is Window)
                                    (content as Window).Close();
                                Click(null);
                            }
                        }
                        break;
                    case ViewCommand.diagnostics:
                        {
                            if (downclick)
                            {
                                try
                                {
                                    if (this is ISelectElement)
                                    {
                                        LoadProject.UpdateStation((this as ISelectElement).StationControl, NameObject);
                                        Click(null);
                                    }
                                }
                                catch { }
                            }
                        }
                        break;
                    case ViewCommand.style:
                        {
                            if (downclick)
                            {
                                if (OpenColorDialog != null)
                                    OpenColorDialog();
                                Click(null);
                            }
                            break;
                        }
                }
            }
        }

        private void NewColor()
        {
            ViewModel.Stroke = _color_stroke;
            //
            switch (viewcommand)
            {
                case ViewCommand.content_exchange:
                    ViewModel.Fill = _color_helpstring_fon;
                    ViewModel.Stroke = _color_helpstring_stroke;
                    ViewModel.Foreground = _color_text_help;
                    break;
                case ViewCommand.content_help:
                    ViewModel.Fill = _color_helpstring_fon;
                    Stroke = _color_helpstring_stroke;
                    ViewModel.Foreground = _color_text_help;
                    break;
                default:
                    if (downclick)
                        ViewModel.Fill = _coloractiv;
                    else ViewModel.Fill = _colorpasiv;
                    ViewModel.Foreground = _color_text_switch_button;
                    break;
            }     
        }

    }
}
