using Infrastructure.DAL.Interfaces;
using Infrastructure.DbContext;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Repositories
{
    public class SystemUsersRepository : IRepository<SystemUser>
    {
        private readonly CoursesSystemDbContext context;
        public SystemUsersRepository(CoursesSystemDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(SystemUser entity)
        {
            await context.SystemUsers.AddAsync(entity);
        }

        public void Delete(SystemUser entity)
        {
            context.SystemUsers.Remove(entity);
        }

        public async Task<SystemUser> FindAsync(Expression<Func<SystemUser, bool>> predicate)
        {
            return await context.SystemUsers.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<SystemUser> FindAll(Expression<Func<SystemUser, bool>> predicate)
        {
            return context.SystemUsers.Where(predicate);
        }

        public async Task<SystemUser> GetAsync(int id)
        {
            return await context.SystemUsers.FindAsync(id);
        }

        public IQueryable<SystemUser> GetAll()
        {
            return context.SystemUsers.AsQueryable();
        }

        public async Task<SystemUser> GetFullAsync(int id)
        {
            return await context.SystemUsers.Include(x => x.SystemRole).Include(x => x.SystemUsersTrainingCourses).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(SystemUser oldEntity, SystemUser newEntity)
        {
            oldEntity = UpdateHelper<SystemUser>.Update(context, oldEntity, newEntity);
        }
    }
}
