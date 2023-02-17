using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext dbContext,
            IMapper mapper) 
            : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public override async Task<Product> UpdateEntityAsync(Product updatedEntity)
        {
            var objFromDb = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == updatedEntity.Id);

            if (objFromDb == null)
            {
                return null;
            }

            _mapper.Map(updatedEntity, objFromDb);
            _dbContext.Entry(objFromDb).State = EntityState.Modified;

            return objFromDb;
        }
    }
}
