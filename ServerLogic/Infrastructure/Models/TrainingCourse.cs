using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class TrainingCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public virtual ICollection<SystemUsersTrainingCourses> SystemUsersTrainingCourses { get; set; }

        public TrainingCourse()
        {
            SystemUsersTrainingCourses = new HashSet<SystemUsersTrainingCourses>();
        }
    }
}
