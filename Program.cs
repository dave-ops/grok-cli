using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCS;

// Top-level statements start here
Console.WriteLine("Starting grok...");
if (args == null || args.Length == 0)
{
    Console.WriteLine("No arguments provided. Defaulting to 'grok' with message: 'Default Grok message'");
    await new Grok3().Execute("Default Grok message");
    return;
}
Console.WriteLine($"Args: {string.Join(", ", args)}");

string commandPrefix = args.Length > 0 && args[0].ToLower() == "grok" ? "grok" : "";
string command = args.Length > (commandPrefix == "grok" ? 1 : 0) ? 
    (commandPrefix == "grok" ? args[1].ToLower() : args[0].ToLower()) : "grok";
string parameter = args.Length > (commandPrefix == "grok" ? 2 : 1) ? args[commandPrefix == "grok" ? 2 : 1] : null;

Console.WriteLine($"Command: {command}, Parameter: {parameter}");
if (string.IsNullOrEmpty(command))
{
    Console.WriteLine("No valid command provided. Defaulting to 'grok' with message: 'Default Grok message'");
    await new Grok3().Execute("Default Grok message");
    return;
}

switch (command)
{
    case "upload":
        if (string.IsNullOrEmpty(parameter))
        {
            Console.WriteLine("Error: Please provide a filepath for upload (e.g., grok upload C:\\temp\\screenshot.png)");
            return;
        }
        FileInfo f = FileHelper.GetFileInfo(parameter);
        await new Upload().Execute(f);
        break;

    case "grok":
        if (string.IsNullOrEmpty(parameter))
        {
            Console.WriteLine("Error: Please provide a message for Grok (e.g., grok grok \"Hello, Grok!\")");
            return;
        }
        await new Grok3().Execute(parameter); // Use the parameter as the message
        break;

    case "ratelimit":
        await new GetRateLimit().Execute();
        break;

    default:
        Console.WriteLine($"Unknown command '{command}'. Defaulting to Grok with message: 'Default Grok message'");
        await new Grok3().Execute("Default Grok message");
        break;
}