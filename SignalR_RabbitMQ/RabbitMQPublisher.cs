using Newtonsoft.Json;
using RabbitMQ.Client;
using SignalR_RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SignalR_RabbitMQ
{
    public static class RabbitMQPublisher
    {
        public static void SendMessage(MessageLog log)
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

                string _messageLog = JsonConvert.SerializeObject(log);
                var body = Encoding.UTF8.GetBytes(_messageLog);

                channel.BasicPublish(exchange: "",
                                     routingKey: "MessageLog",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" From: {log.Nick} [{log.Text}]");
            }
        }
    }
}