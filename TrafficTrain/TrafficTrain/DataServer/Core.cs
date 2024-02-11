using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ARM_SHN;
using RW.KTC.ORPO.Berezina.DataStream.Common;
using RW.KTC.ORPO.Berezina.DataStream.Common.Interfaces;
using RW.KTC.ORPO.Berezina.Configuration;
using RW.KTC.ORPO.Berezina.Configuration.Records;
using RW.KTC.ORPO.Berezina.Server.Core.BusinessObjects;
using RW.KTC.ORPO.Berezina.Common.Interfaces;
using RW.KTC.ORPO.Berezina.Server.Common.Interfaces;

namespace ARM_SHN.DataServer
{
    public class Core
    {
        private const string StationsPathKey = "StationsRootFolder";
        private static readonly IDictionary<int, IDictionary<int, IServerTable>> stations = new Dictionary<int, IDictionary<int, IServerTable>>();

        public static IDictionary<int, IDictionary<int, IServerTable>> Stations
        {
            get
            {
                return stations;
            }
        }

        private static DataSource dataSource;

        public static void Load(string conf)
        {
            IList<string> errors = new List<string>();
            ServerConfiguration.Initialize(conf);
            //
            try
            {
                FillDataSources(errors);
                FillStations(errors, conf);
            }
            catch (Exception e)
            {
                errors.Add(string.Format("{0}", e));
            }
            finally
            {
                PrintErrors(errors);
            }
        }

        public static void Start()
        {
            try
            {
                if (dataSource != null)
                    dataSource.Start();
            }
            catch (Exception e)
            {
                LoadProject.Log.Error(e);
            }
        }

        public static void Stop()
        {
            try
            {
                if (dataSource != null)
                    dataSource.Stop();
            }
            catch (Exception e)
            {
                LoadProject.Log.Error(e);
            }
        }

        private static void PrintErrors(IList<string> errors)
        {
            foreach (string str in errors)
            {
                LoadProject.Log.Error(str);
            }
        }

        private static void FillDataSources(IList<string> errors)
        {
            if (ServerConfiguration.Instance.DataSources != null && ServerConfiguration.Instance.DataSources.Length > 0)
            {
                foreach (DataSourceRecord dataSourceRecord in ServerConfiguration.Instance.DataSources)
                {
                    dataSource = new DataSource(FramedDataStreamPluginManager.FrameDataStream(DataStreamPluginWrapper.Instance[dataSourceRecord.DataStream.Type].CreateClientStream(
                        dataSourceRecord.DataStream, null), "SHF"), dataSourceRecord.RequestTimeout);
                    break;
                }
            }
            else
                errors.Add("В конфигурации не описан источник инфрмации !!!");
        }

        private static void FillStations(IList<string> errors, string conf)
        {
            foreach (StationRecord stationRecord in ServerConfiguration.Instance.Stations)
            {
                foreach (TableRecord tableRecord in stationRecord.Tables)
                {
                    string stationsPath = string.Empty;
                    try
                    {
                        stationsPath = ServerConfiguration.Instance[StationsPathKey];
                    }
                    catch
                    {
                        errors.Add("Отсутвует путь для записи конфигурационного файла StationsRootFolder");
                    }
                    //
                    if (!Path.IsPathRooted(stationsPath))
                    {
                        string configDirectory =
                            Path.GetDirectoryName(Path.GetFullPath(conf));
                        stationsPath = Path.Combine(configDirectory, stationsPath);
                    }
                    //string itemsFilePath =
                    //    Path.Combine(
                    //        Path.Combine(Path.Combine(stationsPath, stationRecord.Code.ToString()),
                    //                     string.Format("{0}.{1}", tableRecord.Type.ToLower(), tableRecord.Id)), "items");
                    string itemsDirPath =
                        /*    Path.Combine(
    */                            Path.Combine(Path.Combine(stationsPath, stationRecord.Code.ToString(/*"D6"*/)),
                                        string.Format("{0}.{1}", tableRecord.Type.ToLower(), tableRecord.Id));//, tableRecord.Type);
                    var table = ServerTablePluginWrapper.Instance[tableRecord.Type].Create(tableRecord);
                    table.Load(tableRecord.Bindings, itemsDirPath, errors, null, null,  stationRecord.Code, tableRecord);
                    if (!stations.ContainsKey(stationRecord.Code))
                        stations.Add(stationRecord.Code, new Dictionary<int, IServerTable>());
                    else
                        errors.Add(string.Format("Станция под номером - {0} повторяется.", stationRecord.Code));
                    //
                    if (!stations[stationRecord.Code].ContainsKey(table.Id))
                        stations[stationRecord.Code].Add(table.Id, table);
                    else
                        errors.Add(string.Format("Для станции под номером - {0}, таблица с id = {1} повторяется.", stationRecord.Code, table.Id));
                }
            }
        }
    }
}
