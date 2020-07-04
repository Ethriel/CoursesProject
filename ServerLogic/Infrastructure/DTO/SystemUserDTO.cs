using System;
using System.Collections.Generic;

namespace Infrastructure.DTO
{
    public class SystemUserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime StudyDate { get; set; }
        public string AvatarPath { get; set; }
        public ICollection<SystemUsersTrainingCoursesDTO> SystemUsersTrainingCourses { get; set; }
    }
}
