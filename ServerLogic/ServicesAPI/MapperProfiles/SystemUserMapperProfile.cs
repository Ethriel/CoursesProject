using AutoMapper;
using ServicesAPI.DTO;
using Infrastructure.Models;
using System;

namespace ServicesAPI.MapperProfiles
{
    public class SystemUserMapperProfile : Profile
    {
        public SystemUserMapperProfile()
        {
            CreateMap<SystemUser, SystemUserDTO>()
                .ForMember(u => u.RoleName, o => o.MapFrom(su => su.SystemRole.Name))
                .ForMember(u => u.BirthDate, o => o.MapFrom(su => su.BirthDate.ToShortDateString()))
                .ForMember(u => u.RegisteredDate, o => o.MapFrom(su => su.RegisteredDate.ToShortDateString()));


            CreateMap<SystemUserDTO, SystemUser>()
                .ForMember(su => su.BirthDate, o => o.MapFrom(u => DateTime.Parse(u.BirthDate)))
                .ForMember(su => su.RegisteredDate, o => o.MapFrom(u => DateTime.Parse(u.BirthDate)));
        }
    }
}
