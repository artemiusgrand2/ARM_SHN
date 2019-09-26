using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Move;

namespace TrafficTrain
{
    class UpdateMessageTrain
    {

        #region Variable
        /// <summary>
        /// таймер обновления справок по поездам
        /// </summary>
        Timer _timerUpdate = new Timer();
        /// <summary>
        /// частота обновления справок по поездам
        /// </summary>
        double _timeupdate = 10;
        #endregion

         public UpdateMessageTrain(string time)
        {
            double buffer = 0;
            if (time != null && double.TryParse(time, out buffer))
                _timeupdate = double.Parse(time);
             //
            if (_timeupdate < 2)
                _timeupdate = 2;
            //
            _timerUpdate.Interval = _timeupdate*60000;
            _timerUpdate.Elapsed += _timerUpdate_Elapsed;
        }

        public  void StartUpdate()
        {
            _timerUpdate.Start();
        }

        public  void StopUpdate()
        {
            _timerUpdate.Stop();
        }

        private  void _timerUpdate_Elapsed(object sender, ElapsedEventArgs e)
        {
            ////обновляем таблицу четных поездов
            //if (SelectMessageTrain.ConnectDataBase())
            //{
            //    SelectMessageTrain trainselect = new SelectMessageTrain();
            //    foreach (TableRow row in MainWindow._collectionTrainEven)
            //        trainselect.FindMessageInfo(row, ViewSelectTrain.infotrainmessage, Even.even);
            //    //обновляем таблицу нечетных поездов
            //    foreach (TableRow row in MainWindow._collectionTrainOdd)
            //        trainselect.FindMessageInfo(row, ViewSelectTrain.infotrainmessage, Even.odd);
            //}
        }
    }
}
