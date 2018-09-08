using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RmqChecker
{
    public class Runner : IDisposable
    {
        private Options _options;
        private RmqConnection _rmqConnection;

        public void Dispose()
        {
            if (_rmqConnection != null)
                _rmqConnection.Dispose();
        }

        public void Run(Options opts)
        {
            _options = opts;
            _rmqConnection = new RmqConnection(opts);

            int success = 0, error = 0;
            Stopwatch getMsg = new Stopwatch(), setMsg = new Stopwatch(), onQueue = new Stopwatch();

            foreach (var queue in _options.Queues)
            {
                Console.WriteLine($"Start processing on {queue} queue");
                onQueue.Start();

                for (int i = 0; i < _options.MessageCount; i++)
                {
                    string msg = null;
                    try
                    {
                        getMsg.Start();
                        msg = _rmqConnection.GetMessage(queue);
                        getMsg.Stop();
                        Console.WriteLine($"Succes get message from {queue} on {getMsg.ElapsedMilliseconds} ms");
                        getMsg.Reset();
                    }
                    catch (Exception ex)
                    {
                        getMsg.Reset();
                        Console.WriteLine($"Error get message from {queue}. {ex.Message}");
                    }


                    if (string.IsNullOrEmpty(msg))
                        Console.WriteLine("WARNING: no take message");
                    else
                    {
                        try
                        {
                            setMsg.Start();
                            _rmqConnection.SendMessage(msg, queue);
                            setMsg.Stop();
                            Console.WriteLine($"Succes send message to {queue} on {setMsg.ElapsedMilliseconds} ms");
                            setMsg.Reset();
                        }
                        catch (Exception ex)
                        {
                            setMsg.Reset();
                            Console.WriteLine($"Error send message to {queue}. {ex.Message}");
                        }
                    }
                }

                onQueue.Stop();
                Console.WriteLine($"End processing on {queue} queue. Finished in {onQueue.ElapsedMilliseconds} ms");
                onQueue.Reset();
            }
        }
    }
}
