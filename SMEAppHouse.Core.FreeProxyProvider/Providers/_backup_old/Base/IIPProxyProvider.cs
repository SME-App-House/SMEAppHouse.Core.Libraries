using System;
using System.Collections.Generic;
using SMEAppHouse.Core.FreeIPProxy.Handlers;
using SMEAppHouse.Core.FreeIPProxy.Models;
using SMEAppHouse.Core.ProcessService.Engines.Interfaces;

namespace SMEAppHouse.Core.FreeIPProxy.Providers.Base
{
    public interface IIPProxyProvider : IProcessAgent
    {
        string HostPageUrlPattern { get; set; }
        int HostPageMax { get; set; }
        Queue<FreeProxy> FreeProxies { get; set; }
        bool ConsumeSelf { get; set; }
        FreeProxy GetFreeProxy(bool dequeue = true);
        Queue<FreeProxy> LoadUpIPProxies(string pageContentDocument, Action<FreeProxy> freeProxyFetched);

        event EventHandlers.FreeIPProxyFetchedEventHandler OnFreeIPProxyFetched;
        event EventHandlers.FreeIPProxiesFetchedEventHandler OnFreeIPProxiesFetched;
        event EventHandlers.FreeIPGeneratorExceptionEventHandler OnFreeIPGeneratorException;

        void RemoveProxy(FreeProxy proxy);
        bool RemoveFreeProxy(FreeProxy proxy);

        void InvokeEventFreeIPProxyFetched(EventHandlers.FreeIPProxyFetchedEventArgs a);
        void InvokeEventFreeIPProxiesFetched(EventHandlers.FreeIPProxiesFetchedEventArgs a);
        void InvokeEventFreeIPGeneratorException(EventHandlers.FreeIPGeneratorExceptionEventArgs a);

    }
}
