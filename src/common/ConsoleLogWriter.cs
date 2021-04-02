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

        private void AppendLogLevel(LogLevels logLevel)
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

        private ConsoleColor GetLogLevelColor(LogLevels logLevel)
        {
            switch (logLevel)
            {
                case LogLevels.Fatal:
                case LogLevels.Error:
                    return ConsoleColor.Red;
                case LogLevels.Info:
                    return ConsoleColor.Green;
                case LogLevels.Warn:
                    return ConsoleColor.Yellow;
                default:
                    return Console.ForegroundColor;
            }
        }

        #endregion

    }
}
