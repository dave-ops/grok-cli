using GrokCLI.Services;
using GrokCLI.Utils;

namespace GrokCLI.Helpers
{
    public class RateLimitCommand : ICommand
    {
        public async Task Execute(string? parameter = null)
        {
            Logger.Info("RateLimit command executing...");
            await new GetRateLimitService().Execute();
        }
    }
}