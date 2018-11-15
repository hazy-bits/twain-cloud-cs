using System;
using System.Collections.Generic;
using System.Linq;
using HazyBits.Twain.Cloud.Telemetry;
using NUnit.Framework;

namespace HazyBits.Twain.Cloud.UnitTests
{
    [TestFixture]
    public class LoggerTests
    {
        [Test]
        public void LoggerAdapterRegistrationAffectsExistingLoggers()
        {
            var logger = Logger.GetLogger<LoggerTests>();
            Assert.That(logger.LoggerAdapters.Count(), Is.EqualTo(1));

            Logger.RegisteredLoggerAdapters.Add(new TestLoggerAdapter());
            Assert.That(logger.LoggerAdapters.Count(), Is.EqualTo(2));
        }

        [Test]
        public void TestLoggerAdapterRegistration()
        {
            var logger = Logger.GetLogger<LoggerTests>();

            var adapter = new TestLoggerAdapter();
            Logger.RegisteredLoggerAdapters.Add(adapter);
            
            logger.LogCritical("critical message");
            logger.LogError("error message");
            logger.LogWarning("warning message");
            logger.LogInfo("info message");
            logger.LogDebug("debug message");
            logger.LogException(LogLevel.Critical, new Exception(), "exception message");
            using (logger.StartActivity("test activity"));

            Assert.That(adapter.MessagesLogged, Is.EqualTo(5));
            Assert.That(adapter.ExceptionsLogged, Is.EqualTo(1));
            Assert.That(adapter.ActivitiesStarted, Is.EqualTo(1));

            Logger.RegisteredLoggerAdapters.TryTake(out var _);
        }

        private class TestLoggerAdapter : ILoggerAdapter
        {
            public int ExceptionsLogged { get; private set; }

            public int MessagesLogged { get; private set; }

            public int ActivitiesStarted { get; private set; }

            public void LogException(TelemetryContext context, LogLevel level, Exception ex, string message)
            {
                ExceptionsLogged++;                
            }

            public void LogMessage(TelemetryContext context, LogLevel level, string message, Dictionary<string, string> props)
            {
                MessagesLogged++;
            }

            public ActivityScope StartActivity(TelemetryContext context, string name)
            {
                ActivitiesStarted++;
                return ActivityScope.Empty;
            }
        }
    }
}
