using AutoMapper;
using ServicesAPI.DTO;
using Infrastructure.Models;
using System;

namespace ServicesAPI.MapperProfiles
{
    public class SystemUsersTrainingCoursesMapperProfile : Profile
    {
        public SystemUsersTrainingCoursesMapperProfile()
        {
            CreateMap<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO>()
                //.ForMember(sutcdto => sutcdto.SystemUser, o => o.Ignore())
                //.ForMember(sutcdto => sutcdto.TrainingCourse, o => o.Ignore())
                .ForMember(sutcdto => sutcdto.StudyDate, o => o.MapFrom(x => x.StudyDate.ToShortDateString()))
                .ForMember(sutcdto => sutcdto.Title, o => o.MapFrom(x => x.TrainingCourse.Title));

            CreateMap<SystemUsersTrainingCoursesDTO, SystemUsersTrainingCourses>()
                //.ForMember(sutc => sutc.SystemUser, o => o.Ignore())
                //.ForMember(sutc => sutc.TrainingCourse, o => o.Ignore())
                .ForMember(sutc => sutc.StudyDate, o => o.MapFrom(x => DateTime.Parse(x.StudyDate)));
        }
    }
}
