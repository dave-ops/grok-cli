namespace GrokCLI.Helpers;

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
}