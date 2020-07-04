using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Helpers
{
    public class UpdateHelper<T> where T : class
    {
        public static T Update(CoursesSystemDbContext context, T oldEntity, T newEntity)
        {
            context.Entry(oldEntity).State = EntityState.Detached;
            var type = oldEntity.GetType();
            var properties = type.GetProperties();
            object propValue = null;
            var propName = "";
            foreach (var p in properties)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    propName = properties[i].Name;
                    if (i == 0 && propName.Contains("Id"))
                    {
                        continue;
                    }
                    propValue = type.GetProperty(propName).GetValue(newEntity);
                    properties[i].SetValue(oldEntity, propValue);
                }
            }
            context.Entry(oldEntity).State = EntityState.Modified; 
            return oldEntity;
        }
    }
}
