using NLog;

namespace SMEAppHouse.Core.TopshelfAdapter.Common
{
    public interface ITopshelfClient
    {
        InitializationStatusEnum InitializationStatus { get; set; }
        bool IsPaused { get; }
        bool IsResumed { get; }
        bool IsTerminated { get; }

        Logger Logger { get; set; }

        void Resume();
        void Suspend();
        void Shutdown();

        void NLog(string data);
        void NLog(string data, bool includeWriteToConsole);
        void NLog(NLogLevelEnum nLogLevel, string data);
        void NLog(NLogLevelEnum nLogLevel, string data, bool includeWriteToConsole);

    }
}