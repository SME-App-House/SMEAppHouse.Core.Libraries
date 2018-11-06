using System;
using System.Collections.Generic;

namespace SMEAppHouse.Core.AppMgt.Messaging
{
    public interface IPayloadsEnvelope
    {
        Queue<Tuple<string, object>> PayloadsQueue { get; set; }
        void QueuePayload(string preamble, object payload);
        Tuple<string, object> DequeuePayload();
    }
}