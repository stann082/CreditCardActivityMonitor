using System;
using System.IO;
using System.Text;

namespace Common
{
    public class ApplicationLogger
    {

        public static ApplicationLogger Singleton = new ApplicationLogger();

        #region Constructors

        private ApplicationLogger()
        {
            EnabledConsoleLevels = LogLevel.All;
            EnabledFileLevels = LogLevel.All;

            LogDirectory = "logs";
        }

        #endregion

        #region Properties

        public LogLevel EnabledConsoleLevels { get; set; }
        public LogLevel EnabledFileLevels { get; set; }

        private string LogDirectory { get; set; }

        #endregion

        #region Public Methods

        public void Initialize(string logDirectory)
        {
            if (!Path.IsPathRooted(logDirectory))
            {
                logDirectory = Path.Combine(Environment.CurrentDirectory, logDirectory);
            }

            LogDirectory = logDirectory;

            LogInfo("Log Directory Initialized: [{0}]", logDirectory);
        }

        public void LogDebug(string inputText, params object[] replacements)
        {
            LogMessage(LogLevel.Debug, "DEBUG", inputText, replacements);
        }

        public void LogError(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(new string('*', 100));
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.Append(new string('*', 100));
            LogMessage(LogLevel.Error, "ERROR", sb.ToString());
        }

        public void LogError(string inputText, params object[] replacements)
        {
            LogMessage(LogLevel.Error, "ERROR", inputText, replacements);
        }

        public void LogFatal(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(new string('*', 100));
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.Append(new string('*', 100));
            LogMessage(LogLevel.Fatal, "FATAL", sb.ToString());
        }

        public void LogFatal(string inputText, params object[] replacements)
        {
            LogMessage(LogLevel.Fatal, "FATAL", inputText, replacements);
        }

        public void LogInfo(string inputText, params object[] replacements)
        {
            LogMessage(LogLevel.Info, "INFO", inputText, replacements);
        }

        public void LogWarn(string inputText, params object[] replacements)
        {
            LogMessage(LogLevel.Warn, "WARN", inputText, replacements);
        }

        #endregion

        #region Helper Methods

        private bool IsConsoleLogLevelEnabled(LogLevel logLevel)
        {
            return IsLogLevelEnabled(EnabledConsoleLevels, logLevel);
        }

        private bool IsFileLogLevelEnabled(LogLevel logLevel)
        {
            return IsLogLevelEnabled(EnabledFileLevels, logLevel);
        }

        private bool IsLogLevelEnabled(LogLevel enabledLogLevels, LogLevel logLevel)
        {
            return (enabledLogLevels & logLevel) != 0;
        }

        private void LogMessage(LogLevel logLevel, string logLevelText, string inputText, params object[] replacements)
        {
            // guard clause - do nothing
            bool isFileLogLevelEnabled = IsFileLogLevelEnabled(logLevel);
            bool isConsoleLogLevelEnabled = IsConsoleLogLevelEnabled(logLevel);
            if (!isFileLogLevelEnabled && !isConsoleLogLevelEnabled)
            {
                return;
            }

            bool shouldWriteToStandardError = logLevel == LogLevel.Error || logLevel == LogLevel.Fatal;

            DateTime now = DateTime.Now;
            try
            {
                StringBuilder logText = new StringBuilder();
                logText.Append(now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                logText.Append("|");
                logText.Append(logLevelText);
                logText.Append("|");
                if (replacements.Length == 0)
                {
                    logText.Append(inputText);
                }
                else
                {
                    logText.AppendFormat(inputText, replacements);
                }
                logText.Append(Environment.NewLine);

                string logTextValue = logText.ToString();
                WriteToConsoleIfApplicable(logTextValue, isConsoleLogLevelEnabled, shouldWriteToStandardError);
                WriteToFileIfApplicable(logTextValue, isFileLogLevelEnabled, now);
            }
            catch
            {
                // squash errors
            }
        }

        private void WriteToConsoleIfApplicable(string logTextValue, bool isConsoleLogLevelEnabled, bool shouldWriteToStandardError)
        {
            // guard clause - not enabled
            if (!isConsoleLogLevelEnabled)
            {
                return;
            }

            if (shouldWriteToStandardError)
            {
                Console.Error.WriteLine(logTextValue);
            }
            else
            {
                Console.WriteLine(logTextValue);
            }
        }

        private void WriteToFileIfApplicable(string logTextValue, bool isFileLogLevelEnabled, DateTime now)
        {
            // guard clause - not enabled
            if (!isFileLogLevelEnabled)
            {
                return;
            }

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
