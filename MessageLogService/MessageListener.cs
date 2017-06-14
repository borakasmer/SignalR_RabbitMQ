using Dapper;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageLogService
{
    class MessageListener
    {
        public void DoWork()
        {
            try
            {
                new Thread(() =>
                {
                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (IConnection connection = factory.CreateConnection())
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "MessageLog",
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);

                        var consumer = new QueueingBasicConsumer(channel);

                        channel.BasicConsume(queue: "MessageLog",
                                                 noAck: true,
                                                 consumer: consumer);
                        while (true)
                        {
                            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            MessageLog _messageLog = JsonConvert.DeserializeObject<MessageLog>(message);
                            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Message"].ToString()))
                            {
                                byte[] data = Convert.FromBase64String(_messageLog.Text);
                                string decodedString = Encoding.UTF8.GetString(data);
                                sqlConnection.Open();
                                MessageLog log = new MessageLog() { Nick = _messageLog.Nick, Text = decodedString, CreatedDate = DateTime.Now };
                                string sqlQuery = "Insert into [dbo].[MessageLog]([Nick],[Text],[CreatedDate]) VALUES (@Nick,@Text,@CreatedDate)";
                                sqlConnection.Execute(sqlQuery, log);

                                Console.WriteLine($" From: {_messageLog.Nick} Say:[{decodedString}]");
                                sqlConnection.Close();
                            }
                        }
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
    }
}
