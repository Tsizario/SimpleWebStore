namespace SimpleWebStore.DAL.Repositories.OrderHeaderRepository
{
    public interface IOrderHeaderRepository
    {
        Task<bool> UpdateStatus(Guid id, string status, string? paymentStatus = null);
    }
}