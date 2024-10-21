using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NetworkCore
{
    public enum LogLevel
    { 
        Debug,
        Info,
        Warning,
        Error,
    }


    public class Logger : MonoBehaviour
    {
        static string GetTimeStamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        static void Log(LogLevel level, string message)
        {
            string msg = $"[{level.ToString().ToUpper()}][{GetTimeStamp()}] {message}";
            LogQueue.Instance.Push(msg);
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
