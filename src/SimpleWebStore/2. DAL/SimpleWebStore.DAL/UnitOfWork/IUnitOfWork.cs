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
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ICoverTypeRepository CoverTypeRepository { get; }
        IProductRepository ProductRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IAppUserRepository AppUserRepository { get; }
        IOrderHeaderRepository OrderHeaderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }

        Task<bool> SaveAsync();
    }
}