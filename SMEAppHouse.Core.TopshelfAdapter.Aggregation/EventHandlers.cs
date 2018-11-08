// ReSharper disable InconsistentNaming

using SMEAppHouse.Core.TopshelfAdapter.Common;
using System;

namespace SMEAppHouse.Core.TopshelfAdapter.Aggregation
{

    #region Service Initializer

    public class ServiceWorkerInitializedEventArgs : EventArgs
    {
        public ITopshelfClient ServiceWorker { get; private set; }

        public ServiceWorkerInitializedEventArgs(ITopshelfClient serviceWorker)
        {
            ServiceWorker = serviceWorker;
        }

    }

    public delegate void ServiceWorkerInitializedEventHandler(object sender, ServiceWorkerInitializedEventArgs e);

    #endregion
    
    public static class EventHandlers
    {
        public static void InvokeEvent(this ServiceWorkerInitializedEventArgs e, object sender, ServiceWorkerInitializedEventHandler handler)
        {
            handler?.Invoke(sender, e);
        }
        
    }
}
