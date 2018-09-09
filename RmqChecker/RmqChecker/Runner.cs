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

            Stopwatch getMsg = new Stopwatch(), setMsg = new Stopwatch(), onQueue = new Stopwatch();

            foreach (var queue in _options.Queues)
            {
                int success = 0, error = 0;
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
                        Console.Error.WriteLine($"Error get message from {queue}. {ex.MessageToConsoleString()}");
                    }


                    if (string.IsNullOrEmpty(msg))
                    {
                        Console.WriteLine("WARNING: no take message");
                        error++;
                    }
                    else
                    {
                        try
                        {
                            setMsg.Start();
                            _rmqConnection.SendMessage(msg, queue);
                            setMsg.Stop();
                            Console.WriteLine($"Succes send message to {queue} on {setMsg.ElapsedMilliseconds} ms");
                            setMsg.Reset();
                            success++;
                        }
                        catch (Exception ex)
                        {
                            setMsg.Reset();
                            Console.Error.WriteLine($"Error send message to {queue}. {ex.MessageToConsoleString()}");
                            error++;
                        }
                    }
                }

                onQueue.Stop();
                Console.WriteLine($"End processing on {queue} queue. Count: {_options.MessageCount} success: {success}, errors: {error}. Finished in {onQueue.ElapsedMilliseconds} ms");
                onQueue.Reset();
            }
        }
    }
}
