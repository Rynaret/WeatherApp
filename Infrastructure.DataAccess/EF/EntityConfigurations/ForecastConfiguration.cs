using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EF.EntityConfigurations
{
    public class ForecastConfiguration : IEntityTypeConfiguration<Forecast>
    {
        public void Configure(EntityTypeBuilder<Forecast> builder)
        {
            builder.HasOne(x => x.GeoObject).WithMany().HasForeignKey(x => x.GeoObjectId);

            builder.ToTable("Forecasts");
        }
    }
}
