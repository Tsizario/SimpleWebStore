using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.CoverTypeRepository
{
    public class CoverTypeRepository : GenericRepository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CoverTypeRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
