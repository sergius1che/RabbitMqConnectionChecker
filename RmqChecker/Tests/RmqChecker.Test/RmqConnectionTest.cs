using System;
using Xunit;

namespace RmqChecker.Test
{
    public class RmqConnectionTest
    {
        private Options _options;
        private RmqConnection _rmqConnection;

        public RmqConnectionTest()
        {
            _options = new Options
            {
                Host = "192.168.17.51",
                Port = 5672,
                Login = "admin",
                Password = "R@5q7H81$71!",
                VirtualHost = "Queue.Trcont",
            };
            _rmqConnection = new RmqConnection(_options);
        }

        [Fact]
        public void CanGetMessage()
        {
            var msg1 = _rmqConnection.GetMessage("Live.Authentication.WorkingQueue.Notification");
            var msg2 = _rmqConnection.GetMessage("Live.Authentication.WorkingDemand.Errors");
        }

        [Fact]
        public void CanSendMessage()
        {
            var queue = "Live.Ris.WorkingDemand.Notification";
            var msg1 = _rmqConnection.GetMessage(queue);
            
            if (msg1 != null)
            {
                _rmqConnection.SendMessage(msg1, queue);
            }
        }
    }
}
