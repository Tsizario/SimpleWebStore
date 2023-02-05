using Microsoft.EntityFrameworkCore;
using SimpleWebStore.Domain.Abstractions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleWebStore.DAL.Repositories.Abstractions
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> AllItems => _dbSet;

        public virtual async Task<IEnumerable<TEntity>> GetAllEntitiesAsync(
            Expression<Func<TEntity, bool>> filter = null, 
            string includeProps = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProps != null)
            {
                foreach (var includeProperty in includeProps.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetEntityAsync(
            Expression<Func<TEntity, bool>>? filter = null, 
            bool tracked = true, 
            string includeProps = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProps != null)
            {
                foreach (var includeProperty in includeProps.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public virtual Task<TEntity> UpdateEntityAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);

            return Task.FromResult(entity);
        }

        public virtual async Task<bool> RemoveEntityAsync(Guid id)
        {
            TEntity candidate = await AllItems.FirstOrDefaultAsync(e => e.Id == id);

            if (candidate == null)
            {
                return false;
            }

            _dbSet.Remove(candidate);

            return true;
        }

        public virtual async Task<bool> RemoveEntitiesAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                return false;
            }

            _dbSet.RemoveRange(entities);

            return true;
        }
    }
}