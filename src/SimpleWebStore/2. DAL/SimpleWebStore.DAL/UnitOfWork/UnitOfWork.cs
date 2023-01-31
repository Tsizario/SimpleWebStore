using SimpleWebStore.DAL.Repositories.CategoryRepository;
using SimpleWebStore.DAL.Repositories.CoverTypeRepository;

namespace SimpleWebStore.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            CategoryRepository = new CategoryRepository(_dbContext);
            CoverTypeRepository = new CoverTypeRepository(_dbContext);
        }

        public ICategoryRepository CategoryRepository { get; private set; }

        public ICoverTypeRepository CoverTypeRepository { get; private set; }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
