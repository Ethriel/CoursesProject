using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class CoursesService : ICoursesService
    {
        private readonly IMapperWrapper<TrainingCourse, TrainingCourseDTO> mapperWrapper;
        private readonly IExtendedDataService<TrainingCourse> courses;
        private readonly IExtendedDataService<SystemUser> users;

        public CoursesService(IMapperWrapper<TrainingCourse,
                              TrainingCourseDTO> mapperWrapper,
                              IExtendedDataService<TrainingCourse> courses,
                              IExtendedDataService<SystemUser> users)
        {
            this.mapperWrapper = mapperWrapper;
            this.courses = courses;
            this.users = users;
        }

        public async Task<ApiResult> CheckCourseAsync(int userId, int courseId)
        {
            var result = default(ApiResult);

            // find user
            var user = await users.GetByIdAsync(userId);

            if (user == null)
            {
                var message = "User not found";
                var loggerMessage = "Unable to fetch course data";
                var errors = new string[] { message };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                // find course
                var course = await courses.GetByIdAsync(courseId);

                if (course == null)
                {
                    var message = "Course not found";
                    var loggerMessage = "Unable to fetch course data";
                    var errors = new string[] { "Course not found" };
                    result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
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

                    result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: courseData);
                }
            }

            return result;
        }

        public async Task<ApiResult> CreateCourseAsync(TrainingCourseDTO courseDTO)
        {
            var result = default(ApiResult);
            var course = await courses.GetEntityByConditionAsync(x => x.Title.Equals(courseDTO.Title));

            if (course != null)
            {
                var message = "Course creation has failed";
                var loggerMessage = $"Course with title {courseDTO.Title} already exists";
                var errors = new string[] { loggerMessage };
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }
            else
            {
                course = mapperWrapper.MapEntity(courseDTO);
                await courses.CreateAsync(course);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, "Course created");
            }

            return result;
        }

        public async Task<ApiResult> GetAllCoursesAsync()
        {
            var result = default(ApiResult);
            var coursesData = await courses.Read()
                                           .ToArrayAsync();

            var data = mapperWrapper.MapModels(coursesData);

            result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> GetById(int id)
        {
            var result = default(ApiResult);

            var course = await courses.GetByIdAsync(id);

            if (course == null)
            {
                var message = "Course not found";
                var errors = new string[] { $"Course with id = {id} was not found" };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, message, message, errors);
            }
            else
            {
                var data = mapperWrapper.MapModel(course);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);
            }

            return result;
        }

        public async Task<ApiResult> GetPagedAsync(CoursesPagination coursesPagination)
        {
            var amount = await this.courses.GetCountAsync();

            var pagination = new Pagination();
            pagination.SetDefaults(amount, pageSize: 3);

            coursesPagination.Pagination ??= pagination;

            var skip = coursesPagination.Pagination
                                        .GetSkip();

            var take = coursesPagination.Pagination
                                        .GetTake();

            var coursesData = await courses.GetPortion(skip, take)
                                           .ToArrayAsync();

            var models = mapperWrapper.MapModels(coursesData);

            var data = new { pagination = coursesPagination.Pagination, courses = models };

            var result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> UpdateCourseAsync(TrainingCourseDTO courseDTO)
        {
            var result = default(ApiResult);
            var course = await courses.GetByIdAsync(courseDTO.Id);

            if (course == null)
            {
                var message = "Course was not found";
                var loggerMessage = $"Course with title {courseDTO.Title} was not found";
                var errors = new string[] { loggerMessage };
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }
            else
            {
                var newCourse = mapperWrapper.MapEntity(courseDTO);
                course = await courses.UpdateAsync(course, newCourse);
                var model = mapperWrapper.MapModel(course);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: model);
            }

            return result;
        }
    }
}
