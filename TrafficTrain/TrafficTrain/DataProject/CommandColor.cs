using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using TrafficTrain.DataGrafik;
using TrafficTrain.WorkWindow;

namespace TrafficTrain
{
    class CommandColor
    {
        public static void SetAllColor()
        {
            LoadColorControl.AnalisLoad();
            for (int i = 1; i <= 151; i++)
            {
                ValueColor value = ControlColor.FindColor(GetNameColor((EnumColor)i));
                if (value != null)
                    SetColor((EnumColor)i, Color.FromRgb(value.Value.R, value.Value.G, value.Value.B));
            }
        }

        public static string GetNameColor(EnumColor id)
        {
            switch (id)
            {
                case EnumColor.ScreenFon:
                    return LoadProject.ColorConfiguration.Screen.Fon;
                case EnumColor.ScreenArrowCommand:
                    return LoadProject.ColorConfiguration.Screen.ArrowCommand;
                case EnumColor.NameStationTrain:
                    return LoadProject.ColorConfiguration.NameStation.Train;
                case EnumColor.NameStationTrack:
                    return LoadProject.ColorConfiguration.NameStation.Track;
                case EnumColor.AreaStationFon:
                    return LoadProject.ColorConfiguration.AreaStation.Fon;
                case EnumColor.AreaStationStroke:
                    return LoadProject.ColorConfiguration.AreaStation.Stroke;
                case EnumColor.AreaStragePasiveFon:
                    return LoadProject.ColorConfiguration.AreaStrage.PasiveFon;
                case EnumColor.AreaStrageActiveFon:
                    return LoadProject.ColorConfiguration.AreaStrage.ActiveFon;
                case EnumColor.AreaStrageNotControlFon:
                    return LoadProject.ColorConfiguration.AreaStrage.NotControlFon;
                case EnumColor.AreaStrageNormalStroke:
                    return LoadProject.ColorConfiguration.AreaStrage.NormalStroke;
                case EnumColor.AreaStrageNotControlStroke:
                    return LoadProject.ColorConfiguration.AreaStrage.NotControlStroke;
                case EnumColor.AreaStrageNormalText:
                    return LoadProject.ColorConfiguration.AreaStrage.NormalText;
                case EnumColor.AreaStrageNotNormalText:
                    return LoadProject.ColorConfiguration.AreaStrage.NotNormalText;
                case EnumColor.TrackStragePasive:
                    return LoadProject.ColorConfiguration.TrackStrage.Pasive;
                case EnumColor.TrackStrageActive:
                    return LoadProject.ColorConfiguration.TrackStrage.Active;
                case EnumColor.TrackStrageNotControl:
                    return LoadProject.ColorConfiguration.TrackStrage.NotControl;
                case EnumColor.TrackPasiveFon:
                    return LoadProject.ColorConfiguration.Track.PasiveFon;
                case EnumColor.TrackActiveFon:
                    return LoadProject.ColorConfiguration.Track.ActiveFon;
                case EnumColor.TrackNotControlFon:
                    return LoadProject.ColorConfiguration.Track.NotControlFon;
                case EnumColor.TrackFencingFon:
                    return LoadProject.ColorConfiguration.Track.FencingFon;
                case EnumColor.TrackLockingFon:
                    return LoadProject.ColorConfiguration.Track.LockingFon;
                case EnumColor.TrackElectroStroke:
                    return LoadProject.ColorConfiguration.Track.ElectroStroke;
                case EnumColor.TrackDiselStroke:
                    return LoadProject.ColorConfiguration.Track.DiselStroke;
                case EnumColor.TrackNotControlStroke:
                    return LoadProject.ColorConfiguration.Track.NotControlStroke;
                case EnumColor.TrackTrackText:
                    return LoadProject.ColorConfiguration.Track.TrackText;
                case EnumColor.TrackTrainText:
                    return LoadProject.ColorConfiguration.Track.TrainText;
                case EnumColor.TrackVectorText:
                    return LoadProject.ColorConfiguration.Track.VectorText;
                case EnumColor.TrackTrainPlanText:
                    return LoadProject.ColorConfiguration.Track.TrainPlanText;
                case EnumColor.MoveFaultStroke:
                    return LoadProject.ColorConfiguration.Move.FaultStroke;
                case EnumColor.MoveAccidentStroke:
                    return LoadProject.ColorConfiguration.Move.AccidentStroke;
                case EnumColor.MoveNotControlStroke:
                    return LoadProject.ColorConfiguration.Move.NotControlStroke;
                case EnumColor.MoveDefultStroke:
                    return LoadProject.ColorConfiguration.Move.DefultStroke;
                case EnumColor.MoveDefultFon:
                    return LoadProject.ColorConfiguration.Move.DefultFon;
                case EnumColor.MoveNotControlFon:
                    return LoadProject.ColorConfiguration.Move.NotControlFon;
                case EnumColor.MoveButtonClosedFon:
                    return LoadProject.ColorConfiguration.Move.ButtonClosedFon;
                case EnumColor.MoveAutoClosedFon:
                    return LoadProject.ColorConfiguration.Move.AutoClosedFon;
                case EnumColor.ControlObjectFaultStroke:
                    return LoadProject.ColorConfiguration.ControlObject.FaultStroke;
                case EnumColor.ControlObjectNotControlStroke:
                    return LoadProject.ColorConfiguration.ControlObject.NotControlStroke;
                case EnumColor.ControlObjectDefultStroke:
                    return LoadProject.ColorConfiguration.ControlObject.DefultStroke;
                case EnumColor.ControlObjectPlayFon:
                    return LoadProject.ColorConfiguration.ControlObject.PlayFon;
                case EnumColor.ControlObjectNotControlFon:
                    return LoadProject.ColorConfiguration.ControlObject.NotControlFon;
                case EnumColor.ControlObjectDefultFon:
                    return LoadProject.ColorConfiguration.ControlObject.DefultFon;
                case EnumColor.ButtonStationReserveControlFon:
                    return LoadProject.ColorConfiguration.ButtonStation.ReserveControlFon;
                case EnumColor.ButtonStationSesonControlFon:
                    return LoadProject.ColorConfiguration.ButtonStation.SesonControlFon;
                case EnumColor.ButtonStationDispatcherControlFon:
                    return LoadProject.ColorConfiguration.ButtonStation.DispatcherControlFon;
                case EnumColor.ButtonStationAutoDispatcherControlFon:
                    return LoadProject.ColorConfiguration.ButtonStation.AutoDispatcherControlFon;
                case EnumColor.ButtonStationAutonomousСontrolFon:
                    return LoadProject.ColorConfiguration.ButtonStation.AutonomousСontrolFon;
                case EnumColor.ButtonStationNotDispatcherControlFon:
                    return LoadProject.ColorConfiguration.ButtonStation.NotDispatcherControlFon;
                case EnumColor.ButtonStationNotControlFon:
                    return LoadProject.ColorConfiguration.ButtonStation.NotControlFon;
                case EnumColor.ButtonStationDefultFon:
                    return LoadProject.ColorConfiguration.ButtonStation.DefultFon;
                case EnumColor.ButtonStationFireControlFon:
                    return LoadProject.ColorConfiguration.ButtonStation.FireControlFon;
                case EnumColor.ButtonStationFaultStroke:
                    return LoadProject.ColorConfiguration.ButtonStation.FaultStroke;
                case EnumColor.ButtonStationAccidentStroke:
                    return LoadProject.ColorConfiguration.ButtonStation.AccidentStroke;
                case EnumColor.ButtonStationNotControlStroke:
                    return LoadProject.ColorConfiguration.ButtonStation.NotControlStroke;
                case EnumColor.ButtonStationDefultStroke:
                    return LoadProject.ColorConfiguration.ButtonStation.DefultStroke;
                case EnumColor.ActiveLineActiveStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.ActiveStroke;
                case EnumColor.ActiveLinePasiveStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.PasiveStroke;
                case EnumColor.ActiveLineNotControlStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.NotControlStroke;
                case EnumColor.ActiveLineLocingStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.LocingStroke;
                case EnumColor.ActiveLineFencingStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.FencingStroke;
                case EnumColor.ActiveLinePassegeStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.PassegeStroke;
                case EnumColor.ActiveLineСuttingOneStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.СuttingOneStroke;
                case EnumColor.ActiveLineСuttingTyStroke:
                    return LoadProject.ColorConfiguration.ActiveLine.СuttingTyStroke;
                case EnumColor.DirectionActiveStrageFon:
                    return LoadProject.ColorConfiguration.Direction.ActiveStrageFon;
                case EnumColor.DirectionPasiveStrageFon:
                    return LoadProject.ColorConfiguration.Direction.PasiveStrageFon;
                case EnumColor.DirectionNotControlStrageFon:
                    return LoadProject.ColorConfiguration.Direction.NotControlStrageFon;
                case EnumColor.DirectionDepartureDirectonFon:
                    return LoadProject.ColorConfiguration.Direction.DepartureDirectonFon;
                case EnumColor.DirectionWaitingDepartureDirectonFon:
                    return LoadProject.ColorConfiguration.Direction.WaitingDepartureDirectonFon;
                case EnumColor.DirectionOKDepartureDirectonFon:
                    return LoadProject.ColorConfiguration.Direction.OKDepartureDirectonFon;
                case EnumColor.DirectionNotControlStroke:
                    return LoadProject.ColorConfiguration.Direction.NotControlStroke;
                case EnumColor.DirectionDefultStroke:
                    return LoadProject.ColorConfiguration.Direction.DefultStroke;
                case EnumColor.RouteSignalDefultStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.DefultStroke;
                case EnumColor.RouteSignalReceivedOneStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.ReceivedOneStroke;
                case EnumColor.RouteSignalReceivedTyStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.ReceivedTyStroke;
                case EnumColor.RouteSignalCheckRouteStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.CheckRouteStroke;
                case EnumColor.RouteSignalWaitInstallOneStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.WaitInstallOneStroke;
                case EnumColor.RouteSignalWaitInstallTyStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.WaitInstallTyStroke;
                case EnumColor.RouteSignalFaultStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.FaultStroke;
                case EnumColor.RouteSignalNotControlStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.NotControlStroke;
                case EnumColor.RouteSignalActiceFon:
                    return LoadProject.ColorConfiguration.RouteSignal.ActiceFon;
                case EnumColor.RouteSignalPasiveFon:
                    return LoadProject.ColorConfiguration.RouteSignal.PasiveFon;
                case EnumColor.RouteSignalNotControlFon:
                    return LoadProject.ColorConfiguration.RouteSignal.NotControlFon;
                case EnumColor.RouteSignalLockingFon:
                    return LoadProject.ColorConfiguration.RouteSignal.LockingFon;
                case EnumColor.RouteSignalFencingFon:
                    return LoadProject.ColorConfiguration.RouteSignal.FencingFon;
                case EnumColor.RouteSignalPassageFon:
                    return LoadProject.ColorConfiguration.RouteSignal.PassageFon;
                case EnumColor.RouteSignalInvitationalOneFon:
                    return LoadProject.ColorConfiguration.RouteSignal.InvitationalOneFon;
                case EnumColor.RouteSignalInvitationalTyFon:
                    return LoadProject.ColorConfiguration.RouteSignal.InvitationalTyFon;
                case EnumColor.RouteSignalInstallOneStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.InstallOneStroke;
                case EnumColor.RouteSignalInstallTyStroke:
                    return LoadProject.ColorConfiguration.RouteSignal.InstallTyStroke;
                case EnumColor.RouteSignalShuntingFon:
                    return LoadProject.ColorConfiguration.RouteSignal.ShuntingFon;
                case EnumColor.RouteSignalSignalFon:
                    return LoadProject.ColorConfiguration.RouteSignal.SignalFon;
                case EnumColor.HelpElementFaultStroke:
                    return LoadProject.ColorConfiguration.HelpElement.FaultStroke;
                case EnumColor.HelpElementAccidentFon:
                    return LoadProject.ColorConfiguration.HelpElement.AccidentFon;
                case EnumColor.HelpElementAccidentDGAStroke:
                    return LoadProject.ColorConfiguration.HelpElement.AccidentDGAStroke;
                case EnumColor.HelpElementNotControlFon:
                    return LoadProject.ColorConfiguration.HelpElement.NotControlFon;
                case EnumColor.HelpElementNotControlStroke:
                    return LoadProject.ColorConfiguration.HelpElement.NotControlStroke;
                case EnumColor.HelpElementDefultFon:
                    return LoadProject.ColorConfiguration.HelpElement.DefultFon;
                case EnumColor.HelpElementDefultStroke:
                    return LoadProject.ColorConfiguration.HelpElement.DefultStroke;
                case EnumColor.HelpElementOffWeightFon:
                    return LoadProject.ColorConfiguration.HelpElement.OffWeightFon;
                case EnumColor.HelpElementOnWeightFon:
                    return LoadProject.ColorConfiguration.HelpElement.OnWeightFon;
                case EnumColor.HelpElementText:
                    return LoadProject.ColorConfiguration.HelpElement.Text;
                case EnumColor.HelpTextAndTimeTimeFon:
                    return LoadProject.ColorConfiguration.HelpTextAndTime.TimeFon;
                case EnumColor.HelpTextAndTimeStrokeTime:
                    return LoadProject.ColorConfiguration.HelpTextAndTime.StrokeTime;
                case EnumColor.HelpTextAndTimeTextTime:
                    return LoadProject.ColorConfiguration.HelpTextAndTime.TextTime;
                case EnumColor.HelpTextAndTimeTextHelp:
                    return LoadProject.ColorConfiguration.HelpTextAndTime.TextHelp;
                case EnumColor.HelpTextAndTimeTextMessage:
                    return LoadProject.ColorConfiguration.HelpTextAndTime.TextMessage;
                case EnumColor.ContexMenuDefult:
                    return LoadProject.ColorConfiguration.ContexMenu.Defult;
                case EnumColor.ContexMenuCommandTU:
                    return LoadProject.ColorConfiguration.ContexMenu.CommandTU;
                case EnumColor.ContexMenuCommandGID:
                    return LoadProject.ColorConfiguration.ContexMenu.CommandGID;
                case EnumColor.ContexMenuPlanning:
                    return LoadProject.ColorConfiguration.ContexMenu.Planning;
                case EnumColor.CommonTableGrid:
                    return LoadProject.ColorConfiguration.CommonTable.Grid;
                case EnumColor.CommonTableIsSelectFon:
                    return LoadProject.ColorConfiguration.CommonTable.IsSelectFon;
                case EnumColor.CommonTableIsSelectText:
                    return LoadProject.ColorConfiguration.CommonTable.IsSelectText;
                case EnumColor.AutoPilotTableTextDefult:
                    return LoadProject.ColorConfiguration.AutoPilotTable.TextDefult;
                case EnumColor.AutoPilotTableTextHeader:
                    return LoadProject.ColorConfiguration.AutoPilotTable.TextHeader;
                case EnumColor.AutoPilotTableStrokeDefult:
                    return LoadProject.ColorConfiguration.AutoPilotTable.StrokeDefult;
                case EnumColor.AutoPilotTableCommandReceivedFon:
                    return LoadProject.ColorConfiguration.AutoPilotTable.CommandReceivedFon;
                case EnumColor.AutoPilotTableCommandCheckFon:
                    return LoadProject.ColorConfiguration.AutoPilotTable.CommandCheckFon;
                case EnumColor.AutoPilotTableCommandSendFon:
                    return LoadProject.ColorConfiguration.AutoPilotTable.CommandSendFon;
                case EnumColor.AutoPilotTableCommandExecutedFon:
                    return LoadProject.ColorConfiguration.AutoPilotTable.CommandExecutedFon;
                case EnumColor.AutoPilotTableCommandErrorFon:
                    return LoadProject.ColorConfiguration.AutoPilotTable.CommandErrorFon;
                case EnumColor.AutoPilotTableHeaderColumn:
                    return LoadProject.ColorConfiguration.AutoPilotTable.HeaderColumn;
                case EnumColor.TrainTableTextDefult:
                    return LoadProject.ColorConfiguration.TrainTable.TextDefult;
                case EnumColor.TrainTableTextHeader:
                    return LoadProject.ColorConfiguration.TrainTable.TextHeader;
                case EnumColor.TrainTableTextTrainPlan:
                    return LoadProject.ColorConfiguration.TrainTable.TextTrainPlan;
                case EnumColor.TrainTableStrokeDefult:
                    return LoadProject.ColorConfiguration.TrainTable.StrokeDefult;
                case EnumColor.TrainTableNotFixedReferenceInsideFon:
                    return LoadProject.ColorConfiguration.TrainTable.NotFixedReferenceInsideFon;
                case EnumColor.TrainTableNotFixedReferenceOutsideFon:
                    return LoadProject.ColorConfiguration.TrainTable.NotFixedReferenceOutsideFon;
                case EnumColor.TrainTableTrainWithoutReferenceFon:
                    return LoadProject.ColorConfiguration.TrainTable.TrainWithoutReferenceFon;
                case EnumColor.TrainTableTrainWithReferenceFon:
                    return LoadProject.ColorConfiguration.TrainTable.TrainWithReferenceFon;
                case EnumColor.TrainTableHeaderColumn:
                    return LoadProject.ColorConfiguration.TrainTable.HeaderColumn;
                case EnumColor.ManagmentElementTextHelpMessage:
                    return LoadProject.ColorConfiguration.ManagmentElement.TextHelpMessage;
                case EnumColor.ManagmentElementTextSwitchButton:
                    return LoadProject.ColorConfiguration.ManagmentElement.TextSwitchButton;
                case EnumColor.ManagmentElementTextJournal:
                    return LoadProject.ColorConfiguration.ManagmentElement.TextJournal;
                case EnumColor.ManagmentElementStrokeDefult:
                    return LoadProject.ColorConfiguration.ManagmentElement.StrokeDefult;
                case EnumColor.ManagmentElementOkMessageFon:
                    return LoadProject.ColorConfiguration.ManagmentElement.OkMessageFon;
                case EnumColor.ManagmentElementNotMessageFon:
                    return LoadProject.ColorConfiguration.ManagmentElement.NotMessageFon;
                case EnumColor.ManagmentElementOnSwitchFon:
                    return LoadProject.ColorConfiguration.ManagmentElement.OnSwitchFon;
                case EnumColor.ManagmentElementOffSwitchFon:
                    return LoadProject.ColorConfiguration.ManagmentElement.OffSwitchFon;
                case EnumColor.ManagmentElementHelpStringFon:
                    return LoadProject.ColorConfiguration.ManagmentElement.HelpStringFon;
                case EnumColor.ManagmentElementHelpStringStroke:
                    return LoadProject.ColorConfiguration.ManagmentElement.HelpStringStroke;
                case EnumColor.SignalDefultStroke:
                    return LoadProject.ColorConfiguration.Signal.DefultStroke;
                case EnumColor.SignalInstallStroke:
                    return LoadProject.ColorConfiguration.Signal.InstallStroke;
                case EnumColor.SignalNotControlStroke:
                    return LoadProject.ColorConfiguration.Signal.NotControlStroke;
                case EnumColor.SignalSignalFon:
                    return LoadProject.ColorConfiguration.Signal.SignalFon;
                case EnumColor.SignalShuntingFon:
                    return LoadProject.ColorConfiguration.Signal.ShuntingFon;
                case EnumColor.SignalInvitationalOneFon:
                    return LoadProject.ColorConfiguration.Signal.InvitationalOneFon;
                case EnumColor.SignalInvitationalTyFon:
                    return LoadProject.ColorConfiguration.Signal.InvitationalTyFon;
                case EnumColor.SignalDefultFon:
                    return LoadProject.ColorConfiguration.Signal.DefultFon;
                case EnumColor.SignalCloseSignalFon:
                    return LoadProject.ColorConfiguration.Signal.CloseSignalFon;
                case EnumColor.SignalNotControlFon:
                    return LoadProject.ColorConfiguration.Signal.NotControlFon;
            }
            //
            return string.Empty;
        }

        public static void SetNameColor(EnumColor id, string name)
        {
            switch (id)
            {
                case EnumColor.ScreenFon:
                    LoadProject.ColorConfiguration.Screen.Fon = name;break;
                case EnumColor.ScreenArrowCommand:
                    LoadProject.ColorConfiguration.Screen.ArrowCommand = name;break;
                case EnumColor.NameStationTrain:
                    LoadProject.ColorConfiguration.NameStation.Train = name;break;
                case EnumColor.NameStationTrack:
                    LoadProject.ColorConfiguration.NameStation.Track = name;break;
                case EnumColor.AreaStationFon:
                    LoadProject.ColorConfiguration.AreaStation.Fon = name;break;
                case EnumColor.AreaStationStroke:
                    LoadProject.ColorConfiguration.AreaStation.Stroke = name;break;
                case EnumColor.AreaStragePasiveFon:
                    LoadProject.ColorConfiguration.AreaStrage.PasiveFon = name;break;
                case EnumColor.AreaStrageActiveFon:
                    LoadProject.ColorConfiguration.AreaStrage.ActiveFon = name;break;
                case EnumColor.AreaStrageNotControlFon:
                    LoadProject.ColorConfiguration.AreaStrage.NotControlFon = name;break;
                case EnumColor.AreaStrageNormalStroke:
                    LoadProject.ColorConfiguration.AreaStrage.NormalStroke = name;break;
                case EnumColor.AreaStrageNotControlStroke:
                    LoadProject.ColorConfiguration.AreaStrage.NotControlStroke = name;break;
                case EnumColor.AreaStrageNormalText:
                    LoadProject.ColorConfiguration.AreaStrage.NormalText = name;break;
                case EnumColor.AreaStrageNotNormalText:
                    LoadProject.ColorConfiguration.AreaStrage.NotNormalText = name;break;
                case EnumColor.TrackStragePasive:
                    LoadProject.ColorConfiguration.TrackStrage.Pasive = name;break;
                case EnumColor.TrackStrageActive:
                    LoadProject.ColorConfiguration.TrackStrage.Active = name;break;
                case EnumColor.TrackStrageNotControl:
                    LoadProject.ColorConfiguration.TrackStrage.NotControl = name;break;
                case EnumColor.TrackPasiveFon:
                    LoadProject.ColorConfiguration.Track.PasiveFon = name;break;
                case EnumColor.TrackActiveFon:
                    LoadProject.ColorConfiguration.Track.ActiveFon = name;break;
                case EnumColor.TrackNotControlFon:
                    LoadProject.ColorConfiguration.Track.NotControlFon = name;break;
                case EnumColor.TrackFencingFon:
                    LoadProject.ColorConfiguration.Track.FencingFon = name;break;
                case EnumColor.TrackLockingFon:
                    LoadProject.ColorConfiguration.Track.LockingFon = name;break;
                case EnumColor.TrackElectroStroke:
                    LoadProject.ColorConfiguration.Track.ElectroStroke = name;break;
                case EnumColor.TrackDiselStroke:
                    LoadProject.ColorConfiguration.Track.DiselStroke = name;break;
                case EnumColor.TrackNotControlStroke:
                    LoadProject.ColorConfiguration.Track.NotControlStroke = name;break;
                case EnumColor.TrackTrackText:
                    LoadProject.ColorConfiguration.Track.TrackText = name;break;
                case EnumColor.TrackTrainText:
                    LoadProject.ColorConfiguration.Track.TrainText = name;break;
                case EnumColor.TrackVectorText:
                    LoadProject.ColorConfiguration.Track.VectorText = name;break;
                case EnumColor.TrackTrainPlanText:
                    LoadProject.ColorConfiguration.Track.TrainPlanText = name;break;
                case EnumColor.MoveFaultStroke:
                    LoadProject.ColorConfiguration.Move.FaultStroke = name;break;
                case EnumColor.MoveAccidentStroke:
                    LoadProject.ColorConfiguration.Move.AccidentStroke = name;break;
                case EnumColor.MoveNotControlStroke:
                    LoadProject.ColorConfiguration.Move.NotControlStroke = name;break;
                case EnumColor.MoveDefultStroke:
                    LoadProject.ColorConfiguration.Move.DefultStroke = name;break;
                case EnumColor.MoveDefultFon:
                    LoadProject.ColorConfiguration.Move.DefultFon = name;break;
                case EnumColor.MoveNotControlFon:
                    LoadProject.ColorConfiguration.Move.NotControlFon = name;break;
                case EnumColor.MoveButtonClosedFon:
                    LoadProject.ColorConfiguration.Move.ButtonClosedFon = name;break;
                case EnumColor.MoveAutoClosedFon:
                    LoadProject.ColorConfiguration.Move.AutoClosedFon = name;break;
                case EnumColor.ControlObjectFaultStroke:
                    LoadProject.ColorConfiguration.ControlObject.FaultStroke = name;break;
                case EnumColor.ControlObjectNotControlStroke:
                    LoadProject.ColorConfiguration.ControlObject.NotControlStroke = name;break;
                case EnumColor.ControlObjectDefultStroke:
                    LoadProject.ColorConfiguration.ControlObject.DefultStroke = name;break;
                case EnumColor.ControlObjectPlayFon:
                    LoadProject.ColorConfiguration.ControlObject.PlayFon = name;break;
                case EnumColor.ControlObjectNotControlFon:
                    LoadProject.ColorConfiguration.ControlObject.NotControlFon = name;break;
                case EnumColor.ControlObjectDefultFon:
                    LoadProject.ColorConfiguration.ControlObject.DefultFon = name;break;
                case EnumColor.ButtonStationReserveControlFon:
                    LoadProject.ColorConfiguration.ButtonStation.ReserveControlFon = name;break;
                case EnumColor.ButtonStationSesonControlFon:
                    LoadProject.ColorConfiguration.ButtonStation.SesonControlFon = name;break;
                case EnumColor.ButtonStationDispatcherControlFon:
                    LoadProject.ColorConfiguration.ButtonStation.DispatcherControlFon = name;break;
                case EnumColor.ButtonStationAutoDispatcherControlFon:
                    LoadProject.ColorConfiguration.ButtonStation.AutoDispatcherControlFon = name;break;
                case EnumColor.ButtonStationAutonomousСontrolFon:
                    LoadProject.ColorConfiguration.ButtonStation.AutonomousСontrolFon = name;break;
                case EnumColor.ButtonStationNotDispatcherControlFon:
                    LoadProject.ColorConfiguration.ButtonStation.NotDispatcherControlFon = name;break;
                case EnumColor.ButtonStationNotControlFon:
                    LoadProject.ColorConfiguration.ButtonStation.NotControlFon = name;break;
                case EnumColor.ButtonStationDefultFon:
                    LoadProject.ColorConfiguration.ButtonStation.DefultFon = name;break;
                case EnumColor.ButtonStationFireControlFon:
                    LoadProject.ColorConfiguration.ButtonStation.FireControlFon = name;break;
                case EnumColor.ButtonStationFaultStroke:
                    LoadProject.ColorConfiguration.ButtonStation.FaultStroke = name;break;
                case EnumColor.ButtonStationAccidentStroke:
                    LoadProject.ColorConfiguration.ButtonStation.AccidentStroke = name;break;
                case EnumColor.ButtonStationNotControlStroke:
                    LoadProject.ColorConfiguration.ButtonStation.NotControlStroke = name;break;
                case EnumColor.ButtonStationDefultStroke:
                    LoadProject.ColorConfiguration.ButtonStation.DefultStroke = name;break;
                case EnumColor.ActiveLineActiveStroke:
                    LoadProject.ColorConfiguration.ActiveLine.ActiveStroke = name;break;
                case EnumColor.ActiveLinePasiveStroke:
                    LoadProject.ColorConfiguration.ActiveLine.PasiveStroke = name; break;
                case EnumColor.ActiveLineNotControlStroke:
                    LoadProject.ColorConfiguration.ActiveLine.NotControlStroke = name;break;
                case EnumColor.ActiveLineLocingStroke:
                    LoadProject.ColorConfiguration.ActiveLine.LocingStroke = name; break;
                case EnumColor.ActiveLineFencingStroke:
                    LoadProject.ColorConfiguration.ActiveLine.FencingStroke = name; break;
                case EnumColor.ActiveLinePassegeStroke:
                    LoadProject.ColorConfiguration.ActiveLine.PassegeStroke = name; break;
                case EnumColor.ActiveLineСuttingOneStroke:
                    LoadProject.ColorConfiguration.ActiveLine.СuttingOneStroke = name; break;
                case EnumColor.ActiveLineСuttingTyStroke:
                    LoadProject.ColorConfiguration.ActiveLine.СuttingTyStroke = name; break;
                case EnumColor.DirectionActiveStrageFon:
                    LoadProject.ColorConfiguration.Direction.ActiveStrageFon = name; break;
                case EnumColor.DirectionPasiveStrageFon:
                    LoadProject.ColorConfiguration.Direction.PasiveStrageFon = name; break;
                case EnumColor.DirectionNotControlStrageFon:
                    LoadProject.ColorConfiguration.Direction.NotControlStrageFon = name; break;
                case EnumColor.DirectionDepartureDirectonFon:
                    LoadProject.ColorConfiguration.Direction.DepartureDirectonFon = name; break;
                case EnumColor.DirectionWaitingDepartureDirectonFon:
                    LoadProject.ColorConfiguration.Direction.WaitingDepartureDirectonFon = name; break;
                case EnumColor.DirectionOKDepartureDirectonFon:
                    LoadProject.ColorConfiguration.Direction.OKDepartureDirectonFon = name; break;
                case EnumColor.DirectionNotControlStroke:
                    LoadProject.ColorConfiguration.Direction.NotControlStroke = name; break;
                case EnumColor.DirectionDefultStroke:
                    LoadProject.ColorConfiguration.Direction.DefultStroke = name; break;
                case EnumColor.RouteSignalDefultStroke:
                    LoadProject.ColorConfiguration.RouteSignal.DefultStroke = name; break;
                case EnumColor.RouteSignalReceivedOneStroke:
                    LoadProject.ColorConfiguration.RouteSignal.ReceivedOneStroke = name; break;
                case EnumColor.RouteSignalReceivedTyStroke:
                    LoadProject.ColorConfiguration.RouteSignal.ReceivedTyStroke = name; break;
                case EnumColor.RouteSignalCheckRouteStroke:
                    LoadProject.ColorConfiguration.RouteSignal.CheckRouteStroke = name; break;
                case EnumColor.RouteSignalWaitInstallOneStroke:
                    LoadProject.ColorConfiguration.RouteSignal.WaitInstallOneStroke = name; break;
                case EnumColor.RouteSignalWaitInstallTyStroke:
                    LoadProject.ColorConfiguration.RouteSignal.WaitInstallTyStroke = name; break;
                case EnumColor.RouteSignalFaultStroke:
                    LoadProject.ColorConfiguration.RouteSignal.FaultStroke = name; break;
                case EnumColor.RouteSignalNotControlStroke:
                    LoadProject.ColorConfiguration.RouteSignal.NotControlStroke = name; break;
                case EnumColor.RouteSignalActiceFon:
                    LoadProject.ColorConfiguration.RouteSignal.ActiceFon = name; break;
                case EnumColor.RouteSignalPasiveFon:
                    LoadProject.ColorConfiguration.RouteSignal.PasiveFon = name; break;
                case EnumColor.RouteSignalNotControlFon:
                    LoadProject.ColorConfiguration.RouteSignal.NotControlFon = name; break;
                case EnumColor.RouteSignalLockingFon:
                    LoadProject.ColorConfiguration.RouteSignal.LockingFon = name; break;
                case EnumColor.RouteSignalFencingFon:
                    LoadProject.ColorConfiguration.RouteSignal.FencingFon = name; break;
                case EnumColor.RouteSignalPassageFon:
                    LoadProject.ColorConfiguration.RouteSignal.PassageFon = name; break;
                case EnumColor.RouteSignalInvitationalOneFon:
                    LoadProject.ColorConfiguration.RouteSignal.InvitationalOneFon = name; break;
                case EnumColor.RouteSignalInvitationalTyFon:
                    LoadProject.ColorConfiguration.RouteSignal.InvitationalTyFon = name; break;
                case EnumColor.RouteSignalInstallOneStroke:
                    LoadProject.ColorConfiguration.RouteSignal.InstallOneStroke = name; break;
                case EnumColor.RouteSignalInstallTyStroke:
                    LoadProject.ColorConfiguration.RouteSignal.InstallTyStroke = name; break;
                case EnumColor.RouteSignalShuntingFon:
                    LoadProject.ColorConfiguration.RouteSignal.ShuntingFon = name; break;
                case EnumColor.RouteSignalSignalFon:
                    LoadProject.ColorConfiguration.RouteSignal.SignalFon = name; break;
                case EnumColor.HelpElementFaultStroke:
                    LoadProject.ColorConfiguration.HelpElement.FaultStroke = name; break;
                case EnumColor.HelpElementAccidentFon:
                    LoadProject.ColorConfiguration.HelpElement.AccidentFon = name; break;
                case EnumColor.HelpElementAccidentDGAStroke:
                    LoadProject.ColorConfiguration.HelpElement.AccidentDGAStroke = name; break;
                case EnumColor.HelpElementNotControlFon:
                    LoadProject.ColorConfiguration.HelpElement.NotControlFon = name; break;
                case EnumColor.HelpElementNotControlStroke:
                    LoadProject.ColorConfiguration.HelpElement.NotControlStroke = name; break;
                case EnumColor.HelpElementDefultFon:
                    LoadProject.ColorConfiguration.HelpElement.DefultFon = name; break;
                case EnumColor.HelpElementDefultStroke:
                    LoadProject.ColorConfiguration.HelpElement.DefultStroke = name; break;
                case EnumColor.HelpElementOffWeightFon:
                    LoadProject.ColorConfiguration.HelpElement.OffWeightFon = name; break;
                case EnumColor.HelpElementOnWeightFon:
                    LoadProject.ColorConfiguration.HelpElement.OnWeightFon = name; break;
                case EnumColor.HelpElementText:
                    LoadProject.ColorConfiguration.HelpElement.Text = name; break;
                case EnumColor.HelpTextAndTimeTimeFon:
                    LoadProject.ColorConfiguration.HelpTextAndTime.TimeFon = name; break;
                case EnumColor.HelpTextAndTimeStrokeTime:
                    LoadProject.ColorConfiguration.HelpTextAndTime.StrokeTime = name; break;
                case EnumColor.HelpTextAndTimeTextTime:
                    LoadProject.ColorConfiguration.HelpTextAndTime.TextTime = name; break;
                case EnumColor.HelpTextAndTimeTextHelp:
                    LoadProject.ColorConfiguration.HelpTextAndTime.TextHelp = name; break;
                case EnumColor.HelpTextAndTimeTextMessage:
                    LoadProject.ColorConfiguration.HelpTextAndTime.TextMessage = name; break;
                case EnumColor.ContexMenuDefult:
                    LoadProject.ColorConfiguration.ContexMenu.Defult = name; break;
                case EnumColor.ContexMenuCommandTU:
                    LoadProject.ColorConfiguration.ContexMenu.CommandTU = name; break;
                case EnumColor.ContexMenuCommandGID:
                    LoadProject.ColorConfiguration.ContexMenu.CommandGID = name; break;
                case EnumColor.ContexMenuPlanning:
                    LoadProject.ColorConfiguration.ContexMenu.Planning = name; break;
                case EnumColor.CommonTableGrid:
                    LoadProject.ColorConfiguration.CommonTable.Grid = name; break;
                case EnumColor.CommonTableIsSelectFon:
                    LoadProject.ColorConfiguration.CommonTable.IsSelectFon = name; break;
                case EnumColor.CommonTableIsSelectText:
                    LoadProject.ColorConfiguration.CommonTable.IsSelectText = name; break;
                case EnumColor.AutoPilotTableTextDefult:
                    LoadProject.ColorConfiguration.AutoPilotTable.TextDefult = name; break;
                case EnumColor.AutoPilotTableTextHeader:
                    LoadProject.ColorConfiguration.AutoPilotTable.TextHeader = name; break;
                case EnumColor.AutoPilotTableStrokeDefult:
                    LoadProject.ColorConfiguration.AutoPilotTable.StrokeDefult = name; break;
                case EnumColor.AutoPilotTableCommandReceivedFon:
                    LoadProject.ColorConfiguration.AutoPilotTable.CommandReceivedFon = name; break;
                case EnumColor.AutoPilotTableCommandCheckFon:
                    LoadProject.ColorConfiguration.AutoPilotTable.CommandCheckFon = name; break;
                case EnumColor.AutoPilotTableCommandSendFon:
                    LoadProject.ColorConfiguration.AutoPilotTable.CommandSendFon = name; break;
                case EnumColor.AutoPilotTableCommandExecutedFon:
                    LoadProject.ColorConfiguration.AutoPilotTable.CommandExecutedFon = name; break;
                case EnumColor.AutoPilotTableCommandErrorFon:
                    LoadProject.ColorConfiguration.AutoPilotTable.CommandErrorFon = name; break;
                case EnumColor.AutoPilotTableHeaderColumn:
                    LoadProject.ColorConfiguration.AutoPilotTable.HeaderColumn = name; break;
                case EnumColor.TrainTableTextDefult:
                    LoadProject.ColorConfiguration.TrainTable.TextDefult = name; break;
                case EnumColor.TrainTableTextHeader:
                    LoadProject.ColorConfiguration.TrainTable.TextHeader = name; break;
                case EnumColor.TrainTableTextTrainPlan:
                    LoadProject.ColorConfiguration.TrainTable.TextTrainPlan = name; break;
                case EnumColor.TrainTableStrokeDefult:
                    LoadProject.ColorConfiguration.TrainTable.StrokeDefult = name; break;
                case EnumColor.TrainTableNotFixedReferenceInsideFon:
                    LoadProject.ColorConfiguration.TrainTable.NotFixedReferenceInsideFon = name; break;
                case EnumColor.TrainTableNotFixedReferenceOutsideFon:
                    LoadProject.ColorConfiguration.TrainTable.NotFixedReferenceOutsideFon = name; break;
                case EnumColor.TrainTableTrainWithoutReferenceFon:
                    LoadProject.ColorConfiguration.TrainTable.TrainWithoutReferenceFon = name; break;
                case EnumColor.TrainTableTrainWithReferenceFon:
                    LoadProject.ColorConfiguration.TrainTable.TrainWithReferenceFon = name; break;
                case EnumColor.TrainTableHeaderColumn:
                    LoadProject.ColorConfiguration.TrainTable.HeaderColumn = name; break;
                case EnumColor.ManagmentElementTextHelpMessage:
                    LoadProject.ColorConfiguration.ManagmentElement.TextHelpMessage = name; break;
                case EnumColor.ManagmentElementTextSwitchButton:
                    LoadProject.ColorConfiguration.ManagmentElement.TextSwitchButton = name; break;
                case EnumColor.ManagmentElementTextJournal:
                    LoadProject.ColorConfiguration.ManagmentElement.TextJournal = name; break;
                case EnumColor.ManagmentElementStrokeDefult:
                    LoadProject.ColorConfiguration.ManagmentElement.StrokeDefult = name; break;
                case EnumColor.ManagmentElementOkMessageFon:
                    LoadProject.ColorConfiguration.ManagmentElement.OkMessageFon = name; break;
                case EnumColor.ManagmentElementNotMessageFon:
                    LoadProject.ColorConfiguration.ManagmentElement.NotMessageFon = name; break;
                case EnumColor.ManagmentElementOnSwitchFon:
                    LoadProject.ColorConfiguration.ManagmentElement.OnSwitchFon = name; break;
                case EnumColor.ManagmentElementOffSwitchFon:
                    LoadProject.ColorConfiguration.ManagmentElement.OffSwitchFon = name; break;
                case EnumColor.ManagmentElementHelpStringFon:
                    LoadProject.ColorConfiguration.ManagmentElement.HelpStringFon = name; break;
                case EnumColor.ManagmentElementHelpStringStroke:
                    LoadProject.ColorConfiguration.ManagmentElement.HelpStringStroke = name; break;
                case EnumColor.SignalDefultStroke:
                    LoadProject.ColorConfiguration.Signal.DefultStroke = name; break;
                case EnumColor.SignalInstallStroke:
                    LoadProject.ColorConfiguration.Signal.InstallStroke = name; break;
                case EnumColor.SignalNotControlStroke:
                    LoadProject.ColorConfiguration.Signal.NotControlStroke = name; break;
                case EnumColor.SignalSignalFon:
                    LoadProject.ColorConfiguration.Signal.SignalFon = name; break;
                case EnumColor.SignalShuntingFon:
                    LoadProject.ColorConfiguration.Signal.ShuntingFon = name; break;
                case EnumColor.SignalInvitationalOneFon:
                    LoadProject.ColorConfiguration.Signal.InvitationalOneFon = name; break;
                case EnumColor.SignalInvitationalTyFon:
                    LoadProject.ColorConfiguration.Signal.InvitationalTyFon = name; break;
                case EnumColor.SignalDefultFon:
                    LoadProject.ColorConfiguration.Signal.DefultFon = name; break;
                case EnumColor.SignalCloseSignalFon:
                    LoadProject.ColorConfiguration.Signal.CloseSignalFon = name; break;
                case EnumColor.SignalNotControlFon:
                    LoadProject.ColorConfiguration.Signal.NotControlFon = name; break;
            }
        }

        public static void SetColor(EnumColor id, Color color)
        {
            switch (id)
            {
                case EnumColor.ScreenFon:
                    SettingsWindow.m_colorfon = new SolidColorBrush(color);
                    break;
                case EnumColor.NameStationTrain:
                    NameStation._color_train = new SolidColorBrush(color);
                    break;
                case EnumColor.NameStationTrack:
                    NameStation._color_track = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStationFon:
                    RamkaStation._colorfill = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStationStroke:
                    RamkaStation._colorstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStragePasiveFon:
                    NumberTrainRamka._color_pasiv = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStrageActiveFon:
                    NumberTrainRamka._coloractiv = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStrageNotControlFon:
                    NumberTrainRamka._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStrageNormalStroke:
                    NumberTrainRamka._color_ramka = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStrageNotControlStroke:
                    NumberTrainRamka._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStrageNormalText:
                    NumberTrainRamka._color_text_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.AreaStrageNotNormalText:
                    NumberTrainRamka._color_text_notnormal = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackStragePasive:
                    BlockSection._colorpassiv = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackStrageActive:
                    BlockSection._coloractiv = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackStrageNotControl:
                    BlockSection._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackPasiveFon:
                    StationPath._colorpassiv = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackActiveFon:
                    StationPath._coloractiv = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackNotControlFon:
                    StationPath._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackFencingFon:
                    StationPath._colorfencing = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackLockingFon:
                    StationPath._color_loking = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackElectroStroke:
                    StationPath._colorelectric_traction = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackDiselStroke:
                    StationPath.m_colordiesel_traction = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackNotControlStroke:
                    StationPath._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackTrackText:
                    StationPath._color_path = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackTrainText:
                    StationPath._color_train = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackVectorText:
                    StationPath._color_vertor_train = new SolidColorBrush(color);
                    break;
                case EnumColor.TrackTrainPlanText:
                    StationPath._color_train_plan = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveFaultStroke:
                    Moves._color_faultmove = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveAccidentStroke:
                    Moves._color_accident = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveNotControlStroke:
                    Moves._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveDefultStroke:
                    Moves._color_moveopen = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveDefultFon:
                    Moves._color_fon_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveNotControlFon:
                    Moves._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveButtonClosedFon:
                    Moves._color_closingmove_button = new SolidColorBrush(color);
                    break;
                case EnumColor.MoveAutoClosedFon:
                    Moves._color_closingmove_auto = new SolidColorBrush(color);
                    break;
                case EnumColor.ControlObjectFaultStroke:
                    ControlObject._color_fault = new SolidColorBrush(color);
                    break;
                case EnumColor.ControlObjectNotControlStroke:
                    ControlObject._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.ControlObjectDefultStroke:
                    ControlObject._color_normal = new SolidColorBrush(color);
                    break;
                case EnumColor.ControlObjectPlayFon:
                    ControlObject._color_break = new SolidColorBrush(color);
                    break;
                case EnumColor.ControlObjectNotControlFon:
                    ControlObject._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.ControlObjectDefultFon:
                    ControlObject._color_fon_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationReserveControlFon:
                    ButtonStation._color_reserve_control = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationSesonControlFon:
                    ButtonStation._color_sesoncontol = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationDispatcherControlFon:
                    ButtonStation._color_dispatcher = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationAutoDispatcherControlFon:
                    ButtonStation._color_auto_dispatcher = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationAutonomousСontrolFon:
                    ButtonStation._color_autonomous_control = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationNotDispatcherControlFon:
                    ButtonStation._color_not_dispatcher = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationNotControlFon:
                    ButtonStation._color_notlink = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationDefultFon:
                    ButtonStation._color_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationFireControlFon:
                    ButtonStation._color_fire = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationFaultStroke:
                    ButtonStation._color_fault = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationAccidentStroke:
                    ButtonStation._color_accident = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationNotControlStroke:
                    ButtonStation._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.ButtonStationDefultStroke:
                    ButtonStation._color_stroke_normal = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLineActiveStroke:
                    LineHelp._color_active = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLinePasiveStroke:
                    LineHelp._color_pasive = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLineNotControlStroke:
                    LineHelp._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLineLocingStroke:
                    LineHelp._colorlocking = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLineFencingStroke:
                    LineHelp._colorfencing = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLinePassegeStroke:
                    LineHelp._color_passage = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLineСuttingOneStroke:
                    LineHelp._color_cutting_one = new SolidColorBrush(color);
                    break;
                case EnumColor.ActiveLineСuttingTyStroke:
                    LineHelp._color_cutting_ty = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionActiveStrageFon:
                //    CenterDirection._color_occupation = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionPasiveStrageFon:
                    Direction._color_pasive = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionNotControlStrageFon:
                    Direction._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionDepartureDirectonFon:
                    Direction._color_departure = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionWaitingDepartureDirectonFon:
                    Direction._color_wait_departure = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionOKDepartureDirectonFon:
                    Direction._color_ok_departure = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionNotControlStroke:
                    Direction._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.DirectionDefultStroke:
                    Direction._color_ramka = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalDefultStroke:
                    RouteSignal._color_ramka_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalReceivedOneStroke:
                    RouteSignal._color_command_received_one = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalReceivedTyStroke:
                    RouteSignal._color_command_received_ty = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalCheckRouteStroke:
                    RouteSignal._color_check_route = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalWaitInstallOneStroke:
                    RouteSignal._color_wait_install_one = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalWaitInstallTyStroke:
                    RouteSignal._color_wait_install_ty = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalFaultStroke:
                    RouteSignal._color_fault = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalNotControlStroke:
                    RouteSignal._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalActiceFon:
                    RouteSignal._color_active = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalPasiveFon:
                    RouteSignal._color_pasive = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalNotControlFon:
                    RouteSignal._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalLockingFon:
                    RouteSignal._color_locing = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalFencingFon:
                    RouteSignal._color_fencing = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalPassageFon:
                    RouteSignal._color_passage = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalInvitationalOneFon:
                    RouteSignal._color_invitational_one = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalInvitationalTyFon:
                    RouteSignal._color_invitational_ty = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalInstallOneStroke:
                    RouteSignal._color_install_route_one = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalInstallTyStroke:
                    RouteSignal._color_install_route_ty = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalShuntingFon:
                    RouteSignal._color_shunting = new SolidColorBrush(color);
                    break;
                case EnumColor.RouteSignalSignalFon:
                    RouteSignal._color_open = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementAccidentFon:
                    HelpElement._colorRed = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementAccidentDGAStroke:
                    HelpElement._colorYellow = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementNotControlFon:
                    HelpElement._colornotcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementNotControlStroke:
                    HelpElement._colornotcontrolstroke = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementDefultFon:
                    HelpElement._color_fon_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementDefultStroke:
                    HelpElement._color_stroke_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementOnWeightFon:
                    HelpElement._colorWhite = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpElementText:
                    HelpElement._colortext = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpTextAndTimeTimeFon:
                    TimeElement._color_fon = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpTextAndTimeStrokeTime:
                    TimeElement._color_stroke = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpTextAndTimeTextTime:
                    TimeElement._color_text = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpTextAndTimeTextHelp:
                    TextHelp._color_text = new SolidColorBrush(color);
                    break;
                case EnumColor.HelpTextAndTimeTextMessage:
                    SettingsWindow.m_color_text_message = new SolidColorBrush(color);
                    break;
                case EnumColor.CommonTableGrid:
                    ColorCommonTable.Grid = new SolidColorBrush(color);
                    break;
                case EnumColor.CommonTableIsSelectFon:
                    ColorCommonTable.IsSelectFon = new SolidColorBrush(color);
                    break;
                case EnumColor.CommonTableIsSelectText:
                    ColorCommonTable.IsSelectText = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableTextDefult:
                    ColorStateTableAutoPilot.Text = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableTextHeader:
                    ColorStateTableAutoPilot.TextHeader = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableStrokeDefult:
                    ColorStateTableAutoPilot.Stroke = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableCommandReceivedFon:
                    ColorStateTableAutoPilot.ColorReceived = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableCommandCheckFon:
                    ColorStateTableAutoPilot.ColorCheck = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableCommandSendFon:
                    ColorStateTableAutoPilot.ColorSend = new SolidColorBrush(color);;
                    break;
                case EnumColor.AutoPilotTableCommandExecutedFon:
                    ColorStateTableAutoPilot.ColorExecuted = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableCommandErrorFon:
                    ColorStateTableAutoPilot.ColorError = new SolidColorBrush(color);
                    break;
                case EnumColor.AutoPilotTableHeaderColumn:
                    ColorStateTableAutoPilot.ColumnHeader = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableTextDefult:
                    ColorStateTableTrain.Text = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableTextHeader:
                    ColorStateTableTrain.TextHeader = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableTextTrainPlan:
                    ColorStateTableTrain.TextPlan = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableStrokeDefult:
                    ColorStateTableTrain.StrokeDefult = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableNotFixedReferenceInsideFon:
                    ColorStateTableTrain.NotFixedReferenceInsideFon = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableNotFixedReferenceOutsideFon:
                    ColorStateTableTrain.NotFixedReferenceOutsideFon = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableTrainWithoutReferenceFon:
                    ColorStateTableTrain.TrainWithoutReferenceFon = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableTrainWithReferenceFon:
                    ColorStateTableTrain.TrainWithReferenceFon = new SolidColorBrush(color);
                    break;
                case EnumColor.TrainTableHeaderColumn:
                    ColorStateTableTrain.ColumnHeader = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementTextHelpMessage:
                    CommandButton._color_text_help = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementTextSwitchButton:
                    CommandButton._color_text_switch_button = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementTextJournal:
                    CommandButton._color_text_journal = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementStrokeDefult:
                    CommandButton._color_stroke = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementOkMessageFon:
                    CommandButton._color_yes_message = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementNotMessageFon:
                    CommandButton._color_no_message = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementOnSwitchFon:
                    CommandButton._coloractiv = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementOffSwitchFon:
                    CommandButton._colorpasiv = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementHelpStringFon:
                    CommandButton._color_helpstring_fon = new SolidColorBrush(color);
                    break;
                case EnumColor.ManagmentElementHelpStringStroke:
                    CommandButton._color_helpstring_stroke = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalDefultStroke:
                    LightTrain.m_color_stroke = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalInstallStroke:
                    LightTrain.m_color_install = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalNotControlStroke:
                    LightTrain.m_color_stroke_notcontrol = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalSignalFon:
                    LightTrain.m_color_open = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalShuntingFon:
                    LightTrain.m_color_shunting = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalInvitationalOneFon:
                    LightTrain.m_color_invitation_one = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalInvitationalTyFon:
                    LightTrain.m_color_invitation_ty = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalDefultFon:
                    LightTrain.m_color_defult = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalCloseSignalFon:
                    LightTrain.m_color_close = new SolidColorBrush(color);
                    break;
                case EnumColor.SignalNotControlFon:
                    LightTrain.m_color_fon_notcontrol = new SolidColorBrush(color);
                    break;
            }
        }
    }
}
