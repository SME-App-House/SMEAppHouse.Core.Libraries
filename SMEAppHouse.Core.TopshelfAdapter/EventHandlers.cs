using System;

namespace SMEAppHouse.Core.TopshelfAdapter
{
    public class ServiceInitializedEventArgs : EventArgs
    {
        public ServiceInitializedEventArgs()
        {
        }
    }

    public delegate void ServiceInitializedEventHandler(object sender, ServiceInitializedEventArgs e);

    public static class EventHandlers
    {
        public static void InvokeEvent(this ServiceInitializedEventArgs e, object sender, ServiceInitializedEventHandler handler)
        {
            handler?.Invoke(sender, e);
        }
    }
}
