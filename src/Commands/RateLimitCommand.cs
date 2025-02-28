namespace GrokCLI;

public static class RateLimitCommand
{
    public static async Task Execute()
    {
        await new GetRateLimitService().Execute();
    }
}