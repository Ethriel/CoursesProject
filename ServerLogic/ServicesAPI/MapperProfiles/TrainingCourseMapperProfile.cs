using AutoMapper;
using Infrastructure.Models;
using ServicesAPI.DTO;

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
