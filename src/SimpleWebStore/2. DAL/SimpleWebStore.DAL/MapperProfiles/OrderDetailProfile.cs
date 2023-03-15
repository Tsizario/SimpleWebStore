using AutoMapper;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.MapperProfiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetail>()
                .ForAllMembers(opts => opts.Condition((source, destination, sourceMember) =>
                    sourceMember != null));
        }
    }
}
