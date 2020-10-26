using Infrastructure.DbContext;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Services.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public abstract class DataService<TEntity> : IDataService<TEntity>
        where TEntity : class
    {
        public DbSet<TEntity> Set { get; set; }
        public CoursesSystemDbContext Context { get; }
        public DataService(CoursesSystemDbContext context)
        {
            Set = context.Set<TEntity>();
            Context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            Set.Add(entity);
            await ConfirmChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Set.Remove(entity);
            await ConfirmChangesAsync();
        }

        public IQueryable<TEntity> Read()
        {
            var entities = Set.AsQueryable();
            return entities;
        }

        public async Task<TEntity> UpdateAsync(TEntity old, TEntity @new)
        {
            old = UpdateHelper<TEntity>.Update(Context.Model, old, @new);
            await ConfirmChangesAsync();
            return old;
        }

        private async Task<int> ConfirmChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
