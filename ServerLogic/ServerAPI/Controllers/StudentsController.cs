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
    [Route("[controller]")]
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
            try
            {
                var data = await studentsService.GetAllStudentsAsync();
                return Ok(new { data });
            }
            catch
            {

                throw;
            }
        }

        [HttpGet("get/amount")]
        public async Task<IActionResult> GetAmount()
        {
            try
            {
                var amount = await studentsService.GetAmountOfStudentsAync();
                return Ok(new { amount });
            }
            catch
            {

                throw;
            }
        }

        [HttpPost("post/sort")]
        public async Task<IActionResult> GetSortedStudents([FromBody] Sorting sorting)
        {
            try
            {
                var data = await studentsService.GetSortedStudentsAsync(sorting);
                return Ok(new { data });
            }
            catch
            {

                throw;
            }
        }
    }
}
