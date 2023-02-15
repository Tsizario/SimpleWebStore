using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.DAL.Repositories.CompanyRepository;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.OrderRepository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderDetailRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}