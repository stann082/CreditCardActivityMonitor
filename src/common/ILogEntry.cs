using System;

namespace common
{
    public interface ILogEntry
    {

        string FullMessage { get; }
        LogLevels LogLevel { get; }
        string Message { get; }
        DateTime Now { get; }
        string Type { get; }

    }
}
