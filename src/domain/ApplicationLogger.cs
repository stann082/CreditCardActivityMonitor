using Microsoft.Extensions.Logging;

namespace Domain
{
    public class ApplicationLogger
    {

        private static ILoggerFactory _Factory = null;

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                }

                return _Factory;
            }
            set { _Factory = value; }
        }

        public static ILogger<T> CreateLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }

    }
}
