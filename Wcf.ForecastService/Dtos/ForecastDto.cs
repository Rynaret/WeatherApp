using Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Wcf.ForecastService.Dtos
{
    [DataContract]
    public class ForecastDto
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public double Precipitation { get; set; }

        [DataMember]
        public double MaxTemperature { get; set; }

        [DataMember]
        public double MinTemperature { get; set; }

        public ForecastDto(Forecast forecast)
        {
            if (forecast == null)
            {
                return;
            }

            Date = forecast.Date;
            Precipitation = forecast.Precipitation;
            MaxTemperature = forecast.MaxTemperature;
            MinTemperature = forecast.MinTemperature;
        }
    }
}
