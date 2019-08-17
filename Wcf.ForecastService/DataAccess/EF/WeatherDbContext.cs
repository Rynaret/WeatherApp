using Domain.Entities;
using System.Data.Entity;
using System.Reflection;

namespace Wcf.DataAccess.EF
{
    public class WeatherDbContext : DbContext
    {

        public WeatherDbContext() : base("Weather")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
