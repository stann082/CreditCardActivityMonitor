using System;
using System.IO;

namespace common
{
    public class FileLogWriter : ILogWriter
    {

        #region Constructors

        public FileLogWriter(string logDirectory, DateTime now)
        {
            LogDirectory = logDirectory;
            Now = now;
        }

        #endregion

        private string LogDirectory { get; set; }
        private DateTime Now { get; set; }

        #region Public Methods

        public void Write(ILogEntry logEntry)
        {
            string fileName = Now.ToString("yyyy-MM-dd") + ".log";
            string filePath = Path.Combine(LogDirectory, fileName);

            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            File.AppendAllText(filePath, logEntry.FullMessage);
        }

        #endregion

    }
}
