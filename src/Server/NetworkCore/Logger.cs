using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }


    public class Logger
    {
        static string GetTimeStamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        static void Log(LogLevel level, string message)
        {
            Console.ForegroundColor = GetLogColor(level);
            Console.WriteLine($"[{level.ToString().ToUpper()}][{GetTimeStamp()}] {message}");
            Console.ResetColor();
        }

        static ConsoleColor GetLogColor(LogLevel level)
        {
            return level switch
            {
                LogLevel.Info => ConsoleColor.Cyan,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Debug => ConsoleColor.Gray,
                _ => ConsoleColor.White
            };
        }

        public static void DebugLog(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public static void InfoLog(string message)
        {
            Log(LogLevel.Info, message);
        }

        public static void ErrorLog(string message)
        {
            Log(LogLevel.Error, message);
        }

        public static void WarningLog(string message)
        {
            Log(LogLevel.Warning, message);
        }

    }
}
