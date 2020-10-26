using Hangfire;
using Infrastructure.DbContext;
using Infrastructure.Models;
using ServicesAPI.Extensions;
using ServicesAPI.Services.Abstractions;
using System;
using System.Collections.Generic;

namespace ServicesAPI.BackgroundJobs
{
    public class EmailNotifyJob : IEmailNotifyJob
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly ICourseJobUserHandler courseJobUser;
        private const string STANDARD_MESSAGE = "Your course \"{0}\" starts {1}";
        private const string SUBJECT = "Course start notify";

        public EmailNotifyJob(IBackgroundJobClient backgroundJobClient, ICourseJobUserHandler courseJobUser)
        {
            this.backgroundJobClient = backgroundJobClient;
            this.courseJobUser = courseJobUser;
        }
        public ICollection<string> CreateJobs(SystemUser user, TrainingCourse course, DateTime studyDate)
        {
            ICollection<string> jobs = new List<string>();

            var job = ScheduleTaskForMonth(user, course, studyDate);
            jobs = AddJob(job, jobs);

            job = ScheduleTaskForWeek(user, course, studyDate);
            jobs = AddJob(job, jobs);

            job = StartTaskAtStudyDate(user, course, studyDate);
            jobs = AddJob(job, jobs);

            return jobs;
        }
        private string ScheduleTaskForWeek(SystemUser user, TrainingCourse course, DateTime studyDate)
        {
            // no need to schedule task for one week
            if (studyDate.IsDateInCurrentWeek())
            {
                return null;
            }

            var notifyDate = studyDate.AddDays(-7);
            var job = ScheduleTask(notifyDate, studyDate, course, user);
            return job;
        }
        private string ScheduleTaskForMonth(SystemUser user, TrainingCourse course, DateTime studyDate)
        {
            // no need to schedule task for one month
            if (studyDate.IsDateInCurrentMonth())
            {
                return null;
            }

            var notifyDate = studyDate.AddMonths(-1);
            var job = ScheduleTask(notifyDate, studyDate, course, user);
            return job;
        }
        private string StartTaskAtStudyDate(SystemUser user, TrainingCourse course, DateTime studyDate)
        {
            var notifyDate = studyDate;
            var messageBody = $"today at {studyDate.ToShortDateString()}!";
            var job = ScheduleTask(notifyDate, studyDate, course, user, messageBody);
            return job;
        }

        private string ScheduleTask(DateTime notifyDate, DateTime studyDate, TrainingCourse course, SystemUser user, string messageBody = null)
        {
            var days = (studyDate - notifyDate).TotalDays;
            messageBody ??= $"in {days} at {studyDate.ToShortDateString()}";
            var message = string.Format(STANDARD_MESSAGE, course.Title, messageBody);
            notifyDate = notifyDate.AddHours(8);
            var job = backgroundJobClient.Schedule<ISendEmailService>((x) => x.SendEmail(user.Email, SUBJECT, message), notifyDate);
            return job;
        }

        private ICollection<string> AddJob(string job, ICollection<string> jobs)
        {
            if (!string.IsNullOrWhiteSpace(job))
            {
                jobs.Add(job);
            }
            return jobs;
        }
    }
}
