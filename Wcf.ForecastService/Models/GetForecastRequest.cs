using System;

namespace Wcf.ForecastService.Models
{
    public class GetForecastRequest
    {
        public string GeoObjectName { get; set; }
        public DateTime Date { get; set; }
    }
}
