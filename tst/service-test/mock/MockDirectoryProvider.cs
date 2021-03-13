using common;

namespace service_test.mock
{
    public class MockDirectoryProvider : IDirectoryProvider
    {

        public string ArchiveRootDir => "archived-credit-card-statements";
        public string DownloadsDir => "Downloads";

    }
}
