using System;

namespace Domain.Entities
{
    public class Forecast : EntityBase
    {
        public DateTime Date { get; set; }

        public double Precipitation { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }

        public long GeoObjectId { get; set; }

        public GeoObject GeoObject { get; set; }

        public void UpdateData(Forecast forecast)
        {
            UpdateData(forecast.Precipitation, forecast.MaxTemperature, forecast.MinTemperature);
        }

        public void UpdateData(double precipitation, double maxTemperature, double minTemperature)
        {
            Precipitation = precipitation;
            MaxTemperature = maxTemperature;
            MinTemperature = minTemperature;
        }
    }
}
