using System;
using System.IO;

namespace TES3.Core
{
    public enum TES3LogLevel
    {
        Info,
        Warn,
        Error
    }

    public static class TES3Logger
    {

        public static void Log(TES3LogLevel level, string message)
        {
            var target = level.GetWriter();
            if (target == null)
            {
                return;
            }

            target.WriteLine(message);
        }

        public static void Log(TES3LogLevel level, string formatString, params object[] args)
        {
            var target = level.GetWriter();
            if (target == null)
            {
                return;
            }

            target.WriteLine($"{level.GetLogIndicator()}: {string.Format(formatString, args)}");
        }
    }

    static class TES3LogLevelExtensions
    {
        public static TextWriter GetWriter(this TES3LogLevel level)
        {
            switch (level)
            {
                case TES3LogLevel.Info:
                    return GlobalConfig.InfoWriter;
                case TES3LogLevel.Warn:
                    return GlobalConfig.WarnWriter;
                case TES3LogLevel.Error:
                    return GlobalConfig.ErrorWriter;
            }

            throw new InvalidOperationException($"Unrecognized log level: {level}");
        }

        public static string GetLogIndicator(this TES3LogLevel level)
        {
            switch (level)
            {
                case TES3LogLevel.Info:
                    return "INFO";
                case TES3LogLevel.Warn:
                    return "WARN";
                case TES3LogLevel.Error:
                    return "ERROR";
            }

            throw new InvalidOperationException($"Unrecognized log level: {level}");
        }
    }
}
