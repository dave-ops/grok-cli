using GrokCLI.Helpers;

namespace GrokCLI;

public static class GrokCommand
{
    public static async Task Execute(string? prompt)
    {
        Logger.Info("execute");
        if (string.IsNullOrEmpty(prompt))
        {
            Logger.Info("Error: Please provide a message for Grok (e.g., grok grok \"Hello, Grok!\")");
            return;
        }
        await new GrokService().Execute(prompt);
    }
}