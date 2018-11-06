using Newtonsoft.Json.Linq;
using System;

namespace SMEAppHouse.Core.GHClientLib.Exceptions
{
    public class GHCalculationException : Exception
    {
        public dynamic AuditTrace { get; set; }

        public GHCalculationException()
        {
        }

        public GHCalculationException(string message)
            : base(message)
        {
        }

        public GHCalculationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GHCalculationException(string message, Exception inner, dynamic auditData)
            : this(message, inner)
        {
            var trcJson = AuditTraceToJson(auditData);
            base.Data.Add("auditTrace", trcJson);
        }

        private static string AuditTraceToJson(dynamic auditTrace)
        {
            if (auditTrace == null) return "{}";
            var trc = JToken.FromObject(auditTrace);
            {
                return trc.ToString();
            }
        }
    }
}