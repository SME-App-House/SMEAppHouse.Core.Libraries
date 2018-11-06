using System.Threading;

#pragma warning disable 1591
namespace SMEAppHouse.Core.QuartzExt
{
    public interface IJobService
    {
        void Execute(ThreadPriority? threadPrio = ThreadPriority.Normal);
        void SubscriberInitialize();
        void SubscriberExecute();
    }
}