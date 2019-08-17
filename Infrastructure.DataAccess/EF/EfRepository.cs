using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EF
{
    public class EfRepository : IRepository
    {
        #region ctor

        private readonly WeatherDbContext _dbContext;

        public EfRepository(WeatherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        public Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            return _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public IQueryable<TEntity> Read<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>().AsQueryable().AsNoTracking();
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }

        public Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
