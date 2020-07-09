using Infrastructure.DbContext;
using Infrastructure.DTO;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.MapperWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServerAPI.Sorts;

namespace ServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : Controller
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;

        public StudentsController(CoursesSystemDbContext context, IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await context.Users.Where(x => x.SystemRole.Name.Equals("USER")).ToArrayAsync();
            var data = mapperWrapper.MapCollectionFromEntities(students);
            return Ok(new { data = data });
        }

        [HttpGet("get/amount")]
        public async Task<IActionResult> GetAmount()
        {
            var students = await context.Users.Where(x => x.SystemRole.Name.Equals("USER")).ToArrayAsync();
            var data = students.Length;
            return Ok(new { amount = data });
        }

        [HttpGet("get/forpage/{skip}/{take}")]
        public async Task<IActionResult> GetForPage(int skip, int take)
        {
            var allStudents = await context.Users.Where(x => x.SystemRole.Name.Equals("USER")).ToArrayAsync();
            var students = allStudents.Skip(skip).Take(take);
            var data = mapperWrapper.MapCollectionFromEntities(students);
            return Ok(new { data = data });
        }

        [HttpPost("post/sort")]
        public async Task<IActionResult> GetSortedStudents([FromBody] Sorting sorting)
        {
            var page = sorting.Pagination.Current;
            var pageSize = sorting.Pagination.PageSize;
            var skip = (page * pageSize) - pageSize;
            var take = pageSize == 1 ? pageSize : page * pageSize;
            var allStudents = await context.Users.Where(x => x.SystemRole.Name.Equals("USER")).ToArrayAsync();
            var students = allStudents.Skip(skip).Take(take);
            var data = mapperWrapper.MapCollectionFromEntities(students);

            return Ok(new { data = data });
        }
    }
}
