using Common;
using Service;
using System;
using System.IO;

namespace Interface
{
    public class ConsoleApplication
    {

        #region Constants

        private const string ARCHIVE_ROOT_DIR = @"C:\Temp\archived-credit-card-statements";

        #endregion

        #region Constructors

        public ConsoleApplication(ICreditCardService creditCardService)
        {
            CreditCardService = creditCardService;
        }

        #endregion

        #region Properties

        private string ArchiveDirectory { get; set; }
        private ICreditCardService CreditCardService { get; set; }
        private ApplicationLogger Logger { get { return ApplicationLogger.Singleton; } }

        #endregion

        #region Application Starting Point

        public void Run()
        {
            string downloadsDir = Environment.ExpandEnvironmentVariables(Constants.USER_DOWNLOADS_DIR);
            string[] postedActivityFiles = Directory.GetFiles(downloadsDir, "Discover-*.csv");
            if (postedActivityFiles.Length == 0)
            {
                Logger.LogWarn("No pending or posted activity files were found...Exiting the app");
                return;
            }

            CreditCardActivityProcessor processor = new CreditCardActivityProcessor(CreditCardService);
            processor.Process(postedActivityFiles);

            InitializeArchiveDirectory();
            foreach (string activityFile in postedActivityFiles)
            {
                Logger.LogInfo($"Moving {activityFile}");
                ArchiveFile(activityFile);
            }
        }

        #endregion

        #region Helper Methods

        private void ArchiveFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string fileName = Path.GetFileName(filePath);
            File.Move(filePath, Path.Combine(ArchiveDirectory, fileName));
        }

        private void InitializeArchiveDirectory()
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            ArchiveDirectory = Path.Combine(ARCHIVE_ROOT_DIR, timeStamp);
            if (Directory.Exists(ArchiveDirectory))
            {
                return;
            }

            Directory.CreateDirectory(ArchiveDirectory);
        }

        #endregion

    }
}
