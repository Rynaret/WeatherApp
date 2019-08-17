using Infrastructure.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace GismeteoGrabber
{
    public class LifetimeEventsHostedService : IHostedService
    {
        #region ctor

        private readonly WeatherDbContext _dbContext;

        public LifetimeEventsHostedService(WeatherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _dbContext.Database.MigrateAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
