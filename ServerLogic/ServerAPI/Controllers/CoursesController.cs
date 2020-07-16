using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAPI.Services.Abstractions;
using System;
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
            var amount = await coursesService.GetAmountAsync();
            return Ok(new { amount });

        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await coursesService.GetAllCoursesAsync();
            return Ok(new { courses });
        }
        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            var courses = await coursesService.GetForPage(skip, take);
            return Ok(new { courses });

        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await coursesService.GetById(id);
            return Ok(new { course });
        }
    }
}
