using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Constants;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.OrderHeaderRepository
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderHeaderRepository(ApplicationDbContext dbContext,
            IMapper mapper)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;   
        }

        public override async Task<OrderHeader> UpdateEntityAsync(OrderHeader updatedEntity)
        {
            var objFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(p => p.Id == updatedEntity.Id);

            if (objFromDb == null)
            {
                return null;
            }

            _mapper.Map(updatedEntity, objFromDb);
            _dbContext.Entry(objFromDb).State = EntityState.Modified;

            return objFromDb;
        }

        public async Task<bool> UpdateStatus(Guid id, string status, string? paymentStatus = null)
        {
            var orderFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(o => o.Id == id);

            if (orderFromDb == null)
            {
                return false;
            }

            var newOrderHeader = new OrderHeader()
            {
                OrderStatus = status,
                PaymentStatus = paymentStatus
            };

            _mapper.Map(newOrderHeader, orderFromDb);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStripePaymentId(Guid id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(o => o.Id == id);

            if (orderFromDb == null)
            {
                return false;
            }

            orderFromDb.PaymentDate = DateTime.Now;
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentStatus = paymentIntentId;

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}