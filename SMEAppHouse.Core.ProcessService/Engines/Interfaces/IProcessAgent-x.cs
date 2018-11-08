using System;

namespace SMEAppHouse.Core.ProcessService.Engines.Interfaces
{
    public interface IProcessAgent
    {

        ProcessAgentStatusEnum ProcessAgentStatus { get; set; }

        int PauseBetween { get; set; }
        Action<bool> BeforeStartCallback { get; set; }
        Action AfterStartCallback { get; set; }
        Action<bool> BeforeSuspendCallback { get; set; }
        Action AfterSuspendCallback { get; set; }
        Action<bool> BeforeResumeCallback { get; set; }
        Action AfterResumeCallback { get; set; }
        Action<bool> BeforeStopCallback { get; set; }
        Action AfterStopCallback { get; set; }
        Action ProcessActionCallback { get; set; }

        void Start();
        void Suspend(int timeout = 0);
        void Resume();
        void Stop(int timeout = 0);
        void WaitForPause();
    }
}
