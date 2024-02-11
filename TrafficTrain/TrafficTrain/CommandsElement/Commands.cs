using System;
using System.Windows.Controls;

using ARM_SHN.Interface;
using ARM_SHN.ElementControl;

namespace ARM_SHN.CommandsElement
{
    /// <summary>
    /// класс отвечает за составление комманд, анализ и их выполнение
    /// </summary>
    class Commands
    {
        /// <summary>
        /// анализируем новую точку для команду
        /// </summary>
        /// <returns>можно или нет добавлять команду в списокыы</returns>
        public static bool AnalisCommand(ISelectElement element, ContentControl content)
        {
            //если выбран элемент название станции
            var command = element as CommandButton;
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
            if (!string.IsNullOrEmpty(element.FileClick) && System.IO.File.Exists(element.FileClick))
                System.Diagnostics.Process.Start(element.FileClick);
            else
                LoadProject.UpdateStation(element.StationTransition, element.NameUl);
            //
            return true;
        }
      
    }
}
