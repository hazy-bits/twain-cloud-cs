namespace HazyBits.Twain.Cloud.Telemetry
{
    /// <summary>
    /// Identifies the type of event that has caused the trace.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Disabled logging.
        /// </summary>
        Off,
        /// <summary>
        /// Fatal error or application crash.
        /// </summary>
        Critical,
        /// <summary>
        /// Recoverable error.
        /// </summary>
        Error,
        /// <summary>
        /// Noncritical problem.
        /// </summary>
        Warning,
        /// <summary>
        /// Informational message.
        /// </summary>
        Info,
        /// <summary>
        /// Debugging trace.
        /// </summary>
        Debug
    }
}
