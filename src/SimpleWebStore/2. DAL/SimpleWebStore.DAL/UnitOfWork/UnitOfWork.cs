using SimpleWebStore.DAL.Repositories.CategoryRepository;
using SimpleWebStore.DAL.Repositories.CompanyRepository;
using SimpleWebStore.DAL.Repositories.CoverTypeRepository;
using SimpleWebStore.DAL.Repositories.ProductRepository;

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
            ProductRepository = new ProductRepository(_dbContext);
            CompanyRepository = new CompanyRepository(_dbContext);
        }

        public ICategoryRepository CategoryRepository { get; private set; }

        public ICoverTypeRepository CoverTypeRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ICompanyRepository CompanyRepository { get; private set; }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
