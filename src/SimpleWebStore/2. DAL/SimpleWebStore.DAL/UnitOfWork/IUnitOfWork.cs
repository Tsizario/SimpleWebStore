using SimpleWebStore.DAL.Repositories.CategoryRepository;

namespace SimpleWebStore.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }

        Task<bool> SaveAsync();
    }
}
