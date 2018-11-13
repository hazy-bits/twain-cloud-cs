using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HazyBits.Twain.Cloud.Telemetry.Adapters;

namespace HazyBits.Twain.Cloud.Telemetry
{
    /// <summary>
    /// Main Logger adapter for BankingOn product.
    /// </summary>
    public class Logger
    {
        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="context">Type context for this logger instance.</param>
        protected Logger(Type context)
        {
            Context = context;
            LoggerAdapters = RegisteredLoggerAdapters.ToList();
        }
        
        protected Logger(Type context, IEnumerable<ILoggerAdapter> adapters)
        {
            Context = context;
            LoggerAdapters = adapters;
        }

        #endregion

        #region Properties

        public static IList<IContextExtender> RegisteredContextExtenders => new List<IContextExtender>();

        public static IList<ILoggerAdapter> RegisteredLoggerAdapters => new List<ILoggerAdapter> { new TraceSourceLoggerAdapter() };

        public Type Context { get; }

        public IEnumerable<ILoggerAdapter> LoggerAdapters { get; }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Fabric method that returns new instance of the logger initialized 
        /// with context of the specified type.
        /// </summary>
        /// <typeparam name="T">
        /// Context of the logger.
        /// </typeparam>
        /// <returns>Configured instance of the logger.</returns>
        public static Logger GetLogger<T>()
        {
            return new Logger(typeof(T));
        }

        /// <summary>
        /// Fabric method that returns new instance of the logger initialized 
        /// with context of the specified type and adapters to use for log entries.
        /// </summary>
        /// <typeparam name="T">
        /// Context of the logger.
        /// </typeparam>
        /// <returns>Configured instance of the logger.</returns>
        public static Logger GetLogger<T>(IEnumerable<ILoggerAdapter> adapters)
        {
            return new Logger(typeof(T), adapters);
        }

        #endregion

        #region Main Logger Methods

        /// <summary>
        /// Convenient method that returns new activity scope with specified name.
        /// </summary>
        /// <example>
        /// <code>
        /// Logger log = ...
        /// using(log.StartActivity("Some Interesting Activity"))
        /// { 
        ///    // ...
        /// }
        /// </code>
        /// </example>
        /// <param name="activityName">New activity name.</param>
        /// <returns>Activity scope</returns>
        public virtual ActivityScope StartActivity(string activityName)
        {
            var context = GatherContext(Context);
            return new CompositeActivityScope(activityName, LoggerAdapters.Select(l => l.StartActivity(context, activityName)));
        }

        /// <summary>
        /// Writes an critical message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogCritical(string message)
        {
            LogMessage(LogLevel.Critical, message, null);
        }

        /// <summary>
        /// Writes an error message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogError(string message)
        {
            LogMessage(LogLevel.Error, message, null);
        }

        /// <summary>
        /// Writes a warning message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogWarning(string message)
        {
            LogMessage(LogLevel.Warning, message, null);
        }

        /// <summary>
        /// Writes an informational message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogInfo(string message)
        {
            LogMessage(LogLevel.Info, message, null);
        }

        /// <summary>
        /// Writes a verbose message to the log using specified message.
        /// </summary>
        /// <param name="message">The informative message to write.</param>
        public void LogDebug(string message)
        {
            LogMessage(LogLevel.Debug, message, null);
        }

        public void LogException(LogLevel level, Exception ex, string message)
        {
            var context = GatherContext(Context);
            foreach (var adapter in LoggerAdapters)
                adapter.LogException(context, level, ex, message);
        }

        public void LogMessage(LogLevel level, string message, Dictionary<string, string> props)
        {
            var context = GatherContext(Context);
            foreach (var adapter in LoggerAdapters)
                adapter.LogMessage(context, level, message, props);
        }

        #endregion

        #region Private Methods

        private TelemetryContext GatherContext(Type typeContext)
        {
            var loggerContext = new TelemetryContext
            {
                Type = typeContext,
                TypeContext = typeContext.Name,
                ThreadId = Thread.CurrentThread.ManagedThreadId
            };

            // Extend the context with additional information
            foreach (var contextExtender in RegisteredContextExtenders)
                contextExtender.Extend(loggerContext);

            return loggerContext;
        }

        #endregion

        #region Nested Classes

        private class CompositeActivityScope : ActivityScope
        {
            private readonly IList<ActivityScope> _scopes;

            public CompositeActivityScope(string name, IEnumerable<ActivityScope> scopes) : base(name)
            {
                _scopes = scopes.ToList();
            }

            protected override void Dispose(bool disposing)
            {
                foreach (var s in _scopes)
                    s.Dispose();
            }
        }

        #endregion
    }
}
