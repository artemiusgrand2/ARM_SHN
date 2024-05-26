using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using ARM_SHN.Interface;
using ARM_SHN.ElementControl;


namespace ARM_SHN.CommandsElement
{
    /// <summary>
    /// выбираем перегон или элементы для отправки команды ТУ
    /// </summary>
    class SelectElements
    {
        /// <summary>
        /// выбираем один элемент
        /// </summary>
        /// <param name="collection">коллекция нарисованных объектов</param>
        /// <param name="pointclick">точка нажатия на мышь</param>
        public static bool ClickElement(IList<UIElement> collection, Point pointclick, ContentControl content)
        {
            foreach (var el in collection)
            {
                var command = el as ISelectElement;
                if (command != null)
                {
                    if (CheckIsMouseOver(el))
                        return Commands.AnalisCommand(command, content);
                }
            }
            //
            return false;
        }

        static bool CheckIsMouseOver(UIElement el)
        {
            if (el.IsMouseOver || ((el is IText) && (el as IText).Text.IsMouseOver))
                return true;
            //
            return false;
        }

        public static bool FindInfoElement(IList<UIElement> collection, Point pointclick, CommandButton commandbutton)
        {
            var selectElement = new List<IInfoElement>();
            var result = false;
            foreach (var el in collection)
            {
                var command = el as ISelectElement;
                if(command != null)
                {
                    if (CheckIsMouseOver(el))
                        if (el is IInfoElement)
                            selectElement.Add(el as IInfoElement);
                }
            }
            //
            var note = string.Empty;
            if (selectElement.Count > 0)
            {
                selectElement.Sort(Sort);
                note = (selectElement[selectElement.Count - 1]).InfoElement();  
                result = true;
            }
            if (commandbutton != null)
            {
                if (!string.IsNullOrEmpty(note))
                {
                    commandbutton.Visibility = Visibility.Visible;
                    commandbutton.Text.Visibility = Visibility.Visible;
                    commandbutton.ViewModel.Text = note;
                    commandbutton.LocationText();
                }
                else if (commandbutton.Visibility == Visibility.Visible)
                {
                    commandbutton.Visibility = Visibility.Collapsed;
                    commandbutton.Text.Visibility = Visibility.Collapsed;
                }
            }
            //
            return result;
        }

        private static int Sort(IInfoElement x, IInfoElement y)
        {
            if (x.ZIntex > y.ZIntex)
                return 1;
            if (x.ZIntex < y.ZIntex)
                return -1;
            return 0;
        }

    }
}
