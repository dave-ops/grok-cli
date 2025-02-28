using GrokCLI.Helpers;
using GrokCLI.Services;
using GrokCLI.Utils;

namespace GrokCLI.Commands;

public class RateLimitCommand : ICommand
{
    public const string CommandName = "ratelimit";

    public async Task Execute(string? parameter = null)
    {
        Logger.Info($"{CommandName} command executing...");
        await new GetRateLimitService().Execute();
    }
}