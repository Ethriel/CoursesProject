using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DataPresentation;
using System.Collections.Generic;
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
            return set.Where(x => x.SystemRole
                                   .Name
                                   .Equals("USER"));
        }

        /// <summary>
        /// Sort users in <paramref name="queryable"/> by <paramref name="sortField"/> and order by <paramref name="sortOrder"/>
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="sortOrder"></param>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public static IQueryable<SystemUser> GetSortedUsers(this IQueryable<SystemUser> queryable, Sort sort)
        {
            IQueryable<SystemUser> result = null;
            var descend = "descend";
            var sortOrder = sort.SortOrder ?? descend;
            var sortField = sort.SortField ?? "id";
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
        public static IQueryable<SystemUser> SearchStudents(this IQueryable<SystemUser> queryable, string criteria)
        {
            criteria = criteria.ToLower();

            if (criteria.Contains(" "))
            {
                var criterias = criteria.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

                return queryable.SearchByCriterias(criterias);
            }
            else
            {
                return queryable.SearchByCriteria(criteria);
            }
        }
        /// <summary>
        /// Search users in <paramref name="queryable"/> by <paramref name="criteria"/>
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IQueryable<SystemUser> SearchByCriteria(this IQueryable<SystemUser> queryable, string criteria)
        {
            var users = queryable.Where(x => x.FirstName.ToLower()
                                                        .Contains(criteria));

            if (!users.Any())
            {
                users = queryable.Where(x => x.LastName.ToLower()
                                                       .Contains(criteria));

                if (!users.Any())
                {
                    users = queryable.Where(x => x.Email.ToLower()
                                                        .Contains(criteria));
                }
            }

            return users;
        }

        /// <summary>
        /// Search users in <paramref name="queryable"/> by <paramref name="criterias"/>
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="criterias"></param>
        /// <returns></returns>
        public static IQueryable<SystemUser> SearchByCriterias(this IQueryable<SystemUser> queryable, IEnumerable<string> criterias)
        {
            IQueryable<SystemUser> users = default;

            foreach (var criteria in criterias)
            {
                users = queryable.SearchByCriteria(criteria);

                if (users.Any())
                {
                    break;
                }
            }

            return users;
        }
    }
}
