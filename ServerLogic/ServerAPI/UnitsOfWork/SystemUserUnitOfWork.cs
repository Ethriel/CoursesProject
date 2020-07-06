using Infrastructure.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ServerAPI.UnitsOfWork
{
    //public class SystemUserUnitOfWork : IUnitOfWork<SystemUser>
    //{
    //    private readonly CoursesSystemDbContext context;
    //    public IRepository<SystemUser> Users { get; }
    //    public UserManager<SystemUser> UserManager { get; }
    //    public SignInManager<SystemUser> SignInManager { get; }

    //    public SystemUserUnitOfWork(CoursesSystemDbContext context, UserManager<SystemUser> userManager,
    //        SignInManager<SystemUser> signInManager)
    //    {
    //        this.context = context;
    //        Users = new SystemUsersRepository(context);
    //        UserManager = userManager;
    //        SignInManager = signInManager;
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
