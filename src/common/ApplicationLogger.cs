using System;
using System.IO;
using System.Linq;
using System.Text;

namespace common
{
    public class ApplicationLogger : IApplicationLogger
    {

        private static ApplicationLogger instance = null;
        private static readonly object padlock = new object();

        public static IApplicationLogger Singleton
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ApplicationLogger();
                    }

                    return instance;
                }
            }
        }

        #region Constructors

        private ApplicationLogger()
        {
            EnableConsoleLogging = LogLevels.All;
            EnableFileLogging = LogLevels.All;
            UseUtcTimestamp = false;

            LogDirectory = "logs";
        }

        #endregion

        #region Properties

        private LogLevels EnableConsoleLogging { get; set; }
        private LogLevels EnableFileLogging { get; set; }
        private string LogDirectory { get; set; }
        private Type Type { get; set; }
        private bool UseUtcTimestamp { get; set; }

        #endregion

        #region Public Methods

        public void Initialize(string logDirectory, string[] logArgs)
        {
            SetLoggingLevel(logArgs);

            if (!Path.IsPathRooted(logDirectory))
            {
                logDirectory = Path.Combine(Environment.CurrentDirectory, logDirectory);
            }

            LogDirectory = logDirectory;

            LogInfo($"Log Directory Initialized: [{logDirectory}]");
        }

        public void LogError(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(new string('*', 100));
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.Append(new string('*', 100));
            LogMessage(sb.ToString(), LogLevels.Error);
        }

        public void LogError(string message)
        {
            LogMessage(message, LogLevels.Error);
        }

        public void LogInfo(string message)
        {
            LogMessage(message, LogLevels.Info);
        }

        public void LogWarn(string message)
        {
            LogMessage(message, LogLevels.Warn);
        }

        public void SetType(Type type)
        {
            Type = type;
        }

        #endregion

        #region Helper Methods

        private bool IsConsoleLoggingEnabled(LogLevels logLevel)
        {
            return IsLogLevelEnabled(EnableConsoleLogging, logLevel);
        }

        private bool IsFileLoggingEnabled(LogLevels logLevel)
        {
            return IsLogLevelEnabled(EnableFileLogging, logLevel);
        }

        private bool IsLogLevelEnabled(LogLevels enabledLogLevels, LogLevels logLevel)
        {
            return (enabledLogLevels & logLevel) != 0;
        }

        private void LogMessage(string message, LogLevels logLevel)
        {
            // guard clause - do nothing
            bool isFileLogLevelEnabled = IsFileLoggingEnabled(logLevel);
            bool isConsoleLogLevelEnabled = IsConsoleLoggingEnabled(logLevel);
            if (!isFileLogLevelEnabled && !isConsoleLogLevelEnabled)
            {
                return;
            }

            DateTime now = !UseUtcTimestamp ? DateTime.Now : DateTime.UtcNow;
            try
            {
                ILogBuilder builder = new LogBuilder(message, logLevel, Type, now);
                builder.BuildLogEntry();

                ILogWriter writer;

                if (isConsoleLogLevelEnabled)
                {
                    writer = new ConsoleLogWriter();
                    writer.Write(builder);
                }

                if (isFileLogLevelEnabled)
                {
                    writer = new FileLogWriter(LogDirectory, now);
                    writer.Write(builder);
                }
            }
            catch
            {
                // squash errors
            }
        }

        private void SetLoggingLevel(string[] logArgs)
        {
            EnableConsoleLogging = logArgs.Any(a => a == "--no-console-log") ? LogLevels.None : EnableConsoleLogging;
            EnableFileLogging = logArgs.Any(a => a == "--no-file-log") ? LogLevels.None : EnableConsoleLogging;
            UseUtcTimestamp = logArgs.Any(a => a == "--use-utc");
        }

        #endregion

    }
}
