using GrokCLI.Helpers;
using GrokCLI.Services;
using GrokCLI.Utils;

namespace GrokCLI.Commands;
public class MultiLineInputCommand : ICommand
{
    public const string CommandName = "multiline";

    public async Task Execute(string? parameter = null)
    {
        Logger.Info($"{CommandName} command executing with parameter: {parameter}");

        Console.WriteLine("Paste your multi-line text (press Enter twice or type 'END' on a new line to finish):");
        Console.WriteLine("(To paste, use Ctrl+V or right-click in the console, then signal end with Enter + Enter or 'END')");

        var lines = new List<string>();
        string? line;

        while (true)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line) || line.ToUpper() == "END")
                break; // Exit loop on empty line or "END"

            lines.Add(line ?? "");
        }

        Logger.Info("Processing multi-line input:");
        foreach (var l in lines)
        {
            Logger.Info($"Line: {l}");
            // Add your logic here to process each line (e.g., save to a file, parse, etc.)
        }

        await Task.CompletedTask; // Ensure async compatibility, even if no async operations are needed
    }
}