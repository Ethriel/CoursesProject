using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.DTO;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserCoursesController : Controller
    {
        private readonly IUserCoursesService userCoursesService;

        public UserCoursesController(IUserCoursesService userCoursesService)
        {
            this.userCoursesService = userCoursesService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourseToUser([FromBody] SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            try
            {
                await userCoursesService.AddCourseToUserASync(userCourseDTO);
                return Ok();
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllUsersWithCourses()
        {
            try
            {
                var data = await userCoursesService.GetAllAsync();
                return Ok(new { data });
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("get/ammount")]
        public async Task<IActionResult> GetAmmount()
        {
            try
            {
                var amount = await userCoursesService.GetAmountAsync();
                return Ok(new { amount });
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            try
            {
                var data = await userCoursesService.GetForPageAsync(skip, take);
                return Ok(new { data });
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetCoursesByUserId(int userId)
        {
            try
            {
                var data = await userCoursesService.GetByUserIdAsync(userId);
                return Ok(new { data });
            }
            catch
            {
                throw;
            }
        }
    }
}
