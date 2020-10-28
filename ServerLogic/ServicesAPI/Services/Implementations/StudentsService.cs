﻿using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class StudentsService : IStudentsService
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;
        private readonly IExtendedDataService<SystemUser> users;

        public StudentsService(CoursesSystemDbContext context,
                              IMapperWrapper<SystemUser,
                              SystemUserDTO> mapperWrapper,
                              IExtendedDataService<SystemUser> users)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
            this.users = users;
        }

        public async Task<ApiResult> GetAllStudentsAsync()
        {
            var students = await context.SystemUsers
                                        .GetOnlyUsers()
                                        .ToArrayAsync();

            var data = mapperWrapper.MapModels(students);

            var result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> GetUserByIdAsync(int id)
        {
            var result = default(ApiResult);
            var user = await users.GetByIdAsync(id);

            if (user == null)
            {
                var message = "User was not found";
                var loggerMessage = $"User id = {id} was not found";
                var errors = new string[] { message };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                var data = mapperWrapper.MapModel(user);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);
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

            var result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }
    }
}
