#pragma warning disable 1591
namespace SMED.Core.WebAPI.Patterns.ServiceWrapper
{
    public interface IJobService
    {
        void Execute();
        void SubscriberInitialize();
        void SubscriberExecute();
    }
}