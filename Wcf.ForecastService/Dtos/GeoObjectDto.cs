using Domain.Entities;
using System.Runtime.Serialization;

namespace Wcf.ForecastService.Dtos
{
    [DataContract]
    public class GeoObjectDto
    {
        [DataMember]
        public string Name { get; set; }

        public GeoObjectDto(GeoObject geoObject)
        {
            Name = geoObject.Name;
        }
    }
}
