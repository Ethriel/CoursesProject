using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Services.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class ExtendedDataService<TEntity> : DataService<TEntity>, IExtendedDataService<TEntity>
        where TEntity : class
    {
        public ExtendedDataService(CoursesSystemDbContext context) : base(context)
        {

        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await Set.FindAsync(id);
        }

        public IQueryable<TEntity> GetEntitiesByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return Set.Where(expression);
        }

        public async Task<TEntity> GetEntityByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return await Set.FirstOrDefaultAsync(expression);
        }
    }
}
