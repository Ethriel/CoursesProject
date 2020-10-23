using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Linq;

namespace Infrastructure.Helpers
{
    public class UpdateHelper<T> where T : class
    {
        /// <summary>
        /// Update values of properties of <paramref name="oldEntity"/> with new values of properties of <paramref name="newEntity"/>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="oldEntity"></param>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public static T Update(IModel model, T oldEntity, T newEntity)
        {
            // get the original type
            var type = UnProxy(model, newEntity.GetType());

            // get only needed properties
            var filteredProperties = type.GetProperties()
                                         .Where(x => x.DeclaringType.Namespace.Equals("Infrastructure.Models"));
            var oldValue = default(object);
            var newValue = default(object);

            var propName = "";
            foreach (var property in filteredProperties)
            {
                propName = property.Name;
                oldValue = type.GetProperty(propName)
                               .GetValue(oldEntity);

                newValue = type.GetProperty(propName)
                               .GetValue(newEntity);

                if (newValue != null)
                {
                    if (!(oldValue is ICollection))
                    {
                        property.SetValue(oldEntity, newValue);
                    }
                }

            }
            return oldEntity;
        }

        /// <summary>
        /// Get the original type of old entity which equals the <paramref name="type"/> of new one
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type UnProxy(IModel model, Type type)
        {
            // get all entity types from db context
            var entityTypes = model.GetEntityTypes();
            Type originalType = null;
            foreach (var entityType in entityTypes)
            {
                originalType = entityType.ClrType;
                if (originalType.Name.Equals(type.Name))
                {
                    break;
                }
            }
            return originalType;
        }
    }
}
