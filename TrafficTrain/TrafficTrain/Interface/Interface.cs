using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;

namespace ARM_SHN.Interface
{

    public interface IText
    {
        TextBlock Text { get; }
    }

    public interface IIndicationEl
    {
        IList<string> Analis();
        void ServerClose();
        Dictionary<Viewmode, StateElement> Impulses { get; set; }
    }

    public interface IGraficElement
    {
        /// <summary>
        /// Пояснения
        /// </summary>
        string Notes { get; set; }
        /// <summary>
        /// Индекс слоя
        /// </summary>
        int ZIntex { get; set; }
        /// <summary>
        /// Видимость
        /// </summary>
        Visibility Visibility { get; set; }
    }

    public interface IInfoElement
    {
        string InfoElement();
        /// <summary>
        /// Индекс слоя
        /// </summary>
        int ZIntex { get; set; }
    }

    /// <summary>
    /// для элементов которые отображают показания тс импульсов
    /// </summary>
    public interface ISelectElement
    {
        /// <summary>
        /// толщина элемента
        /// </summary>
        //double StrokeThickness { get; set; }

        int StationControl { get; set; }
        int StationTransition { get; set; }
        string FileClick { get; set; }
        string NameUl { get; }
    }

}
