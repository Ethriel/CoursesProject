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
        private readonly ICourseJobUserHandler coursesJobsUsers;
        private readonly IExtendedDataService<TrainingCourse> courses;
        private readonly IExtendedDataService<SystemUser> users;
        private readonly IExtendedDataService<SystemUsersTrainingCourses> usersCourses;

        public UserCoursesService(CoursesSystemDbContext context,
                                  IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper,
                                  IEmailNotifyJob emailNotifyJob,
                                  ICourseJobUserHandler coursesJobsUsers,
                                  IExtendedDataService<TrainingCourse> courses,
                                  IExtendedDataService<SystemUser> users,
                                  IExtendedDataService<SystemUsersTrainingCourses> usersCourses)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
            this.emailNotifyJob = emailNotifyJob;
            this.coursesJobsUsers = coursesJobsUsers;
            this.courses = courses;
            this.users = users;
            this.usersCourses = usersCourses;
        }
        public async Task<ApiResult> AddCourseToUserAsync(SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            var courseId = userCourseDTO.TrainingCourseId;
            var userId = userCourseDTO.SystemUserId;

            var userCourse = mapperWrapper.MapEntity(userCourseDTO);

            var course = await courses.GetByIdAsync(courseId);

            var user = await users.GetByIdAsync(userId);

            var result = await GetAddCourseToUserResultAsync(user, course, userCourse);

            return result;
        }
        public async Task<ApiResult> GetAllAsync()
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses
                                                .ToArrayAsync();

            var data = mapperWrapper.MapModels(usersWithCourses);

            var result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> GetByUserIdAsync(int id)
        {
            var userCourses = await usersCourses.GetEntitiesByCondition(x => x.SystemUserId.Equals(id))
                                                .Include(x => x.TrainingCourse)
                                                .ToArrayAsync();

            var data = mapperWrapper.MapModels(userCourses);

            var result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> Unsubscribe(int userId, int courseId)
        {
            return await coursesJobsUsers.DeleteAsync(courseId, userId);
        }

        private async Task<ApiResult> GetAddCourseToUserResultAsync(SystemUser user, TrainingCourse course, SystemUsersTrainingCourses userCourse)
        {
            var result = default(ApiResult);
            var loggerMessage = "Adding course to user. Result:{0}";
            var message = "{0} id = {1} was not found";
            var errors = default(IEnumerable<string>);

            if (course == null)
            {
                message = string.Format(message, "Course", course.Id);
                loggerMessage = string.Format(loggerMessage, message);
                errors = new string[] { message };

                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else if (user == null)
            {
                message = string.Format(message, "User", user.Id);
                loggerMessage = string.Format(loggerMessage, message);
                errors = new string[] { message };

                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                userCourse.SystemUser = user;
                userCourse.TrainingCourse = course;

                result = ApiResult.GetOkResult(ApiResultStatus.Ok);

                context.SystemUsersTrainingCourses.Add(userCourse);

                await usersCourses.CreateAsync(userCourse);

                var jobs = emailNotifyJob.CreateJobs(user, course, userCourse.StudyDate);

                foreach (var job in jobs)
                {
                    await coursesJobsUsers.AddAsync(job, course.Id, user.Id);
                }
            }

            return result;
        }
    }
}
