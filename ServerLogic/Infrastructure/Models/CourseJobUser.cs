using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class CourseJobUser
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string JobId { get; set; }
        public int UserId { get; set; }
    }
}
