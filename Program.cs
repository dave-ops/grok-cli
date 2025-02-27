using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCS;

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
            FileInfo f = FileHelper.GetFileInfo(parameter);
            await new Upload().Execute(f);
            break;

        case "grok":
            if (string.IsNullOrEmpty(parameter))
            {
                Console.WriteLine("Error: Please provide a message for Grok (e.g., dotnet run -- grok \"Hello, Grok!\")");
                return;
            }
            await new Grok3().Execute();
            break;

        case "ratelimit":
            await new GetRateLimit().Execute();
            break;

        default:
            await new Grok3().Execute();
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
    var upload = new Upload(); // Create an instance of the Upload class
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