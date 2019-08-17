using Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Wcf.ForecastService.DataAccess.EF.EntityConfigurations
{
    public class ForecastConfiguration : EntityTypeConfiguration<Forecast>
    {
        public ForecastConfiguration()
        {
            HasRequired(x => x.GeoObject).WithMany().HasForeignKey(x => x.GeoObjectId);
            ToTable("Forecasts");
        }
    }
}
