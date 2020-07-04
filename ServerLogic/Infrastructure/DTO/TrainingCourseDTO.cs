﻿using System;
using System.Collections.Generic;

namespace Infrastructure.DTO
{
    public class TrainingCourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<SystemUsersTrainingCoursesDTO> SystemUsersTrainingCourses { get; set; }
    }
}
