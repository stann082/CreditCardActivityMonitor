using System;

namespace common
{
    public class ApplicationEnvironment
    {

        public static ApplicationEnvironment Singleton = new ApplicationEnvironment();

        #region Constants

        private const string ARCHIVE_ROOT_DIR = @"C:\Temp\archived-credit-card-statements";
        private const string ARCHIVE_ROOT_DIR_ENV_VAR = "ARCHIVE_ROOT_DIR";
        private const string DOWNLOADS_DIR = @"C:\Temp\user-downloads";
        private const string DOWNLOADS_DIR_ENV_VAR = "DOWNLOADS_DIR";

        #endregion

        #region Constructors

        private ApplicationEnvironment()
        {
            ArchiveRootDir = ARCHIVE_ROOT_DIR;
            DownloadsDir = DOWNLOADS_DIR;
        }

        #endregion

        #region Properties

        public string ArchiveRootDir { get; private set; }
        public string DownloadsDir { get; private set; }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            string archiveRootDirValue = Environment.GetEnvironmentVariable(ARCHIVE_ROOT_DIR_ENV_VAR);
            if (archiveRootDirValue != null)
            {
                ArchiveRootDir = archiveRootDirValue;
                ApplicationLogger.Singleton.LogWarn($"Using overriden environment variable {ARCHIVE_ROOT_DIR_ENV_VAR} with value [{archiveRootDirValue}].");
            }

            string downloadsDirValue = Environment.GetEnvironmentVariable(DOWNLOADS_DIR_ENV_VAR);
            if (downloadsDirValue != null)
            {
                DownloadsDir = downloadsDirValue;
                ApplicationLogger.Singleton.LogWarn($"Using overriden environment variable {DOWNLOADS_DIR_ENV_VAR} with value [{downloadsDirValue}].");
            }
        }

        #endregion

    }
}
