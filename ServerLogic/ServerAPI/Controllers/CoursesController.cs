using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
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

            var loggerMessage = $"Returning amount of courses: {result.Data}";

            return this.GetActionResult(result, logger, loggerMessage);
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await coursesService.GetAllCoursesAsync();

            var loggerMessage = "Returning all courses";

            return this.GetActionResult(result, logger, loggerMessage);
        }
        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            var result = await coursesService.GetForPage(skip, take);

            var loggerMessage = $"Returning a portion of courses: {take}";

            return this.GetActionResult(result, logger, loggerMessage);

        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await coursesService.GetById(id);

            var loggerMessage = $"Returning a course. Id = {id}";

            return this.GetActionResult(result, logger, loggerMessage);
        }
    }
}
