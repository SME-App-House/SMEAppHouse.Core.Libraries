using System;

namespace SMEAppHouse.Core.ProcessService.Exceptions
{
    /// <summary>
    ///     Exception for Category actions.
    /// </summary>
    public class DoNotExecMethodException : Exception
    {
        public DoNotExecMethodException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoNotExecMethodException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The original response exception.</param>
        public DoNotExecMethodException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        ///     Gets the detail exception.
        /// </summary>
        /// <value>
        ///     The detail exception.
        /// </value>
        public Exception DetailException
        {
            get { return InnerException ?? this; }
        }
    }
}