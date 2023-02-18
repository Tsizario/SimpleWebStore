using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.OrderHeaderRepository
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        Task<bool> UpdateStatus(Guid id, string status, string? paymentStatus = null);
        Task<bool> UpdateStripePaymentId(Guid id, string sessionId, string? paymentIntentId);
    }
}