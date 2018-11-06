using System;

namespace SMEAppHouse.Core.CodeKits.Helpers
{
    public static class EventHandlerHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="handler"></param>
        public static void InvokeEvent(this EventArgs e, object sender, Delegate handler)
        {
            handler?.DynamicInvoke(sender, e);
        }

        /// <summary>
        /// http://stackoverflow.com/questions/340610/create-empty-c-sharp-event-handlers-automatically
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void RaiseEvent<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            handler?.Invoke(sender, args);
        }
    }
}
