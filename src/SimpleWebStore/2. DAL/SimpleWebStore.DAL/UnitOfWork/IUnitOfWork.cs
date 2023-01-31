using SimpleWebStore.DAL.Repositories.CategoryRepository;
using SimpleWebStore.DAL.Repositories.CoverTypeRepository;

namespace SimpleWebStore.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ICoverTypeRepository CoverTypeRepository { get; }

        Task<bool> SaveAsync();
    }
}
