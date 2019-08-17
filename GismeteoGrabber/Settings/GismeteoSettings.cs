using System;

namespace GismeteoGrabber.Settings
{
    public class GismeteoSettings
    {
        public Uri BaseUrl { get; set; }

        public GeoObjectSettings GeoObject { get; set; }

        public string TenDaysUrlPart { get; set; }
        public string CityFrameRegex { get; set; }
        public string PrecipitationXPath { get; set; }
        public string MaxTemperatureXPath { get; set; }
        public string MinTemperatureXPath { get; set; }
    }
}
