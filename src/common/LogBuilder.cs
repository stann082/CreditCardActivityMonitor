using System;
using System.Text;

namespace common
{
    public class LogBuilder : ILogBuilder
    {

        #region Constructors

        public LogBuilder(string message, LogLevels logLevel, Type type, DateTime now)
        {
            Message = message;
            LogLevel = logLevel;
            Type = type.ToString();
            Now = now;
        }

        #endregion

        #region Properties

        public string FullMessage { get; private set; }
        public LogLevels LogLevel { get; private set; }
        public string Message { get; private set; }
        public DateTime Now { get; private set; }
        public string Type { get; private set; }

        #endregion

        #region Public Methods

        public void BuildLogEntry()
        {
            StringBuilder levelLogLine = new StringBuilder();
            levelLogLine.Append($"{Now:HH:mm:ss.ffff}|");
            levelLogLine.Append($"{LogLevel.ToString().ToLower()}|");
            levelLogLine.Append($"{Type}|");
            levelLogLine.Append($"{Message}");
            levelLogLine.Append(Environment.NewLine);

            FullMessage = levelLogLine.ToString();
        }

        #endregion

    }
}
