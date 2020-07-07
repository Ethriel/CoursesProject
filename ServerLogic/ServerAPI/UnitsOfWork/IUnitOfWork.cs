using Infrastructure.DAL.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace ServerAPI.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<SystemUser> Users { get; }
        public UserManager<SystemUser> UserManager { get; }
        public SignInManager<SystemUser> SignInManager { get; }
        public RoleManager<SystemRole> RoleManager { get; }
        public IRepository<TrainingCourse> Courses { get; }
        Task SaveChangesAsync();
    }
}
