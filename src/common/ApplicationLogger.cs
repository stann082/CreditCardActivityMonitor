using System;
using System.IO;
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
            EnabledFileLogging = LogLevel.All;

            LogDirectory = "logs";
        }

        #endregion

        #region Properties

        private LogLevel EnableConsoleLogging { get; set; }
        private LogLevel EnabledFileLogging { get; set; }
        private string LogDirectory { get; set; }
        private Type Type { get; set; }

        #endregion

        #region Public Methods

        public void DisableLogging()
        {
            EnableConsoleLogging = LogLevel.None;
            EnabledFileLogging = LogLevel.None;
        }

        public void Initialize(string logDirectory)
        {
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
            return IsLogLevelEnabled(EnabledFileLogging, logLevel);
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

            DateTime now = DateTime.Now;
            try
            {
                StringBuilder levelLogLine = new StringBuilder();
                levelLogLine.Append(now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                levelLogLine.Append("  ");
                levelLogLine.Append(logLevel.ToString().ToLower());
                levelLogLine.Append(": ");
                levelLogLine.Append($"{Type}");
                levelLogLine.Append(Environment.NewLine);
                StringBuilder messageLogLine = new StringBuilder();
                messageLogLine.Append(message);
                messageLogLine.Append(Environment.NewLine);

                string logTextValue = levelLogLine.ToString() + messageLogLine.ToString();
                if (isConsoleLogLevelEnabled)
                {
                    bool shouldWriteToStandardError = logLevel == LogLevel.Error || logLevel == LogLevel.Fatal;
                    WriteToConsole(logTextValue, shouldWriteToStandardError);
                }

                if (isFileLogLevelEnabled)
                {
                    WriteToFile(logTextValue, now);
                }
            }
            catch
            {
                // squash errors
            }
        }

        private void WriteToConsole(string logTextValue, bool shouldWriteToStandardError)
        {
            if (shouldWriteToStandardError)
            {
                Console.Error.WriteLine(logTextValue);
            }
            else
            {
                Console.WriteLine(logTextValue);
            }

            Console.ResetColor();
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
