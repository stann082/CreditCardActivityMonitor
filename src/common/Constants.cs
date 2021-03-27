using System;

namespace common
{

    public enum ActivityType
    {
        Purchase = 0,
        Payment = 1,
    }

    [Flags]
    public enum LogLevel
    {
        None = 0x00000000,
        Debug = 0x00000001,
        Error = 0x00000002,
        Fatal = 0x00000004,
        Info = 0x00000008,
        Warn = 0x00000010,

        All = Debug | Error | Fatal | Info | Warn,
    }

}
