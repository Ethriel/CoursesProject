using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
using ServicesAPI.Services.Abstractions;
using ServicesAPI.DataPresentation;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [Authorize(Roles = "ADMIN", AuthenticationSchemes = "Bearer")]
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

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await studentsService.GetAllStudentsAsync();

            return this.GetActionResult(result, logger);
        }

        [HttpPost("post/searchAndSort")]
        public async Task<IActionResult> SearchStudents([FromBody] SearchAndSort searchAndSort)
        {
            var result = await studentsService.SearchAndSortStudentsAsync(searchAndSort);

            return this.GetActionResult(result, logger);
        }
    }
}
