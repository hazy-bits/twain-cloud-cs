using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace HazyBits.Twain.Cloud.Telemetry.Adapters
{
    /// <summary>
    /// Logger adapter for standard .NET TraceSource logger.
    /// </summary>
    /// <seealso cref="HazyBits.Twain.Cloud.Telemetry.ILoggerAdapter" />
    public class TraceSourceLoggerAdapter : ILoggerAdapter
    {
        private static readonly TraceSource GlobalTranceSource = new TraceSource("HazyBits.Twain.Cloud");

        #region Public Properties

        /// <summary>
        /// Gets trace source that is used to write messages to.
        /// </summary>
        public TraceSource Source { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceLoggerAdapter"/> class.
        /// </summary>
        public TraceSourceLoggerAdapter()
        {
            // For now. If we want to use more detailed sources, we can do that here.
            Source = GlobalTranceSource;
        }

        /// <inheritdocs />
        public ActivityScope StartActivity(TelemetryContext context, string name)
        {
            return new CorrelationActivityScope(this, context, name);
        }

        /// <inheritdocs />
        public virtual void LogMessage(TelemetryContext context, LogLevel level, string message, Dictionary<string, string> props)
        {
            if (ShouldLog(level))
            {
                var logEntry = props == null ? message : $"{message}: { JsonConvert.SerializeObject(props, Formatting.Indented) }";
                var eventType = ToEventType(level);
                LogTraceEvent(context, eventType, logEntry);
            }
        }

        /// <inheritdocs />
        public virtual void LogException(TelemetryContext context, LogLevel level, Exception ex, string message)
        {
            if (ShouldLog(level))
            {
                var logEntry = $"{message}: {ex}";
                var eventType = ToEventType(level);
                LogTraceEvent(context, eventType, logEntry);
            }
        }

        #region Private Methods

        protected bool ShouldLog(LogLevel level)
        {
            return Source.Switch.ShouldTrace(ToEventType(level));
        }

        protected virtual void LogTraceEvent(TelemetryContext context, TraceEventType eventType, string message)
        {
            Source.TraceEvent(eventType, 0, $"[{context.TypeContext}]: {message}");
        }

        internal static TraceEventType ToEventType(LogLevel level)
        {
            switch(level)
            {
                case LogLevel.Critical:
                    return TraceEventType.Critical;
                case LogLevel.Error:
                    return TraceEventType.Error;
                case LogLevel.Warning:
                    return TraceEventType.Warning;
                case LogLevel.Info:
                    return TraceEventType.Information;
                case LogLevel.Debug:
                default:
                    return TraceEventType.Verbose;
            }
        }

        #endregion

        #region Nested Classes

        private class CorrelationActivityScope : ActivityScope
        {
            private static readonly object CorrelationLock = new object();

            private readonly TraceSourceLoggerAdapter _logger;
            private readonly TelemetryContext _context;
            private readonly string _activityName;            
            private readonly Guid _oldActivityId;

            public CorrelationActivityScope(TraceSourceLoggerAdapter logger, TelemetryContext context, string activityName) : base(activityName)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));                
                _activityName = activityName;
                _context = context;

                var newActivityId = Guid.NewGuid();
                _logger.Source.TraceTransfer(0, "Starting", newActivityId);

                lock (CorrelationLock)
                {
                    _oldActivityId = Trace.CorrelationManager.ActivityId;
                    Trace.CorrelationManager.ActivityId = newActivityId;
                    Trace.CorrelationManager.StartLogicalOperation(activityName);
                }

                _logger.LogTraceEvent(_context, TraceEventType.Start, activityName);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _logger.LogTraceEvent(_context, TraceEventType.Stop, _activityName);
                    _logger.Source.TraceTransfer(0, "Finishing", _oldActivityId);

                    lock (CorrelationLock)
                    {
                        Trace.CorrelationManager.StopLogicalOperation();
                        Trace.CorrelationManager.ActivityId = _oldActivityId;
                    }
                }
            }
        }

        #endregion
    }
}
