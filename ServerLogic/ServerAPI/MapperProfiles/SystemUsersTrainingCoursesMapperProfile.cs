using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;

namespace ServerAPI.MapperProfiles
{
    public class SystemUsersTrainingCoursesMapperProfile : Profile
    {
        public SystemUsersTrainingCoursesMapperProfile()
        {
            CreateMap<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>()
                .ForMember(sutcdto => sutcdto.SystemUser, o => o.Ignore());

            CreateMap<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>()
                .ForMember(sutcdto => sutcdto.TrainingCourse, o => o.Ignore());

            CreateMap<SystemUsersTrainingCoursesDTO, SystemUsersTrainingCourses>()
                .ForMember(sutc => sutc.SystemUser, o => o.Ignore());

            CreateMap<SystemUsersTrainingCoursesDTO, SystemUsersTrainingCourses>()
                .ForMember(sutc => sutc.TrainingCourse, o => o.Ignore());
        }
    }
}
