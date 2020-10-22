using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Services.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class CRUDService<TEntity> : ICRUDService<TEntity> where TEntity : class
    {
        private readonly CoursesSystemDbContext context;
        private readonly DbSet<TEntity> set;

        public CRUDService(CoursesSystemDbContext context)
        {
            this.context = context;
            set = context.Set<TEntity>();
        }
        public async Task CreateAsync(TEntity entity)
        {
            set.Add(entity);
            await ConfirmChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            set.Remove(entity);
            await ConfirmChangesAsync();
        }

        public IQueryable<TEntity> Read()
        {
            var entities = set.AsQueryable();
            return entities;
        }

        public async Task<TEntity> UpdateAsync(TEntity old, TEntity @new)
        {
            context.Entry(old).CurrentValues.SetValues(@new);
            await ConfirmChangesAsync();
            return old;
        }

        private async Task<int> ConfirmChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
