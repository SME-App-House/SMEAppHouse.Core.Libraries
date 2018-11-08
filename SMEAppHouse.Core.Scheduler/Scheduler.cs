using NodaTime;
using SMEAppHouse.Core.ProcessService.Engines;

namespace SMEAppHouse.Core.Scheduler
{
    public class Scheduler : ProcessAgentViaTask
    {
        public event ScheduleReachedEventHandler OnScheduleReached = delegate { };

        public Schedule[] Schedules { get; set; }
        public Duration Duration { get; set; }
        public Schedule LastScheduleReached { get; set; }

        #region constructors
        public Scheduler()
            : base()
        {
        }

        public Scheduler(Schedule[] schedules)
            : this(schedules, NodaTime.Duration.FromSeconds(30))
        {
        }

        public Scheduler(Schedule[] schedules, Duration duration)
            : base()
        {
            Schedules = schedules;
            Duration = duration;
        }
        #endregion

        protected override void ServiceActionCallback()
        {
            var newSched = Helpers.GetScheduleOfTheMoment(this.Schedules, this.Duration);

            if (newSched == null || (LastScheduleReached != null && LastScheduleReached.Id != newSched.Id))
                return;

            (new ScheduleReachedEventArg(newSched, LastScheduleReached)).InvokeEvent(this, OnScheduleReached);
            LastScheduleReached = newSched;
        }
    }
}
