// ReSharper disable InconsistentNaming

using System;

namespace SMEAppHouse.Core.Scheduler
{

    public class ScheduleReachedEventArg : EventArgs
    {
        public Schedule NewSchedule { get; set; }
        public Schedule LastSchedule { get; set; }

        public ScheduleReachedEventArg(Schedule newSchedule, Schedule lastSchedule)
        {
            NewSchedule = newSchedule;
            LastSchedule = lastSchedule;
        }
    }
    public delegate void ScheduleReachedEventHandler(object sender, ScheduleReachedEventArg e);

    public static class EventHandlers
    {
        public static void InvokeEvent(this ScheduleReachedEventArg e, object sender, ScheduleReachedEventHandler handler)
        {
            handler?.Invoke(sender, e);
        }
    }
}
