using System;
using System.Collections.Generic;
using System.Linq;

namespace SMEAppHouse.Core.AppMgt.Messaging
{
    public class PayloadsEnvelope: IPayloadsEnvelope
    {
        /// <inheritdoc />
        public PayloadsEnvelope()
        {
            PayloadsQueue = new Queue<Tuple<string, object>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Queue<Tuple<string, object>> PayloadsQueue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preamble"></param>
        /// <param name="payload"></param>
        public void QueuePayload(string preamble, object payload)
        {
            lock (PayloadsQueue)
            {
                PayloadsQueue.Enqueue(new Tuple<string, object>(preamble, payload));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tuple<string, object> DequeuePayload()
        {
            lock (PayloadsQueue)
            {
                return PayloadsQueue.Any() ? PayloadsQueue.Dequeue() : null;
            }
        }

    }
}