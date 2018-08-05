using NLog;
using NLog.Fluent;
using System;
using IMessagingLogger = OK.Messaging.Core.Logging.ILogger;

namespace OK.Messaging.Engine.Logging
{
    public class NLoggerImpl : IMessagingLogger
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        static NLoggerImpl()
        {
            LogManager.LoadConfiguration("nlog.config");
        }

        public void Debug(string source, string message)
        {
            _logger.Debug()
                   .Message(message)
                   .Property("Source", source)
                   .Write();
        }

        public void DebugData(string source, string message, object data)
        {
            _logger.Debug()
                   .Message(message)
                   .Property("Source", source)
                   .Property("Data", data)
                   .Write();
        }

        public void DebugFormat(string source, string message, params object[] args)
        {
            _logger.Debug()
                   .Message(message, args)
                   .Property("Source", source)
                   .Write();
        }

        public void Error(string source, string message, Exception exception)
        {
            _logger.Error()
                   .Message(message)
                   .Exception(exception)
                   .Property("Source", source)
                   .Write();
        }

        public void Error(string source, string message)
        {
            _logger.Error()
                   .Message(message)
                   .Property("Source", source)
                   .Write();
        }

        public void Error(string source, Exception exception)
        {
            _logger.Error()
                   .Message(exception.Message)
                   .Exception(exception)
                   .Property("Source", source)
                   .Write();
        }

        public void ErrorFormat(string source, string message, params object[] args)
        {
            _logger.Error()
                   .Message(message, args)
                   .Property("Source", source)
                   .Write();
        }

        public void Fatal(string source, string message, Exception exception)
        {
            _logger.Fatal()
                   .Message(message)
                   .Exception(exception)
                   .Property("Source", source)
                   .Write();
        }

        public void Fatal(string source, string message)
        {
            _logger.Fatal()
                   .Message(message)
                   .Property("Source", source)
                   .Write();
        }

        public void Fatal(string source, Exception exception)
        {
            _logger.Fatal()
                   .Message(exception.Message)
                   .Exception(exception)
                   .Property("Source", source)
                   .Write();
        }

        public void FatalFormat(string source, string message, params object[] args)
        {
            _logger.Fatal()
                   .Message(message, args)
                   .Property("Source", source)
                   .Write();
        }

        public void Info(string source, string message)
        {
            _logger.Info()
                   .Message(message)
                   .Property("Source", source)
                   .Write();
        }

        public void InfoData(string source, string message, object data)
        {
            _logger.Info()
                   .Message(message)
                   .Property("Source", source)
                   .Property("Data", data)
                   .Write();
        }

        public void InfoFormat(string source, string message, params object[] args)
        {
            _logger.Info()
                   .Message(message, args)
                   .Property("Source", source)
                   .Write();
        }

        public void Warning(string source, string message, Exception exception)
        {
            _logger.Info()
                   .Message(message)
                   .Exception(exception)
                   .Property("Source", source)
                   .Write();
        }

        public void Warning(string source, string message)
        {
            _logger.Info()
                   .Message(message)
                   .Property("Source", source)
                   .Write();
        }

        public void Warning(string source, Exception exception)
        {
            _logger.Warn()
                   .Message(exception.Message)
                   .Exception(exception)
                   .Property("Source", source)
                   .Write();
        }

        public void WarningData(string source, string message, object data)
        {
            _logger.Warn()
                   .Message(message)
                   .Property("Source", source)
                   .Property("Data", data)
                   .Write();
        }

        public void WarningFormat(string source, string message, params object[] args)
        {
            _logger.Warn()
                   .Message(message, args)
                   .Property("Source", source)
                   .Write();
        }

        public void SetGlobalProperty(string key, object value)
        {
            GlobalDiagnosticsContext.Set(key, value);
        }

        public void SetThreadProperty(string key, object value)
        {
            MappedDiagnosticsContext.Set(key, value);
        }
    }
}