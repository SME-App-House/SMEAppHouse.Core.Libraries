// ***********************************************************************
// Assembly         : SMEAppHouse.Core.Patterns.WebApi
// Author           : jcman
// Created          : 07-04-2018
//
// Last Modified By : jcman
// Last Modified On : 07-04-2018
// ***********************************************************************
// <copyright file="HttpActionException.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Net;

namespace SMEAppHouse.Core.Patterns.WebApi.Exceptions
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
