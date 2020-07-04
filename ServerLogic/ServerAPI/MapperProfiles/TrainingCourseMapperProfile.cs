using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;

namespace ServerAPI.MapperProfiles
{
    public class TrainingCourseMapperProfile : Profile
    {
        public TrainingCourseMapperProfile()
        {
            CreateMap<TrainingCourse, TrainingCourseDTO>();
            CreateMap<TrainingCourseDTO, TrainingCourse>();
        }
    }
}
