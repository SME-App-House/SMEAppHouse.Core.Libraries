using System;
using System.Threading;

namespace SMEAppHouse.Core.ProcessService.Engines.Interfaces
{
    public interface IProcessAgent
    {
        EngineStatusEnum EngineStatus { get; set; }

        TimeSpan PauseTime { get; set; }
        int PauseBetween { get; set; }

        Action<bool> BeforeSuspendCallback { get; set; }
        Action AfterSuspendCallback { get; set; }
        Action<bool> BeforeResumeCallback { get; set; }
        Action AfterResumeCallback { get; set; }
        Action<bool> BeforeStopCallback { get; set; }
        Action<bool> BeforeStartCallback { get; set; }
        Action AfterStartCallback { get; set; }
        Action AfterShutdownCallback { get; set; }
        Action ProcessActionCallback { get; set; }
        
        void Suspend(int timeout = 0);

        void Shutdown(int timeout = 0);
        void Start();
        void Resume();

        void WaitForPause();

        Thread Thread { get; }

        //void ServiceActionCallback();

    }
}
