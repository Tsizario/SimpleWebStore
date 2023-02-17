using AutoMapper;   
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, Product>()
                .ForAllMembers(opts => opts.Condition((source, destination, sourceMember) =>
                    sourceMember != null));
        }
    }
}
