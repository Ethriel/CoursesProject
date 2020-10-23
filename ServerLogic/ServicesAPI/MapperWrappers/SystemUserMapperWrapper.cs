using AutoMapper;
using ServicesAPI.DTO;
using Infrastructure.Models;
using System.Collections.Generic;

namespace ServicesAPI.MapperWrappers
{
    public class SystemUserMapperWrapper : IMapperWrapper<SystemUser, SystemUserDTO>
    {
        private readonly IMapper mapper;

        public SystemUserMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<SystemUser> MapEntities(IEnumerable<SystemUserDTO> dtos)
        {
            return mapper.Map<IEnumerable<SystemUserDTO>, IEnumerable<SystemUser>>(dtos);
        }

        public IEnumerable<SystemUserDTO> MapModels(IEnumerable<SystemUser> entities)
        {
            return mapper.Map<IEnumerable<SystemUser>, IEnumerable<SystemUserDTO>>(entities);
        }

        public SystemUser MapEntity(SystemUserDTO dto)
        {
            return mapper.Map<SystemUserDTO, SystemUser>(dto);
        }

        public SystemUserDTO MapModel(SystemUser entity)
        {
            return mapper.Map<SystemUser, SystemUserDTO>(entity);
        }
    }
}
