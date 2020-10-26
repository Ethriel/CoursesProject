using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.BackgroundJobs;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class UserCoursesService : IUserCoursesService
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper;
        private readonly IEmailNotifyJob emailNotifyJob;
        private readonly ICourseJobUserHandler courseJobUser;

        public UserCoursesService(CoursesSystemDbContext context,
                                  IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper,
                                  IEmailNotifyJob emailNotifyJob,
                                  ICourseJobUserHandler courseJobUser)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
            this.emailNotifyJob = emailNotifyJob;
            this.courseJobUser = courseJobUser;
        }
        public async Task<ApiResult> AddCourseToUserAsync(SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            var result = new ApiResult();
            

            var courseId = userCourseDTO.TrainingCourseId;
            var userId = userCourseDTO.SystemUserId;

            var userCourse = mapperWrapper.MapEntity(userCourseDTO);

            var course = await context.TrainingCourses
                                      .FindAsync(courseId);

            var user = await context.SystemUsers
                                    .FindAsync(userId);

            result = await GetAddCourseToUserResultAsync(user, course, userCourse, result);

            return result;
        }
        public async Task<ApiResult> GetAllAsync()
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses
                                                .ToArrayAsync();

            var data = mapperWrapper.MapModels(usersWithCourses);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning all users with courses to the client. Count = {usersWithCourses.Length}", data);

            return result;
        }

        public async Task<ApiResult> GetByUserIdAsync(int id)
        {
            var userCourses = await context.SystemUsersTrainingCourses
                                           .GetCoursesByUserId(id)
                                           .ToArrayAsync();

            var data = mapperWrapper.MapModels(userCourses);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning courses of user id = {id}", data);

            return result;
        }

        public async Task<ApiResult> Unsubscribe(int userId, int courseId)
        {
            return await courseJobUser.DeleteAsync(courseId, userId);
        }

        private async Task<ApiResult> GetAddCourseToUserResultAsync(SystemUser user, TrainingCourse course, SystemUsersTrainingCourses userCourse, ApiResult result)
        {
            var loggerMessage = "Adding course to user. Result:{0}";
            var message = "{0} id = {1} was not found";
            IEnumerable<string> errors = default;

            if (course == null)
            {
                message = string.Format(message, "Course", course.Id);
                loggerMessage = string.Format(loggerMessage, message);
                errors = new string[] { message };

                result.SetApiResult(ApiResultStatus.NotFound, loggerMessage, message: message, errors: errors);
            }
            else if (user == null)
            {
                message = string.Format(message, "User", user.Id);
                loggerMessage = string.Format(loggerMessage, message);
                errors = new string[] { message };

                result.SetApiResult(ApiResultStatus.NotFound, loggerMessage, message: message, errors: errors);
            }
            else
            {
                userCourse.SystemUser = user;
                userCourse.TrainingCourse = course;

                loggerMessage = string.Format(loggerMessage, $"Course id = {course.Id}, user id = {user.Id}");

                result.SetApiResult(ApiResultStatus.Ok, loggerMessage);

                context.SystemUsersTrainingCourses.Add(userCourse);

                var saved = await context.SaveChangesAsync();

                var jobs = emailNotifyJob.CreateJobs(user, course, userCourse.StudyDate);

                foreach (var job in jobs)
                {
                    await courseJobUser.AddAsync(job, course.Id, user.Id);
                }
            }

            return result;
        }
    }
}
