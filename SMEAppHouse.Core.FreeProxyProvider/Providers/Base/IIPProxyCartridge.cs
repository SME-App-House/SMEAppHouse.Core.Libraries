using System.Collections.Generic;
using SMEAppHouse.Core.FreeIPProxy.Handlers;
using SMEAppHouse.Core.ProcessService.Engines.Interfaces;
using SMEAppHouse.Core.ScraperBox.Models;

namespace SMEAppHouse.Core.FreeIPProxy.Providers.Base
{
    public interface IIPProxyCartridge : IProcessAgentBasic
    {
        string TargetPageUrlPattern { get; }
        int PageNo { get; }
        int TotalPagesScraped { get; }
        PageInstruction PageInstruction { get; }
        IList<IPProxy> IPProxies { get; }
        IPProxyAgentStatusEnum AgentStatus { get; set; }

        void GrabFirstOrNextPage(IPProxy proxy = null);
        void ClearProxyBucket();

        event EventHandlers.FreeIPProxyParsedEventHandler OnFreeIPProxyParsed;
        event EventHandlers.FreeIPProxiesParsedEventHandler OnFreeIPProxiesParsed;
        event EventHandlers.FreeIPGeneratorExceptionEventHandler OnFreeIPGeneratorException;

        event EventHandlers.FreeIPProxiesReadingEventHandler OnFreeIPProxiesReading;
        event EventHandlers.FreeIPProviderSourceCompletedEventHandler OnFreeIPProviderSourceCompleted;

        void InvokeEventFreeIPProxyParsed(EventHandlers.FreeIPProxyParsedEventArgs a);
        void InvokeEventFreeIPProxiesParsed(EventHandlers.FreeIPProxiesParsedEventArgs a);
        void InvokeEventFreeIPGeneratorException(EventHandlers.FreeIPGeneratorExceptionEventArgs a);

        void InvokeEventFreeIPProxiesReading(EventHandlers.FreeIPProxiesReadingEventArgs a);
        void InvokeEventFreeIPProviderSourceCompleted(EventHandlers.FreeIPProviderSourceCompletedEventArgs a);
    }

    public enum IPProxyAgentStatusEnum
    {
        Idle,
        Reading,
        Parsing,
        Parsed,
        Completed,
    }
}
