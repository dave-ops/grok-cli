using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCS;
using static GrokCS.ParseGrokResponse;
using static GrokCS.GetRateLimit;

await Upload();

async Task Grok()
{
    var app = new Grok3();
    var result = await app.Execute(); // Await the async method
    Console.WriteLine(result);
}

async Task Upload()
{
    Console.WriteLine("uploading...");
    var upload = new Upload();
    var file = new FileInfo("C:\\temp\\mic.txt");
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

