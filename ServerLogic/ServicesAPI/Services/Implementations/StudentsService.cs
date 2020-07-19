using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using ServicesAPI.Sorts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class StudentsService : IStudentsService
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;

        public StudentsService(CoursesSystemDbContext context, IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
        }

        public async Task<ApiResult> GetAllStudentsAsync()
        {
            var students = await context.SystemUsers
                                        .GetOnlyUsers()
                                        .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(students);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning all students. Count = {students.Length}", data);

            return result;
        }

        public async Task<ApiResult> GetAmountOfStudentsAync()
        {
            var amount = await context.SystemUsers
                                      .GetOnlyUsers()
                                      .CountAsync();

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning amount of students. Amount = {amount}", amount);

            return result;
        }

        public async Task<ApiResult> GetSortedStudentsAsync(Sorting sorting)
        {
            var skip = sorting.Pagination.GetSkip();
            var take = sorting.Pagination.GetTake();

            var students = await context.SystemUsers
                                        .GetOnlyUsers()
                                        .GetPortionOfQueryable(skip, take)
                                        .GetSortedUsers(sorting.SortOrder, sorting.SortField)
                                        .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(students);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning a portion of students. Count = {students.Length}", data);

            return result;
        }

        public async Task<ApiResult> SearchStudentsAsync(string search)
        {
            IEnumerable<SystemUser> users = default;
            var result = new ApiResult();

            search = search.ToLower();
            if (search.Contains(" "))
            {
                var criterias = search.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

                users = await context.SystemUsers
                                    .GetOnlyUsers()
                                    .SearchByCriterias(criterias)
                                    .ToArrayAsync();
            }
            else
            {
                users = await context.SystemUsers
                                    .GetOnlyUsers()
                                    .SearchByCriteria(search)
                                    .ToArrayAsync();
            }

            var data = mapperWrapper.MapCollectionFromEntities(users);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning users by search criteria: \"{search}\"", data);

            return result;
        }
    }
}
