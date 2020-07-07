using Infrastructure.DAL.Interfaces;
using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ServerAPI.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoursesSystemDbContext context;
        public IRepository<SystemUser> Users { get; }

        public UserManager<SystemUser> UserManager { get; }

        public SignInManager<SystemUser> SignInManager { get; }
        public RoleManager<SystemRole> RoleManager { get; }

        public IRepository<TrainingCourse> Courses { get; }

        public UnitOfWork(CoursesSystemDbContext context, UserManager<SystemUser> userManager,
            SignInManager<SystemUser> signInManager, RoleManager<SystemRole> roleManager, IRepository<SystemUser> users, IRepository<TrainingCourse> courses)
        {
            this.context = context;
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Users = users;
            Courses = courses;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
