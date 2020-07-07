using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Models;
using System;

namespace ServerAPI.MapperProfiles
{
    public class TrainingCourseMapperProfile : Profile
    {
        public TrainingCourseMapperProfile()
        {
            CreateMap<TrainingCourse, TrainingCourseDTO>()
                .ForMember(tcd => tcd.StartDate, o => o.MapFrom(x => x.StartDate.ToShortDateString()));

            CreateMap<TrainingCourseDTO, TrainingCourse>()
                .ForMember(tc => tc.StartDate, o => o.MapFrom(x => DateTime.Parse(x.StartDate)));
        }
    }
}
