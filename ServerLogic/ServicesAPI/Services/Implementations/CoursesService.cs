using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class CoursesService : ICoursesService
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<TrainingCourse, TrainingCourseDTO> mapperWrapper;

        public CoursesService(CoursesSystemDbContext context, IMapperWrapper<TrainingCourse, TrainingCourseDTO> mapperWrapper)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
        }
        public async Task<IEnumerable<TrainingCourseDTO>> GetAllCoursesAsync()
        {
            try
            {
                var courses = await context.TrainingCourses
                                           .ToArrayAsync();

                var data = mapperWrapper.MapCollectionFromEntities(courses);
                return data;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetAmountAsync()
        {
            try
            {
                var amount = await context.TrainingCourses
                                          .CountAsync();
                return amount;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TrainingCourseDTO> GetById(int id)
        {
            try
            {
                var course = await context.TrainingCourses
                                          .FindAsync(id);

                var data = mapperWrapper.MapFromEntity(course);
                return data;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<TrainingCourseDTO>> GetForPage(int skip, int take)
        {
            try
            {
                var courses = await context.TrainingCourses
                                           .GetPortionOfQueryable(skip, take)
                                           .ToArrayAsync();

                var data = mapperWrapper.MapCollectionFromEntities(courses);
                return data;
            }
            catch
            {
                throw;
            }
        }
    }
}
