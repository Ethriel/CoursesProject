using Infrastructure.Models;
using System;

namespace ServicesAPI.BackgroundJobs
{
    public interface IEmailNotifyJob
    {
        void CreateJobs(SystemUser user, TrainingCourse course, DateTime studyDate);
    }
}
