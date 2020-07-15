using Infrastructure.DbContext;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ServicesAPI.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        private readonly CoursesSystemDbContext context;
        public RepositoryBase(CoursesSystemDbContext context)
        {
            this.context = context;
        }
        public void Create(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
            }
            catch
            {
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
            }
            catch
            {
                throw;
            }
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            try
            {
                return context.Set<T>().Where(expression);
            }
            catch
            {
                throw;
            }
        }

        public IQueryable<T> GetAll()
        {
            try
            {
                return context.Set<T>().AsQueryable();
            }
            catch
            {
                throw;
            }
        }

        public void Update(T entity)
        {
            try
            {
                context.Set<T>().Update(entity);
            }
            catch
            {
                throw;
            }
        }
    }
}
