using GrokCLI.Services;
using GrokCLI.Utils;

namespace GrokCLI.Commands;
public class GrokCommand : ICommand
{
    public const string CommandName = "grok";

    public async Task Execute(string? parameter = null)
    {
        Logger.Info($"{CommandName} command executing with parameter: {parameter}");
        await new GrokService().Execute(parameter ?? "Default Grok message");
    }
}
