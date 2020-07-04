using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DAL.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll();
        Task<T> Get(int id);
        Task<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        void Update(T oldEntity, T newEntity);
        void Delete(T entity);

        Task Commit();
    }
}
