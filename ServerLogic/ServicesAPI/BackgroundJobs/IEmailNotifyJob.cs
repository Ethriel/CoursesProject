using Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace ServicesAPI.BackgroundJobs
{
    public interface IEmailNotifyJob
    {
        ICollection<string> CreateJobs(SystemUser user, TrainingCourse course, DateTime studyDate);
    }
}
