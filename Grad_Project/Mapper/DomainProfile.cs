using AutoMapper;
using Grad_Project.DTO;
using Grad_Project.Entity;

namespace Grad_Project.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<CounterData, CounterDataDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Counter.User.Address))
                .ForMember(dest => dest.IsTheftReported, opt => opt.Ignore());
            CreateMap<CreateCounterDataDto, CounterData>();
            CreateMap<Address, AddressDto>();
        }
    }
}