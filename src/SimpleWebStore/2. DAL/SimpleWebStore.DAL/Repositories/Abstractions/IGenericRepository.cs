using SimpleWebStore.Domain.Abstractions;
using System.Linq.Expressions;

namespace SimpleWebStore.DAL.Repositories.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> AllItems { get; }

        Task<List<TEntity>> GetAllEntitiesAsync(Expression<Func<TEntity, bool>> filter = null, string includeProps = null);

        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>>? filter = null, bool tracked = true, string includeProps = null);

        Task<TEntity> AddEntityAsync(TEntity entity);

        Task<TEntity> UpdateEntityAsync(TEntity entity);

        Task<bool> RemoveEntityAsync(TEntity entity);

        Task<bool> RemoveEntitiesAsync(IEnumerable<TEntity> entities);
    }
}