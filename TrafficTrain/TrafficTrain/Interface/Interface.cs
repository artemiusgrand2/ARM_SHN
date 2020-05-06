using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

using SCADA.Common.Enums;
using SCADA.Common.SaveElement;

namespace TrafficTrain.Interface
{

    public interface IText
    {
        TextBlock Text { get; set; }
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
        /// Фигура
        /// </summary>
        PathGeometry Figure { get; }
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
        /// коллекция линий
        /// </summary>
        List<Line> Lines { get; }
        /// <summary>
        /// коллекция вершин
        /// </summary>
        PointCollection Points { get; }
        /// <summary>
        /// ценр фигуры
        /// </summary>
        Point PointCenter { get; }
        /// <summary>
        /// создаем коллекция линий
        /// </summary>
        void CreateCollectionLines();
        /// <summary>
        /// толщина элемента
        /// </summary>
        double StrokeThickness { get; set; }

        int StationControl { get; set; }
        int StationTransition { get; set; }
        string FileClick { get; set; }
        string NameUl { get; }
    }

    public interface IActriveControl
    {
        void Analis();
        void ServerClosing();
    }


}
