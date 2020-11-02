using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
using ServicesAPI.Services.Abstractions;
using ServicesAPI.DataPresentation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStudentsService studentsService;
        private readonly ILogger<StudentsController> logger;

        public StudentsController(IStudentsService studentsService, ILogger<StudentsController> logger)
        {
            this.studentsService = studentsService;
            this.logger = logger;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await studentsService.GetAllStudentsAsync();

            return this.GetActionResult(result, logger);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("post/searchAndSort")]
        public async Task<IActionResult> SearchStudents([FromBody] SearchAndSort searchAndSort)
        {
            var result = await studentsService.SearchAndSortStudentsAsync(searchAndSort);

            return this.GetActionResult(result, logger);
        }

        [HttpGet("get/user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await studentsService.GetUserByIdAsync(id);

            return this.GetActionResult(result, logger);
        }

        [HttpPost("user/uploadImage/{id}")]
        public async Task<IActionResult> UploadImage([FromForm(Name = "image")] IFormFile image, int id)
        {
            var result = await studentsService.UploadImageAsync(image, id, HttpContext);

            return this.GetActionResult(result, logger);
        }
    }
}
