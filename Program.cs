using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCS;
using static GrokCS.ParseGrokResponse;
using static GrokCS.GetRateLimit;

static async Task Main(string[] args)
{
    // Default to Grok if no arguments or invalid command is provided
    string command = args.Length > 0 ? args[0].ToLower() : "grok";
    string parameter = args.Length > 1 ? args[1] : null;

    switch (command)
    {
        case "upload":
            if (string.IsNullOrEmpty(parameter))
            {
                Console.WriteLine("Error: Please provide a filepath for upload (e.g., dotnet run -- upload C:\\temp\\screenshot.png)");
                return;
            }
            await Upload(parameter);
            break;

        case "grok":
            if (string.IsNullOrEmpty(parameter))
            {
                Console.WriteLine("Error: Please provide a message for Grok (e.g., dotnet run -- grok \"Hello, Grok!\")");
                return;
            }
            await Grok(parameter);
            break;

        case "ratelimit":
            await GetRateLimit();
            break;

        default:
            // Default to Grok with no parameter (or an empty message if you want)
            await Grok(parameter ?? "Default Grok message");
            break;
    }
}

async Task Grok(string message)
{
    var app = new Grok3();
    var result = await app.Execute(message); // Assuming Grok3.Execute accepts a message parameter
    Console.WriteLine(result);
}

async Task Upload(string filePath)
{
    Console.WriteLine("uploading...");
    var upload = new Upload();
    var file = new FileInfo(filePath);
    string result = await upload.Execute(file);
    Console.WriteLine(result);
    Console.WriteLine("done.");
}

async Task GetRateLimit()
{
    var app = new GetRateLimit();
    var result = await app.Execute(); // Await the async method
    Console.WriteLine(result);
}