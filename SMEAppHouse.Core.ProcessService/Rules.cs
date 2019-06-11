using System;

namespace SMEAppHouse.Core.ProcessService
{
    public enum EngineStatusEnum
    {
        NonState,
        RunningState,
        PausedState
    }

    public class EngineStatusChangedEventArgs : EventArgs
    {
        public EngineStatusEnum EngineStatus { get; private set; }

        public EngineStatusChangedEventArgs(EngineStatusEnum engineStatus)
        {
            EngineStatus = engineStatus;
        }
    }
    public delegate void EngineStatusChangedEventHandler(object sender, EngineStatusChangedEventArgs e);

    public class BeforeEngineStatusChangedEventArgs : EngineStatusChangedEventArgs
    {
        public bool Cancel { get; set; }

        public BeforeEngineStatusChangedEventArgs(EngineStatusEnum engineStatus)
            : base(engineStatus)
        {
        }
    }
    public delegate void BeforeEngineStatusChangedEventHandler(object sender, BeforeEngineStatusChangedEventArgs e);

}
