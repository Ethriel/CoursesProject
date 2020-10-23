using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface IExtendedDataService<TEntity> : IDataService<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetByIdAsync(object id);
        IQueryable<TEntity> GetEntitiesByCondition(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetEntityByConditionAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetPortion(int skip, int take);
        Task<int> GetCountAsync();
    }
}
