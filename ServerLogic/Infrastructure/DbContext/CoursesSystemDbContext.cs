using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext
{
    public class CoursesSystemDbContext : IdentityDbContext<SystemUser, SystemRole, int>
    {
        #region Db sets

        public virtual DbSet<SystemUser> SystemUsers { get; set; }
        public virtual DbSet<SystemRole> SystemRoles { get; set; }
        public virtual DbSet<TrainingCourse> TrainingCourses { get; set; }
        public virtual DbSet<SystemUsersTrainingCourses> SystemUsersTrainingCourses { get; set; }

        #endregion
        public CoursesSystemDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionStringHelper.Connection);
            }
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // configure composite primary key
            builder.Entity<SystemUsersTrainingCourses>()
                .HasKey(sutc => new { sutc.SystemUserId, sutc.TrainingCourseId });

            builder.Entity<SystemUsersTrainingCourses>()
                .HasOne(sutc => sutc.SystemUser)
                .WithMany(su => su.SystemUsersTrainingCourses)
                .HasForeignKey(sutc => sutc.SystemUserId);

            builder.Entity<SystemUsersTrainingCourses>()
                .HasOne(sutc => sutc.TrainingCourse)
                .WithMany(tc => tc.SystemUsersTrainingCourses)
                .HasForeignKey(sutc => sutc.TrainingCourseId);

            builder.Entity<SystemRole>()
                .HasMany(x => x.SystemUsers)
                .WithOne(x => x.SystemRole);
        }
    }
}
