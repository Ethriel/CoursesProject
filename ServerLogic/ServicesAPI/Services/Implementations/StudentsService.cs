using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.Helpers;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class StudentsService : IStudentsService
    {
        private readonly CoursesSystemDbContext context;
        private readonly IMapperWrapper<SystemUser, SystemUserDTO> mapperWrapper;
        private readonly IExtendedDataService<SystemUser> users;
        private readonly IImageUploader imageUploader;
        private readonly IServerService serverService;

        public StudentsService(CoursesSystemDbContext context,
                              IMapperWrapper<SystemUser,
                              SystemUserDTO> mapperWrapper,
                              IExtendedDataService<SystemUser> users,
                              IImageUploader imageUploader,
                              IServerService serverService)
        {
            this.context = context;
            this.mapperWrapper = mapperWrapper;
            this.users = users;
            this.imageUploader = imageUploader;
            this.serverService = serverService;
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

        public async Task<ApiResult> UploadImageAsync(IFormFile image, int id, HttpContext httpContext)
        {
            var result = default(ApiResult);

            var fileName = imageUploader.UploadImage(image, "users");

            if (string.IsNullOrWhiteSpace(fileName))
            {
                var message = "An error occured during file upload";
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, "Error in upload file for USER", message, new string[] { message });
            }
            else
            {
                var user = await users.GetByIdAsync(id);

                if (user == null)
                {
                    var message = "User was not found";
                    result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, $"Error: user id={id} was not found in UPLOAD FILE", message, new string[] { message });
                }
                else
                {
                    if (File.Exists(user.AvatarPath))
                    {
                        File.Delete(user.AvatarPath);
                    }

                    user.AvatarPath = fileName;
                    await context.SaveChangesAsync();
                    var model = mapperWrapper.MapModel(user);

                    var avatarPath = serverService.GetServerURL(httpContext);
                    avatarPath += imageUploader.GetPathForURL(fileName, "users");
                    model.AvatarPath = avatarPath;

                    result = ApiResult.GetOkResult(ApiResultStatus.Ok, "Image was uploaded", model);
                }
            }

            return result;
        }
    }
}
