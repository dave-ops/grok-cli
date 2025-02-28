using GrokCLI.Helpers;

namespace GrokCLI;

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
        
        await ExecuteCommand(parsedArgs.Command, parsedArgs.Parameter);
    }

    private static (string Command, string? Parameter) ParseArgs(string[] args)
    {
        string commandPrefix = args[0].ToLower() == "grok" ? "grok" : "";
        string command = args.Length > (commandPrefix == "grok" ? 1 : 0) 
            ? (commandPrefix == "grok" ? args[1].ToLower() : args[0].ToLower()) 
            : "grok";
        string? parameter = args.Length > (commandPrefix == "grok" ? 2 : 1) 
            ? args[commandPrefix == "grok" ? 2 : 1] 
            : null;

        Logger.Info($"Command: {command}, Parameter: {parameter}");
        return (command, parameter);
    }

    private static async Task ExecuteCommand(string command, string? parameter)
    {
        if (string.IsNullOrEmpty(command))
        {
            await ExecuteDefaultGrok();
            return;
        }

        switch (command)
        {
            case "upload":
                await UploadCommand.Execute(parameter);
                break;
            case "grok":
                await GrokCommand.Execute(parameter);
                break;
            case "ratelimit":
                await RateLimitCommand.Execute();
                break;
            default:
                Logger.Info($"Unknown command '{command}'. Defaulting to Grok with message: 'Default Grok message'");
                await ExecuteDefaultGrok();
                break;
        }
    }

    private static async Task ExecuteDefaultGrok()
    {
        Logger.Info("No valid command provided. Defaulting to 'grok' with message: 'Default Grok message'");
        await new GrokService().Execute("Default Grok message");
    }
}