namespace GrokCLI.Helpers;

public static class CommandProcessor
{
    public static async Task ProcessArgs(string[]? args)
    {
        if (args == null || args.Length == 0)
        {
            Logger.Info("No arguments provided. Defaulting to 'grok' with message: 'Default Grok message'");
            await new GrokService().Execute("Default Grok message");
            return;
        }

        Logger.Info($"Args: {string.Join(", ", args)}");
        var parsedArgs = ParseArgs(args);
        Logger.Info(parsedArgs.ToString());
        
        await ExecuteCommand(parsedArgs.cmd, parsedArgs.prompt);
    }

    private static (string cmd, string? prompt) ParseArgs(string[] args)
    {
        string cmd = args[0];
        string prompt = args[1];
        Logger.Info($"cmd: {cmd}, Parameter: {prompt}");
        return (cmd, prompt);
    }

    private static async Task ExecuteCommand(string command, string? parameter)
    {
        Logger.Info($"excuting... {command} {parameter}");
        if (string.IsNullOrEmpty(command))
        {
            Logger.Info("Command Required.");
            return;
        }

        switch (command)
        {
            case "upload":
                await UploadCommand.Execute(parameter);
                break;
            case "grok":
                Logger.Info("in grok");
                await GrokCommand.Execute(parameter);
                break;
            case "ratelimit":
                await RateLimitCommand.Execute();
                break;
            default:
                await GrokCommand.Execute(parameter);
                break;
        }
    }
}