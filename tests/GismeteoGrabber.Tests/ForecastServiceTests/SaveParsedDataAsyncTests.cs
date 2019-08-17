using AutoFixture;
using Domain.Entities;
using GismeteoGrabber.Implementations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GismeteoGrabber.Tests.ForecastServiceTests
{
    public class SaveParsedDataAsyncTests : TestBase
    {
        [Fact]
        public async Task ShouldAddCitiesAndForecasts()
        {
            var repository = GetInMemoryRepository();

            var fixture = new Fixture();
            var forecasts = fixture.CreateMany<Forecast>(2).ToArray();
            var geoObjects = forecasts.Select(x => x.GeoObject).ToArray();

            var sut = new ForecastService(repository);
            await sut.SaveParsedDataAsync(forecasts);

            Assert.True(repository.Read<GeoObject>().Count() == 2);
            Assert.True(repository.Read<Forecast>().Count() == 2);
        }

        [Fact]
        public async Task ShouldNotAddCities_IfTheSameInDb()
        {
            var repository = GetInMemoryRepository();

            var fixture = new Fixture();
            var forecasts = fixture.CreateMany<Forecast>(2).ToArray();
            var geoObjects = forecasts.Select(x => x.GeoObject).ToArray();

            await repository.AddRangeAsync(geoObjects);
            await repository.SaveChangesAsync();

            var sut = new ForecastService(repository);
            await sut.SaveParsedDataAsync(forecasts);

            Assert.True(repository.Read<GeoObject>().Count() == 2);
            Assert.True(repository.Read<Forecast>().Count() == 2);
        }

        [Fact]
        public async Task ShouldUpdateForecast_IfGeoObjectAndDateAreSame()
        {
            var repository = GetInMemoryRepository();

            var fixture = new Fixture();
            var forecasts = fixture.CreateMany<Forecast>(2).ToArray();
            var geoObjects = forecasts.Select(x => x.GeoObject).ToArray();

            await repository.AddRangeAsync(geoObjects);
            await repository.AddRangeAsync(forecasts);
            await repository.SaveChangesAsync();

            var checkingForecast = forecasts.First();
            checkingForecast.Precipitation += 1;
            checkingForecast.MaxTemperature += 1;
            checkingForecast.MinTemperature += 1;

            var sut = new ForecastService(repository);
            await sut.SaveParsedDataAsync(forecasts);

            Assert.True(repository.Query<GeoObject>().Count() == 2);
            Assert.True(repository.Query<Forecast>().Count() == 2);

            var dbCheckingForecast = repository.Query<Forecast>().First(x => x.Id == checkingForecast.Id);
            Assert.True(dbCheckingForecast.MaxTemperature == checkingForecast.MaxTemperature);
            Assert.True(dbCheckingForecast.MinTemperature == checkingForecast.MinTemperature);
            Assert.True(dbCheckingForecast.Precipitation == checkingForecast.Precipitation);
        }
    }
}
