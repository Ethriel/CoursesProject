using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class StudentsService : IStudentsService
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;
        private readonly IExtendedDataService<SystemUser> userService;

        public StudentsService(CoursesSystemDbContext context,
                              IMapperWrapper<SystemUser,
                              SystemUserDTO> mapperWrapper,
                              IExtendedDataService<SystemUser> userService)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
            this.userService = userService;
        }

        public async Task<ApiResult> GetAllStudentsAsync()
        {
            var students = await context.SystemUsers
                                        .GetOnlyUsers()
                                        .ToArrayAsync();

            var data = mapperWrapper.MapModels(students);

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning all students. Count = {students.Length}", data);

            return result;
        }

        public async Task<ApiResult> GetUserByIdAsync(int id)
        {
            var result = new ApiResult();

            var errors = default(IEnumerable<string>);

            var user = await userService.GetByIdAsync(id);

            if (user == null)
            {
                var message = $"User id = {id} was not found";
                errors = new string[] { message };
                result.SetApiResult(ApiResultStatus.NotFound, loggerMessage: message, message: message, errors: errors);
            }
            else
            {
                var data = mapperWrapper.MapModel(user);
                result.SetApiResult(ApiResultStatus.Ok, $"Returning user id = {id}", data);
            }

            return result;
        }

        public async Task<ApiResult> SearchAndSortStudentsAsync(SearchAndSort searchAndSort)
        {
            var systemUsers = default(IQueryable<SystemUser>);

            var allUsers = context.SystemUsers
                                  .GetOnlyUsers();

            var amount = await allUsers.CountAsync();

            var pagination = new Pagination();
            pagination.SetDefaults(amount, pageSize: 5);

            searchAndSort.Pagination ??= pagination;

            var skip = searchAndSort.Pagination
                                    .GetSkip();

            var take = searchAndSort.Pagination
                                    .GetTake();

            if (searchAndSort.IsSearch)
            {
                systemUsers = allUsers.SearchStudents(searchAndSort.SearchField)
                                      .GetSortedUsers(searchAndSort.Sort);
            }
            else
            {
                systemUsers = allUsers.GetSortedUsers(searchAndSort.Sort);
            }

            var students = await systemUsers.GetPortionOfQueryable(skip, take)
                                            .ToArrayAsync();

            var users = mapperWrapper.MapModels(students);

            var data = new { pagination = searchAndSort.Pagination, users = users };

            var result = new ApiResult(ApiResultStatus.Ok, $"Returning a portion of students.", data);

            return result;
        }
    }
}
