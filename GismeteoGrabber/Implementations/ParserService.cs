using Domain.Entities;
using GismeteoGrabber.Exceptions;
using GismeteoGrabber.Extensions;
using GismeteoGrabber.Interfaces;
using GismeteoGrabber.Models;
using GismeteoGrabber.Settings;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GismeteoGrabber.Implementations
{
    public class ParserService : IParserService
    {
        #region ctor

        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ParserService> _logger;

        public ParserService(IOptions<AppSettings> options, HttpClient httpClient, ILogger<ParserService> logger)
        {
            _appSettings = options.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        #endregion

        public async Task<Forecast[]> ParseAsync()
        {
            var geoObjectsData = await GetGeoObjectsDataAsync();

            var date = DateTime.Now.Date;
            var citiesForecast = await geoObjectsData.ParallelForEachAsync(
                x =>
                {
                    try
                    {
                        return ParseForecastAsync(x, date);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"An error occured for {JsonConvert.SerializeObject(x)}");
                        throw;
                    }
                },
                _appSettings.System.DegreeOfParallelism
            );

            return citiesForecast.SelectMany(x => x).ToArray();
        }

        public async Task<Forecast[]> ParseForecastAsync(GeoObjectModel geoObject, DateTime date)
        {
            var response = await _httpClient.GetAsync($"{geoObject.Url}{_appSettings.Gismeteo.TenDaysUrlPart}");
            var html = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var precipitationDivs = htmlDoc.DocumentNode
                .SelectNodes(_appSettings.Gismeteo.PrecipitationXPath);
            var precipitationValues = precipitationDivs
                ?.Select(x => x.InnerText.ToDouble())
                ?.ToArray();

            var maxTemperatureDivs = htmlDoc.DocumentNode
                .SelectNodes(_appSettings.Gismeteo.MaxTemperatureXPath);
            var maxTemperatureValues = maxTemperatureDivs
                .Select(x => x.InnerText.ToDouble())
                .ToArray();

            var minTemperatureDivs = htmlDoc.DocumentNode
                .SelectNodes(_appSettings.Gismeteo.MinTemperatureXPath);
            var minTemperatureValues = minTemperatureDivs
                .Select(x => x.InnerText.ToDouble())
                .ToArray();

            var geoObjectIdRegex = new Regex("(\\d+)");

            var result = new Forecast[10];
            for (int i = 0; i < 10; i++)
            {
                result[i] = new Forecast
                {
                    GeoObjectId = geoObject.Id,
                    GeoObject = new GeoObject { Id = geoObject.Id, Name = geoObject.Name },
                    Date = date.AddDays(i),
                    MaxTemperature = maxTemperatureValues[i],
                    MinTemperature = minTemperatureValues[i],
                    Precipitation = precipitationValues != null ? precipitationValues[i] : 0
                };
            }
            return result;
        }

        public async Task<GeoObjectModel[]> GetGeoObjectsDataAsync()
        {
            var html = await _httpClient.GetStringAsync("");

            var blockWithCities = GetBlockWithCities(html);

            return GetGeoObjectsData(blockWithCities);
        }

        private string GetBlockWithCities(string html)
        {
            var cityFrameReg = new Regex(_appSettings.Gismeteo.CityFrameRegex, RegexOptions.Singleline);
            var blockWithCities = cityFrameReg.Match(html);

            if (blockWithCities.Success == false)
            {
                throw new ParsingException("Cannot find a block with cities!");
            }

            return blockWithCities.Value;
        }

        private GeoObjectModel[] GetGeoObjectsData(string blockWithCities)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(blockWithCities);

            var geoObjectSettings = _appSettings.Gismeteo.GeoObject;
            var aTags = htmlDoc.DocumentNode.SelectNodes(geoObjectSettings.XPath);

            var needAttributes = new[] { geoObjectSettings.IdAttr, geoObjectSettings.UrlAttr, geoObjectSettings.NameAttr };
            var allAttributesPrecense = aTags
                .All(x => x.Attributes
                    .Select(attr => attr.Name)
                    .Intersect(needAttributes).Count() == needAttributes.Length
                );

            if (allAttributesPrecense == false)
            {
                throw new ParsingException("Some attributes were not found!");
            }

            return aTags
                .Select(x => new GeoObjectModel
                {
                    Id = long.Parse(x.Attributes[geoObjectSettings.IdAttr].Value),
                    Url = x.Attributes[geoObjectSettings.UrlAttr].Value,
                    Name = x.Attributes[geoObjectSettings.NameAttr].Value
                })
                .ToArray();
        }
    }
}
