using Domain.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Wcf.DataAccess.EF;
using Wcf.ForecastService.Dtos;
using Wcf.ForecastService.Models;

namespace Wcf.ForecastService
{
    public class ForecastService : IForecastService
    {
        #region ctor

        private readonly WeatherDbContext _weatherDbContext;

        public ForecastService()
        {
            _weatherDbContext = new WeatherDbContext();
        }

        #endregion

        public async Task<GeoObjectDto[]> GetAvailableGeoObjectsAsync()
        {
            var geoObjects = await _weatherDbContext
                .Set<GeoObject>()
                .ToArrayAsync();
            return geoObjects.Select(x => new GeoObjectDto(x)).ToArray();
        }

        public async Task<ForecastDto> GetForecastAsync(GetForecastRequest requestModel)
        {
            var forecast = await _weatherDbContext
                .Set<Forecast>()
                .Where(x => x.GeoObject.Name == requestModel.GeoObjectName)
                .Where(x => x.Date == requestModel.Date)
                .FirstOrDefaultAsync();

            return forecast != null ? new ForecastDto(forecast) : null;
        }
    }
}
