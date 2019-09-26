using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Timers;
using log4net;

namespace TrafficTrain
{
    public delegate void NewTaktEvent();

    public class TableRow
    {
        /// <summary>
        /// порядковый номер
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// номер поезда
        /// </summary>
        public string NumberTrain { get; set; }
        /// <summary>
        /// префикс
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// суффикс
        /// </summary>
        public string Sufix { get; set; }
        /// <summary>
        /// отметка
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// длина условная
        /// </summary>
        public double Lenght { get; set; }
        /// <summary>
        /// вес брутто
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// станция назначения
        /// </summary>
        public string StationEnd { get; set; }
        /// <summary>
        /// номер справки
        /// </summary>
        public int IdMessage { get; set; }
        /// <summary>
        /// идентификатор поезда
        /// </summary>
        public string IdTrain { get; set; }
    }


   public enum ViewSelectTrain
   {
       infotrainmessage = 0,
       messagetrain = 1
   }


    /// <summary>
    /// получаем сообщение по справкам
    /// </summary>
    class SelectMessageTrain:IDisposable
    {

        #region Variable
        /// <summary>
        /// сообщение ополучении справки
        /// </summary>
        public static event InfoMessageTrain InfoMessage;
        /// <summary>
        /// выбранный поезд
        /// </summary>
       // TableRow _currenttrain = null;
        /// <summary>
        /// текущая выбранная таблица
        /// </summary>
        Move.Even _currentTable;
        /// <summary>
        /// строка подключения к базе Asoup
        /// </summary>
        static  string _connectionStringAsoup =  string.Empty;
        /// <summary>
        /// Строка подключения к базе GAppDb
        /// </summary>
        static string _connectionStringGAppDb = string.Empty;
        /// <summary>
        /// текущий вид запроса
        /// </summary>
        ViewSelectTrain _currentviewselect;
        /// <summary>
        /// логирование
        /// </summary>
        static readonly ILog log = LogManager.GetLogger(typeof(SelectMessageTrain));
        #endregion


        public static void SetConnectionString()
        {
            _connectionStringAsoup = string.Format("Data Source={0}\\SqlExpress;Initial Catalog = Asoup;Persist Security Info=True; User ID=sa;Password=1", MainWindow.GetServerMessage());
            _connectionStringGAppDb = string.Format("Data Source={0}\\SqlExpress;Initial Catalog = GAppDb;Persist Security Info=True; User ID=sa;Password=1", MainWindow.GetServerMessage());
            log4net.Config.XmlConfigurator.Configure();
        }

        public void FindMessageInfo(List<TableRow> trains, ViewSelectTrain selectview, Move.Even eventable)
        {
           // _currenttrain = new TableRow() { Id = train.Id, NumberTrain = train.NumberTrain, IdMessage = train.IdMessage };
            _currentTable = eventable;
            _currentviewselect = selectview;
            //
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(FindInfoTableTrain), trains);
        }

        //public void FindMessagesInfo(List<TableRow> trains, ViewSelectTrain selectview, Move.Even eventable)
        //{
        //    _currenttrain = new TableRow() { Id = train.Id, NumberTrain = train.NumberTrain, IdMessage = train.IdMessage };
        //    _currentTable = eventable;
        //    _currentviewselect = selectview;
        //    //
        //    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(FindInfoTableTrain));
        //}

        public void Dispose()
        {

        }

        /// <summary>
        /// проверяем есть ли подключение к базе данных
        /// </summary>
        /// <returns></returns>
        public static bool ConnectDataBase()
        {

            SqlConnection sqlcon;
            try
            {
                using (sqlcon = new SqlConnection(_connectionStringGAppDb))
                    sqlcon.Open();
                using (sqlcon = new SqlConnection(_connectionStringAsoup))
                {
                    sqlcon.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        //static private void FindMessage(object sender)
        //{
        //    lock("myLock")
        //    {
        //        string answer = "Нет подходящих справок";
        //        try
        //        {
        //            bool message_received = false;
        //            SqlConnection sqlcon;
        //            using (sqlcon = new SqlConnection(_connectionStringAsoup))
        //            {

        //                string sql = string.Format("select top(1) MessageText from RawMessages rm inner join Messages142 ms on rm.RecId = ms.RawMessageRecId where ms.TrainNumber = {0} and rm.ReceivingTime >= @TIME order by rm.ReceivingTime desc",  _numbertrain);
        //                using (SqlCommand comm = sqlcon.CreateCommand())
        //                {

        //                    comm.CommandText = sql;
        //                    //
        //                    SqlParameter timeparametr = new SqlParameter("@TIME", System.Data.SqlDbType.DateTime);
        //                    timeparametr.Value = DateTime.Now.AddHours(_conthours);
        //                    comm.Parameters.Add(timeparametr);
        //                    //
        //                    sqlcon.Open();
        //                    SqlDataReader read = (SqlDataReader)comm.ExecuteReader();

        //                    while (read.Read())
        //                    {
        //                        answer = read[0].ToString();
        //                        message_received = true;
        //                    }

        //                }

        //            }
        //            if (!message_received)
        //            {
        //                using (sqlcon = new SqlConnection(_connectionStringAsoup))
        //                {

        //                    string sql = string.Format("select top(1) MessageText from RawMessages rm inner join Messages1042 ms on rm.RecId = ms.RawMessageRecId where ms.TrainNumber = {0} and rm.ReceivingTime >= @TIME order by rm.ReceivingTime desc",  _numbertrain);
        //                    using (SqlCommand comm = sqlcon.CreateCommand())
        //                    {
        //                        comm.CommandText = sql;
        //                        SqlParameter timeparametr = new SqlParameter("@TIME", System.Data.SqlDbType.DateTime); 
        //                        timeparametr.Value = DateTime.Now.AddHours(_conthours);
        //                        comm.Parameters.Add(timeparametr);
        //                        sqlcon.Open();
        //                        SqlDataReader read = (SqlDataReader)comm.ExecuteReader();

        //                        while (read.Read())
        //                        {
        //                            answer = read[0].ToString();
        //                            message_received = true;
        //                        }
        //                    }
        //                }
        //            }


        //        }
        //        catch (Exception error)
        //        { answer = error.Message; }
        //        //
        //        if (InfoMessage != null)
        //            InfoMessage(answer.TrimStart(new char[] { ' ', '\n', '\r' }));
        //    }
        //}


        /// <summary>
        /// находим информацию по поезду
        /// </summary>
        /// <param name="sender"></param>
        private void FindInfoTableTrain(object sender)
        {
            lock ("myLock")
            {
                List<TableRow> trains = (List<TableRow>)sender;
                if (trains != null && trains.Count > 0)
                {
                    string answer = "Нет подходящих справок";
                    TableRow row = null;
                    try
                    {
                        SqlConnection sqlcon;
                        if (_currentviewselect == ViewSelectTrain.infotrainmessage)
                        {
                            //заполняем таблицу информации по поездам
                            foreach (TableRow train in trains)
                            {
                                using (sqlcon = new SqlConnection(_connectionStringGAppDb))
                                {

                                    string sql = string.Format("select Index3, GrossWeight, ConvWagonCount, Index4, RawMessageRecId  from TrainRecords where RawMessageRecId = {0}", train.IdMessage);
                                    using (SqlCommand comm = sqlcon.CreateCommand())
                                    {

                                        comm.CommandText = sql;
                                        sqlcon.Open();
                                        SqlDataReader read = (SqlDataReader)comm.ExecuteReader();

                                        while (read.Read())
                                        {
                                            row = new TableRow();
                                            row.Id = train.Id;
                                            row.NumberTrain = train.NumberTrain;
                                            row.StationEnd = read["Index3"].ToString();
                                            row.Weight = double.Parse(read["GrossWeight"].ToString());
                                            row.Lenght = double.Parse(read["ConvWagonCount"].ToString());
                                            row.Mark = read["Index4"].ToString();
                                        }
                                    }
                                    //
                                }
                                //
                                SendMessageInfo(row, answer);
                            }
                        }
                        //
                        if (_currentviewselect == ViewSelectTrain.messagetrain)
                        {
                            //получаем справку по поезду
                            using (sqlcon = new SqlConnection(_connectionStringAsoup))
                            {

                                string sql = string.Format("select  MessageText from RawMessages where RecId = {0}", trains[trains.Count-1].IdMessage);
                                using (SqlCommand comm = sqlcon.CreateCommand())
                                {
                                    comm.CommandText = sql;
                                    //
                                    sqlcon.Open();
                                    var read = comm.ExecuteScalar();
                                    if (read is string)
                                        answer = (string)read;
                                }

                            }
                            //
                            SendMessageInfo(row, answer);
                        }
                    }
                    catch (Exception error)
                    {
                        log.Error(error.Message, error);
                    }
                }
            }
        }

        private void SendMessageInfo(TableRow row, string answer)
        {
            if (InfoMessage != null)
            {
                switch (_currentviewselect)
                {
                    case ViewSelectTrain.messagetrain:
                        InfoMessage(answer.TrimStart(new char[] { ' ', '\n', '\r' }), row, _currentTable);
                        break;
                    case ViewSelectTrain.infotrainmessage:
                        InfoMessage(string.Empty, row, _currentTable);
                        break;
                }
            }
        }
 

        //   private void FindInfoTableTrain(object sender)
        //{
        //    lock ("myLock")
        //    {
        //        string answer = "Нет подходящих справок";
        //        int recid = -1;
        //        TableRow row = null;
        //        try
        //        {
        //            SqlConnection sqlcon;
        //            //получаем id справки
        //            using (sqlcon = new SqlConnection(_connectionStringGAppDb))
        //            {

        //                string sql = string.Format("select Index3, GrossWeight, ConvWagonCount, Index4, RawMessageRecId  from TrainRecords where RawMessageRecId = {0} and MessageTime >= @TIME order by MessageTime desc", _currenttrain.);//string.Format("select top(1) Index3, GrossWeight, ConvWagonCount, Index4, RawMessageRecId  from TrainRecords where TrainNumber = {0} and MessageTime >= @TIME order by MessageTime desc", _currenttrain.NumberTrain);
        //                using (SqlCommand comm = sqlcon.CreateCommand())
        //                {

        //                    comm.CommandText = sql;
        //                    //
        //                    SqlParameter timeparametr = new SqlParameter("@TIME", System.Data.SqlDbType.DateTime);
        //                    timeparametr.Value = DateTime.Now.AddHours(_conthours);
        //                    comm.Parameters.Add(timeparametr);
        //                    //
        //                    sqlcon.Open();
        //                    SqlDataReader read = (SqlDataReader)comm.ExecuteReader();

        //                    while (read.Read())
        //                    {
        //                        row = new TableRow();
        //                        row.Id = _currenttrain.Id;
        //                        row.NumberTrain = _currenttrain.NumberTrain;
        //                        row.StationEnd = read["Index3"].ToString();
        //                        row.Weight = double.Parse(read["GrossWeight"].ToString());
        //                        row.Lenght = double.Parse(read["ConvWagonCount"].ToString());
        //                        row.Mark = read["Index4"].ToString();
        //                        if (read["RawMessageRecId"] is int)
        //                            recid = (int)read["RawMessageRecId"];
        //                    }
        //                }

        //            }
        //            //получаем справку по поезду
        //            using (sqlcon = new SqlConnection(_connectionStringAsoup))
        //            {

        //                string sql = "select  MessageText from RawMessages where RecId = @Id";
        //                using (SqlCommand comm = sqlcon.CreateCommand())
        //                {

        //                    comm.CommandText = sql;
        //                    //
        //                    SqlParameter timeparametr = new SqlParameter("@Id", System.Data.SqlDbType.Int);
        //                    timeparametr.Value = recid;
        //                    comm.Parameters.Add(timeparametr);
        //                    //
        //                    sqlcon.Open();
        //                    var read = comm.ExecuteScalar();
        //                    if (read is string)
        //                        answer = (string)read;
        //                }

        //            }
        //        }
        //        catch (Exception error)
        //        { answer = error.Message; }
        //        //
        //        if (InfoMessage != null)
        //        {
        //            switch (_currentviewselect)
        //            {
        //                case ViewSelectTrain.messagetrain:
        //                    InfoMessage(answer.TrimStart(new char[] { ' ', '\n', '\r' }), row, _currentTable);
        //                    break;
        //                case ViewSelectTrain.infotrainmessage:
        //                    InfoMessage(string.Empty, row, _currentTable);
        //                    break;
        //            }
        //        }
        //    }
        //}

    }
}
