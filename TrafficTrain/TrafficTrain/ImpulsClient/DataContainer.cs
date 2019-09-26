using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace TrafficTrain.Impulsesver.Client
{
	public class DataContainer
	{
		private Dictionary<int, Station> m_stations;

        /// <summary>
        /// логирование
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(DataContainer));
		
		public DataContainer()
		{
			m_stations = new Dictionary<int, Station>();
		}

		public Dictionary<int, Station> Stations
		{
			get
			{
				return m_stations;
			}
		}

		public bool LoadStationsData(StationRecord[] inp_station_records, string tables_path)
		{
            foreach (StationRecord st_config in inp_station_records)
 			{
                Station st = new Station(st_config.Name, st_config.Code);
 				ImpulseRecord[] ts_impulses = null;
				ImpulseRecord[] tu_impulses = null;
				try
				{
                    TableLoader.GetStdImpulses(tables_path, st.Code, false, out ts_impulses);
					TableLoader.GetStdImpulses(tables_path, st.Code, true, out tu_impulses);
                }
				catch(SystemException ex)
				{
                    log.Error(string.Format("TI for {0} not found. {1}", st.Code, ex.Message));
					continue;
				}
				if(ts_impulses == null)
				{
					log.Error(string.Format("TI for {0} not found.", st.Code));
					continue;
				}
				
				try
				{
					st.LoadData(ts_impulses, tu_impulses);
				}
				catch(Exception ex)
				{
					log.Error(string.Format("Can't load TI for {0}. {1}", st.Code, ex.Message));
					continue;
				}
 				try
 				{
					m_stations.Add(st.Code, st);
				}
				catch(ArgumentException ex)
				{
					log.Error(string.Format(ex.Message));
					continue;
				}
			}
			if(m_stations.Count == 0)
				return false;
			else
				return true;
		}
	}
}
