using GrokCLI.Utils;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GrokCLI.Helpers
{
    public static class Logger
    {
        /// <summary>
        /// Writes a message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes a formatted message to the console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using the format.</param>
        public static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Writes an error message to the console with "[ERROR]" prefix.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public static void Error(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        /// <summary>
        /// Writes an informational message to the console with "[INFO]" prefix.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        public static void Info(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        /// <summary>
        /// Writes a formatted informational message to the console with "[INFO]" prefix.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using the format.</param>
        public static void Info(string format, params object[] args)
        {
            Console.WriteLine($"[INFO] {format}", args);
        }

        /// <summary>
        /// Writes a raw message to the console as-is.
        /// </summary>
        /// <param name="message">The message to out.</param>
        public static void Output(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes a raw message to the console as-is.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using the format.</param>
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

        /// <summary>
        /// Writes a configurable message to the configured output (console and/or file) based on log level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The log level (e.g., "Info", "Error") to determine if the message should be logged.</param>
        public static void ConfigurableLog(string message, string level = "Info")
        {
            if (ShouldLog(level))
            {
                string formattedMessage = _logFormat.Replace("{Level}", level).Replace("{Message}", message);
                WriteToOutput(formattedMessage);
            }
        }

        /// <summary>
        /// Writes a configurable formatted message to the configured output based on log level.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="level">The log level (e.g., "Info", "Error") to determine if the message should be logged.</param>
        /// <param name="args">An array of objects to write using the format.</param>
        public static void ConfigurableLog(string format, string level, params object[] args)
        {
            if (ShouldLog(level))
            {
                string formattedMessage = _logFormat.Replace("{Level}", level).Replace("{Message}", string.Format(format, args));
                WriteToOutput(formattedMessage);
            }
        }

        /// <summary>
        /// Writes a configurable error message to the configured output with dynamic formatting.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public static void ConfigurableError(string message)
        {
            ConfigurableLog(message, "Error");
        }

        /// <summary>
        /// Writes a configurable formatted error message to the configured output.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using the format.</param>
        public static void ConfigurableError(string format, params object[] args)
        {
            ConfigurableLog(format, "Error", args);
        }

        /// <summary>
        /// Writes a configurable informational message to the configured output with dynamic formatting.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        public static void ConfigurableInfo(string message)
        {
            ConfigurableLog(message, "Info");
        }

        /// <summary>
        /// Writes a configurable formatted informational message to the configured output.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using the format.</param>
        public static void ConfigurableInfo(string format, params object[] args)
        {
            ConfigurableLog(format, "Info", args);
        }

        private static bool ShouldLog(string level)
        {
            string currentLevel = _logLevel.ToLowerInvariant();
            string checkLevel = level.ToLowerInvariant();

            // Simple log level comparison (you can expand this with a proper hierarchy: Trace < Debug < Info < Warning < Error < Critical)
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