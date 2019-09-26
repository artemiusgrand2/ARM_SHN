using System;
using System.Collections.Generic;
using System.Threading;
using TrafficTrain;
using RW.KTC.ORPO.Berezina.DataStream.Common.Interfaces;
using RW.KTC.ORPO.Berezina.Server.Common.Interfaces;
using RW.KTC.ORPO.Berezina.Communication.Protocol.Dsccp;
using RW.KTC.ORPO.Berezina.Communication.Protocol.Dsccp.Requests;
using RW.KTC.ORPO.Berezina.Communication.Protocol.Dsccp.Answers;
using RW.KTC.ORPO.Berezina.Communication.Protocol.Dsccp.Enums;
using RW.KTC.ORPO.Berezina.Communication.Protocol.Dsccp.Converters;

namespace TrafficTrain.DataServer
{
    public class DataSource
    {
        private readonly IDataStream dataStream;
        private readonly Thread parsingThread;
        private readonly Thread processingThread;
        private readonly Thread showThread;
        private readonly uint requestTimeout = 0;
        private readonly Converter converter;
        private readonly byte[] echoMessageData;
        private DateTime lastCommunicationTime;
        private readonly TimeSpan echoMessageTimeout;
        private  DateTime endTime = DateTime.MinValue;
        private bool isStop;
        private readonly Queue<TableValuesAnswer> awaitingProcessTable;


        public IDataStream DataStream
        {
            get
            {
                return dataStream;
            }
        }

        public DataSource(IDataStream stream, uint RequestTimeout)
        {
            parsingThread = new Thread(Parse)
                {
                    Priority = ThreadPriority.Highest
                };

            processingThread = new Thread(Process)
            {
                Priority = ThreadPriority.Highest
            };

            showThread = new Thread(Show)
            {
                Priority = ThreadPriority.Highest
            };
            //
            dataStream = stream;
            requestTimeout = (RequestTimeout == 0) ? 200 : RequestTimeout;
            echoMessageTimeout = TimeSpan.FromSeconds(10);
            converter = Converter.CreateInstance();
            echoMessageData = converter.ToBytes(new EchoMessage(), 0);
            awaitingProcessTable = new Queue<TableValuesAnswer>();
        }

        public void Start()
        {
            if (!isStop)
            {
                isStop = false;
                parsingThread.Start();
                processingThread.Start();
                showThread.Start();
            }
        }
        public void Stop()
        {
            isStop = true;
            parsingThread.Join();
            processingThread.Join();
            showThread.Join();
        }

        private Answer GetAnswer(Request request)
        {
            Answer answer = null;
            lock (converter)
            {
                if(request is CurrentValueRequest)
                {

                }
                byte[] requestData = converter.ToBytes(request, 0);
                dataStream.ConnectOnWrite = true;
                dataStream.Write(requestData);
                bool isStop = false;
                while (!isStop)
                {
                    byte[] data;
                    if (dataStream.Read(out data))
                    {
                        lastCommunicationTime = DateTime.Now;
                        Message message = converter.FromBytes(data);
                        if (message.MType == MessageType.Echo)
                        {
                            //   Logger.Log.LogDebug("Recieved Dsccp Echo");
                            continue;
                        }
                        if (message.MType == MessageType.Error)
                        {
                            throw new Exception("Ошибка связи");
                        }
                        if (message.MType == MessageType.Request)
                        {
                            throw new InvalidOperationException("Client can't recieve requests");
                        }
                        if (message.MType == MessageType.Answer)
                        {
                            answer = message as Answer;
                            isStop = true;
                        }
                    }
                    else
                    {
                        if (DateTime.Now - lastCommunicationTime > echoMessageTimeout)
                        {
                            //Logger.Log.LogDebug("Sending Dsccp Echo");
                            dataStream.ConnectOnWrite = false;
                            dataStream.Write(echoMessageData);
                            lastCommunicationTime = DateTime.Now;
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }
                }
                if (answer == null)
                {
                    throw new NotSupportedException("Message has a answer type but it is not an answer");
                }
            }
            return answer;
        }

        private void Parse()
        {
            while (!isStop)
            {
                foreach (var st in Core.Stations)
                {
                    foreach (var table in st.Value)
                    {
                        try
                        {
                            var request = new TableValuesRequest() { StationCode = st.Key, TableId = table.Key };
                            var answer = GetAnswer(request);
                            var TableAnswer = answer as TableValuesAnswer;
                            if (TableAnswer != null)
                            {
                                var message = TableAnswer;
                                lock (awaitingProcessTable)
                                {
                                    awaitingProcessTable.Enqueue(message);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LoadProject.Log.Error(string.Format("Error parsing message. {0} . Station - {1}, tableId - {2}", e, st.Key, table.Key), e);
                        }
                        finally
                        {
                            Thread.Sleep(1);
                        }
                    }
                }
            }
        }

        private void Process()
        {
            while (!isStop)
            {
                TableValuesAnswer message = null;
                try
                {
                    lock (awaitingProcessTable)
                    {
                        if (awaitingProcessTable.Count > 0)
                        {
                            message = awaitingProcessTable.Dequeue();
                        }
                    }
                    if (message != null)
                    {
                        if (Core.Stations.ContainsKey(message.StationCode))
                        {
                            if (Core.Stations[message.StationCode].ContainsKey(message.TableId))
                            {
                                if (message.TableId == 17)
                                {
                                }
                                endTime = DateTime.Now;
                                DateTime start = DateTime.Now;
                                if(!Core.Stations[message.StationCode][message.TableId].SetValues(message.Values))
                                    LoadProject.Log.Error(string.Format("Таблица {0} {1} {2} не обновилась, проверьте ее размерность (параметр - 'DataSize') в конфигурации", Core.Stations[message.StationCode][message.TableId].Type,
                                                            Core.Stations[message.StationCode][message.TableId].Id, Core.Stations[message.StationCode][message.TableId].Name));
                                else
                                {
                                    DateTime end = DateTime.Now;
                                    LoadProject.Log.Info(string.Format("Processed table {0} {1} {2} in {3}", Core.Stations[message.StationCode][message.TableId].Type,
                                                                 Core.Stations[message.StationCode][message.TableId].Id, Core.Stations[message.StationCode][message.TableId].Name, end - start));
                                }
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                catch (Exception e)
                {
                    if (message == null)
                        LoadProject.Log.Error("Error processing message. {0}", e);
                    else
                        LoadProject.Log.Error(string.Format("Error parsing message. {0} . Station - {1}, tableId - {2}", e, message.StationCode, message.TableId), e);
                }
            }
        }

        private void Show()
        {
            while (!isStop)
            {
                try
                {
                    foreach (var grafic in LoadProject.AnalogGrafic)
                        grafic.AnalisNewData();
                    //
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    LoadProject.Log.Error("Error parsing message. {0}", e);
                }
            }
        }

    }
}
