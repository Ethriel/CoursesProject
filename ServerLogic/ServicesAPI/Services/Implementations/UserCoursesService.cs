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

        public UserCoursesService(CoursesSystemDbContext context,
                                  IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper,
                                  IEmailNotifyJob emailNotifyJob)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
            this.emailNotifyJob = emailNotifyJob;
        }
        public async Task<ApiResult> AddCourseToUserAsync(SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            var result = new ApiResult();
            var loggerMessage = "Adding course to user. Result:{0}";
            var message = "{0} id = {1} was not found";
            IEnumerable<string> errors = default;

            var courseId = userCourseDTO.TrainingCourseId;
            var userId = userCourseDTO.SystemUserId;

            var userCourse = mapperWrapper.MapFromDTO(userCourseDTO);

            var course = await context.TrainingCourses
                                      .FindAsync(courseId);

            var user = await context.SystemUsers
                                    .FindAsync(userId);
            if (course == null)
            {
                message = string.Format(message, "Course", courseId);
                loggerMessage = string.Format(loggerMessage, message);
                errors = new string[] { message };

                result.SetApiResult(ApiResultStatus.NotFound, loggerMessage, message: message, errors: errors);
            }
            else if (user == null)
            {
                message = string.Format(message, "User", userId);
                loggerMessage = string.Format(loggerMessage, message);
                errors = new string[] { message };

                result.SetApiResult(ApiResultStatus.NotFound, loggerMessage, message: message, errors: errors);
            }
            else
            {
                userCourse.SystemUser = user;
                userCourse.TrainingCourse = course;

                loggerMessage = string.Format(loggerMessage, $"Course id = {courseId}, user id = {userId}");

                result.SetApiResult(ApiResultStatus.Ok, loggerMessage);

                context.SystemUsersTrainingCourses.Add(userCourse);

                var saved = await context.SaveChangesAsync();

                emailNotifyJob.CreateJobs(user, course, userCourse.StudyDate);
            }

            return result;
        }
        public async Task<ApiResult> GetAllAsync()
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses
                                                .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(usersWithCourses);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning all users with courses to the client. Count = {usersWithCourses.Length}", data);

            return result;
        }

        public async Task<ApiResult> GetAmountAsync()
        {
            var amount = await context.SystemUsersTrainingCourses
                                      .CountAsync();

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning amount of users with courses: {amount}", amount);

            return result;
        }

        public async Task<ApiResult> GetByUserIdAsync(int id)
        {
            var userCourses = await context.SystemUsersTrainingCourses
                                           .GetCoursesByUserId(id)
                                           .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(userCourses);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning courses of user id = {id}", data);

            return result;
        }

        public async Task<ApiResult> GetForPageAsync(int skip, int take)
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses
                                                .GetPortionOfQueryable(skip, take)
                                                .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(usersWithCourses);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning a portion of users and courses. Count = {usersWithCourses.Length}", data);

            return result;
        }
    }
}
