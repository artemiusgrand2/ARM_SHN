using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrafficTrain.TrainNumber;
using TrafficTrain.Strage.DataStrage;

namespace TrafficTrain
{
    public interface GraficElement
    {
        /// <summary>
        /// Фигура
        /// </summary>
        PathGeometry Figure { get; }
    }
    /// <summary>
    /// виды точек маршрута (блокучасток или путь)
    /// </summary>
    public interface PointRoute
    {
        /// <summary>
        /// коллекция возможных состояний активного элемента
        /// </summary>
        Dictionary<Viewmode, StateElement> Impulses { get; set; }
    }

    public interface ElementRoute { }

    public interface ClickElement { }

    public interface TrainsInfo
    {
        List<t_train> Trains { get; }
        int StationNumber { get; }
    }

    public interface ElementInfo
    {
        string InfoElement();
    }

    public interface ElementViewStation
    {
        void UpdateStation(TrafficTrain.DataGrafik.StrageProject grafic);
        int CurrentStation {get; set;}
    }

    /// <summary>
    /// для элементов которые отображают показания тс импульсов
    /// </summary>
    public interface CommandElement
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
    }
}
