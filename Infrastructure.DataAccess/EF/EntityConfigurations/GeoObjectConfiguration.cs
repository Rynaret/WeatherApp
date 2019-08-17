using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EF.EntityConfigurations
{
    public class GeoObjectConfiguration : IEntityTypeConfiguration<GeoObject>
    {
        public void Configure(EntityTypeBuilder<GeoObject> builder)
        {
            builder.ToTable("GeoObjects");
        }
    }
}
