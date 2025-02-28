using GrokCLI.Utils;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GrokCLI.Helpers
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[ERROR]");
            Console.ResetColor();
            Console.WriteLine($" {message}");
        }
        
        public static void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[DEBU]");
            Console.ResetColor();
            Console.WriteLine($" {message}");
        }

        public static void Debug(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[DEBU]");
            Console.ResetColor();
            Console.WriteLine($" {string.Format(format, args)}");
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[INFO]");
            Console.ResetColor();
            Console.WriteLine($" {message}");
        }

        public static void Info(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[INFO]");
            Console.ResetColor();
            Console.WriteLine($" {string.Format(format, args)}");
        }

        public static void Output(string message)
        {
            Console.WriteLine(message);
        }

        public static void Output(string format, params object[] args)
        {
            Console.WriteLine($"{format}", args);
        }

        #region Configuration-Based Logging (Optional Integration with LoggingHelper)

        private static readonly string _logLevel = LoggingHelper.GetLogLevel();
        private static readonly string _logPath = GetLogPath();
        private static readonly string _logFormat = GetLogFormat();

        private static string GetLogPath()
        {
            var settings = LoggingHelper.GetLoggingSettings();
            return settings.TryGetValue("Path", out string? path) ? path : null;
        }

        private static string GetLogFormat()
        {
            var settings = LoggingHelper.GetLoggingSettings();
            return settings.TryGetValue("Format", out string? format) ? format : "[{Level}] {Message}";
        }

        public static void ConfigurableLog(string message, string level = "Info")
        {
            if (ShouldLog(level))
            {
                string formattedMessage = _logFormat.Replace("{Level}", level).Replace("{Message}", message);
                WriteToOutput(formattedMessage);
            }
        }

        public static void ConfigurableLog(string format, string level, params object[] args)
        {
            if (ShouldLog(level))
            {
                string formattedMessage = _logFormat.Replace("{Level}", level).Replace("{Message}", string.Format(format, args));
                WriteToOutput(formattedMessage);
            }
        }

        public static void ConfigurableError(string message)
        {
            ConfigurableLog(message, "Error");
        }

        public static void ConfigurableError(string format, params object[] args)
        {
            ConfigurableLog(format, "Error", args);
        }

        public static void ConfigurableInfo(string message)
        {
            ConfigurableLog(message, "Info");
        }

        public static void ConfigurableInfo(string format, params object[] args)
        {
            ConfigurableLog(format, "Info", args);
        }

        private static bool ShouldLog(string level)
        {
            string currentLevel = _logLevel.ToLowerInvariant();
            string checkLevel = level.ToLowerInvariant();

            // Simple log level comparison (you can expand this with a proper 
            // hierarchy: Trace < Debug < Info < Warning < Error < Critical)
            if (currentLevel == "debug" || currentLevel == "trace")
                return true;
            if (currentLevel == "info" && (checkLevel == "info" || checkLevel == "error"))
                return true;
            if (currentLevel == "error" && checkLevel == "error")
                return true;

            return false;
        }

        private static void WriteToOutput(string message)
        {
            // Output to console (always)
            Console.WriteLine(message);

            // Output to file if configured
            if (!string.IsNullOrEmpty(_logPath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_logPath) ?? "./");
                    File.AppendAllText(_logPath, message + Environment.NewLine);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"[ERROR] Failed to write to log file: {ex.Message}");
                }
            }
        }

        #endregion
    }
}