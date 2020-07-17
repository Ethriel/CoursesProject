using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
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
        public async Task<ApiResult> GetAllCoursesAsync()
        {
            var courses = await context.TrainingCourses
                                       .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(courses);

            var result = new ApiResult(ApiResultStatus.Ok, data);

            return result;
        }

        public async Task<ApiResult> GetAmountAsync()
        {
            var amount = await context.TrainingCourses
                                      .CountAsync();

            var result = new ApiResult(ApiResultStatus.Ok, amount);

            return result;
        }

        public async Task<ApiResult> GetById(int id)
        {
            ApiResult result = null;

            var course = await context.TrainingCourses
                                      .FindAsync(id);

            if (course == null)
            {
                result = new ApiResult(ApiResultStatus.NotFound, message: $"Course with id = {id} was not found");
            }
            else
            {
                var data = mapperWrapper.MapFromEntity(course);

                result = new ApiResult(ApiResultStatus.Ok, data);

            }

            return result;
        }

        public async Task<ApiResult> GetForPage(int skip, int take)
        {
            var courses = await context.TrainingCourses
                                       .GetPortionOfQueryable(skip, take)
                                       .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(courses);

            var result = new ApiResult(ApiResultStatus.Ok, data);

            return result;
        }
    }
}
