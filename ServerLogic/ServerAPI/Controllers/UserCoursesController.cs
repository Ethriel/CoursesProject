using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
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
            var result = await userCoursesService.AddCourseToUserAsync(userCourseDTO);

            return this.GetActionResult(result, logger);
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllUsersWithCourses()
        {
            var result = await userCoursesService.GetAllAsync();

            return this.GetActionResult(result, logger);
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetCoursesByUserId(int userId)
        {
            var result = await userCoursesService.GetByUserIdAsync(userId);

            return this.GetActionResult(result, logger);
        }
    }
}
