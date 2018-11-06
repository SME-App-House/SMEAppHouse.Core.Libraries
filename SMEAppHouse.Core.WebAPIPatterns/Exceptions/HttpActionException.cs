using System;
using System.Net;

namespace SMED.Core.WebAPI.Patterns.Exceptions
{
    public class HttpActionException : Exception
    {

        public HttpStatusCode HttpStatusCode { get; set; }

        public HttpActionException()
        {
        }

        public HttpActionException(string message)
            : base(message)
        {
        }

        public HttpActionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
