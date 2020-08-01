using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System.Linq;
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

        public async Task<ApiResult> CheckCourseAsync(int userId, int courseId)
        {
            var result = new ApiResult();

            var user = await context.SystemUsers
                                    .FindAsync(userId);
            // find user
            if (user == null)
            {
                var message = "Unable to fetch course data";
                var errors = new string[] { "User not found" };
                result.SetApiResult(ApiResultStatus.NotFound, message: message, errors: errors);
            }
            else
            {
                // find course
                var course = await context.TrainingCourses
                                          .FindAsync(courseId);

                if (course == null)
                {
                    var message = "Unable to fetch course data";
                    var errors = new string[] { "Course not found" };
                    result.SetApiResult(ApiResultStatus.NotFound, message: message, errors: errors);
                }
                else
                {
                    // check if user has course
                    var userCourse = user.SystemUsersTrainingCourses
                                         .FirstOrDefault(x => x.TrainingCourseId
                                                               .Equals(courseId));

                    var courseData = new CourseData();

                    // set course data
                    if (userCourse == null)
                    {
                        courseData.IsPresent = false;
                    }
                    else
                    {
                        courseData.IsPresent = true;
                        courseData.StudyDate = userCourse.StudyDate
                                                         .ToShortDateString();
                    }

                    result.SetApiResult(ApiResultStatus.Ok, data: courseData);
                }
            }

            return result;
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

        public async Task<ApiResult> GetById(int id)
        {
            var result = new ApiResult();

            var course = await context.TrainingCourses
                                      .FindAsync(id);

            if (course == null)
            {
                var message = "Course not found";
                var errors = new string[] { $"Course with id = {id} was not found" };
                result.SetApiResult(ApiResultStatus.NotFound, message, message: message, errors: errors);
            }
            else
            {
                var data = mapperWrapper.MapFromEntity(course);
                result.SetApiResult(ApiResultStatus.Ok, $"Returning a course id = {course.Id}", data);
            }

            return result;
        }

        public async Task<ApiResult> GetPagedAsync(CoursesPagination coursesPagination)
        {
            var amount = await context.TrainingCourses
                                      .CountAsync();

            var pagination = new Pagination();
            pagination.SetDefaults(amount, pageSize: 3);

            coursesPagination.Pagination ??= pagination;

            var skip = coursesPagination.Pagination
                                        .GetSkip();

            var take = coursesPagination.Pagination
                                        .GetTake();

            var coursesData = await context.TrainingCourses
                                           .GetPortionOfQueryable(skip, take)
                                           .ToArrayAsync();

            var courses = mapperWrapper.MapCollectionFromEntities(coursesData);

            var data = new { pagination = coursesPagination.Pagination, courses = courses };

            var result = new ApiResult(ApiResultStatus.Ok, data: data);

            return result;
        }
    }
}
