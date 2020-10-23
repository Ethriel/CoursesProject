using AutoMapper;
using ServicesAPI.DTO;
using Infrastructure.Models;
using System.Collections.Generic;

namespace ServicesAPI.MapperWrappers
{
    public class SystemUsersTrainingCoursesMapperWrapper : IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>
    {
        private readonly IMapper mapper;

        public SystemUsersTrainingCoursesMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<SystemUsersTrainingCourses> MapEntities(IEnumerable<SystemUsersTrainingCoursesDTO> dtos)
        {
            return mapper.Map<IEnumerable<SystemUsersTrainingCoursesDTO>, IEnumerable<SystemUsersTrainingCourses>>(dtos);
        }

        public IEnumerable<SystemUsersTrainingCoursesDTO> MapModels(IEnumerable<SystemUsersTrainingCourses> entities)
        {
            return mapper.Map<IEnumerable<SystemUsersTrainingCourses>, IEnumerable<SystemUsersTrainingCoursesDTO>>(entities);
        }

        public SystemUsersTrainingCourses MapEntity(SystemUsersTrainingCoursesDTO dto)
        {
            return mapper.Map<SystemUsersTrainingCoursesDTO, SystemUsersTrainingCourses>(dto);
        }

        public SystemUsersTrainingCoursesDTO MapModel(SystemUsersTrainingCourses entity)
        {
            return mapper.Map<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>(entity);
        }
    }
}
