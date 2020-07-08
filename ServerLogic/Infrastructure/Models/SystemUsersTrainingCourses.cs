using System;

namespace Infrastructure.Models
{
    public class SystemUsersTrainingCourses
    {
        public int SystemUserId { get; set; }
        public virtual SystemUser SystemUser { get; set; }
        public int TrainingCourseId { get; set; }
        public virtual TrainingCourse TrainingCourse { get; set; }
        public DateTime StudyDate { get; set; }
    }
}
