using System;
using NodaTime;
using NodaTime.Text;

namespace SMEAppHouse.Core.Scheduler
{
    public class Schedule
    {
        public Guid Id { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }
        public LocalTime StartOfTime { get; set; }

        public LocalTime? EndOfTime { get; set; }

        public string Title { get; set; }

        public LocalDateTime Actual => (_date + StartOfTime);

        private readonly LocalDate _date;

        public Schedule()
            : this(default(LocalTime))
        { }

        /// <summary>
        /// endOfTime is advanced 30 seconds by default after startOfTime 
        /// since value is not specified by this constructor.
        /// </summary>
        /// <param name="startOfTime"></param>
        public Schedule(LocalTime startOfTime)
            : this(startOfTime, startOfTime.PlusSeconds(30))
        {

        }

        public Schedule(LocalTime startOfTime, LocalTime endOfTime)
            : this(GetTimezoneCurrentDateTime().Date, startOfTime, endOfTime)
        {
        }

        public Schedule(LocalDate setDate) 
            : this(setDate, default(LocalTime), default(LocalTime).PlusSeconds(30))
        {
        }

        public Schedule(LocalDate setDate, LocalTime startOfTime, LocalTime endOfTime)
        {
            _date = setDate;
            Id = Guid.NewGuid();
            StartOfTime = startOfTime;
            EndOfTime = endOfTime;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/27853402/create-a-nodatime-localdate-representing-today
        /// First recognize that when you say "today", the answer could be different for different people in different parts 
        /// of the world. Therefore, in order to get the current local date, you must have a time zone in mind.
        /// </summary>
        /// <returns></returns>
        public static ZonedDateTime GetTimezoneCurrentDateTime()
        {
            // get the current time from the system clock
            var now = SystemClock.Instance.GetCurrentInstant();
            // get a time zone
            var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            // use now and tz to get "today"
            return now.InZone(tz);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static LocalTime Parse(string time)
        {
            try
            {
                var timePattern = LocalTimePattern.CreateWithInvariantCulture("h:mmtt");
                var parseResult = timePattern.Parse(time);
                if (parseResult.Success)
                    return parseResult.Value;

                throw new InvalidOperationException("Time is in incorrect format (suggested:\"h:mmtt\" or 5:30PM)");
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
