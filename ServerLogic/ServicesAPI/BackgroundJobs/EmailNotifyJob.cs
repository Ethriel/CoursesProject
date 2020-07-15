using Hangfire;
using Infrastructure.Models;
using ServicesAPI.Extensions;
using ServicesAPI.Services.Abstractions;
using System;

namespace ServicesAPI.BackgroundJobs
{
    public class EmailNotifyJob : IEmailNotifyJob
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        private const string STANDARD_MESSAGE = "Your course \"{0}\" starts {1}";
        private const string SUBJECT = "Course start notify";

        public EmailNotifyJob(IBackgroundJobClient backgroundJobClient)
        {
            this.backgroundJobClient = backgroundJobClient;
        }
        public void CreateJobs(SystemUser user, TrainingCourse course, DateTime studyDate)
        {
            var email = user.Email;
            var title = course.Title;
            ScheduleTaskForMonth(email, title, studyDate);
            ScheduleTaskForWeek(email, title, studyDate);
            StartTaskAtStudyDate(email, title, studyDate);
        }
        private void ScheduleTaskForWeek(string email, string title, DateTime studyDate)
        {
            // no need to schedule task for one week
            if (studyDate.IsDateInCurrentWeek())
            {
                return;
            }

            var notifyDate = studyDate.AddDays(-7);
            ScheduleTask(notifyDate, studyDate, title, email);
        }
        private void ScheduleTaskForMonth(string email, string title, DateTime studyDate)
        {
            // no need to schedule task for one month
            if (studyDate.IsDateInCurrentMonth())
            {
                return;
            }

            var notifyDate = studyDate.AddMonths(-1);
            ScheduleTask(notifyDate, studyDate, title, email);
        }
        private void StartTaskAtStudyDate(string email, string title, DateTime studyDate)
        {
            var notifyDate = studyDate;
            var insideMessage = $"today at {studyDate.ToShortDateString()}!";
            ScheduleTask(notifyDate, studyDate, title, email, insideMessage);
        }

        private void ScheduleTask(DateTime notifyDate, DateTime studyDate, string title, string email, string insideMessage = null)
        {
            var days = (studyDate - notifyDate).TotalDays;
            insideMessage ??= $"in {days} at {studyDate.ToShortDateString()}";
            var message = string.Format(STANDARD_MESSAGE, title, insideMessage);
            notifyDate.AddHours(8);
            backgroundJobClient.Schedule<ISendEmailService>((x) => x.SendEmail(email, SUBJECT, message), notifyDate);
        }
    }
}
