using System;
using System.IO;
namespace proper_ws.Utils
{
    public class LoggerHelper
    {
        public static string GetLogFilePath()
        {
            string logDirectory = Path.Combine("C:\\temp\\logs", DateTime.Now.ToString("yyyy-MM-dd"));
            Directory.CreateDirectory(logDirectory);
            return Path.Combine(logDirectory, "WebServiceLog.txt");
        }

        public static void LogMessage(string message)
        {
            string fullPath = GetLogFilePath();
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
            File.AppendAllText(fullPath, logEntry + Environment.NewLine);
        }
        
        public static void CleanOldLogs(int daysToKeep = 30)
        {
            string basePath = "C:\\temp\\logs";
            if (!Directory.Exists(basePath)) return;

            var directories = Directory.GetDirectories(basePath);
            foreach (var dir in directories)
            {
                if (DateTime.TryParse(Path.GetFileName(dir), out DateTime dirDate))
                {
                    if ((DateTime.Now - dirDate).TotalDays > daysToKeep)
                    {
                        Directory.Delete(dir, true);
                    }
                }
            }
        }
    }
}
