using NLog;
using NodaTime;
using SMEAppHouse.Core.Scheduler;

namespace SMEAppHouse.Core.TopshelfAdapter.Scheduler
{
    public class Scheduler : TopshelfSocketBase<Scheduler>
    {
        public event ScheduleReachedEventHandler OnScheduleReached = delegate { };

        public Schedule[] Schedules { get; set; }
        public Duration Duration { get; set; }
        public Schedule LastScheduleReached { get; set; }

        #region constructors
        public Scheduler()
            : this(null, null)
        {
        }

        public Scheduler(Logger logger, Schedule[] schedules)
            : this(logger, schedules, NodaTime.Duration.FromSeconds(30))
        {

        }

        public Scheduler(Logger logger, Schedule[] schedules, Duration duration)
            : base(logger)
        {
            Schedules = schedules;
            Duration = duration;
        }

        #endregion

        protected override void ServiceInitializeCallback()
        {
            //throw new NotImplementedException();
        }

        protected override void ServiceTerminateCallback()
        {
            //throw new NotImplementedException();
        }

        protected override void ServiceActionCallback()
        {
            var newSched = Helpers.GetScheduleOfTheMoment(this.Schedules, this.Duration);

            // if no schedule is hit or last detected schedule is same as this one, exit!
            if (newSched == null || (LastScheduleReached != null && LastScheduleReached.Id == newSched.Id))
                return;

            (new ScheduleReachedEventArg(newSched, LastScheduleReached)).InvokeEvent(this, OnScheduleReached);
            LastScheduleReached = newSched;
        }
    }
}


