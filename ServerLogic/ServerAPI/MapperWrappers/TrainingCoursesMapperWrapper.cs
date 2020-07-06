using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;
using System.Collections.Generic;

namespace ServerAPI.MapperWrappers
{
    public class TrainingCoursesMapperWrapper : IMapperWrapper<TrainingCourse, TrainingCourseDTO>
    {
        private readonly IMapper mapper;

        public TrainingCoursesMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<TrainingCourse> MapCollectionFromDTOs(IEnumerable<TrainingCourseDTO> dtos)
        {
            return mapper.Map<IEnumerable<TrainingCourseDTO>, IEnumerable<TrainingCourse>>(dtos);
        }

        public IEnumerable<TrainingCourseDTO> MapCollectionFromEntities(IEnumerable<TrainingCourse> entities)
        {
            return mapper.Map<IEnumerable<TrainingCourse>, IEnumerable<TrainingCourseDTO>>(entities);
        }

        public TrainingCourse MapFromDTO(TrainingCourseDTO dto)
        {
            return mapper.Map<TrainingCourseDTO, TrainingCourse>(dto);
        }

        public TrainingCourseDTO MapFromEntity(TrainingCourse entity)
        {
            return mapper.Map<TrainingCourse, TrainingCourseDTO>(entity);
        }
    }
}
