using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.CompanyRepository
{
    internal class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        internal CompanyRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
