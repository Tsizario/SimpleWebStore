using AutoMapper;
using SimpleWebStore.DAL.Repositories.AppUserRepository;
using SimpleWebStore.DAL.Repositories.CategoryRepository;
using SimpleWebStore.DAL.Repositories.CompanyRepository;
using SimpleWebStore.DAL.Repositories.CoverTypeRepository;
using SimpleWebStore.DAL.Repositories.OrderHeaderRepository;
using SimpleWebStore.DAL.Repositories.OrderRepository;
using SimpleWebStore.DAL.Repositories.ProductRepository;
using SimpleWebStore.DAL.Repositories.ShoppingCartRepository;

namespace SimpleWebStore.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            CategoryRepository = new CategoryRepository(_dbContext);
            CoverTypeRepository = new CoverTypeRepository(_dbContext);
            ProductRepository = new ProductRepository(_dbContext, _mapper);
            CompanyRepository = new CompanyRepository(_dbContext);
            ShoppingCartRepository = new ShoppingCartRepository(_dbContext);
            AppUserRepository = new AppUserRepository(_dbContext);
            OrderDetailRepository = new OrderDetailRepository(_dbContext);
            OrderHeaderRepository = new OrderHeaderRepository(_dbContext, _mapper);
        }

        public ICategoryRepository CategoryRepository { get; private set; }

        public ICoverTypeRepository CoverTypeRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ICompanyRepository CompanyRepository { get; private set; }

        public IShoppingCartRepository ShoppingCartRepository { get; private set; }

        public IAppUserRepository AppUserRepository { get; private set; }

        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
