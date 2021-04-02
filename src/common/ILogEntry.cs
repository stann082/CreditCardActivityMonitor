using System;

namespace common
{
    public interface ILogEntry
    {

        string FullMessage { get; }
        LogLevel LogLevel { get; }
        string Message { get; }
        DateTime Now { get; }
        string Type { get; }

    }
}
