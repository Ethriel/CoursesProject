using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;

namespace ServerAPI.MapperProfiles
{
    public class SystemUserMapperProfile : Profile
    {
        public SystemUserMapperProfile()
        {
            CreateMap<SystemUser, SystemUserDTO>()
                .ForMember(sudto => sudto.RoleName, o => o.MapFrom(su => su.SystemRole.Name));
            CreateMap<SystemUserDTO, SystemUser>();
        }
    }
}
