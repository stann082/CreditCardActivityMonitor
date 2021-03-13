namespace common
{
    public class DirectoryProvider : IDirectoryProvider
    {

        public static IDirectoryProvider Singleton = new DirectoryProvider();

        #region Constructors

        private DirectoryProvider()
        {
        }

        #endregion

        #region Properties

        public string ArchiveRootDir => Constants.ARCHIVE_ROOT_DIR;
        public string DownloadsDir => Constants.DOWNLOADS_DIR;

        #endregion

    }
}
