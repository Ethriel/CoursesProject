using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServicesAPI.Extensions
{
    public static class UserCoursesSetExtensions
    {
        public static IQueryable<SystemUsersTrainingCourses> GetCoursesByUserId(this IQueryable<SystemUsersTrainingCourses> queryable, int userId)
        {
            return queryable.Include(x => x.TrainingCourse)
                            .Where(x => x.SystemUserId.Equals(userId));
        }
    }
}
