using Infrastructure.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using Infrastructure.DbContext;
using Infrastructure.Models;
using System.Threading.Tasks;

namespace ServerAPI.UnitsOfWork
{
    //public class TrainingCoursesUnitOfWork : IUnitOfWork<TrainingCourse>
    //{
    //    private readonly CoursesSystemDbContext context;
    //    public IRepository<TrainingCourse> Courses { get; }
    //    public TrainingCoursesUnitOfWork(CoursesSystemDbContext context)
    //    {
    //        this.context = context;
    //        Courses = new TrainingCoursesRepository(context);
    //    }
    //    public void Dispose()
    //    {
    //        context.Dispose();
    //    }

    //    public async Task SaveChangesAsync()
    //    {
    //        await context.SaveChangesAsync();
    //    }
    //}
}
