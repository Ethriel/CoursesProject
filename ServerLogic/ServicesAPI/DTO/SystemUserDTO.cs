using System.Collections.Generic;

namespace ServicesAPI.DTO
{
    public class SystemUserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string BirthDate { get; set; }
        public int Age { get; set; }
        public string RegisteredDate { get; set; }
        public string StudyDate { get; set; }
        public string AvatarPath { get; set; }
        public ICollection<SystemUsersTrainingCoursesDTO> SystemUsersTrainingCourses { get; set; }
    }
}
