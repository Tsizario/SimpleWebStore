using AutoMapper;
using SimpleWebStore.Domain.Abstractions;
using SimpleWebStore.Domain.Entities;

namespace SimpleWebStore.DAL.MapperProfiles
{
    public class OrderHeaderProfile : Profile
    {
        public OrderHeaderProfile()
        {
            CreateMap<OrderHeader, OrderHeader>()
                .ForMember(src => src.Id, opt => opt.Ignore())
                .ForMember(src => src.OrderTotal, opt => opt.Ignore())
                .ForMember(src => src.OrderDate, opt => opt.Ignore())
                .ForMember(src => src.PaymentDate, opt => opt.Ignore())
                .ForMember(src => src.PaymentStatus, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, sourceMember) =>
                    sourceMember != null));

            CreateMap<AppUser, OrderHeader>()
                .ForAllMembers(opts => opts.Condition((source, destination, sourceMember) => 
                    sourceMember != null));
        }
    }
}
