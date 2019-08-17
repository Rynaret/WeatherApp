using GismeteoGrabber.Interfaces;
using GismeteoGrabber.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GismeteoGrabber
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        #region ctor

        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private AppSettings _appSettings;
        private Timer _timer;

        public TimedHostedService(
            ILogger<TimedHostedService> logger,
            IOptionsMonitor<AppSettings> options,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _appSettings = options.CurrentValue;
            options.OnChange(OnOptionsChange);
        }

        #endregion

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(TimedHostedService)} is starting");

            _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(_appSettings.GrabPeriodInMinutes));

            return Task.CompletedTask;
        }

        private async void DoWorkAsync(object state)
        {
            _logger.LogInformation($"{nameof(TimedHostedService)} is working.");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var parser = scope.ServiceProvider.GetService<IParserService>();
                    var forecasts = await parser.ParseAsync();

                    var forecastService = scope.ServiceProvider.GetService<IForecastService>();
                    await forecastService.SaveParsedDataAsync(forecasts);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured.");
            }

            _logger.LogInformation($"{nameof(TimedHostedService)} has completed the work.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(TimedHostedService)} is stopping");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void OnOptionsChange(AppSettings newAppSettings)
        {
            _logger.LogInformation($"{nameof(AppSettings)} on change");

            _logger.LogTrace($"Old settings: {JsonConvert.SerializeObject(_appSettings)}");
            _logger.LogTrace($"New settings: {JsonConvert.SerializeObject(newAppSettings)}");

            if (_appSettings.GrabPeriodInMinutes != newAppSettings.GrabPeriodInMinutes)
            {
                _timer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(newAppSettings.GrabPeriodInMinutes));
            }

            _appSettings = newAppSettings;
        }
    }
}
