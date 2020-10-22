using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Abstractions
{
    public interface ICRUDService<TEntity> where TEntity : class
    {
        Task CreateAsync(TEntity entity);
        IQueryable<TEntity> Read();
        Task DeleteAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity old, TEntity @new);
    }
}
