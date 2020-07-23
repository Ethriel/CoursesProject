using Infrastructure.DbContext;
using System;
using System.Linq;

namespace Infrastructure.Helpers
{
    public class UpdateHelper<T> where T : class
    {
        public static T Update(CoursesSystemDbContext context, T oldEntity, T newEntity)
        {
            var type = UnProxy(context, newEntity.GetType());
            var properties = type.GetProperties();
            var filteredProperties = properties.Where(x => x.DeclaringType
                                                            .Namespace
                                                            .Equals("Infrastructure.Models"));
            object propValue = null;
            var propName = "";
            foreach (var property in filteredProperties)
            {
                propName = property.Name;
                propValue = type.GetProperty(propName)
                                .GetValue(newEntity);

                property.SetValue(oldEntity, propValue);
            }
            return oldEntity;
        }
        private static Type UnProxy(CoursesSystemDbContext context, Type type)
        {
            var entityTypes = context.Model.GetEntityTypes();
            Type tmp = null;
            foreach (var entityType in entityTypes)
            {
                tmp = entityType.ClrType;
                if (tmp.Name.Equals(type.Name))
                {
                    break;
                }
            }
            return tmp;
        }
    }
}
