namespace GrokCLI.Helpers;

using GrokCLI.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

/*
| Name          | Value | Description                                |
|---------------|-------|--------------------------------------------|
| Black         | 0     | Black                                      |
| DarkBlue      | 1     | Blue                                       |
| DarkGreen     | 2     | Dark green                                 |
| DarkCyan      | 3     | Dark cyan (dark blue-green)                |
| DarkRed       | 4     | The color dark red                         |
| DarkMagenta   | 5     | The color dark magenta (dark purplish-red) |
| DarkYellow    | 6     | The color dark yellow (ochre)              |
| Gray          | 7     | The color gray                             |
| DarkGray      | 8     | The color dark gray                        |
| Blue          | 9     | The color blue                             |
| Green         | 10    | The color green                            |
| Cyan          | 11    | The color cyan (blue-green)                |
| Red           | 12    | The color red                              |
| Magenta       | 13    | The color magenta (purplish-red)           |
| Yellow        | 14    | The color yellow                           |
| White         | 15    | The color white                            |
*/


public static class Logger
{
    private static ConsoleColor _infoColor = ConsoleColor.White;
    private static ConsoleColor _errorColor = ConsoleColor.DarkRed;
    private static ConsoleColor _debugColor = ConsoleColor.Blue;
    private static ConsoleColor _messageColor = ConsoleColor.DarkCyan;
    private static ConsoleColor _grokColor = ConsoleColor.DarkGray;
    private static string _theme = "light"; // Default theme

    static Logger()
    {
        LoadThemeFromConfig();
    }

    private static void LoadThemeFromConfig()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        _theme = configuration["Logging:Theme"] ?? "light";
        SetThemeColors(_theme);
    }

    public static void SetTheme(string theme)
    {
        _theme = theme.ToLowerInvariant();
        SetThemeColors(_theme);
    }

    private static void SetThemeColors(string theme)
    {
        switch (theme)
        {
            case "light":
                _infoColor = ConsoleColor.White;
                _errorColor = ConsoleColor.DarkRed;
                _debugColor = ConsoleColor.Blue;
                _messageColor = ConsoleColor.DarkCyan;
                _grokColor = ConsoleColor.DarkGray;
                break;
            case "dark":
                _infoColor = ConsoleColor.Gray;
                _errorColor = ConsoleColor.Red;
                _debugColor = ConsoleColor.Cyan;
                _messageColor = ConsoleColor.DarkCyan;
                _grokColor = ConsoleColor.DarkGray;
                break;
            default:
                _infoColor = ConsoleColor.White;
                _errorColor = ConsoleColor.DarkRed;
                _debugColor = ConsoleColor.Blue;
                _messageColor = ConsoleColor.DarkCyan;
                _grokColor = ConsoleColor.DarkGray;
                break;
        }
    }

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
        Console.ForegroundColor = _errorColor;
        Console.Write("[ERROR]");
        Console.ResetColor();
        Console.ForegroundColor = _messageColor;
        Console.WriteLine($" {message}");
        Console.ResetColor();
    }
    
    public static void Debug(string message)
    {
        Console.ForegroundColor = _debugColor;
        Console.Write("[DEBU]");
        Console.ResetColor();
        Console.ForegroundColor = _messageColor;
        Console.WriteLine($" {message}");
        Console.ResetColor();
    }

    public static void Debug(string format, params object[] args)
    {
        Console.ForegroundColor = _debugColor;
        Console.Write("[DEBU]");
        Console.ResetColor();
        Console.ForegroundColor = _messageColor;
        Console.WriteLine($" {string.Format(format, args)}");
        Console.ResetColor();
    }

    public static void Info(string message)
    {
        Console.ForegroundColor = _infoColor;
        Console.Write("[INFO]");
        Console.ResetColor();
        Console.ForegroundColor = _messageColor;
        Console.WriteLine($" {message}");
        Console.ResetColor();
    }

    public static void Info(string format, params object[] args)
    {
        Console.ForegroundColor = _infoColor;
        Console.Write("[INFO]");
        Console.ResetColor();
        Console.ForegroundColor = _messageColor;
        Console.WriteLine($" {string.Format(format, args)}");
        Console.ResetColor();
    }

    public static void Output(string message)
    {
        Console.ForegroundColor = _grokColor;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Output(string format, params object[] args)
    {
        Console.ForegroundColor = _grokColor;
        Console.WriteLine($"{format}", args);
        Console.ResetColor();
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
        Console.WriteLine(message);

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
