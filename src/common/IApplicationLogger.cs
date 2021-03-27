using System;

namespace common
{
    public interface IApplicationLogger
    {

        // TODO: find a more elegant way to disable logging in tests
        void DisableLogging();

        void Initialize(string logDirectory);

        void LogError(Exception ex);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarn(string message);

        void SetType(Type type);

    }
}
