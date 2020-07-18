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
            var result = new ApiResult();
            var courses = await context.TrainingCourses
                                       .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(courses);

            result.SetApiResult(ApiResultStatus.Ok, $"Returning all coureses to the client. Count = {courses.Length}", data);

            return result;
        }

        public async Task<ApiResult> GetAmountAsync()
        {
            var amount = await context.TrainingCourses
                                      .CountAsync();

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning amount of courses: {amount}", amount);

            return result;
        }

        public async Task<ApiResult> GetById(int id)
        {
            var result = new ApiResult();

            var course = await context.TrainingCourses
                                      .FindAsync(id);

            if (course == null)
            {
                var message = $"Course with id = {id} was not found";
                result.SetApiResult(ApiResultStatus.NotFound, message, message: message);
            }
            else
            {
                var data = mapperWrapper.MapFromEntity(course);
                result.SetApiResult(ApiResultStatus.Ok, $"Returning a course id = {course.Id}", data);
            }

            return result;
        }

        public async Task<ApiResult> GetForPage(int skip, int take)
        {
            var courses = await context.TrainingCourses
                                       .GetPortionOfQueryable(skip, take)
                                       .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(courses);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning a portion of courses. Count = {courses.Length}", data);

            return result;
        }
    }
}
