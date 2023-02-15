using Microsoft.EntityFrameworkCore;
using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.OrderHeaderRepository
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderHeaderRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UpdateStatus(Guid id, string status, string? paymentStatus = null)
        {
            var orderFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(o => o.Id == id);

            if (orderFromDb == null)
            {
                return false;
            }

            orderFromDb.OrderStatus = status;

            if (paymentStatus != null)
            {
                orderFromDb.PaymentStatus = paymentStatus;
            }

            return true;
        }
    }
}