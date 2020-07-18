using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
using ServicesAPI.Services.Abstractions;
using ServicesAPI.Sorts;
using System.Threading.Tasks;

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

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await studentsService.GetAllStudentsAsync();

            return this.GetActionResult(result, logger);
        }

        [HttpGet("get/amount")]
        public async Task<IActionResult> GetAmount()
        {
            var result = await studentsService.GetAmountOfStudentsAync();

            return this.GetActionResult(result, logger);
        }

        [HttpPost("post/sort")]
        public async Task<IActionResult> GetSortedStudents([FromBody] Sorting sorting)
        {
            var result = await studentsService.GetSortedStudentsAsync(sorting);

            return this.GetActionResult(result, logger);
        }

        [HttpGet("get/{search}")]
        public async Task<IActionResult> SearchStudents(string search)
        {
            var result = await studentsService.SearchStudentsAsync(search);

            return this.GetActionResult(result, logger);
        }
    }
}
