using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace TrafficTrain
{
    public partial class UpdateColorElements : Form
    {
        #region Переменные и свойства

        #endregion

        public UpdateColorElements()
        {
            InitializeComponent();
            BackColor = Color.FromArgb(((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.R, ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.G,
                ((System.Windows.Media.SolidColorBrush)MainWindow._colorfon).Color.B);
            comboBox_namecolor.DataSource = LoadProject.NameColorsLineHelp;
        }

        private void UpdateColorElements_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawRectangle(new Pen(Color.Brown, 4), new Rectangle(0, 0, Width/2, Height));
        }


        private void UpdateColorElements_FormClosing(object sender, FormClosingEventArgs e)
        {
            Opacity = 0;
            ShowInTaskbar = false;
            e.Cancel = true;
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel)
            {
                Panel el = sender as Panel;
                ColorDialog color = new ColorDialog(); 
                //
                //если выбран цвет фона
                if (el.Name == "panel_colorfon")
                    color.Color = Color.FromArgb(LoadProject.ColorProject.ColorFon.R, LoadProject.ColorProject.ColorFon.G, LoadProject.ColorProject.ColorFon.B);
                //если выбран цвет управляющей рамки
                if (el.Name == "panel_arrow_command")
                    color.Color = Color.FromArgb(LoadProject.ColorProject.ColorArrowCommand.R, LoadProject.ColorProject.ColorArrowCommand.G, LoadProject.ColorProject.ColorArrowCommand.B);
                ////    //------------------------------------------------------
                ////    ////Цветовая расскраска пути
                ////Цвет занятого пути
                //if (el.Name == "panel_coloractivpath")
                //    color.Color = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorActiv.R, LoadProject.ColorProject.ColorArrowCommand.G, LoadProject.ColorProject.ColorArrowCommand.B);
                ////Цвет не занятого пути
                //if (el.Name == "panel_colorpasivpath")
                //{
                //    el.BackColor = color.Color;
                //    SetColor();
                //    return;
                //}
                ////Цвет пути в автодействии
                //if (el.Name == "panel_colorautopath")
                //{
                //    el.BackColor = color.Color;
                //    SetColor();
                //    return;
                //}
                ////Цвет пути в ограждении
                //if (el.Name == "panel_colorfensingpath")
                //{
                //    el.BackColor = color.Color;
                //    SetColor();
                //    return;
                //}
                ////Цвет пути не контролируемого
                //if (el.Name == "panel_colornotcontrolpath")
                //{
                //    el.BackColor = color.Color;
                //    SetColor();
                //    return;
                //}
                ////Цвет границы пути не контролируемого
                //if (el.Name == "panel_colornotcontrolstrokepath")
                //{
                //    el.BackColor = color.Color;
                //    SetColor();
                //    return;
                //}
                ////Цвет границы пути при автономноя тяге
                //if (el.Name == "panel_colordisel")
                //{
                //    el.BackColor = color.Color;
                //    SetColor();
                //    return;
                //}
                //    //Цвет границы пути при электрической тяге
                //    if (el.Name == "panel_colorelectro")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет текста названия поезда
                //    if (el.Name == "panel_colornametrain")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет текста названия пути
                //    if (el.Name == "panel_colornamepath")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет текста названия пути
                //    if (el.Name == "panel_colornametrainvertor")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }


                //    //------------------------------------------------------
                //    ////Цветовая расскраска сигнала
                //    //Цвет занятого сигнала
                //    if (el.Name == "panel_colorsignalbusy")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет замкнутого маршрута
                //    if (el.Name == "panel_colorsignalclosed")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки сигнала при перегорании лампы
                //    if (el.Name == "panel_colorsignalfault")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона если неконтролируется стгнал
                //    if (el.Name == "panel_colornotcontrolsignal")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет псвободного сигнала
                //    if (el.Name == "panel_colorsignalfree")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет проезда
                //    if (el.Name == "panel_colorsignalinstall")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет мигания при установке маршрута
                //    if (el.Name == "panel_colorsignalinstallroute")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет первый мигания пригласительного
                //    if (el.Name == "panel_signalinvitationaleone")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет второй мигания пригласительного
                //    if (el.Name == "panel_signalinvitationalety")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет границы если некотролируется 
                //    if (el.Name == "panel_colornotcontrolstrokesignal")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет поездного сигнала
                //    if (el.Name == "panel_colorsignalopen")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки по умолчанию
                //    if (el.Name == "panel_colorsignalramkadefult")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет маневрового
                //    if (el.Name == "panel_colorsignalshunting")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }

                //    //------------------------------------------------------
                //    ////Цветовая расскраска кнопки станции
                //    //Цвет диспетчерского контроля
                //    if (el.Name == "panel_buttonstation_dispatcher")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет сезонного контроля
                //    if (el.Name == "panel_buttonstation_sesoncontrol")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет автономного управления
                //    if (el.Name == "panel_buttonstation_automon")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет резервного контроля
                //    if (el.Name == "panel_buttonstation_reserve")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет пожара
                //    if (el.Name == "panel_buttonstation_fire")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет аварии
                //    if (el.Name == "panel_buttonstation_accident")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет неисправности
                //    if (el.Name == "panel_buttonstation_fault")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет потери связи
                //    if (el.Name == "panel_buttonstation_notlink")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки по умолчанию
                //    if (el.Name == "panel_buttonstation_ramkadefult")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона неконтролируемого состояния
                //    if (el.Name == "panel_buttonstation_notcontrol")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет границы неконтролируемого состояния
                //    if (el.Name == "panel_buttonstation_notcontrolstroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона
                //    if (el.Name == "panel_buttonstation_fon")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }

                //    //------------------------------------------------------
                //    ////Цветовая расскраска переезда
                //    //Цвет рамки при нормальной работе
                //    if (el.Name == "panel_movenormalramka")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки при аварии
                //    if (el.Name == "panel_moveaccident")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки при неисправности
                //    if (el.Name == "panel_movefault")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет закрытого переезда
                //    if (el.Name == "panel_moveclose")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона
                //    if (el.Name == "panel_movefon")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона если нет контроля
                //    if (el.Name == "panel_movenotcontrol")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки если нет контроля
                //    if (el.Name == "panel_movenotcontrolstroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //------------------------------------------------------
                //    ////Цветовая расскраска КТСМ
                //    //Цвет рамки при нормальной работе
                //    if (el.Name == "panel_KTCMnormalramka")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки при аварии
                //    if (el.Name == "panel_KTCMrun")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки при неисправности
                //    if (el.Name == "panel_KTCMfault")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона если нет контроля
                //    if (el.Name == "panel_KTCMnotcontrol")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки если нет контроля
                //    if (el.Name == "panel_KTCMnotcontrolstroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона 
                //    if (el.Name == "panel_KTCMfon")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //------------------------------------------------------
                //    ////Цветовая расскраска КГУ
                //    //Цвет рамки при нормальной работе
                //    if (el.Name == "panel_KGUnormalramka")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки при аварии
                //    if (el.Name == "panel_KGUrun")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки при неисправности
                //    if (el.Name == "panel_KGUfault")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона если нет контроля
                //    if (el.Name == "panel_KGUnotcontrol")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки если нет контроля
                //    if (el.Name == "panel_KGUnotcontrolstroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона 
                //    if (el.Name == "panel_KGUfon")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }


                //    //------------------------------------------------------
                //    ////Цветовая расскраска Номера поездов
                //    //Цвет рамки при нормальной работе
                //    if (el.Name == "panel_number_train_ramkadefult")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет занятого перегона
                //    if (el.Name == "panel_number_train_activ")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет свободного перегона перегона
                //    if (el.Name == "panel_number_train_pasiv")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона если нет контроля
                //    if (el.Name == "panel_number_train_notcontrol")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки если нет контроля
                //    if (el.Name == "panel_number_train_notcontrolstroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет текста номера поезда
                //    if (el.Name == "panel_number_train_colortrain")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }

                //    //------------------------------------------------------
                //    ////Цветовая расскраска Стрелка поворотов
                //    //Цвет рамки при нормальной работе
                //    if (el.Name == "panel_arrow_ramkadefult")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет отправления
                //    if (el.Name == "panel_arrow_departure")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет занятого перегона перегона
                //    if (el.Name == "panel_arrow_occupation")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона  при разрешении отправления
                //    if (el.Name == "panel_arrow_okdeparture")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет мигания при ожиданиии отправления
                //    if (el.Name == "panel_arrow_waitdeparture")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона при нормальной работе
                //    if (el.Name == "panel_arrow_normal")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет фона если нет контроля
                //    if (el.Name == "panel_arrow_notcontrol")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет рамки если нет контроля
                //    if (el.Name == "panel_arrow_notcontrolstroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }

                //    //------------------------------------------------------
                //    ////Цветовая расскраска блок участков
                //    //Цвет автивного блок участка
                //    if (el.Name == "panel_block_activ")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет пассивного блок участка
                //    if (el.Name == "panel_block_pasiv")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет неконтролируемого блок участка
                //    if (el.Name == "panel_block_notcontrolstroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }

                //    //------------------------------------------------------
                //    ////Цветовая расскраска рамки станции
                //    //Цвет фона
                //    if (el.Name == "panel_ramkastation_fill")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //    //Цвет границы
                //    if (el.Name == "panel_ramkastation_stroke")
                //    {
                //        el.BackColor = color.Color;
                //        SetColor();
                //        return;
                //    }
                //------------------------------------------------------
                //
                if (color.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    ////Цветовая расскраска вспомагательной линии
                    //Цвет автивного блок участка
                    if (el.Name == "panel_help_line")
                    {
                        el.BackColor = color.Color;
                        //
                        if (comboBox_namecolor.SelectedIndex != -1)
                        {
                            int index = LoadProject.FindElement(LoadProject.ColorProject.ColorHelpLines, comboBox_namecolor.SelectedItem.ToString(), false);
                            if (index != -1)
                            {
                                LoadProject.ColorProject.ColorHelpLines[index].ColorHelpLine = new DataGrafik.ColorN()
                                {
                                    R = el.BackColor.R,
                                    G = el.BackColor.G,
                                    B = el.BackColor.B
                                };
                            }
                            else
                            {
                                LoadProject.ColorProject.ColorHelpLines.Add(new DataGrafik.NameColorHelp()
                                {
                                    NameColorNormal = comboBox_namecolor.SelectedItem.ToString(),
                                    ColorHelpLine = new DataGrafik.ColorN()
                                    {
                                        R = el.BackColor.R,
                                        G = el.BackColor.G,
                                        B = el.BackColor.B
                                    }
                                });
                            }
                        }
                        SetColor();
                        return;
                    }
                    //
                    el.BackColor = color.Color;
                    SetColor();
                }
            }
        }


        private void button_ok_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateColorElements_Load(object sender, EventArgs e)
        {
            LoadProejct();
        }

        public void LoadProejct()
        {
            if (LoadProject.ColorProject != null)
            {
                //основные цвета
                panel_colorfon.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorFon.R, LoadProject.ColorProject.ColorFon.G, LoadProject.ColorProject.ColorFon.B);
                panel_arrow_command.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrowCommand.R, LoadProject.ColorProject.ColorArrowCommand.G, LoadProject.ColorProject.ColorArrowCommand.B);
                //цвета главного пути
                panel_coloractivpath.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorActiv.R, LoadProject.ColorProject.ColorPath.ColorActiv.G, LoadProject.ColorProject.ColorPath.ColorActiv.B);
                panel_colorpasivpath.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorPasiv.R, LoadProject.ColorProject.ColorPath.ColorPasiv.G, LoadProject.ColorProject.ColorPath.ColorPasiv.B);
                panel_colorautopath.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorAuto.R, LoadProject.ColorProject.ColorPath.ColorAuto.G, LoadProject.ColorProject.ColorPath.ColorAuto.B);
                panel_colorfensingpath.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorFencing.R, LoadProject.ColorProject.ColorPath.ColorFencing.G, LoadProject.ColorProject.ColorPath.ColorFencing.B);
                panel_colornotcontrolpath.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorFillNotControl.R, LoadProject.ColorProject.ColorPath.ColorFillNotControl.G, LoadProject.ColorProject.ColorPath.ColorFillNotControl.B);
                panel_colornotcontrolstrokepath.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorNotControlStroke.R, LoadProject.ColorProject.ColorPath.ColorNotControlStroke.G, LoadProject.ColorProject.ColorPath.ColorNotControlStroke.B);
                panel_colordisel.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorDieselTraction.R, LoadProject.ColorProject.ColorPath.ColorDieselTraction.G, LoadProject.ColorProject.ColorPath.ColorDieselTraction.B);
                panel_colorelectro.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorElectricTraction.R, LoadProject.ColorProject.ColorPath.ColorElectricTraction.G, LoadProject.ColorProject.ColorPath.ColorElectricTraction.B);
                panel_colornamepath.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorPathName.R, LoadProject.ColorProject.ColorPath.ColorPathName.G, LoadProject.ColorProject.ColorPath.ColorPathName.B);
                panel_colornametrain.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorTrain.R, LoadProject.ColorProject.ColorPath.ColorTrain.G, LoadProject.ColorProject.ColorPath.ColorTrain.B);
                panel_colornametrainvertor.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorPath.ColorTrainVertor.R, LoadProject.ColorProject.ColorPath.ColorTrainVertor.G, LoadProject.ColorProject.ColorPath.ColorTrainVertor.B);
                //цвета сигнала
                panel_colorsignalbusy.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorBusy.R, LoadProject.ColorProject.ColorSignal.ColorBusy.G, LoadProject.ColorProject.ColorSignal.ColorBusy.B);
                panel_colorsignalclosed.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorClosed.R, LoadProject.ColorProject.ColorSignal.ColorClosed.G, LoadProject.ColorProject.ColorSignal.ColorClosed.B);
                panel_colorsignalfault.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorFault.R, LoadProject.ColorProject.ColorSignal.ColorFault.G, LoadProject.ColorProject.ColorSignal.ColorFault.B);
                panel_colornotcontrolsignal.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorFillNotControl.R, LoadProject.ColorProject.ColorSignal.ColorFillNotControl.G, LoadProject.ColorProject.ColorSignal.ColorFillNotControl.B);
                panel_colorsignalfree.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorFree.R, LoadProject.ColorProject.ColorSignal.ColorFree.G, LoadProject.ColorProject.ColorSignal.ColorFree.B);
                panel_colorsignalinstall.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorInstall.R, LoadProject.ColorProject.ColorSignal.ColorInstall.G, LoadProject.ColorProject.ColorSignal.ColorInstall.B);
                panel_colorsignalinstallroute.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorInstallRoute.R, LoadProject.ColorProject.ColorSignal.ColorInstallRoute.G, LoadProject.ColorProject.ColorSignal.ColorInstallRoute.B);
                panel_signalinvitationaleone.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorInvitationalOne.R, LoadProject.ColorProject.ColorSignal.ColorInvitationalOne.G, LoadProject.ColorProject.ColorSignal.ColorInvitationalOne.B);
                panel_signalinvitationalety.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorInvitationalTy.R, LoadProject.ColorProject.ColorSignal.ColorInvitationalTy.G, LoadProject.ColorProject.ColorSignal.ColorInvitationalTy.B);
                panel_colornotcontrolstrokesignal.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorNotControlStroke.R, LoadProject.ColorProject.ColorSignal.ColorNotControlStroke.G, LoadProject.ColorProject.ColorSignal.ColorNotControlStroke.B);
                panel_colorsignalopen.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorOpen.R, LoadProject.ColorProject.ColorSignal.ColorOpen.G, LoadProject.ColorProject.ColorSignal.ColorOpen.B);
                panel_colorsignalramkadefult.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorRamkaDefult.R, LoadProject.ColorProject.ColorSignal.ColorRamkaDefult.G, LoadProject.ColorProject.ColorSignal.ColorRamkaDefult.B);
                panel_colorsignalshunting.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorSignal.ColorShunting.R, LoadProject.ColorProject.ColorSignal.ColorShunting.G, LoadProject.ColorProject.ColorSignal.ColorShunting.B);
                //цвет кнопки станции
                panel_buttonstation_dispatcher.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorDispatcher.R, LoadProject.ColorProject.ColorButtonStation.ColorDispatcher.G, LoadProject.ColorProject.ColorButtonStation.ColorDispatcher.B);
                panel_buttonstation_accident.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorAccident.R, LoadProject.ColorProject.ColorButtonStation.ColorAccident.G, LoadProject.ColorProject.ColorButtonStation.ColorAccident.B);
                panel_buttonstation_automon.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorAutonomousControl.R, LoadProject.ColorProject.ColorButtonStation.ColorAutonomousControl.G, LoadProject.ColorProject.ColorButtonStation.ColorAutonomousControl.B);
                panel_buttonstation_fault.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorFault.R, LoadProject.ColorProject.ColorButtonStation.ColorFault.G, LoadProject.ColorProject.ColorButtonStation.ColorFault.B);
                panel_buttonstation_notcontrol.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorFillNotControl.R, LoadProject.ColorProject.ColorButtonStation.ColorFillNotControl.G, LoadProject.ColorProject.ColorButtonStation.ColorFillNotControl.B);
                panel_buttonstation_fire.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorFire.R, LoadProject.ColorProject.ColorButtonStation.ColorFire.G, LoadProject.ColorProject.ColorButtonStation.ColorFire.B);
                panel_buttonstation_fon.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorFon.R, LoadProject.ColorProject.ColorButtonStation.ColorFon.G, LoadProject.ColorProject.ColorButtonStation.ColorFon.B);
                panel_buttonstation_notcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorNotControlStroke.R, LoadProject.ColorProject.ColorButtonStation.ColorNotControlStroke.G, LoadProject.ColorProject.ColorButtonStation.ColorNotControlStroke.B);
                panel_buttonstation_notlink.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorNotLink.R, LoadProject.ColorProject.ColorButtonStation.ColorNotLink.G, LoadProject.ColorProject.ColorButtonStation.ColorNotLink.B);
                panel_buttonstation_ramkadefult.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorRamkaDefult.R, LoadProject.ColorProject.ColorButtonStation.ColorRamkaDefult.G, LoadProject.ColorProject.ColorButtonStation.ColorRamkaDefult.B);
                panel_buttonstation_reserve.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorReserveControl.R, LoadProject.ColorProject.ColorButtonStation.ColorReserveControl.G, LoadProject.ColorProject.ColorButtonStation.ColorReserveControl.B);
                panel_buttonstation_sesoncontrol.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorButtonStation.ColorSesonContol.R, LoadProject.ColorProject.ColorButtonStation.ColorSesonContol.G, LoadProject.ColorProject.ColorButtonStation.ColorSesonContol.B);
                //цвет переезда
                panel_movenormalramka.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorMoveOpen.R, LoadProject.ColorProject.ColorMove.ColorMoveOpen.G, LoadProject.ColorProject.ColorMove.ColorMoveOpen.B);
                panel_moveaccident.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorAccident.R, LoadProject.ColorProject.ColorMove.ColorAccident.G, LoadProject.ColorProject.ColorMove.ColorAccident.B);
                panel_movefault.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorFault.R, LoadProject.ColorProject.ColorMove.ColorFault.G, LoadProject.ColorProject.ColorMove.ColorFault.B);
                panel_movecloseauto.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorCloseAuto.R, LoadProject.ColorProject.ColorMove.ColorCloseAuto.G, LoadProject.ColorProject.ColorMove.ColorCloseAuto.B);
                panel_moveclosbutton.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorCloseButton.R, LoadProject.ColorProject.ColorMove.ColorCloseButton.G, LoadProject.ColorProject.ColorMove.ColorCloseButton.B);
                panel_movefon.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorFon.R, LoadProject.ColorProject.ColorMove.ColorFon.G, LoadProject.ColorProject.ColorMove.ColorFon.B);
                panel_movenotcontrol.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorFillNotControl.R, LoadProject.ColorProject.ColorMove.ColorFillNotControl.G, LoadProject.ColorProject.ColorMove.ColorFillNotControl.B);
                panel_movenotcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorMove.ColorNotControlStroke.R, LoadProject.ColorProject.ColorMove.ColorNotControlStroke.G, LoadProject.ColorProject.ColorMove.ColorNotControlStroke.B);
                //цвет КТСМ
                panel_KTCMnormalramka.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKTCM.ColorRamkaDefult.R, LoadProject.ColorProject.ColorKTCM.ColorRamkaDefult.G, LoadProject.ColorProject.ColorKTCM.ColorRamkaDefult.B);
                panel_KTCMrun.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKTCM.ColorAccident.R, LoadProject.ColorProject.ColorKTCM.ColorAccident.G, LoadProject.ColorProject.ColorKTCM.ColorAccident.B);
                panel_KTCMfault.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKTCM.ColorFault.R, LoadProject.ColorProject.ColorKTCM.ColorFault.G, LoadProject.ColorProject.ColorKTCM.ColorFault.B);
                panel_KTCMnotcontrol.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKTCM.ColorFillNotControl.R, LoadProject.ColorProject.ColorKTCM.ColorFillNotControl.G, LoadProject.ColorProject.ColorKTCM.ColorFillNotControl.B);
                panel_KTCMfon.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKTCM.ColorFon.R, LoadProject.ColorProject.ColorKTCM.ColorFon.G, LoadProject.ColorProject.ColorKTCM.ColorFon.B);
                panel_KTCMnotcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKTCM.ColorNotControlStroke.R, LoadProject.ColorProject.ColorKTCM.ColorNotControlStroke.G, LoadProject.ColorProject.ColorKTCM.ColorNotControlStroke.B);
                //цвет КГУ
                panel_KGUnormalramka.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKGU.ColorRamkaDefult.R, LoadProject.ColorProject.ColorKGU.ColorRamkaDefult.G, LoadProject.ColorProject.ColorKGU.ColorRamkaDefult.B);
                panel_KGUrun.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKTCM.ColorAccident.R, LoadProject.ColorProject.ColorKGU.ColorAccident.G, LoadProject.ColorProject.ColorKGU.ColorAccident.B);
                panel_KGUfault.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKGU.ColorFault.R, LoadProject.ColorProject.ColorKGU.ColorFault.G, LoadProject.ColorProject.ColorKGU.ColorFault.B);
                panel_KGUnotcontrol.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKGU.ColorFillNotControl.R, LoadProject.ColorProject.ColorKGU.ColorFillNotControl.G, LoadProject.ColorProject.ColorKGU.ColorFillNotControl.B);
                panel_KGUfon.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKGU.ColorFon.R, LoadProject.ColorProject.ColorKGU.ColorFon.G, LoadProject.ColorProject.ColorKGU.ColorFon.B);
                panel_KGUnotcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorKGU.ColorNotControlStroke.R, LoadProject.ColorProject.ColorKGU.ColorNotControlStroke.G, LoadProject.ColorProject.ColorKGU.ColorNotControlStroke.B);
                //цвет Номера поезда
                panel_number_train_ramkadefult.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNumberTrain.ColorRamkaDefult.R, LoadProject.ColorProject.ColorNumberTrain.ColorRamkaDefult.G, LoadProject.ColorProject.ColorNumberTrain.ColorRamkaDefult.B);
                panel_number_train_activ.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNumberTrain.ColorActiv.R, LoadProject.ColorProject.ColorNumberTrain.ColorActiv.G, LoadProject.ColorProject.ColorNumberTrain.ColorActiv.B);
                panel_number_train_pasiv.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNumberTrain.ColorPasiv.R, LoadProject.ColorProject.ColorNumberTrain.ColorPasiv.G, LoadProject.ColorProject.ColorNumberTrain.ColorPasiv.B);
                panel_number_train_notcontrol.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNumberTrain.ColorFillNotControl.R, LoadProject.ColorProject.ColorNumberTrain.ColorFillNotControl.G, LoadProject.ColorProject.ColorNumberTrain.ColorFillNotControl.B);
                panel_number_train_notcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNumberTrain.ColorNotControlStroke.R, LoadProject.ColorProject.ColorNumberTrain.ColorNotControlStroke.G, LoadProject.ColorProject.ColorNumberTrain.ColorNotControlStroke.B);
                panel_number_train_colortrain.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNumberTrain.ColorTrain.R, LoadProject.ColorProject.ColorNumberTrain.ColorTrain.G, LoadProject.ColorProject.ColorNumberTrain.ColorTrain.B);
                panel_number_train_colortrain_defult.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNumberTrain.ColorTrainDefult.R, LoadProject.ColorProject.ColorNumberTrain.ColorTrainDefult.G, LoadProject.ColorProject.ColorNumberTrain.ColorTrainDefult.B);
                //цвет Стрелки поворотов
                panel_arrow_ramkadefult.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorRamkaDefult.R, LoadProject.ColorProject.ColorArrow.ColorRamkaDefult.G, LoadProject.ColorProject.ColorArrow.ColorRamkaDefult.B);
                panel_arrow_departure.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorDeparture.R, LoadProject.ColorProject.ColorArrow.ColorDeparture.G, LoadProject.ColorProject.ColorArrow.ColorDeparture.B);
                panel_arrow_occupation.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorOccupation.R, LoadProject.ColorProject.ColorArrow.ColorOccupation.G, LoadProject.ColorProject.ColorArrow.ColorOccupation.B);
                panel_arrow_okdeparture.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorOkDeparture.R, LoadProject.ColorProject.ColorArrow.ColorOkDeparture.G, LoadProject.ColorProject.ColorArrow.ColorOkDeparture.B);
                panel_arrow_normal.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorNormal.R, LoadProject.ColorProject.ColorArrow.ColorNormal.G, LoadProject.ColorProject.ColorArrow.ColorNormal.B);
                panel_arrow_notcontrol.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorFillNotControl.R, LoadProject.ColorProject.ColorArrow.ColorFillNotControl.G, LoadProject.ColorProject.ColorArrow.ColorFillNotControl.B);
                panel_arrow_notcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorNotControlStroke.R, LoadProject.ColorProject.ColorArrow.ColorNotControlStroke.G, LoadProject.ColorProject.ColorArrow.ColorNotControlStroke.B);
                panel_arrow_waitdeparture.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorArrow.ColorWaitDeparture.R, LoadProject.ColorProject.ColorArrow.ColorWaitDeparture.G, LoadProject.ColorProject.ColorArrow.ColorWaitDeparture.B);
                //цвет блок участков
                panel_block_activ.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorBlockSection.ColorActiv.R, LoadProject.ColorProject.ColorBlockSection.ColorActiv.G, LoadProject.ColorProject.ColorBlockSection.ColorActiv.B);
                panel_block_pasiv.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorBlockSection.ColorPasiv.R, LoadProject.ColorProject.ColorBlockSection.ColorPasiv.G, LoadProject.ColorProject.ColorBlockSection.ColorPasiv.B);
                panel_block_notcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorBlockSection.ColorNotControlStroke.R, LoadProject.ColorProject.ColorBlockSection.ColorNotControlStroke.G, LoadProject.ColorProject.ColorBlockSection.ColorNotControlStroke.B);
                //цвет блок участков
                panel_block_activ.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorBlockSection.ColorActiv.R, LoadProject.ColorProject.ColorBlockSection.ColorActiv.G, LoadProject.ColorProject.ColorBlockSection.ColorActiv.B);
                panel_block_pasiv.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorBlockSection.ColorPasiv.R, LoadProject.ColorProject.ColorBlockSection.ColorPasiv.G, LoadProject.ColorProject.ColorBlockSection.ColorPasiv.B);
                panel_block_notcontrolstroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorBlockSection.ColorNotControlStroke.R, LoadProject.ColorProject.ColorBlockSection.ColorNotControlStroke.G, LoadProject.ColorProject.ColorBlockSection.ColorNotControlStroke.B);
                //цвет рамки поезда
                panel_ramkastation_fill.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorRamkaStation.Fill.R, LoadProject.ColorProject.ColorRamkaStation.Fill.G, LoadProject.ColorProject.ColorRamkaStation.Fill.B);
                panel_ramkastation_stroke.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorRamkaStation.Stroke.R, LoadProject.ColorProject.ColorRamkaStation.Stroke.G, LoadProject.ColorProject.ColorRamkaStation.Stroke.B);
                //цвет названия станции
                panel_namestation_normal.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNameStation.ColorName.R, LoadProject.ColorProject.ColorNameStation.ColorName.G, LoadProject.ColorProject.ColorNameStation.ColorName.B);
                panel_namestation_train.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorNameStation.ColorNameStationTrain.R, LoadProject.ColorProject.ColorNameStation.ColorNameStationTrain.G, LoadProject.ColorProject.ColorNameStation.ColorNameStationTrain.B);
                //цвет часов
                panel_time_fon.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorTime.ColorFon.R, LoadProject.ColorProject.ColorTime.ColorFon.G, LoadProject.ColorProject.ColorTime.ColorFon.B);
                panel_time_font.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorTime.ColorFont.R, LoadProject.ColorProject.ColorTime.ColorFont.G, LoadProject.ColorProject.ColorTime.ColorFont.B);
            }
        }

        private void SetColor() 
        {
            //try
            //{
                if (LoadProject.ColorProject != null)
                {
                    //основные цвета
                    LoadProject.ColorProject.ColorFon = new DataGrafik.ColorN() { R = panel_colorfon.BackColor.R, G = panel_colorfon.BackColor.G, B = panel_colorfon.BackColor.B };
                    LoadProject.ColorProject.ColorArrowCommand = new DataGrafik.ColorN() { R = panel_arrow_command.BackColor.R, G = panel_arrow_command.BackColor.G, B = panel_arrow_command.BackColor.B };
                    //цвета главного пути
                    LoadProject.ColorProject.ColorPath.ColorActiv = new DataGrafik.ColorN() { R = panel_coloractivpath.BackColor.R, G = panel_coloractivpath.BackColor.G, B = panel_coloractivpath.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorAuto = new DataGrafik.ColorN() { R = panel_colorautopath.BackColor.R, G = panel_colorautopath.BackColor.G, B = panel_colorautopath.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorPasiv = new DataGrafik.ColorN() { R = panel_colorpasivpath.BackColor.R, G = panel_colorpasivpath.BackColor.G, B = panel_colorpasivpath.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorFencing = new DataGrafik.ColorN() { R = panel_colorfensingpath.BackColor.R, G = panel_colorfensingpath.BackColor.G, B = panel_colorfensingpath.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_colornotcontrolpath.BackColor.R, G = panel_colornotcontrolpath.BackColor.G, B = panel_colornotcontrolpath.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_colornotcontrolstrokepath.BackColor.R, G = panel_colornotcontrolstrokepath.BackColor.G, B = panel_colornotcontrolstrokepath.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorDieselTraction = new DataGrafik.ColorN() { R = panel_colordisel.BackColor.R, G = panel_colordisel.BackColor.G, B = panel_colordisel.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorElectricTraction = new DataGrafik.ColorN() { R = panel_colorelectro.BackColor.R, G = panel_colorelectro.BackColor.G, B = panel_colorelectro.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorPathName = new DataGrafik.ColorN() { R = panel_colornamepath.BackColor.R, G = panel_colornamepath.BackColor.G, B = panel_colornamepath.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorTrain = new DataGrafik.ColorN() { R = panel_colornametrain.BackColor.R, G = panel_colornametrain.BackColor.G, B = panel_colornametrain.BackColor.B };
                    LoadProject.ColorProject.ColorPath.ColorTrainVertor = new DataGrafik.ColorN() { R = panel_colornametrainvertor.BackColor.R, G = panel_colornametrainvertor.BackColor.G, B = panel_colornametrainvertor.BackColor.B };
                    //цвета сигнала
                    LoadProject.ColorProject.ColorSignal.ColorBusy = new DataGrafik.ColorN() { R = panel_colorsignalbusy.BackColor.R, G = panel_colorsignalbusy.BackColor.G, B = panel_colorsignalbusy.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorClosed = new DataGrafik.ColorN() { R = panel_colorsignalclosed.BackColor.R, G = panel_colorsignalclosed.BackColor.G, B = panel_colorsignalclosed.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorFault = new DataGrafik.ColorN() { R = panel_colorsignalfault.BackColor.R, G = panel_colorsignalfault.BackColor.G, B = panel_colorsignalfault.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_colornotcontrolsignal.BackColor.R, G = panel_colornotcontrolsignal.BackColor.G, B = panel_colornotcontrolsignal.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorFree = new DataGrafik.ColorN() { R = panel_colorsignalfree.BackColor.R, G = panel_colorsignalfree.BackColor.G, B = panel_colorsignalfree.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorInstall = new DataGrafik.ColorN() { R = panel_colorsignalinstall.BackColor.R, G = panel_colorsignalinstall.BackColor.G, B = panel_colorsignalinstall.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorInstallRoute = new DataGrafik.ColorN() { R = panel_colorsignalinstallroute.BackColor.R, G = panel_colorsignalinstallroute.BackColor.G, B = panel_colorsignalinstallroute.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorInvitationalOne = new DataGrafik.ColorN() { R = panel_signalinvitationaleone.BackColor.R, G = panel_signalinvitationaleone.BackColor.G, B = panel_signalinvitationaleone.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorInvitationalTy = new DataGrafik.ColorN() { R = panel_signalinvitationalety.BackColor.R, G = panel_signalinvitationalety.BackColor.G, B = panel_signalinvitationalety.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_colornotcontrolstrokesignal.BackColor.R, G = panel_colornotcontrolstrokesignal.BackColor.G, B = panel_colornotcontrolstrokesignal.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorOpen = new DataGrafik.ColorN() { R = panel_colorsignalopen.BackColor.R, G = panel_colorsignalopen.BackColor.G, B = panel_colorsignalopen.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorRamkaDefult = new DataGrafik.ColorN() { R = panel_colorsignalramkadefult.BackColor.R, G = panel_colorsignalramkadefult.BackColor.G, B = panel_colorsignalramkadefult.BackColor.B };
                    LoadProject.ColorProject.ColorSignal.ColorShunting = new DataGrafik.ColorN() { R = panel_colorsignalshunting.BackColor.R, G = panel_colorsignalshunting.BackColor.G, B = panel_colorsignalshunting.BackColor.B };
                    //цвет кнопки станции
                    LoadProject.ColorProject.ColorButtonStation.ColorDispatcher = new DataGrafik.ColorN() { R = panel_buttonstation_dispatcher.BackColor.R, G = panel_buttonstation_dispatcher.BackColor.G, B = panel_buttonstation_dispatcher.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorAccident = new DataGrafik.ColorN() { R = panel_buttonstation_accident.BackColor.R, G = panel_buttonstation_accident.BackColor.G, B = panel_buttonstation_accident.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorAutonomousControl = new DataGrafik.ColorN() { R = panel_buttonstation_automon.BackColor.R, G = panel_buttonstation_automon.BackColor.G, B = panel_buttonstation_automon.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorFault = new DataGrafik.ColorN() { R = panel_buttonstation_fault.BackColor.R, G = panel_buttonstation_fault.BackColor.G, B = panel_buttonstation_fault.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_buttonstation_notcontrol.BackColor.R, G = panel_buttonstation_notcontrol.BackColor.G, B = panel_buttonstation_notcontrol.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorFire = new DataGrafik.ColorN() { R = panel_buttonstation_fire.BackColor.R, G = panel_buttonstation_fire.BackColor.G, B = panel_buttonstation_fire.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorFon = new DataGrafik.ColorN() { R = panel_buttonstation_fon.BackColor.R, G = panel_buttonstation_fon.BackColor.G, B = panel_buttonstation_fon.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_buttonstation_notcontrolstroke.BackColor.R, G = panel_buttonstation_notcontrolstroke.BackColor.G, B = panel_buttonstation_notcontrolstroke.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorNotLink = new DataGrafik.ColorN() { R = panel_buttonstation_notlink.BackColor.R, G = panel_buttonstation_notlink.BackColor.G, B = panel_buttonstation_notlink.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorRamkaDefult = new DataGrafik.ColorN() { R = panel_buttonstation_ramkadefult.BackColor.R, G = panel_buttonstation_ramkadefult.BackColor.G, B = panel_buttonstation_ramkadefult.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorReserveControl = new DataGrafik.ColorN() { R = panel_buttonstation_reserve.BackColor.R, G = panel_buttonstation_reserve.BackColor.G, B = panel_buttonstation_reserve.BackColor.B };
                    LoadProject.ColorProject.ColorButtonStation.ColorSesonContol = new DataGrafik.ColorN() { R = panel_buttonstation_sesoncontrol.BackColor.R, G = panel_buttonstation_sesoncontrol.BackColor.G, B = panel_buttonstation_sesoncontrol.BackColor.B };
                    //цвет переезда
                    LoadProject.ColorProject.ColorMove.ColorAccident = new DataGrafik.ColorN() { R = panel_moveaccident.BackColor.R, G = panel_moveaccident.BackColor.G, B = panel_moveaccident.BackColor.B };
                    LoadProject.ColorProject.ColorMove.ColorCloseAuto = new DataGrafik.ColorN() { R = panel_movecloseauto.BackColor.R, G = panel_movecloseauto.BackColor.G, B = panel_movecloseauto.BackColor.B };
                    LoadProject.ColorProject.ColorMove.ColorCloseButton = new DataGrafik.ColorN() { R = panel_moveclosbutton.BackColor.R, G = panel_moveclosbutton.BackColor.G, B = panel_moveclosbutton.BackColor.B };
                    LoadProject.ColorProject.ColorMove.ColorFault = new DataGrafik.ColorN() { R = panel_movefault.BackColor.R, G = panel_movefault.BackColor.G, B = panel_movefault.BackColor.B };
                    LoadProject.ColorProject.ColorMove.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_movenotcontrol.BackColor.R, G = panel_movenotcontrol.BackColor.G, B = panel_movenotcontrol.BackColor.B };
                    LoadProject.ColorProject.ColorMove.ColorFon = new DataGrafik.ColorN() { R = panel_movefon.BackColor.R, G = panel_movefon.BackColor.G, B = panel_movefon.BackColor.B };
                    LoadProject.ColorProject.ColorMove.ColorMoveOpen = new DataGrafik.ColorN() { R = panel_movenormalramka.BackColor.R, G = panel_movenormalramka.BackColor.G, B = panel_movenormalramka.BackColor.B };
                    LoadProject.ColorProject.ColorMove.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_movenotcontrolstroke.BackColor.R, G = panel_movenotcontrolstroke.BackColor.G, B = panel_movenotcontrolstroke.BackColor.B };
                    //цвет КТСМ
                    LoadProject.ColorProject.ColorKTCM.ColorAccident = new DataGrafik.ColorN() { R = panel_KTCMrun.BackColor.R, G = panel_KTCMrun.BackColor.G, B = panel_KTCMrun.BackColor.B };
                    LoadProject.ColorProject.ColorKTCM.ColorFault = new DataGrafik.ColorN() { R = panel_KTCMfault.BackColor.R, G = panel_KTCMfault.BackColor.G, B = panel_KTCMfault.BackColor.B };
                    LoadProject.ColorProject.ColorKTCM.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_KTCMnotcontrol.BackColor.R, G = panel_KTCMnotcontrol.BackColor.G, B = panel_KTCMnotcontrol.BackColor.B };
                    LoadProject.ColorProject.ColorKTCM.ColorFon = new DataGrafik.ColorN() { R = panel_KTCMfon.BackColor.R, G = panel_KTCMfon.BackColor.G, B = panel_KTCMfon.BackColor.B };
                    LoadProject.ColorProject.ColorKTCM.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_KTCMnotcontrolstroke.BackColor.R, G = panel_KTCMnotcontrolstroke.BackColor.G, B = panel_KTCMnotcontrolstroke.BackColor.B };
                    LoadProject.ColorProject.ColorKTCM.ColorRamkaDefult = new DataGrafik.ColorN() { R = panel_KTCMnormalramka.BackColor.R, G = panel_KTCMnormalramka.BackColor.G, B = panel_KTCMnormalramka.BackColor.B };
                    //
                    //цвет КГУ
                    LoadProject.ColorProject.ColorKGU.ColorAccident = new DataGrafik.ColorN() { R = panel_KGUrun.BackColor.R, G = panel_KGUrun.BackColor.G, B = panel_KGUrun.BackColor.B };
                    LoadProject.ColorProject.ColorKGU.ColorFault = new DataGrafik.ColorN() { R = panel_KGUfault.BackColor.R, G = panel_KGUfault.BackColor.G, B = panel_KGUfault.BackColor.B };
                    LoadProject.ColorProject.ColorKGU.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_KGUnotcontrol.BackColor.R, G = panel_KGUnotcontrol.BackColor.G, B = panel_KGUnotcontrol.BackColor.B };
                    LoadProject.ColorProject.ColorKGU.ColorFon = new DataGrafik.ColorN() { R = panel_KGUfon.BackColor.R, G = panel_KGUfon.BackColor.G, B = panel_KGUfon.BackColor.B };
                    LoadProject.ColorProject.ColorKGU.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_KGUnotcontrolstroke.BackColor.R, G = panel_KGUnotcontrolstroke.BackColor.G, B = panel_KGUnotcontrolstroke.BackColor.B };
                    LoadProject.ColorProject.ColorKGU.ColorRamkaDefult = new DataGrafik.ColorN() { R = panel_KGUnormalramka.BackColor.R, G = panel_KGUnormalramka.BackColor.G, B = panel_KGUnormalramka.BackColor.B };
                    //
                    //цвет Номера поезда
                    LoadProject.ColorProject.ColorNumberTrain.ColorActiv = new DataGrafik.ColorN() { R = panel_number_train_activ.BackColor.R, G = panel_number_train_activ.BackColor.G, B = panel_number_train_activ.BackColor.B };
                    LoadProject.ColorProject.ColorNumberTrain.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_number_train_notcontrol.BackColor.R, G = panel_number_train_notcontrol.BackColor.G, B = panel_number_train_notcontrol.BackColor.B };
                    LoadProject.ColorProject.ColorNumberTrain.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_number_train_notcontrolstroke.BackColor.R, G = panel_number_train_notcontrolstroke.BackColor.G, B = panel_number_train_notcontrolstroke.BackColor.B };
                    LoadProject.ColorProject.ColorNumberTrain.ColorPasiv = new DataGrafik.ColorN() { R = panel_number_train_pasiv.BackColor.R, G = panel_number_train_pasiv.BackColor.G, B = panel_number_train_pasiv.BackColor.B };
                    LoadProject.ColorProject.ColorNumberTrain.ColorRamkaDefult = new DataGrafik.ColorN() { R = panel_number_train_ramkadefult.BackColor.R, G = panel_number_train_ramkadefult.BackColor.G, B = panel_number_train_ramkadefult.BackColor.B };
                    LoadProject.ColorProject.ColorNumberTrain.ColorTrain = new DataGrafik.ColorN() { R = panel_number_train_colortrain.BackColor.R, G = panel_number_train_colortrain.BackColor.G, B = panel_number_train_colortrain.BackColor.B };
                    LoadProject.ColorProject.ColorNumberTrain.ColorTrainDefult = new DataGrafik.ColorN() { R = panel_number_train_colortrain_defult.BackColor.R, G = panel_number_train_colortrain_defult.BackColor.G, B = panel_number_train_colortrain_defult.BackColor.B };
                    //
                    //цвет Стрелки поворотов
                    LoadProject.ColorProject.ColorArrow.ColorDeparture = new DataGrafik.ColorN() { R = panel_arrow_departure.BackColor.R, G = panel_arrow_departure.BackColor.G, B = panel_arrow_departure.BackColor.B };
                    LoadProject.ColorProject.ColorArrow.ColorFillNotControl = new DataGrafik.ColorN() { R = panel_arrow_notcontrol.BackColor.R, G = panel_arrow_notcontrol.BackColor.G, B = panel_arrow_notcontrol.BackColor.B };
                    LoadProject.ColorProject.ColorArrow.ColorNormal = new DataGrafik.ColorN() { R = panel_arrow_normal.BackColor.R, G = panel_arrow_normal.BackColor.G, B = panel_arrow_normal.BackColor.B };
                    LoadProject.ColorProject.ColorArrow.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_arrow_notcontrolstroke.BackColor.R, G = panel_arrow_notcontrolstroke.BackColor.G, B = panel_arrow_notcontrolstroke.BackColor.B };
                    LoadProject.ColorProject.ColorArrow.ColorOccupation = new DataGrafik.ColorN() { R = panel_arrow_occupation.BackColor.R, G = panel_arrow_occupation.BackColor.G, B = panel_arrow_occupation.BackColor.B };
                    LoadProject.ColorProject.ColorArrow.ColorRamkaDefult = new DataGrafik.ColorN() { R = panel_arrow_ramkadefult.BackColor.R, G = panel_arrow_ramkadefult.BackColor.G, B = panel_arrow_ramkadefult.BackColor.B };
                    LoadProject.ColorProject.ColorArrow.ColorWaitDeparture = new DataGrafik.ColorN() { R = panel_arrow_waitdeparture.BackColor.R, G = panel_arrow_waitdeparture.BackColor.G, B = panel_arrow_waitdeparture.BackColor.B };
                    LoadProject.ColorProject.ColorArrow.ColorOkDeparture = new DataGrafik.ColorN() { R = panel_arrow_okdeparture.BackColor.R, G = panel_arrow_okdeparture.BackColor.G, B = panel_arrow_okdeparture.BackColor.B };
                    //
                    //цвет блок участков
                    LoadProject.ColorProject.ColorBlockSection.ColorActiv = new DataGrafik.ColorN() { R = panel_block_activ.BackColor.R, G = panel_block_activ.BackColor.G, B = panel_block_activ.BackColor.B };
                    LoadProject.ColorProject.ColorBlockSection.ColorNotControlStroke = new DataGrafik.ColorN() { R = panel_block_notcontrolstroke.BackColor.R, G = panel_block_notcontrolstroke.BackColor.G, B = panel_block_notcontrolstroke.BackColor.B };
                    LoadProject.ColorProject.ColorBlockSection.ColorPasiv = new DataGrafik.ColorN() { R = panel_block_pasiv.BackColor.R, G = panel_block_pasiv.BackColor.G, B = panel_block_pasiv.BackColor.B };
                    //цвет рамки станции panel_namestation_normal
                    LoadProject.ColorProject.ColorRamkaStation.Fill = new DataGrafik.ColorN() { R = panel_ramkastation_fill.BackColor.R, G = panel_ramkastation_fill.BackColor.G, B = panel_ramkastation_fill.BackColor.B };
                    LoadProject.ColorProject.ColorRamkaStation.Stroke = new DataGrafik.ColorN() { R = panel_ramkastation_stroke.BackColor.R, G = panel_ramkastation_stroke.BackColor.G, B = panel_ramkastation_stroke.BackColor.B };
                    //
                    //цвет названия станции
                    LoadProject.ColorProject.ColorNameStation.ColorName = new DataGrafik.ColorN() { R = panel_namestation_normal.BackColor.R, G = panel_namestation_normal.BackColor.G, B = panel_namestation_normal.BackColor.B };
                    LoadProject.ColorProject.ColorNameStation.ColorNameStationTrain = new DataGrafik.ColorN() { R = panel_namestation_train.BackColor.R, G = panel_namestation_train.BackColor.G, B = panel_namestation_train.BackColor.B };
                    //цвет часов
                    LoadProject.ColorProject.ColorTime.ColorFon = new DataGrafik.ColorN() { R = panel_time_fon.BackColor.R, G = panel_time_fon.BackColor.G, B = panel_time_fon.BackColor.B };
                    LoadProject.ColorProject.ColorTime.ColorFont = new DataGrafik.ColorN() { R = panel_time_font.BackColor.R, G = panel_time_font.BackColor.G, B = panel_time_font.BackColor.B };
                    //
                    LoadProject.SetColor();
                }
            //}
            //catch { }
        }

        private void ColorHelpLine()
        {
            if (comboBox_namecolor.Items.Count == 0)
            {
                panel_help_line.Enabled = !panel_help_line.Enabled;
                comboBox_namecolor.Enabled = !comboBox_namecolor.Enabled;
            }
        }

        private void Save()
        {
            try
            {
                if (!string.IsNullOrEmpty(LoadProject.ColorProject.Name))
                {
                    Stream savestream = new FileStream(LoadProject.ColorProject.File, FileMode.Create);
                    // Указываем тип того объекта, который сериализуем
                    XmlSerializer xml = new XmlSerializer(typeof(TrafficTrain.DataGrafik.ColorSaveProejct));
                    // Сериализуем
                    xml.Serialize(savestream, LoadProject.ColorProject);
                    savestream.Close();
                }
                //if (ConfigurationManager.AppSettings["file_color_configuration"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["file_color_configuration"]))
                //{
                   
                //}
            }
            catch  { }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                Panel panel = sender as Panel;
                //e.Graphics.FillRectangle(new SolidBrush(panel.BackColor),new Rectangle(new Point(0, 0), new Size(panel.Width , panel.Height)));
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 3), new Rectangle(new Point(0, 0), new Size(panel.Width - 1, panel.Height - 1)));
            }
        }

        private void comboBox_namecolor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_namecolor.SelectedIndex != -1)
            {
                int index = LoadProject.FindElement(LoadProject.ColorProject.ColorHelpLines, comboBox_namecolor.SelectedItem.ToString(), true);
                if (index != -1)
                {
                    panel_help_line.BackColor = Color.FromArgb(LoadProject.ColorProject.ColorHelpLines[index].ColorHelpLine.R, LoadProject.ColorProject.ColorHelpLines[index].ColorHelpLine.G,
                                                        LoadProject.ColorProject.ColorHelpLines[index].ColorHelpLine.B);
                }
                else
                {
                    panel_help_line.BackColor = Color.FromArgb(175, 175, 175);
                }
            }
        }

        private void вверхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlElement.Alignment = TabAlignment.Top;
        }

        private void низToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlElement.Alignment = TabAlignment.Bottom;
        }

        private void слеваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlElement.Alignment = TabAlignment.Left;
        }

        private void справаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlElement.Alignment = TabAlignment.Right;
        }




    }
}
