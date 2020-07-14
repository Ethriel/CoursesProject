using Infrastructure.Models;
using System;

namespace ServerAPI.BackgroundJobs
{
    public interface IEmailNotifyJob
    {
        void CreateJobs(SystemUser user, TrainingCourse course, DateTime studyDate);
    }
}
