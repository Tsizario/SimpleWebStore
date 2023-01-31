using SimpleWebStore.Domain.Abstractions;
using System.Linq.Expressions;

namespace SimpleWebStore.DAL.Repositories.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> AllItems { get; }

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync(Expression<Func<TEntity, bool>> filter = null, string includeProps = null);

        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>>? filter = null, bool tracked = true, string includeProps = null);

        Task<TEntity> AddEntityAsync(TEntity entity);

        Task<TEntity> UpdateEntityAsync(TEntity entity);

        Task<bool> RemoveEntityAsync(Guid id);

        Task<bool> RemoveEntitiesAsync(IEnumerable<TEntity> entities);
    }
}