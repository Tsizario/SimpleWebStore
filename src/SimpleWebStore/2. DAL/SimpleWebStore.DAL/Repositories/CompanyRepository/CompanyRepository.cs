using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.CompanyRepository
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
