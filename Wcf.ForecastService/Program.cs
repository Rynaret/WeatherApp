using Serilog;
using System;
using System.ServiceModel;

namespace Wcf.ForecastService
{
    class Program
    {
        static void Main()
        {
            ConfigureLogging();

            using (var host = new ServiceHost(typeof(ForecastService)))
            {
                host.Open();
                Log.Information($"{nameof(ForecastService)} listening");
                WaitForExitSignal();
                Log.Information("Shutting down...");
                host.Close();
            }
        }

        private static void WaitForExitSignal()
        {
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Logging initialized");
        }
    }
}
