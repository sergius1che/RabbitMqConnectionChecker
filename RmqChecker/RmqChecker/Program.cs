using System;
using System.Threading.Tasks;
using CommandLine;

namespace RmqChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Options options = null;
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(opts => options = opts);

                using (var runner = new Runner())
                {
                    runner.Run(options);
                }

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
