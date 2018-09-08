using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace RmqChecker
{
    public class Options
    {
        [Option('h', "host", Required = true, HelpText = "RabbitMQ host")]
        public string Host { get; set; }

        [Option('p', "port", HelpText = "Listining port RabbitMQ (default: 5672)", Default = 5672)]
        public int Port { get; set; }

        [Option('v', "vhost", Required = true, HelpText = "Virtual host RabbitMQ")]
        public string VirtualHost { get; set; }

        [Option('l', "login", Required = true, HelpText = "RabbitMQ user with the ability to read and write to the queue")]
        public string Login { get; set; }

        [Option('w', "password", Required = true, HelpText = "User password")]
        public string Password { get; set; }

        [Option("queue", HelpText = "Lost of queues", Separator = ';')]
        public IEnumerable<string> Queues { get; set; }

        [Option("msgcnt", HelpText = "Count messages to read and send to the queue", Default = 10)]
        public int MessageCount { get; set; }

        /*[Option("newmsg", HelpText = "Create new message")]
        public bool CreateMessage { get; set; }

        [Option("msglen", HelpText = "Message length", Default = 1024)]
        public int MessageLength { get; set; }*/
    }
}
