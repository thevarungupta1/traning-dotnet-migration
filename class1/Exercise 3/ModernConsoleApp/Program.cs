using System;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SampleConsoleApp
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log.Info("Application started");

            try
            {
                var processor = new ProcessorService();
                processor.Run().Wait();
            }
            catch (Exception ex)
            {
                log.Error("Unhandled exception", ex);
            }

            log.Info("Application finished");
            Console.ReadLine();
        }
    }
}
