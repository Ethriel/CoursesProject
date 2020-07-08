using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;
using System.Collections.Generic;

namespace ServerAPI.MapperWrappers
{
    public class SystemUsersTrainingCoursesMapperWrapper : IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>
    {
        private readonly IMapper mapper;

        public SystemUsersTrainingCoursesMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<SystemUsersTrainingCourses> MapCollectionFromDTOs(IEnumerable<SystemUsersTrainingCoursesDTO> dtos)
        {
            return mapper.Map<IEnumerable<SystemUsersTrainingCoursesDTO>, IEnumerable<SystemUsersTrainingCourses>>(dtos);
        }

        public IEnumerable<SystemUsersTrainingCoursesDTO> MapCollectionFromEntities(IEnumerable<SystemUsersTrainingCourses> entities)
        {
            return mapper.Map<IEnumerable<SystemUsersTrainingCourses>, IEnumerable<SystemUsersTrainingCoursesDTO>>(entities);
        }

        public SystemUsersTrainingCourses MapFromDTO(SystemUsersTrainingCoursesDTO dto)
        {
            return mapper.Map<SystemUsersTrainingCoursesDTO, SystemUsersTrainingCourses>(dto);
        }

        public SystemUsersTrainingCoursesDTO MapFromEntity(SystemUsersTrainingCourses entity)
        {
            return mapper.Map<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>(entity);
        }
    }
}
