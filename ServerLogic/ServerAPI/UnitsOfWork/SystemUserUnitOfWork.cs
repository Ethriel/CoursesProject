using Infrastructure.DAL.Interfaces;
using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.UnitsOfWork
{
    public class SystemUserUnitOfWork : IUnitOfWork
    {
        private readonly CoursesSystemDbContext context;
        public IRepository<SystemUser> Users { get; }
        public UserManager<SystemUser> UserManager { get; }
        public SignInManager<SystemUser> SignInManager { get; }

        public SystemUserUnitOfWork(CoursesSystemDbContext context, IRepository<SystemUser> users, UserManager<SystemUser> userManager,
            SignInManager<SystemUser> signInManager)
        {
            this.context = context;
            Users = users;
            UserManager = userManager;
            SignInManager = signInManager;
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
