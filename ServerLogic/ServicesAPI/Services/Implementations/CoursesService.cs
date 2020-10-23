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
        private readonly IExtendedDataService<TrainingCourse> courseService;
        private readonly IExtendedDataService<SystemUser> userService;

        public CoursesService(IMapperWrapper<TrainingCourse,
                              TrainingCourseDTO> mapperWrapper,
                              IExtendedDataService<TrainingCourse> courseService,
                              IExtendedDataService<SystemUser> userService)
        {
            this.mapperWrapper = mapperWrapper;
            this.courseService = courseService;
            this.userService = userService;
        }

        public async Task<ApiResult> CheckCourseAsync(int userId, int courseId)
        {
            var result = new ApiResult();

            // find user
            var user = await userService.GetByIdAsync(userId);

            if (user == null)
            {
                var message = "Unable to fetch course data";
                var errors = new string[] { "User not found" };
                result.SetApiResult(ApiResultStatus.NotFound, message: message, errors: errors);
            }
            else
            {
                // find course
                var course = await courseService.GetByIdAsync(courseId);

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

        public async Task<ApiResult> CreateCourseAsync(TrainingCourseDTO courseDTO)
        {
            var result = new ApiResult();
            var course = await courseService.GetEntityByConditionAsync(x => x.Title.Equals(courseDTO.Title));

            if (course != null)
            {
                var message = "Course creation has failed";
                var loggerMessage = $"Course with title {courseDTO.Title} already exists";
                var errors = new string[] { loggerMessage };
                result.SetApiResult(ApiResultStatus.BadRequest, loggerMessage, message: message, errors: errors);
            }
            else
            {
                course = mapperWrapper.MapEntity(courseDTO);
                result.SetApiResult(ApiResultStatus.Ok, "Course created");
            }

            return result;
        }

        public async Task<ApiResult> GetAllCoursesAsync()
        {
            var result = new ApiResult();
            var courses = await courseService.Read()
                                             .ToArrayAsync();

            var data = mapperWrapper.MapModels(courses);

            result.SetApiResult(ApiResultStatus.Ok, $"Returning all coureses to the client. Count = {courses.Length}", data);

            return result;
        }

        public async Task<ApiResult> GetById(int id)
        {
            var result = new ApiResult();

            var course = await courseService.GetByIdAsync(id);

            if (course == null)
            {
                var message = "Course not found";
                var errors = new string[] { $"Course with id = {id} was not found" };
                result.SetApiResult(ApiResultStatus.NotFound, message, message: message, errors: errors);
            }
            else
            {
                var data = mapperWrapper.MapModel(course);
                result.SetApiResult(ApiResultStatus.Ok, $"Returning a course id = {course.Id}", data);
            }

            return result;
        }

        public async Task<ApiResult> GetPagedAsync(CoursesPagination coursesPagination)
        {
            var amount = await courseService.GetCountAsync();

            var pagination = new Pagination();
            pagination.SetDefaults(amount, pageSize: 3);

            coursesPagination.Pagination ??= pagination;

            var skip = coursesPagination.Pagination
                                        .GetSkip();

            var take = coursesPagination.Pagination
                                        .GetTake();

            var coursesData = await courseService.GetPortion(skip, take)
                                                 .ToArrayAsync();

            var courses = mapperWrapper.MapModels(coursesData);

            var data = new { pagination = coursesPagination.Pagination, courses = courses };

            var result = new ApiResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> UpdateCourseAsync(TrainingCourseDTO courseDTO)
        {
            var result = new ApiResult();
            var course = await courseService.GetByIdAsync(courseDTO.Id);

            if (course == null)
            {
                var message = "Course was not found";
                var loggerMessage = $"Course with title {courseDTO.Title} was not found";
                var errors = new string[] { loggerMessage };
                result.SetApiResult(ApiResultStatus.BadRequest, loggerMessage, message: message, errors: errors);
            }
            else
            {
                var newCourse = mapperWrapper.MapEntity(courseDTO);
                course = await courseService.UpdateAsync(course, newCourse);
                var model = mapperWrapper.MapModel(course);
                result.SetApiResult(ApiResultStatus.Ok, $"Course id = {course.Id} was updated", data: model);
            }

            return result;
        }
    }
}
