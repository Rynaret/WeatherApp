using Domain.Entities;
using Domain.Interfaces;
using GismeteoGrabber.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GismeteoGrabber.Implementations
{
    public class ForecastService : IForecastService
    {
        #region ctor

        private readonly IRepository _repository;

        public ForecastService(IRepository repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task SaveParsedDataAsync(Forecast[] forecasts)
        {
            var geoObjects = forecasts.Select(x => x.GeoObject).ToArray();

            await SaveCitiesAsync(geoObjects);
            await SaveForecastsAsync(forecasts);

            await _repository.SaveChangesAsync();
        }

        private async Task SaveCitiesAsync(GeoObject[] geoObjects)
        {
            var uniqueGeoObjects = geoObjects
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToArray();
            var geoObjectDtosIds = uniqueGeoObjects
                .Select(x => x.Id)
                .ToArray();

            var dbGeoObjects = await _repository
                .Read<GeoObject>()
                .Where(x => geoObjectDtosIds.Contains(x.Id))
                .ToArrayAsync();

            var geoObjectsToSave = uniqueGeoObjects
                .Where(geoObject => dbGeoObjects.Any(dbGeoObject => dbGeoObject.Id == geoObject.Id) == false)
                .ToArray();

            await _repository.AddRangeAsync(geoObjectsToSave);
        }

        private async Task SaveForecastsAsync(Forecast[] forecasts)
        {
            var dates = forecasts.Select(x => x.Date).ToArray();
            var geoObjectIds = forecasts.Select(x => x.GeoObjectId).ToArray();

            var dbForecasts = await _repository
                .Query<Forecast>()
                .Where(x => dates.Contains(x.Date))
                .Where(x => geoObjectIds.Contains(x.GeoObjectId))
                .ToArrayAsync();

            var forecastsToAdd = new List<Forecast>();
            var forecastsToUpdate = new List<Forecast>();

            foreach (var forecast in forecasts)
            {
                var dbForecast = dbForecasts
                    .Where(x => x.GeoObjectId == forecast.GeoObjectId)
                    .Where(x => x.Date == forecast.Date)
                    .FirstOrDefault();

                if (dbForecast == null)
                {
                    forecastsToAdd.Add(forecast);
                }
                else
                {
                    dbForecast.UpdateData(forecast);
                    forecastsToUpdate.Add(dbForecast);
                }
            }

            await _repository.AddRangeAsync(forecastsToAdd);
            await _repository.UpdateRangeAsync(forecastsToUpdate);
        }
    }
}
