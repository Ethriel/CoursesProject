using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var data = await studentsService.GetAllStudentsAsync();
            return Ok(new { data });
        }

        [HttpGet("get/amount")]
        public async Task<IActionResult> GetAmount()
        {
            var amount = await studentsService.GetAmountOfStudentsAync();
            return Ok(new { amount });
        }

        [HttpPost("post/sort")]
        public async Task<IActionResult> GetSortedStudents([FromBody] Sorting sorting)
        {
            var data = await studentsService.GetSortedStudentsAsync(sorting);
            return Ok(new { data });
        }
    }
}
