using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class SystemUsersTrainingCourses
    {
        public int SystemUserId { get; set; }
        public SystemUser SystemUser {get; set; }
        public int TrainingCourseId { get; set; }
        public TrainingCourse TrainingCourse {get; set; }
    }
}
