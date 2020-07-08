using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.MapperWrappers;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserCoursesController : Controller
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper;

        public UserCoursesController(CoursesSystemDbContext context, IMapperWrapper<SystemUsersTrainingCourses, SystemUsersTrainingCoursesDTO> mapperWrapper)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourseToUser([FromBody]SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            var userCourse = mapperWrapper.MapFromDTO(userCourseDTO);
            var course = await context.TrainingCourses.FirstOrDefaultAsync(x => x.Id.Equals(userCourseDTO.TrainingCourseId));
            var user = await context.SystemUsers.FirstOrDefaultAsync(x => x.Id.Equals(userCourseDTO.SystemUserId));
            userCourse.SystemUser = user;
            userCourse.TrainingCourse = course;
            context.SystemUsersTrainingCourses.Add(userCourse);
            var saved = await context.SaveChangesAsync();
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
            var data = await context.SystemUsersTrainingCourses.ToArrayAsync();
            var ammount = data.Length;
            return Ok(new { data = ammount });
        }

        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            var usersWithCourses = await context.SystemUsersTrainingCourses.Skip(skip).Take(take).ToArrayAsync();
            var data = mapperWrapper.MapCollectionFromEntities(usersWithCourses);
            return Ok(new { data = data });
        }
    }
}
