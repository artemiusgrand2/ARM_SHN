using System;
using TrafficTrain.Enums;
using SCADA.Common.Enums;

namespace TrafficTrain.Delegate
{
    /// <summary>
    /// делегат сообщает показывать или нет номера поездов
    /// </summary>
    public delegate void CommandObject(EventVisibleElement visible);
    /// <summary>
    /// делегат сообщает показывать или нет контроль
    /// </summary>
    public delegate void OnOffObject(bool status, ViewCommand view);
    /// <summary>
    /// делегат сообщает какой из фильтров приминен
    /// </summary>
    public delegate void FilterNumberTrain(ViewPanel ViewPanel, ViewCommand ViewCommand);

    public delegate void ShowCommand(ViewCommand view);

    /// <summary>
    /// Произошли изменения в журнале событий
    /// </summary>
    public delegate void UpdateInfo(ViewMessageJournal view);
    /// <summary>
    /// делегат сообщющий о ходе загрузки
    /// </summary>
    /// <param name="info">инфо загрузки</param>
    public delegate void Info(string info);
    /// <summary>
    /// делегат о справке по поезду 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="row"></param>
    public delegate void InfoMessageTrain(string info);
    /// <summary>
    /// делегат для изменения цвета главной формы
    /// </summary>
    /// <param name="fon"></param>
    /// <param name="arrowcommand"></param>
    public delegate void NewColor();

    public delegate void AddMessage();
}
