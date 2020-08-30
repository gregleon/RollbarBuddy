using System;

namespace RollbarBuddy.Utils
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }

        public static bool IsWorkDay(this DateTime date, bool isWorkDaySaturday = false)
        {
            if (date == null) { throw new ArgumentNullException(nameof(date)); }

            return date.DayOfWeek switch
            {
                DayOfWeek.Saturday => isWorkDaySaturday,
                DayOfWeek.Sunday => false,
                _ => true
            };
        }

        /// <summary>
        /// return the previous business date of the date specified.
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public static DateTime PreviousBusinessDay(this DateTime today)
        {
            var result = today.DayOfWeek switch
            {
                DayOfWeek.Sunday => today.AddDays(-2),
                DayOfWeek.Monday => today.AddDays(-3),
                DayOfWeek.Tuesday => today.AddDays(-1),
                DayOfWeek.Wednesday => today.AddDays(-1),
                DayOfWeek.Thursday => today.AddDays(-1),
                DayOfWeek.Friday => today.AddDays(-1),
                DayOfWeek.Saturday => today.AddDays(-1),
                _ => throw new ArgumentOutOfRangeException("DayOfWeek " + today.DayOfWeek)
            };

            return result;
        }
    }
}
