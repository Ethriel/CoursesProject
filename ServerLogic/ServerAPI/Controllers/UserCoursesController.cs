using Infrastructure.DbContext;
using ServicesAPI.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.BackgroundJobs;
using ServicesAPI.MapperWrappers;
using System.Linq;
using System.Threading.Tasks;
using ServicesAPI.Extensions;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserCoursesController : Controller
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper;
        private readonly IEmailNotifyJob emailNotifyJob;

        public UserCoursesController(CoursesSystemDbContext context,
            IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper,
            IMapperWrapper<TrainingCourse, TrainingCourseDTO> mapperWrapperCourses, IEmailNotifyJob emailNotifyJob)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
            this.emailNotifyJob = emailNotifyJob;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourseToUser([FromBody] SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            var userCourse = mapperWrapper.MapFromDTO(userCourseDTO);
            var course = await context.TrainingCourses.FindAsync(userCourseDTO.TrainingCourseId);
            var user = await context.SystemUsers.FindAsync(userCourseDTO.SystemUserId);
            userCourse.SystemUser = user;
            userCourse.TrainingCourse = course;
            context.SystemUsersTrainingCourses.Add(userCourse);
            var saved = await context.SaveChangesAsync();
            emailNotifyJob.CreateJobs(user, course, userCourse.StudyDate);

            return Ok(new { data = "All ok" });
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllUsersWithCourses()
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses.ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(usersWithCourses);
            return Ok(new { data = data });
        }

        [HttpGet("get/ammount")]
        public async Task<IActionResult> GetAmmount()
        {
            var ammount = await context.SystemUsersTrainingCourses.CountAsync();
            return Ok(new { data = ammount });
        }

        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses
                                                .GetPortionOfQueryable(skip, take)
                                                .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(usersWithCourses);
            return Ok(new { data = data });
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetCoursesByUserId(int userId)
        {
            var coursesUser = await context.SystemUsersTrainingCourses
                                           .GetCoursesByUserId(userId)
                                           .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(coursesUser);
            return Ok(new { data = data });
        }
    }
}
