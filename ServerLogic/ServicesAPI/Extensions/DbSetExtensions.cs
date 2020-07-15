using System.Linq;

namespace ServicesAPI.Extensions
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Skips <paramref name="skip"/> items in sequence <paramref name="queryable"/> and then takes <paramref name="take"/> items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static IQueryable<T> GetPortionOfQueryable<T>(this IQueryable<T> queryable, int skip, int take)
            where T : class
        {
            return queryable.Skip(skip).Take(take);
        }
    }
}
