using System;
using System.Collections.Generic;

namespace HazyBits.Twain.Cloud.Telemetry
{
    /// <summary>
    /// Base interface for logger adapters.
    /// </summary>
    public interface ILoggerAdapter
    {
        /// <summary>
        /// Starts new logger activity with specified name.
        /// </summary>
        /// <param name="context">Current telemetry context.</param>
        /// <param name="name">Name of the activity.</param>
        /// <returns>New <see cref="ActivityScope"/> instance.</returns>
        ActivityScope StartActivity(TelemetryContext context, string name);

        /// <summary>
        /// Logs message with provided level and optional additional properties.
        /// </summary>
        /// <param name="context">Current telemetry context.</param>
        /// <param name="level">Level of the log entry.</param>
        /// <param name="message">Log message.</param>
        /// <param name="props">Additional properties to include into log entry.</param>
        void LogMessage(TelemetryContext context, LogLevel level, string message, Dictionary<string, string> props);

        /// <summary>
        /// Logs exception with provided level.
        /// </summary>
        /// <param name="context">Current telemetry context.</param>
        /// <param name="level">Level of the log entry.</param>
        /// <param name="ex">Exception instance to put into logs.</param>
        /// <param name="message">Log message.</param>
        void LogException(TelemetryContext context, LogLevel level, Exception ex, string message);
    }
}