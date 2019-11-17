namespace common
{
    public interface IDirectoryProvider
    {

        string ArchiveRootDir { get; }
        string DownloadsDir { get; }

    }
}