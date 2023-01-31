using Microsoft.EntityFrameworkCore;
using SimpleWebStore.DAL.Repositories.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public override async Task<Product> UpdateEntityAsync(Product entity)
        //{
        //    var objFromDb = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == entity.Id);

        //    if (objFromDb != null)
        //    {
        //        objFromDb.Title = entity.Title;
        //        objFromDb.ISBN = entity.ISBN;
        //        //objFromDb.ListPrice = entity.ListPrice;
        //        //objFromDb. = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //        //objFromDb.Title = entity.Title;
        //    }
        //}
    }
}
