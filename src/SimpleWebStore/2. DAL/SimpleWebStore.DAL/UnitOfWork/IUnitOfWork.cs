using SimpleWebStore.DAL.Repositories.CategoryRepository;
using SimpleWebStore.DAL.Repositories.CompanyRepository;
using SimpleWebStore.DAL.Repositories.CoverTypeRepository;
using SimpleWebStore.DAL.Repositories.ProductRepository;

namespace SimpleWebStore.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ICoverTypeRepository CoverTypeRepository { get; }
        IProductRepository ProductRepository { get; }
        ICompanyRepository CompanyRepository { get; }

        Task<bool> SaveAsync();
    }
}
