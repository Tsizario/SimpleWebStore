using SimpleWebStore.DAL.Repositories.CategoryRepository;

namespace SimpleWebStore.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            CategoryRepository = new CategoryRepository(_dbContext);
        }

        public ICategoryRepository CategoryRepository { get; private set; }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
