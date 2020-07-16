using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.BackgroundJobs;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
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
        public async Task AddCourseToUserASync(SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            var userCourse = mapperWrapper.MapFromDTO(userCourseDTO);
            var course = await context.TrainingCourses.FindAsync(userCourseDTO.TrainingCourseId);
            var user = await context.SystemUsers.FindAsync(userCourseDTO.SystemUserId);
            userCourse.SystemUser = user;
            userCourse.TrainingCourse = course;

            context.SystemUsersTrainingCourses.Add(userCourse);

            var saved = await context.SaveChangesAsync();

            emailNotifyJob.CreateJobs(user, course, userCourse.StudyDate);
        }

        public async Task<IEnumerable<SystemUsersTrainingCoursesDTO>> GetAllAsync()
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses.ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(usersWithCourses);
            return data;
        }

        public async Task<int> GetAmountAsync()
        {
            var amount = await context.SystemUsersTrainingCourses.CountAsync();
            return amount;
        }

        public async Task<IEnumerable<SystemUsersTrainingCoursesDTO>> GetByUserIdAsync(int id)
        {
            var coursesUser = await context.SystemUsersTrainingCourses
                                       .GetCoursesByUserId(id)
                                       .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(coursesUser);
            return data;
        }

        public async Task<IEnumerable<SystemUsersTrainingCoursesDTO>> GetForPageAsync(int skip, int take)
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses
                                            .GetPortionOfQueryable(skip, take)
                                            .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(usersWithCourses);
            return data;
        }
    }
}
