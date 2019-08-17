using GismeteoGrabber.Settings;
using Infrastructure.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;

namespace GismeteoGrabber.Tests
{
    public class TestBase
    {
        public Mock<IOptions<AppSettings>> GetMockOptions()
        {
            var appSettings = new AppSettings();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            configuration.Bind(appSettings);

            var mockOptions = new Mock<IOptions<AppSettings>>();
            mockOptions.Setup(x => x.Value).Returns(appSettings);

            return mockOptions;
        }

        public HttpClient GetMockHttpClient(string respondContent)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("*").Respond("text/html", respondContent);
            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            return client;
        }

        public EfRepository GetInMemoryRepository()
        {
            return new EfRepository(GetInMemoryDbContext());
        }

        public WeatherDbContext GetInMemoryDbContext()
        {
            var root = new InMemoryDatabaseRoot();
            var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
            optionsBuilder.UseInMemoryDatabase("in-memory", root);
            return new WeatherDbContext(optionsBuilder.Options);
        }
    }
}
