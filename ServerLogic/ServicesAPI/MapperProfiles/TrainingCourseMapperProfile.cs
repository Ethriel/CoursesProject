using AutoMapper;
using ServicesAPI.DTO;
using Infrastructure.Models;

namespace ServicesAPI.MapperProfiles
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
