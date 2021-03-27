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

        public string ArchiveRootDir => ApplicationEnvironment.Singleton.ArchiveRootDir;
        public string DownloadsDir => ApplicationEnvironment.Singleton.DownloadsDir;

        #endregion

    }
}
