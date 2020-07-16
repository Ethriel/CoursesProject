using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.DTO;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserCoursesController : Controller
    {
        private readonly IUserCoursesService userCoursesService;
        private readonly ILogger<StudentsController> logger;

        public UserCoursesController(IUserCoursesService userCoursesService, ILogger<StudentsController> logger)
        {
            this.userCoursesService = userCoursesService;
            this.logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourseToUser([FromBody] SystemUsersTrainingCoursesDTO userCourseDTO)
        {
            await userCoursesService.AddCourseToUserASync(userCourseDTO);
            return Ok();
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllUsersWithCourses()
        {
            var data = await userCoursesService.GetAllAsync();
            return Ok(new { data });
        }

        [HttpGet("get/ammount")]
        public async Task<IActionResult> GetAmmount()
        {
            var amount = await userCoursesService.GetAmountAsync();
            return Ok(new { amount });
        }

        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            var data = await userCoursesService.GetForPageAsync(skip, take);
            return Ok(new { data });
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetCoursesByUserId(int userId)
        {
            var data = await userCoursesService.GetByUserIdAsync(userId);
            return Ok(new { data });
        }
    }
}
