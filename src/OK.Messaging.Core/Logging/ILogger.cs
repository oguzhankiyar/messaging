using System;

namespace OK.Messaging.Core.Logging
{
    public interface ILogger
    {
        void Debug(string source, string message);

        void DebugData(string source, string message, object data);

        void DebugFormat(string source, string message, params object[] args);

        void Error(string source, string message, Exception exception);

        void Error(string source, string message);

        void Error(string source, Exception exception);

        void ErrorFormat(string source, string message, params object[] args);

        void Fatal(string source, string message, Exception exception);

        void Fatal(string source, string message);

        void Fatal(string source, Exception exception);

        void FatalFormat(string source, string message, params object[] args);

        void Info(string source, string message);

        void InfoData(string source, string message, object data);

        void InfoFormat(string source, string message, params object[] args);

        void Warning(string source, string message, Exception exception);

        void Warning(string source, string message);

        void Warning(string source, Exception exception);

        void WarningData(string source, string message, object data);

        void WarningFormat(string source, string message, params object[] args);

        void SetGlobalProperty(string key, object value);

        void SetThreadProperty(string key, object value);
    }
}