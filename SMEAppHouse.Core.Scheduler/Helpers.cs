using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using NodaTime.Extensions;

namespace SMEAppHouse.Core.Scheduler
{
    public static class Helpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public static bool IsMomentInSchedule(Schedule[] schedules)
        {
            Schedule sched = null;
            return IsMomentInSchedule(schedules, out sched);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedules"></param>
        /// <param name="scheduleDetected"></param>
        /// <returns></returns>
        public static bool IsMomentInSchedule(Schedule[] schedules, out Schedule scheduleDetected)
        {
            var schedMet = GetScheduleOfTheMoment(schedules);
            scheduleDetected = schedMet;
            return (schedMet != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public static Schedule GetScheduleOfTheMoment(Schedule[] schedules)
        {
            return GetScheduleOfTheMoment(schedules, Duration.FromSeconds(30));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedules"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static Schedule GetScheduleOfTheMoment(Schedule[] schedules, Duration? duration)
        {
            var localDate = Schedule.GetTimezoneCurrentDateTime().LocalDateTime;
            return duration.HasValue ? GetScheduleForTime(schedules, localDate, duration) : GetScheduleForTime(schedules, localDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="schedules"></param>
        /// <returns></returns>
        public static Schedule GetScheduleForTime(Schedule[] schedules, LocalDateTime current)
        {
            return GetScheduleForTime(schedules, current, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedules"></param>
        /// <param name="current"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static Schedule GetScheduleForTime(Schedule[] schedules, LocalDateTime current, Duration? duration)
        {
            try
            {

                Func<Schedule, Duration?, LocalTime> getTrueDuration = (sched, durtion) =>
                {
                    var ticks = duration?.ToTimeSpan().Ticks ?? 0;
                    var trueDuration = sched.EndOfTime.HasValue && sched.EndOfTime.Value != null
                        ? sched.EndOfTime.Value
                        : (sched.StartOfTime.PlusTicks(ticks));

                    return trueDuration;
                };

                // compare schedules with days first.
                var withDays = schedules.Where(p => p.DayOfWeek.HasValue && p.DayOfWeek.Value.ToIsoDayOfWeek() == current.DayOfWeek)?.ToArray();
                if (withDays.Any()) // check if any schedule has a hit on any day set
                    return withDays.FirstOrDefault(sched => (current.TimeOfDay >= sched.StartOfTime && current.TimeOfDay <= getTrueDuration(sched, duration)));

                var timeScheds = schedules.OrderBy(e => e.StartOfTime).ToArray();
                var result = timeScheds.FirstOrDefault(sched => (current.TimeOfDay >= sched.StartOfTime && current.TimeOfDay <= getTrueDuration(sched, duration)));

                return result;
            }
            catch (Exception exception)
            {
                throw;
            }

        }

        #region Extensions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedules"></param>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public static Schedule GetTrailing(this IReadOnlyList<Schedule> schedules, Schedule schedule)
        {

            var sorted = schedules.OrderBy(p => p.Actual)?.ToList();
            for (var i = 0; i < sorted.Count; i++)
            {
                var sched = sorted[i];
                if (sched.Id == schedule.Id && i == 0)
                {
                    var sch = schedules[schedules.Count - 1];
                    var yesterday = Schedule.GetTimezoneCurrentDateTime().Date.Minus(Period.FromDays(-1));
                    var result = new Schedule(yesterday)
                    {
                        Id = sch.Id,
                        StartOfTime = sch.StartOfTime,
                        EndOfTime = sch.EndOfTime,
                        Title = sch.Title,
                    };
                    return result;
                }
                else if (sched.Id == schedule.Id)
                    return schedules[i - 1];
            }
            return null;
        }

        #endregion
    }
}
