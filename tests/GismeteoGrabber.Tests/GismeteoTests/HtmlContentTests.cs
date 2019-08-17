using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GismeteoGrabber.Tests.GismeteoTests
{
    public class HtmlContentTests : TestBase
    {
        [Fact]
        public async Task ShouldReturnMainPageHtml()
        {
            var mockOptions = GetMockOptions();
            var appSettings = mockOptions.Object.Value;

            var httpClient = new HttpClient
            {
                BaseAddress = appSettings.Gismeteo.BaseUrl
            };
            var response = await httpClient.GetAsync("");

            Assert.True(response.IsSuccessStatusCode);
            var html = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(html);
        }

        [Fact]
        public async Task ShouldReturnTenDaysForecastHtml()
        {
            var mockOptions = GetMockOptions();
            var appSettings = mockOptions.Object.Value;

            var httpClient = new HttpClient
            {
                BaseAddress = appSettings.Gismeteo.BaseUrl
            };
            var response = await httpClient.GetAsync($"/weather-moscow-4368/{appSettings.Gismeteo.TenDaysUrlPart}");

            Assert.True(response.IsSuccessStatusCode);
            var html = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(html);
        }
    }
}
