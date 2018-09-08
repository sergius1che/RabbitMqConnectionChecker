using System;
using CommandLine;

namespace RmqChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var runner = new Runner();
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(opts => runner.Run(opts));

                Console.WriteLine("SUCCESS END");
            }
            catch (RCException ex)
            {
                Console.Error.WriteLine($"ERROR: {ex.MessageToConsoleString()}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"FATAL ERROR: {ex.MessageToConsoleString()}");
            }
        }
    }
}
