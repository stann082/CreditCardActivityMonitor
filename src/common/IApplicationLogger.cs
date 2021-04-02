using System;

namespace common
{
    public interface IApplicationLogger
    {

        void Initialize(string logDirectory, string[] logArgs);

        void LogError(Exception ex);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarn(string message);

        void SetType(Type type);

    }
}
