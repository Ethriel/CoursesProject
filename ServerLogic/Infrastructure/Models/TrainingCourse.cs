using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class TrainingCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public DateTime StartDate { get; set; }
        public virtual ICollection<SystemUser> SystemUsers { get; set; }

        public TrainingCourse()
        {
            SystemUsers = new HashSet<SystemUser>();
        }
    }
}
