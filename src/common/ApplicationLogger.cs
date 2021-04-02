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
            EnableConsoleLogging = LogLevel.All;
            EnableFileLogging = LogLevel.All;
            UseUtcTimestamp = false;

            LogDirectory = "logs";
        }

        #endregion

        #region Properties

        private LogLevel EnableConsoleLogging { get; set; }
        private LogLevel EnableFileLogging { get; set; }
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
            LogMessage(sb.ToString(), LogLevel.Error);
        }

        public void LogError(string message)
        {
            LogMessage(message, LogLevel.Error);
        }

        public void LogInfo(string message)
        {
            LogMessage(message, LogLevel.Info);
        }

        public void LogWarn(string message)
        {
            LogMessage(message, LogLevel.Warn);
        }

        public void SetType(Type type)
        {
            Type = type;
        }

        #endregion

        #region Helper Methods

        private bool IsConsoleLoggingEnabled(LogLevel logLevel)
        {
            return IsLogLevelEnabled(EnableConsoleLogging, logLevel);
        }

        private bool IsFileLoggingEnabled(LogLevel logLevel)
        {
            return IsLogLevelEnabled(EnableFileLogging, logLevel);
        }

        private bool IsLogLevelEnabled(LogLevel enabledLogLevels, LogLevel logLevel)
        {
            return (enabledLogLevels & logLevel) != 0;
        }

        private void LogMessage(string message, LogLevel logLevel)
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
            EnableConsoleLogging = logArgs.Any(a => a == "--no-console-log") ? LogLevel.None : EnableConsoleLogging;
            EnableFileLogging = logArgs.Any(a => a == "--no-file-log") ? LogLevel.None : EnableConsoleLogging;
            UseUtcTimestamp = logArgs.Any(a => a == "--use-utc");
        }

        private void WriteToFile(string logTextValue, DateTime now)
        {
            string fileName = now.ToString("yyyy-MM-dd") + ".log";
            string filePath = Path.Combine(LogDirectory, fileName);

            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            File.AppendAllText(filePath, logTextValue);
        }

        #endregion

    }
}
