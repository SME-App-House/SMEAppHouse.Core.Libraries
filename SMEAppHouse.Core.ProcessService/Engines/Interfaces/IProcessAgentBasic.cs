using System;

namespace SMEAppHouse.Core.ProcessService.Engines.Interfaces
{
    public interface IProcessAgentBasic
    {
        void Resume();
        void Suspend(TimeSpan timeout);
        void Suspend(int timeoutMilliSecs = 0);
        void Shutdown(int timeout = 0);
        
    }
}