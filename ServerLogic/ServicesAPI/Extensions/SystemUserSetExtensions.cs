using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServicesAPI.Extensions
{
    public static class SystemUserSetExtensions
    {
        /// <summary>
        /// Get only system users with role USER from <paramref name="set"/>
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static IQueryable<SystemUser> GetOnlyUsers(this DbSet<SystemUser> set)
        {
            return set.Where(x => x.SystemRole.Name.Equals("USER"));
        }

        /// <summary>
        /// Sort users from <paramref name="queryable"/> by <paramref name="sortField"/> and order by <paramref name="sortOrder"/>
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="sortOrder"></param>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public static IQueryable<SystemUser> GetSortedUsers(this IQueryable<SystemUser> queryable, string sortOrder, string sortField)
        {
            IQueryable<SystemUser> result = null;
            var descend = "descend";
            sortOrder ??= descend;
            sortField ??= "id";
            switch (sortField)
            {
                case "id":
                    {
                        result = sortOrder.Equals(descend) ? queryable.OrderByDescending(x => x.Id) : queryable.OrderBy(x => x.Id);
                        break;
                    }
                case "firstname":
                    {
                        result = sortOrder.Equals(descend) ? queryable.OrderByDescending(x => x.FirstName) : queryable.OrderBy(x => x.FirstName);
                        break;
                    }
                case "lastname":
                    {
                        result = sortOrder.Equals(descend) ? queryable.OrderByDescending(x => x.LastName) : queryable.OrderBy(x => x.LastName);
                        break;
                    }
                case "age":
                    {
                        result = sortOrder.Equals(descend) ? queryable.OrderByDescending(x => x.Age) : queryable.OrderBy(x => x.Age);
                        break;
                    }
                default:
                    {
                        result = result.OrderBy(x => x.Id);
                        break;
                    }
            }
            return result;
        }
    }
}
