using Infrastructure.DbContext;
using Infrastructure.Models;

namespace ServicesAPI.Repositories
{
    public class SystemUserRepository : RepositoryBase<SystemUser>
    {
        public SystemUserRepository(CoursesSystemDbContext context) : base(context)
        {

        }
    }
}
