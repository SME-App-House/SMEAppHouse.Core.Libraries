namespace SMEAppHouse.Core.TopshelfAdapter.Common
{
    public enum InitializationStatusEnum
    {
        NonState,
        Initializing,
        Initialized
    }

    /// <summary>
    /// Allowed log levels in NLog
    /// </summary>
    public enum NLogLevelEnum
    {
        /// <summary>
        /// Highest level: important stuff down
        /// </summary>
        Fatal,

        /// <summary>
        /// For example application crashes / exceptions.
        /// </summary>
        Error,

        /// <summary>
        /// Incorrect behavior but the application can continue
        /// </summary>
        Warn,

        /// <summary>
        /// Normal behavior like mail sent, user updated profile etc.
        /// </summary>
        Info,

        /// <summary>
        /// Executed queries, user authenticated, session expired
        /// </summary>
        Debug,

        /// <summary>
        /// Begin method X, end method X etc
        /// </summary>
        Trace,
    }
}