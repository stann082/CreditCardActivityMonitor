using System;

namespace Common
{
    public interface IApplicationLogger
    {

        void Initialize(string logDirectory);

        void LogError(Exception ex);
        void LogInfo(string inputText, params object[] replacements);
        void LogWarn(string inputText, params object[] replacements);

    }
}
