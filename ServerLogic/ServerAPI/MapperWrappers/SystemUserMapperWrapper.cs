using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;
using System.Collections.Generic;

namespace ServerAPI.MapperWrappers
{
    public class SystemUserMapperWrapper : IMapperWrapper<SystemUser, SystemUserDTO>
    {
        private readonly IMapper mapper;

        public SystemUserMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<SystemUser> MapCollectionFromDTOs(IEnumerable<SystemUserDTO> dtos)
        {
            return mapper.Map<IEnumerable<SystemUserDTO>, IEnumerable<SystemUser>>(dtos);
        }

        public IEnumerable<SystemUserDTO> MapCollectionFromEntities(IEnumerable<SystemUser> entities)
        {
            return mapper.Map<IEnumerable<SystemUser>, IEnumerable<SystemUserDTO>>(entities);
        }

        public SystemUser MapFromDTO(SystemUserDTO dto)
        {
            return mapper.Map<SystemUserDTO, SystemUser>(dto);
        }

        public SystemUserDTO MapFromEntity(SystemUser entity)
        {
            return mapper.Map<SystemUser, SystemUserDTO>(entity);
        }
    }
}
