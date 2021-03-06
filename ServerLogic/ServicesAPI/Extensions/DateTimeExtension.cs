﻿using System;

namespace ServicesAPI.Extensions
{
    public static class DateTimeExtension
    {
        public static bool IsDateInCurrentMonth(this DateTime date)
        {
            var now = DateTime.Now;
            return now.Month == date.Month;
        }

        public static bool IsDateInCurrentWeek(this DateTime date)
        {
            var now = DateTime.Now;

            var calendar = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;

            // check if first day of now and passed date are the same
            var firstDayOfNow = now.AddDays(-(int)calendar.GetDayOfWeek(now));
            var firstDayOfDate = date.AddDays(-(int)calendar.GetDayOfWeek(date));

            return firstDayOfNow == firstDayOfDate;
        }
    }
}
