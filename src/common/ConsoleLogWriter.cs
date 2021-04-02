using System;

namespace common
{
    public class ConsoleLogWriter : ILogWriter
    {

        #region Public Methods

        public void Write(ILogEntry logEntry)
        {
            AppendMessage($"{logEntry.Now:HH:mm:ss.ffff}|");
            AppendLogLevel(logEntry.LogLevel);
            AppendMessage("|");
            AppendMessage($"{logEntry.Type}|");
            AppendMessage($"{logEntry.Message}");
            AppendMessage(Environment.NewLine);
        }

        #endregion

        #region Helper Methods

        private void AppendLogLevel(LogLevel logLevel)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = GetLogLevelColor(logLevel);
            AppendMessage(logLevel.ToString().ToLower());
            Console.ForegroundColor = currentColor;
        }

        private void AppendMessage(string logSegment)
        {
            Console.Write(logSegment);
        }

        private ConsoleColor GetLogLevelColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Fatal:
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Info:
                    return ConsoleColor.Green;
                case LogLevel.Warn:
                    return ConsoleColor.Yellow;
                default:
                    return Console.ForegroundColor;
            }
        }

        #endregion

    }
}
