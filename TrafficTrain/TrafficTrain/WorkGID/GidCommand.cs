using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using FirebirdSql.Data.Firebird;
using log4net;

namespace TrafficTrain
{
    class GidCommand
    {
        #region Variable

        /// <summary>
        /// логирование
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(GidCommand));
        /// название команды
        /// </summary>
        private const string InsertCommandText
         = "ADD_NEW_COMMAND";
        /// <summary>
        /// строка подключения
        /// </summary>
        private string _connectionString = @"Dialect=3;Database=10.20.47.33:C:\Неман\Могилев-Езерище\Events\GID.GDB;User Id=NEMAN;Password=NEMAN";
        //список параметров
        private FbParameter _command;
        private FbParameter _com_time;
        private FbParameter _train_idnh1;
        private FbParameter _norm_idnh1;
        private FbParameter _train_numh1;
        private FbParameter _j_st_formh1;
        private FbParameter _j_st_desth1;
        private FbParameter _j_sost_numh1;
        private FbParameter _dop_proph1;
        private FbParameter _rezervh1;
        //
        private FbParameter _st_in_zoneh1;
        private FbParameter _st_out_zoneh1;
        private FbParameter _train_idnh2;
        private FbParameter _norm_idnh2;
        private FbParameter _train_numh2;
        private FbParameter _j_st_formh2;
        private FbParameter _j_st_desth2;
        private FbParameter _j_sost_numh2;
        private FbParameter _dop_proph2;
        private FbParameter _rezervh2;
        //
        private FbParameter _st_in_zoneh2;
        private FbParameter _st_out_zoneh2;
        private FbParameter _train_idne1;
        private FbParameter _ev_type1;
        private FbParameter _ev_time1;
        private FbParameter _ev_statione1;
        private FbParameter _ev_axise1;
        private FbParameter _ev_ndoe1;
        private FbParameter _ev_ne_statione1;
        private FbParameter _ev_res_idne1;
        //
        private FbParameter _ev_flage1;
        private FbParameter _train_idne2;
        private FbParameter _ev_type2;
        private FbParameter _ev_time2;
        private FbParameter _ev_statione2;
        private FbParameter _ev_axise2;
        private FbParameter _ev_ndoe2;
        private FbParameter _ev_ne_statione2;
        private FbParameter _ev_res_idne2;
        private FbParameter _ev_flage2;

        #endregion

        public GidCommand()
        {
            //Конфигурируем логер
            log4net.Config.XmlConfigurator.Configure();
            //
            if (ConfigurationManager.AppSettings["connectionStringGID"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["connectionStringGID"]))
                _connectionString = ConfigurationManager.AppSettings["connectionStringGID"];
            else log.Error(string.Format("Заполните в файле конфигурации строку подключения к базе ГИД , обозначенную {0}", "<<connectionStringGID>>"));
        }

        /// <summary>
        /// отправка команды ГИД
        /// </summary>
        /// <param name="command">номер команды</param>
        /// <param name="id_train">идентификатор поезда</param>
        /// <param name="numberstation">номер станции</param>
        /// <param name="path">название пути</param>
        /// <param name="trainnumber">номер поезда</param>
        /// <param name="time">время</param>
        /// <param name="blockpath">название блок участка</param>
        /// <param name="type_event">тип события прибытие или отправление</param>
        public string SendCommandGid(SettingsCommandGID commnadgid/*byte command, int id_train, string numberstation, string path, string trainnumber, DateTime time, string blockpath, byte type_event, string prefix, string sufix, int reserve, string trainnumbervector,*/ )
        {
            try
            {
                //
                _command = new FbParameter("@COMMAND", FbDbType.SmallInt);
                _com_time = new FbParameter("@COM_TIME", FbDbType.TimeStamp);
                _train_idnh1 = new FbParameter("@TRAIN_IDNH1", FbDbType.Integer);
                _norm_idnh1 = new FbParameter("@NORM_TIME", FbDbType.Integer);
                _train_numh1 = new FbParameter("@TRAIN_NUM1", FbDbType.VarChar, 4);
                _j_st_formh1 = new FbParameter("@J_ST_FORMH1", FbDbType.VarChar, 8);
                _j_st_desth1 = new FbParameter("@J_ST_DESTH1", FbDbType.VarChar, 8);
                _j_sost_numh1 = new FbParameter("@J_ST_NUM1", FbDbType.SmallInt);
                _dop_proph1 = new FbParameter("@DOP_PROPH1", FbDbType.VarChar, 5);
                _rezervh1 = new FbParameter("@REZERVH1", FbDbType.Integer);
                ////
                _st_in_zoneh1 = new FbParameter("@ST_IN_ZONEH1", FbDbType.VarChar, 8);
                _st_out_zoneh1 = new FbParameter("@ST_OUT_ZONEH1", FbDbType.VarChar, 8);
                _train_idnh2 = new FbParameter("@TRAIN_IDNH2", FbDbType.Integer);
                _norm_idnh2 = new FbParameter("@NORM_TIME", FbDbType.Integer);
                _train_numh2 = new FbParameter("@TRAIN_NUM1", FbDbType.VarChar, 4);
                _j_st_formh2 = new FbParameter("@J_ST_FORMH1", FbDbType.VarChar, 8);
                _j_st_desth2 = new FbParameter("@J_ST_DESTH1", FbDbType.VarChar, 8);
                _j_sost_numh2 = new FbParameter("@J_SOST_NUM1", FbDbType.SmallInt);
                _dop_proph2 = new FbParameter("@DOP_PROPH1", FbDbType.VarChar, 4);
                _rezervh2 = new FbParameter("@REZERVH1", FbDbType.Integer);
                ////
                _st_in_zoneh2 = new FbParameter("@ST_IN_ZONEH2", FbDbType.VarChar, 8);
                _st_out_zoneh2 = new FbParameter("@ST_OUT_ZONEH2", FbDbType.VarChar, 8);
                _train_idne1 = new FbParameter("@TRAIN_IDNE1", FbDbType.Integer);
                _ev_type1 = new FbParameter("@EV_TYPE1", FbDbType.SmallInt);
                _ev_time1 = new FbParameter("@EV_TIME1", FbDbType.TimeStamp);
                _ev_statione1 = new FbParameter("@EV_STATIONE1", FbDbType.VarChar, 8);
                _ev_axise1 = new FbParameter("@EV_AXISE1", FbDbType.VarChar, 4);
                _ev_ndoe1 = new FbParameter("@EV_NDOE1", FbDbType.VarChar, 32);
                _ev_ne_statione1 = new FbParameter("@EV_NE_STATIONE1", FbDbType.VarChar, 8);
                _ev_res_idne1 = new FbParameter("@EV_RES_IDNE1", FbDbType.Integer);
                ////
                _ev_flage1 = new FbParameter("@EV_FlAGE1", FbDbType.SmallInt);
                _train_idne2 = new FbParameter("@TRAIN_IDNE1", FbDbType.Integer);
                _ev_type2 = new FbParameter("@EV_TYPE2", FbDbType.SmallInt);
                _ev_time2 = new FbParameter("@EV_TIME2", FbDbType.TimeStamp);
                _ev_statione2 = new FbParameter("@EV_STATIONE2", FbDbType.VarChar, 8);
                _ev_axise2 = new FbParameter("@EV_AXISE2", FbDbType.VarChar, 4);
                _ev_ndoe2 = new FbParameter("@EV_NDOE2", FbDbType.VarChar, 32);
                _ev_ne_statione2 = new FbParameter("@EV_NE_STATIONE2", FbDbType.VarChar, 8);
                _ev_res_idne2 = new FbParameter("@EV_RES_IDNE2", FbDbType.Integer);
                _ev_flage2 = new FbParameter("@EV_FLAGE2", FbDbType.SmallInt);

                using (var con = new FbConnection(_connectionString))
                {
                    con.Open();
                    FbTransaction transaction = con.BeginTransaction();
                    //
                    using (var cmd = new FbCommand(InsertCommandText, con, transaction))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //добавление параметров
                        cmd.Parameters.Add(_command);
                        cmd.Parameters.Add(_com_time);
                        cmd.Parameters.Add(_train_idnh1);
                        cmd.Parameters.Add(_norm_idnh1);
                        cmd.Parameters.Add(_train_numh1);
                        cmd.Parameters.Add(_j_st_formh1);
                        cmd.Parameters.Add(_j_st_desth1);
                        cmd.Parameters.Add(_j_sost_numh1);
                        cmd.Parameters.Add(_dop_proph1);
                        cmd.Parameters.Add(_rezervh1);
                        ////
                        cmd.Parameters.Add(_st_in_zoneh1);
                        cmd.Parameters.Add(_st_out_zoneh1);
                        cmd.Parameters.Add(_train_idnh2);
                        cmd.Parameters.Add(_norm_idnh2);
                        cmd.Parameters.Add(_train_numh2);
                        cmd.Parameters.Add(_j_st_formh2);
                        cmd.Parameters.Add(_j_st_desth2);
                        cmd.Parameters.Add(_j_sost_numh2);
                        cmd.Parameters.Add(_dop_proph2);
                        cmd.Parameters.Add(_rezervh2);
                        ////
                        cmd.Parameters.Add(_st_in_zoneh2);
                        cmd.Parameters.Add(_st_out_zoneh2);
                        cmd.Parameters.Add(_train_idne1);
                        cmd.Parameters.Add(_ev_type1);
                        cmd.Parameters.Add(_ev_time1);
                        cmd.Parameters.Add(_ev_statione1);
                        cmd.Parameters.Add(_ev_axise1);
                        cmd.Parameters.Add(_ev_ndoe1);
                        cmd.Parameters.Add(_ev_ne_statione1);
                        cmd.Parameters.Add(_ev_res_idne1);
                        //
                        cmd.Parameters.Add(_ev_flage1);
                        cmd.Parameters.Add(_train_idne2);
                        cmd.Parameters.Add(_ev_type2);
                        cmd.Parameters.Add(_ev_time2);
                        cmd.Parameters.Add(_ev_statione2);
                        cmd.Parameters.Add(_ev_axise2);
                        cmd.Parameters.Add(_ev_ndoe2);
                        cmd.Parameters.Add(_ev_ne_statione2);
                        cmd.Parameters.Add(_ev_res_idne2);
                        cmd.Parameters.Add(_ev_flage2);
                        //Присваиваем значения
                        _command.Value = commnadgid.Command;
                        _com_time.Value = null;
                        _train_idnh1.Value = commnadgid.IdTrain;
                        _norm_idnh1.Value = null;
                        //
                        if (string.IsNullOrEmpty(commnadgid.NumberTrain) || commnadgid.NumberTrain == ConstName.unknown_number_train)
                            _train_numh1.Value = null;
                        else _train_numh1.Value = commnadgid.NumberTrain;
                        //
                        _j_st_formh1.Value = commnadgid.PrefixVector;
                        _j_st_desth1.Value = commnadgid.SufixVector;
                        _j_sost_numh1.Value = null;
                        _dop_proph1.Value = null;
                        _rezervh1.Value = commnadgid.Resreve;
                        ////
                        _st_in_zoneh1.Value = null;
                        _st_out_zoneh1.Value = null;
                        _train_idnh2.Value = null;
                        _norm_idnh2.Value = null;
                        _train_numh2.Value = null;
                        _j_st_formh2.Value = null;
                        _j_st_desth2.Value = null;
                        _j_sost_numh2.Value = null;
                        _dop_proph2.Value = null;
                        _rezervh2.Value = null;
                        ////
                        _st_in_zoneh2.Value = null;
                        _st_out_zoneh2.Value = null;
                        _train_idne1.Value = null;
                        _ev_type1.Value = commnadgid.TypeEvent;
                        //if (commnadgid.TypeEvent == 1 || commnadgid.TypeEvent == 2 || commnadgid.TypeEvent == 3 ||)
                        //    _ev_type1.Value = commnadgid.TypeEvent;
                        //else _ev_type1.Value = null;
                        //
                        if (commnadgid.TimeEvent == DateTime.MinValue)
                            _ev_time1.Value = null;
                        else
                            _ev_time1.Value = commnadgid.TimeEvent;
                        //
                        if (!string.IsNullOrEmpty(commnadgid.NumberStation))
                            _ev_statione1.Value = ConstName.prefix_station + commnadgid.NumberStation;
                        else
                            _ev_statione1.Value = commnadgid.NumberStation;
                        _ev_axise1.Value = commnadgid.NamePath;
                        _ev_ndoe1.Value = commnadgid.Prefix;
                        _ev_ne_statione1.Value = null;
                        _ev_res_idne1.Value = null;
                        ////
                        _ev_flage1.Value = null;
                        _train_idne2.Value = null;
                        _ev_type2.Value = null;
                        _ev_time2.Value = null;
                        _ev_statione2.Value = null;
                        _ev_axise2.Value = null;
                        _ev_ndoe2.Value = commnadgid.Sufix;
                        _ev_ne_statione2.Value = null;
                        _ev_res_idne2.Value = null;
                        _ev_flage2.Value = null;
                        //
                        cmd.ExecuteNonQuery();
                    }
                    //
                    transaction.Commit();
                }
                return "Команда отправлена";
            }
            catch (Exception error) 
            {
                return error.Message;
            }
        }
    }
}
