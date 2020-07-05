using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        Task<T> GetFullAsync(int id);
        Task AddAsync(T entity);
        void Update(T oldEntity, T newEntity);
        void Delete(T entity);
    }
}
