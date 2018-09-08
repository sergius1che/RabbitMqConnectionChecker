using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RmqChecker
{
    public class RmqConnection : IDisposable
    {
        private readonly IConnection _connection;

        public RmqConnection(Options opts)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = opts.Login,
                Password = opts.Password,
                VirtualHost = opts.VirtualHost,
                Port = opts.Port,
                HostName = opts.Host
            };

            _connection = factory.CreateConnection();
        }

        public string GetMessage(string queueName)
        {
            using (var channel = _connection.CreateModel())
            {
                var data = channel.BasicGet(queueName, true);

                if (data != null)
                    return Encoding.UTF8.GetString(data.Body);
                else
                    return null;
            }
        }

        public bool SendMessage(string message, string queueName)
        {
            using (var channel = _connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
            }

            return true;
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }
    }
}
