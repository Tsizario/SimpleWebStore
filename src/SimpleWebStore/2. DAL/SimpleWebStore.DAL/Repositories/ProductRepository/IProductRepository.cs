using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
    }
}