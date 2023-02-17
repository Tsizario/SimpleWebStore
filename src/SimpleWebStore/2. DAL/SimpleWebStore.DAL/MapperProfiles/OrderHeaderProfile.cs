using AutoMapper;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.MapperProfiles
{
    public class OrderHeaderProfile : Profile
    {
        public OrderHeaderProfile()
        {
            CreateMap<AppUser, OrderHeader>()
                .ForAllMembers(opts => opts.Condition((source, destination, sourceMember) => 
                    sourceMember != null));
        }
    }
}
