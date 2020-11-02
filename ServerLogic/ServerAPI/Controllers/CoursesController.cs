using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAPI.Extensions;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
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
        private readonly IStudentsService studentsService;
        private readonly IWebHostEnvironment webHost;

        public CoursesController(ICoursesService coursesService,
                                 ILogger<CoursesController> logger,
                                 IStudentsService studentsService,
                                 IWebHostEnvironment webHost)
        {
            this.coursesService = coursesService;
            this.logger = logger;
            this.studentsService = studentsService;
            this.webHost = webHost;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await coursesService.GetAllCoursesAsync();

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

        [HttpGet("get/user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await studentsService.GetUserByIdAsync(id);

            return this.GetActionResult(result, logger);
        }

        //[HttpPost("imageUpload")]
        //public IActionResult UploadImage([FromForm(Name = "image")]IFormFile image)
        //{
        //    var result = studentsService.UploadImageTest(image, webHost);

        //    return this.GetActionResult(result, logger);
        //}

        [Authorize(Roles = "ADMIN")]
        [HttpPost("add")]
        public async Task<IActionResult> CreateAsync([FromBody] TrainingCourseDTO course)
        {
            var result = await coursesService.CreateCourseAsync(course);

            return this.GetActionResult(result, logger);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateCourse([FromBody] TrainingCourseDTO course)
        {
            var result = await coursesService.UpdateCourseAsync(course);

            return this.GetActionResult(result, logger);
        }
    }
}
