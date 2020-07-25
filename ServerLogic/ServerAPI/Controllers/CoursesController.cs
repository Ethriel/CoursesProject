using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
using ServicesAPI.DataPresentation;
using ServicesAPI.Services.Abstractions;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : Controller
    {
        private readonly ICoursesService coursesService;
        private readonly ILogger<CoursesController> logger;

        public CoursesController(ICoursesService coursesService, ILogger<CoursesController> logger)
        {
            this.coursesService = coursesService;
            this.logger = logger;
        }

        [HttpGet("get/amount")]
        public async Task<IActionResult> GetAmount()
        {
            var result = await coursesService.GetAmountAsync();

            return this.GetActionResult(result, logger);
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await coursesService.GetAllCoursesAsync();

            return this.GetActionResult(result, logger);
        }
        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            var result = await coursesService.GetForPage(skip, take);

            return this.GetActionResult(result, logger);

        }
        [HttpPost("get/paged")]
        public async Task<IActionResult> GetPaged([FromBody] CoursesPagination coursesPagination)
        {
            var result = await coursesService.GetPagedAsync(coursesPagination);

            return this.GetActionResult(result, logger);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await coursesService.GetById(id);

            return this.GetActionResult(result, logger);
        }
        [HttpGet("check/{userId}/{courseId}")]
        public async Task<IActionResult> CheckCourse(int userId, int courseId)
        {
            var result = await coursesService.CheckCourseAsync(userId, courseId);

            return this.GetActionResult(result, logger);
        }
    }
}
