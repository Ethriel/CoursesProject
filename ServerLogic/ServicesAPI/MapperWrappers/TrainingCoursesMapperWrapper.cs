using AutoMapper;
using ServicesAPI.DTO;
using Infrastructure.Models;
using System.Collections.Generic;

namespace ServicesAPI.MapperWrappers
{
    public class TrainingCoursesMapperWrapper : IMapperWrapper<TrainingCourse, TrainingCourseDTO>
    {
        private readonly IMapper mapper;

        public TrainingCoursesMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<TrainingCourse> MapEntities(IEnumerable<TrainingCourseDTO> dtos)
        {
            return mapper.Map<IEnumerable<TrainingCourseDTO>, IEnumerable<TrainingCourse>>(dtos);
        }

        public IEnumerable<TrainingCourseDTO> MapModels(IEnumerable<TrainingCourse> entities)
        {
            return mapper.Map<IEnumerable<TrainingCourse>, IEnumerable<TrainingCourseDTO>>(entities);
        }

        public TrainingCourse MapEntity(TrainingCourseDTO dto)
        {
            return mapper.Map<TrainingCourseDTO, TrainingCourse>(dto);
        }

        public TrainingCourseDTO MapModel(TrainingCourse entity)
        {
            return mapper.Map<TrainingCourse, TrainingCourseDTO>(entity);
        }
    }
}
