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
            var originalType = UnProxy(model, newEntity.GetType());

            // get all properties
            var properties = originalType.GetProperties();

            // get only needed properties
            var filteredProperties = properties.Where(x => x.DeclaringType
                                                            .Namespace
                                                            .Equals("Infrastructure.Models"));
            var oldType = oldEntity.GetType();

            object oldValue = null;
            object newValue = null;

            var propName = "";
            foreach (var property in filteredProperties)
            {
                propName = property.Name;
                oldValue = oldType.GetProperty(propName)
                                  .GetValue(oldEntity);

                newValue = originalType.GetProperty(propName)
                                       .GetValue(newEntity);

                if (!oldValue.Equals(newValue) && !(oldValue is ICollection))
                {
                    property.SetValue(oldEntity, newValue);
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
