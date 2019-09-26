using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Move;

namespace TrafficTrain
{
    public interface GraficElement
    {
        /// <summary>
        /// масштабируем объект
        /// </summary>
        /// <param name="scaletransform"></param>
        /// <param name="scale"></param>
        void ScrollFigure(ScaleTransform scaletransform, double scale);
        /// <summary>
        /// перемещение объекта 
        /// </summary>
        /// <param name="deltaX">смещение по оси X</param>
        /// <param name="deltaY">смещение по оси Y</param>
        void SizeFigure(double deltaX, double deltaY);
        /// <summary>
        /// возврат к первоначальным координат 
        /// </summary>
        void StartPosition(Point center, double scroll);
    }
    /// <summary>
    /// для элементов которые отображают показания тс импульсов
    /// </summary>
    public interface ImpulsTSElement
    {
        /// <summary>
        /// коллекция возможных состояний активного элемента
        /// </summary>
        List<StateElement> Impulses{get;set;}
    }
    public interface ElementRoute
    {
    }

    public interface ClickElement
    {
    }

    public interface TrainsInfo
    {
        List<t_train> Trains { get; }
        int StationNumber { get; }
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
    }
}
