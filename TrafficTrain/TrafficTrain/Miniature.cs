using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TrafficTrain
{
    public partial class Miniature : Form
    {
        #region Переменные и свойства
        /// <summary>
        /// оформляющая рамка
        /// </summary>
        Rectangle ramka = new Rectangle();
        /// <summary>
        /// основное окно программы
        /// </summary>
        System.Windows.Window window;
        #endregion

        public Miniature(System.Windows.Window winone)
        {
            InitializeComponent();
            label_name.Font = new System.Drawing.Font(label_name.Font.FontFamily,(float)(label_name.Font.Size * System.Windows.SystemParameters.CaretWidth), label_name.Font.Style);
            label_infoload.Font = new System.Drawing.Font(label_infoload.Font.FontFamily, (float)(label_infoload.Font.Size * System.Windows.SystemParameters.CaretWidth), label_infoload.Font.Style);
            //
            window = winone;
            ramka.X = 0; ramka.Y = 0;
            ramka.Height = Height; ramka.Width = Width;
            
            LoadProject.LoadInfo += new Info(LoadData_LoadInfo);
            MainWindow.LoadInfo += new Info(MainWindow_LoadInfo);
        }

        /// <summary>
        /// информация о загрузке графики и перегонов
        /// </summary>
        /// <param name="infoload">инфо</param>
        private void LoadData_LoadInfo(string infoload)
        {
            Invoke(new Action(() =>
            {
                Thread.Sleep(700);
                label_infoload.Text = infoload;                
            }
             ));
        }

        /// <summary>
        /// информация о подключении к серверам
        /// </summary>
        /// <param name="infoload"></param>
        private void MainWindow_LoadInfo(string infoload)
        {
            Invoke(new Action(() =>
            {
                if (infoload != "End")
                {
                    Thread.Sleep(1000);
                    label_infoload.Text = infoload;
                }
                else
                    Close();
            }
         ));
            //активация основного потока
             window.Dispatcher.Invoke(
                new Action(() =>
                {
                    if (infoload == "End")
                    {
                        Thread.Sleep(1000);
                        window.Visibility = System.Windows.Visibility.Visible;
                        //window.Activate();
                        //window.WindowState = System.Windows.WindowState.Maximized;
                    }
                }
                )
                );
        }

        private void Miniature_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Brown,4), ramka);
            Opacity = 1;
        }
    }
}
