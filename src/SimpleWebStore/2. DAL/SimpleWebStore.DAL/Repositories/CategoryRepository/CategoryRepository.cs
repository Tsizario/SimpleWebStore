using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.CategoryRepository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override Task<Category> UpdateEntityAsync(Category item)
        {
            _dbContext.Categories.Update(item);

            return Task.FromResult(item);
        }
    }
}
