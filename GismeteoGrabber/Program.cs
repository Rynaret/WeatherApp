using Domain.Interfaces;
using GismeteoGrabber.Implementations;
using GismeteoGrabber.Interfaces;
using GismeteoGrabber.Settings;
using Infrastructure.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("GismeteoGrabber.Tests")]

namespace GismeteoGrabber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables();
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<AppSettings>(hostContext.Configuration);

                    services.AddTransient<IRepository, EfRepository>();

                    services.AddTransient<IForecastService, ForecastService>();
                    services.AddTransient<IParserService, ParserService>();

                    services.AddHttpClient<IParserService, ParserService>((serviceProvider, client) =>
                    {
                        var appSettings = serviceProvider.GetService<IOptions<AppSettings>>().Value;
                        client.BaseAddress = appSettings.Gismeteo.BaseUrl;
                    });

                    services.AddDbContextPool<WeatherDbContext>((serviceProvider, options) =>
                    {
                        options.UseMySql(hostContext.Configuration.GetConnectionString("weather"));
                    });

                    services.AddHostedService<LifetimeEventsHostedService>();
                    services.AddHostedService<TimedHostedService>();
                })
                .ConfigureLogging((HostBuilderContext hostContext, ILoggingBuilder configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
