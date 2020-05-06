using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

using TrafficTrain.Interface;
using TrafficTrain.Delegate;
using TrafficTrain.Constant;
using SCADA.Common.Enums;

namespace TrafficTrain
{
    /// <summary>
    /// класс отвечает за составление комманд, анализ и их выполнение
    /// </summary>
    class Commands
    {
        #region Переменные и свойства
        /// <summary>
        /// информация о правильности построения команды
        /// </summary>
        public static event Info InfoCommand;
        #endregion

        /// <summary>
        /// анализируем новую точку для команду
        /// </summary>
        /// <returns>можно или нет добавлять команду в списокыы</returns>
        public static bool AnalisCommand(ISelectElement element, ContentControl content)
        {
            //если выбран элемент название станции
            CommandButton command = element as CommandButton;
            if (command != null)
            {
                switch (command.ViewCommand)
                {
                    default:
                        command.Click(content);
                        return true;
                }
                
            }
            //
            //if (element is IShowElement)
            //{
            //    LoadProject.UpdateStation((element as IShowElement).StationNumberRight, element.NameUl);
            //}
            //else
            //{
            if (!string.IsNullOrEmpty(element.FileClick) && System.IO.File.Exists(element.FileClick))
                System.Diagnostics.Process.Start(element.FileClick);
            else
                LoadProject.UpdateStation(element.StationTransition, element.NameUl);
            //}
            //
            return true;
        }
      
    }
}
