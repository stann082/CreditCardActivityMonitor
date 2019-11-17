using common;
using System;

namespace service_test.mock
{
    public class MockApplicationLogger : IApplicationLogger
    {

        #region Public Methods

        public void Initialize(string logDirectory)
        {
            // do nothing
        }

        public void LogError(Exception ex)
        {
            // do nothing
        }

        public void LogError(string inputText, params object[] replacements)
        {
            // do nothing
        }

        public void LogInfo(string inputText, params object[] replacements)
        {
            // do nothing
        }

        public void LogWarn(string inputText, params object[] replacements)
        {
            // do nothing
        }

        #endregion

    }
}
