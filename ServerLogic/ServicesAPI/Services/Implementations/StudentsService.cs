using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DTO;
using ServicesAPI.Extensions;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Services.Abstractions;
using ServicesAPI.Sorts;
using System.Collections.Generic;
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

        public async Task<IEnumerable<SystemUserDTO>> GetAllStudentsAsync()
        {
            var students = await context.Users.GetOnlyUsers()
                                              .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(students);
            return data;

        }

        public async Task<int> GetAmountOfStudentsAync()
        {
            var amount = await context.Users.GetOnlyUsers()
                                            .CountAsync();

            return amount;
        }

        public async Task<IEnumerable<SystemUserDTO>> GetSortedStudentsAsync(Sorting sorting)
        {
            var skip = sorting.Pagination.GetSkip();
            var take = sorting.Pagination.GetTake();

            var students = await context.SystemUsers.GetOnlyUsers()
                                                    .GetPortionOfQueryable(skip, take)
                                                    .GetSortedUsers(sorting.SortOrder, sorting.SortField)
                                                    .ToArrayAsync();

            var data = mapperWrapper.MapCollectionFromEntities(students);

            return data;
        }
    }
}
