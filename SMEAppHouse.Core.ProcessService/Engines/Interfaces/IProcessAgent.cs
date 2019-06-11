using System;

namespace SMEAppHouse.Core.ProcessService.Engines.Interfaces
{
    public interface IProcessAgent : IProcessAgentBasic
    {
        EngineStatusEnum EngineStatus { get; set; }
        
        TimeSpan IntervalDelay { get; set; }
        bool AutoStart { get; set; }
        bool AutoActivate { get; set; }

        event BeforeEngineStatusChangedEventHandler OnBeforeEngineChanged;
        event EngineStatusChangedEventHandler OnEngineStatusChanged;

    }

    
}
