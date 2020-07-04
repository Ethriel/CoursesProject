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
    public class TrainingCoursesRepository : IRepository<TrainingCourse>
    {
        private readonly CoursesSystemDbContext context;
        public TrainingCoursesRepository(CoursesSystemDbContext context)
        {
            this.context = context;
        }
        public async Task Add(TrainingCourse entity)
        {
            await context.TrainingCourses.AddAsync(entity);
        }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }

        public void Delete(TrainingCourse entity)
        {
            context.TrainingCourses.Remove(entity);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<TrainingCourse> Find(Expression<Func<TrainingCourse, bool>> predicate)
        {
            return await context.TrainingCourses.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TrainingCourse> FindAll(Expression<Func<TrainingCourse, bool>> predicate)
        {
            return context.TrainingCourses.Where(predicate);
        }

        public async Task<TrainingCourse> Get(int id)
        {
            return await context.TrainingCourses.FindAsync(id);
        }

        public IQueryable<TrainingCourse> GetAll()
        {
            return context.TrainingCourses.AsQueryable();
        }

        public void Update(TrainingCourse oldEntity, TrainingCourse newEntity)
        {
            oldEntity = UpdateHelper<TrainingCourse>.Update(context, oldEntity, newEntity);
        }
    }
}
