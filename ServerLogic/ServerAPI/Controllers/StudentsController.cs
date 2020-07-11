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
            var allStudents = context.Users.Where(x => x.SystemRole.Name.Equals("USER"));
            var descend = "descend";

            switch (sorting.SortField)
            {
                case "id":
                    {
                        allStudents = sorting.SortOrder.Equals(descend) ? allStudents.OrderByDescending(x => x.Id) : allStudents.OrderBy(x => x.Id);
                        break;
                    }
                case "firstname":
                    {
                        allStudents = sorting.SortOrder.Equals(descend) ? allStudents.OrderByDescending(x => x.FirstName) : allStudents.OrderBy(x => x.FirstName);
                        break;
                    }
                case "lastname":
                    {
                        allStudents = sorting.SortOrder.Equals(descend) ? allStudents.OrderByDescending(x => x.LastName) : allStudents.OrderBy(x => x.LastName);
                        break;
                    }
                case "age":
                    {
                        allStudents = sorting.SortOrder.Equals(descend) ? allStudents.OrderByDescending(x => x.Age) : allStudents.OrderBy(x => x.Age);
                        break;
                    }
                default:
                    {
                        allStudents = allStudents.OrderBy(x => x.Id);
                        break;
                    }
            }

            var students = await allStudents.Skip(skip).Take(take).ToArrayAsync();
            var data = mapperWrapper.MapCollectionFromEntities(students);

            return Ok(new { data = data });
        }
    }
}
