using AutoMapper;
using Grad_Project.DTO;
using Grad_Project.Entity;

namespace Grad_Project.Mapper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<Device, CreateDeviceDto>();
            CreateMap<CreateDeviceDto,Device>().ForMember(x => x.picUrl, y => y.Ignore());
            CreateMap<Device, UpdateDeviceDto>();
            CreateMap<UpdateDeviceDto, Device>().ForMember(x => x.picUrl, y => y.Ignore());
            CreateMap<CreateCounterDataDto, CounterData>().ReverseMap();
            CreateMap<CreateAreaDto, Area>().ReverseMap();
            CreateMap<UpdateAreaDto, Area>().ReverseMap();
            CreateMap<SubAreaDto, SubArea>().ReverseMap();
            CreateMap<UserDeviceDto, UserDevice>().ReverseMap();

            

        }
    }
}
