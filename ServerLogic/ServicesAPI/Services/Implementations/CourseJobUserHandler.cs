using Hangfire;
using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class CourseJobUserHandler : ICourseJobUserHandler
    {
        private readonly CoursesSystemDbContext context;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IExtendedDataService<CourseJobUser> courseJobUser;
        private readonly IExtendedDataService<SystemUsersTrainingCourses> usersCourses;

        public CourseJobUserHandler(CoursesSystemDbContext context,
                                    IBackgroundJobClient backgroundJobClient,
                                    IExtendedDataService<CourseJobUser> courseJobUser,
                                    IExtendedDataService<SystemUsersTrainingCourses> usersCourses)
        {
            this.context = context;
            this.backgroundJobClient = backgroundJobClient;
            this.courseJobUser = courseJobUser;
            this.usersCourses = usersCourses;
        }
        public async Task AddAsync(string jobId, int courseId, int userId)
        {
            var courseJobUserData = new CourseJobUser
            {
                JobId = jobId,
                CourseId = courseId,
                UserId = userId
            };

            await courseJobUser.CreateAsync(courseJobUserData);
        }

        public async Task<ApiResult> DeleteAsync(int courseId, int userId)
        {
            var result = new ApiResult();
            var message = "Unsabscribe has failed";
            var errors = new string[] { $"{message}. Contact administrator, please" };
            var loggerMessage = $"{message}. CourseId = {courseId}, UserId = {userId}";

            var courseJobs = await courseJobUser.GetEntitiesByCondition(x => x.CourseId.Equals(courseId) && x.UserId.Equals(userId))
                                                .ToArrayAsync();

            if (!courseJobs.Any())
            {
                result.SetApiResult(ApiResultStatus.BadRequest, loggerMessage, message: message, errors: errors);
            }
            else
            {
                foreach (var courseJob in courseJobs)
                {
                    var removeJobResult = backgroundJobClient.Delete(courseJob.JobId);

                    if (removeJobResult)
                    {
                        await courseJobUser.DeleteAsync(courseJob);
                        result.SetApiResult(ApiResultStatus.Ok, $"Deleted job: {courseJob.JobId}, CourseId = {courseId}, UserId = {userId}");
                    }
                    else
                    {
                        result.SetApiResult(ApiResultStatus.BadRequest, loggerMessage, message: message, errors: errors);
                        return result;
                    }
                }

                var userCourse = await usersCourses.GetEntityByConditionAsync(x => x.SystemUserId.Equals(userId) && x.TrainingCourseId.Equals(courseId));

                await usersCourses.DeleteAsync(userCourse);
            }

            return result;
        }
    }
}
