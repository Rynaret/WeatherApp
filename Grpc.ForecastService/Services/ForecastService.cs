using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcForecastService
{
    public class ForecastService : Forecaster.ForecasterBase
    {
        #region ctor

        private readonly IServiceProvider _provider;

        public ForecastService(IServiceProvider provider)
        {
            _provider = provider;
        }

        #endregion

        public override async Task<AvailableGeoObjectsResponse> GetAvailableGeoObjectsAsync(Empty request, ServerCallContext context)
        {
            var scoped = _provider.CreateScope();
            var geoObjects = await scoped.ServiceProvider.GetService<IRepository>()
                .Read<GeoObject>()
                .Select(x => new GeoObjectDto
                {
                    Name = x.Name
                })
                .ToListAsync();
            var response = new AvailableGeoObjectsResponse();
            response.GeoObjects.AddRange(geoObjects);
            return response;
        }


        public override async Task<ForecastDto> GetForecastAsync(GetForecastRequest request, ServerCallContext context)
        {
            var scoped = _provider.CreateScope();
            var requestDate = request.Date.ToDateTime().Date;
            var forecast = await scoped.ServiceProvider.GetService<IRepository>()
                .Query<Forecast>()
                .Where(x => x.GeoObject.Name == request.GeoObjectName)
                .Where(x => x.Date == requestDate)
                .FirstOrDefaultAsync();

            return new ForecastDto
            {
                Date = Timestamp.FromDateTime(forecast.Date),
                MaxTemperature = forecast.MaxTemperature,
                MinTemperature = forecast.MinTemperature,
                Precipitation = forecast.Precipitation
            };
        }
    }
}
