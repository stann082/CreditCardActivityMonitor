namespace common
{
    public abstract class AbstractBase
    {

        #region Shared Methods

        protected IApplicationLogger GetApplicationLogger<T>()
        {
            IApplicationLogger logger = ApplicationLogger.Singleton;
            logger.SetType(typeof(T));
            return logger;
        }

        #endregion

    }
}
