using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.Helpers;
using ServerAPI.MapperWrappers;
using ServerAPI.UnitsOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : Controller
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<TrainingCourse, TrainingCourseDTO> mapperWrapper;

        public CoursesController(CoursesSystemDbContext context, IMapperWrapper<TrainingCourse, TrainingCourseDTO> mapperWrapper)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
        }

        [HttpGet("get/amount")]
        public async Task<IActionResult> GetAmount()
        {
            try
            {
                var courses = await context.TrainingCourses.ToArrayAsync();
                var data = courses.Length;
                return Ok(new { amount = data });
            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = $"Exception occured: {ex.Message}", errors = errors });
            }
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courses = await context.TrainingCourses.ToArrayAsync();
                var data = mapperWrapper.MapCollectionFromEntities(courses);
                return Ok(data);
            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = $"Exception occured: {ex.Message}", errors = errors });
            }

        }
        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            try
            {
                var courses = await context.TrainingCourses.Skip(skip).Take(take).ToArrayAsync();
                var data = mapperWrapper.MapCollectionFromEntities(courses);
                return Ok(data);
            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = $"Exception occured: {ex.Message}", errors = errors });
            }
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var course = await context.TrainingCourses.FirstOrDefaultAsync(x=>x.Id.Equals(id));
                var data = mapperWrapper.MapFromEntity(course);
                return Ok(data);
            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromModelState.GetErrors(ModelState);
                return BadRequest(new { message = $"Exception occured: {ex.Message}", errors = errors });
            }
        }
    }
}
