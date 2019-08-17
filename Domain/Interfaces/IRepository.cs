using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository
    {
        IQueryable<TEntity> Read<TEntity>() where TEntity : class;

        IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        Task SaveChangesAsync();
    }
}
