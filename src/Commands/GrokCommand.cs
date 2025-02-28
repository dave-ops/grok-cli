using GrokCLI.Helpers;

namespace GrokCLI;

public static class GrokCommand
{
    public static async Task Execute(string? parameter)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            Logger.Info("Error: Please provide a message for Grok (e.g., grok grok \"Hello, Grok!\")");
            return;
        }
        await new Grok().Execute(parameter);
    }
}