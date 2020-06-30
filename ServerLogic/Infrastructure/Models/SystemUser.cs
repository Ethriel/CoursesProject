using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class SystemUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime StudyDate { get; set; }
        public virtual SystemRole SystemRole { get; set; }
        public virtual ICollection<TrainingCourse> TrainingCourses { get; set; }
        public SystemUser()
        {
            CalculateAge();
            TrainingCourses = new HashSet<TrainingCourse>();
        }

        private void CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age))
                age--;
            Age = age;
        }
    }
}
