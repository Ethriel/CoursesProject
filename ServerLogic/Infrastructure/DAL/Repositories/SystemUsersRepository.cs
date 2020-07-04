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

        public async Task Add(SystemUser entity)
        {
            await context.SystemUsers.AddAsync(entity);
        }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }

        public void Delete(SystemUser entity)
        {
            context.SystemUsers.Remove(entity);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<SystemUser> Find(Expression<Func<SystemUser, bool>> predicate)
        {
            return await context.SystemUsers.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<SystemUser> FindAll(Expression<Func<SystemUser, bool>> predicate)
        {
            return context.SystemUsers.Where(predicate);
        }

        public async Task<SystemUser> Get(int id)
        {
            return await context.SystemUsers.FindAsync(id);
        }

        public IQueryable<SystemUser> GetAll()
        {
            return context.SystemUsers.AsQueryable();
        }

        public void Update(SystemUser oldEntity, SystemUser newEntity)
        {
            oldEntity = UpdateHelper<SystemUser>.Update(context, oldEntity, newEntity);
        }
    }
}
