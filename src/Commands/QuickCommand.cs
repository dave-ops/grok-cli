using GrokCLI.Helpers;
using GrokCLI.Services;
using GrokCLI.Utils;

namespace GrokCLI.Commands;
public class QuickCommand : ICommand
{
    public const string CommandName = "quick";

    public async Task Execute(string? parameter = null)
    {
        if (string.IsNullOrEmpty(parameter)) {
            Logger.Info($"missing command.\n{CommandName} <prompt>");
            return;
        }
        string prompt = $"i need a very quick answer. no explanation. no talk. just code.\nprompt:\n{parameter}";
        await new GrokService().Execute(prompt);
    }
}
