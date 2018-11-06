using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    public static class DateTimeExt
    {
        public static DateTime FirstDayOfThisMonth()
        {
            return DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
        }

        public static DateTime EndOfLastMonth()
        {
            return FirstDayOfThisMonth().AddDays(-1);
        }

        public static DateTime FirstDayOfPreviousMonths(int months = 1)
        {
            return FirstDayOfThisMonth().AddMonths(0 - months);
        }

        public static int CalculateElapsedTime(DateTime since, DateTime now, Rules.TimeIntervalTypesEnum dateTimeScale)
        {
            var result = 0;
            var diff = (now - since);

            switch (dateTimeScale)
            {
                case Rules.TimeIntervalTypesEnum.MilliSeconds:
                    result = (int)diff.TotalMilliseconds;
                    break;
                case Rules.TimeIntervalTypesEnum.Seconds:
                    result = (int)diff.TotalSeconds;
                    break;
                case Rules.TimeIntervalTypesEnum.Minutes:
                    result = (int)diff.TotalMinutes;
                    break;
                case Rules.TimeIntervalTypesEnum.Hours:
                    result = (int)diff.TotalHours;
                    break;
                case Rules.TimeIntervalTypesEnum.Days:
                    result = (int)diff.TotalDays;
                    break;
            }

            return result;
        }

        public static bool IsSameDay(this DateTime datetime1, DateTime datetime2)
        {
            return datetime1.Year == datetime2.Year
                && datetime1.Month == datetime2.Month
                && datetime1.Day == datetime2.Day;
        }

        /// <summary>
        /// Routine which will check if current time is exactly in from among qeued times
        /// </summary>
        /// <param name="xTime"></param>
        /// <param name="qeuedTimes"></param>
        /// <returns></returns>
        public static bool IsInExactTime(DateTime xTime, DateTime[] qeuedTimes)
        {
            return qeuedTimes.Any(d => (xTime.Hour == d.Hour) & (xTime.Minute == d.Minute));
        }

        /// <summary>
        /// Check if the current time is between in a time frame
        /// </summary>
        /// <param name="time"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime == startTime)
            {
                return true;
            }
            if (endTime < startTime)
            {
                return time.TimeOfDay <= endTime ||
                       time.TimeOfDay >= startTime;
            }
            return time.TimeOfDay >= startTime &&
                   time.TimeOfDay <= endTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public static bool IsDateBetween(DateTime target, DateTime dateFrom, DateTime dateTo)
        {
            return (target.Ticks > dateFrom.Ticks && target.Ticks < dateTo.Ticks);
        }

        public static TimeSpan ConvertToTimeSpan(this DateTime dateTime)
        {
            return dateTime - (new DateTime(1970, 1, 9, 0, 0, 00));
        }

        public static string ToReport(this TimeSpan end, TimeSpan start, Rules.TimeSpanReportOptionEnum timeSpanReportOption)
        {
            var span = end.Subtract(start);
            return span.ToReport(timeSpanReportOption);
        }

        public static string ToReport(this TimeSpan timeSpan, Rules.TimeSpanReportOptionEnum timeSpanReportOption)
        {
            switch (timeSpanReportOption)
            {
                case Rules.TimeSpanReportOptionEnum.DatePartOnly: return $"{timeSpan.Days} days";
                case Rules.TimeSpanReportOptionEnum.TimePartOnly:
                    return
$"{timeSpan.Hours} hours {timeSpan.Minutes} minutes and {timeSpan.Seconds} seconds";
                case Rules.TimeSpanReportOptionEnum.BothDateAndTimePart: return string.Format("{3} days {0} hours {1} minutes and {2} seconds", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Days);
            }
            return "";
        }

        public static IEnumerable<DateTime> GetDateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                                .Select(d => fromDate.AddDays(d));
        }

        public static IEnumerable<DateTime> GetAllDates(DateTime startingDate, DateTime endingDate, int daysInterval = 1)
        {
            var allDates = new List<DateTime>();
            DateTime d;
            for (d = startingDate; d <= endingDate; d = d.AddDays(daysInterval))
            {
                allDates.Add(d);
            }

            if (d != endingDate)
                allDates.Add(endingDate);

            return allDates.AsReadOnly();
        }

        /// <summary>
        /// Routine which will check if nominated time is a qeued time
        /// </summary>
        /// <param name="xTime">Datetime in question</param>
        /// <param name="qeuedTimes">Queued datetimes</param>
        /// <returns></returns>
        public static bool IsTimeInQueue(DateTime xTime, DateTime[] qeuedTimes)
        {
            int iCtr;
            var aHH = 0;
            var aMM = 0;
            var bHH = 0;
            var bMM = 0;

            for (iCtr = 0; iCtr <= qeuedTimes.GetUpperBound(0); iCtr++)
            {
                aHH = xTime.Hour;
                aHH = xTime.Minute;
                bHH = qeuedTimes[iCtr].Hour;
                bHH = qeuedTimes[iCtr].Minute;
                if ((aHH == bHH) & (aMM == bMM))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Converts a filename friendly string generated from MakeDatetimeFilenameFriendly 
        /// (function) into valid datetime variable
        /// ref: http://stackoverflow.com/questions/3749516/using-a-datetime-as-filename-and-parse-the-filename-afterwards
        /// </summary>
        /// <param name="filenameWdate"></param>
        /// <returns></returns>
        public static DateTime MakeDateOfFilenameString(string filenameWdate)
        {
            var enUS = new CultureInfo("en-US");
            var da = DateTime.ParseExact(filenameWdate, "yyyy-MM-dd_hh-mm-ss", enUS);
            return da;
            //Format(da, "dd.MM.yyyy HH:mm:ss")
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string MakeDatetimeFilenameFriendly(this DateTime date)
        {
            var enUs = new CultureInfo("en-US");
            return date.ToString("yyyy-MM-dd_hh-mm-ss");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="age"></param>
        /// <param name="defaultMonth"></param>
        /// <param name="defaultDay"></param>
        /// <returns></returns>
        public static DateTime MakeFakeBirthdate(int age, int defaultMonth = 1, int defaultDay = 1)
        {
            var birthDay = DateTime.Now.AddYears(-1 * age);
            var year = birthDay.Year;
            birthDay = new DateTime(year, defaultMonth, defaultDay);
            return birthDay;
        }

        /// <summary>
        /// Truncate milliseconds off of a .NET DateTime
        /// http://stackoverflow.com/a/1005222
        /// 
        /// which is used as follows:
        ///
        ///dateTime = dateTime.Truncate(TimeSpan.FromMilliseconds(1)); // Truncate to whole ms
        ///dateTime = dateTime.Truncate(TimeSpan.FromSeconds(1)); // Truncate to whole second
        ///dateTime = dateTime.Truncate(TimeSpan.FromMinutes(1)); // Truncate to whole minute
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            return timeSpan == default(TimeSpan)
                ? dateTime
                : dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        /// <summary>
        /// <para>Truncates a DateTime to a specified resolution.</para>
        /// <para>A convenient source for resolution is TimeSpan.TicksPerXXXX constants.</para>
        /// </summary>
        /// <param name="date">The DateTime object to truncate</param>
        /// <param name="resolution">e.g. to round to nearest second, TimeSpan.TicksPerSecond</param>
        /// <returns>Truncated DateTime</returns>
        public static DateTime Truncate(this DateTime date, long resolution)
        {
            return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
        }
    }
}
             