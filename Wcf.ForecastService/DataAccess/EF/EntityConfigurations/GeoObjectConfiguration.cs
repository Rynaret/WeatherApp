using Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Wcf.ForecastService.DataAccess.EF.EntityConfigurations
{
    public class GeoObjectConfiguration : EntityTypeConfiguration<GeoObject>
    {
        public GeoObjectConfiguration()
        {
            ToTable("GeoObjects");
        }
    }
}
