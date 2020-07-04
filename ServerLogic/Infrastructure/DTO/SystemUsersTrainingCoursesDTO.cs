namespace Infrastructure.DTO
{
    public class SystemUsersTrainingCoursesDTO
    {
        public int SystemUserId { get; set; }
        public SystemUserDTO SystemUser { get; set; }
        public int TrainingCourseId { get; set; }
        public TrainingCourseDTO TrainingCourse { get; set; }
    }
}
