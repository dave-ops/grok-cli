// using GrokCLI.Helpers;
// Logger.Info("starting grok ...");
// await CommandProcessor.ProcessArgs(args);
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Renci.SshNet;
using Renci.SshNet.Common;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        string userId = "user123"; // Replace with actual user ID
        string privateKeyPath = @"C:\path\to\id_rsa"; // Path to private key
        string serverUrl = "http://localhost:3000/api/authenticate";

        // Load private key
        var privateKey = new PrivateKeyFile(privateKeyPath);
        var sshKey = privateKey.Key as SshKey;

        // Create a message to sign (could be a timestamp or nonce)
        string message = DateTime.UtcNow.ToString("o");
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        // Sign the message with the private key
        byte[] signature = sshKey.Sign(messageBytes);
        string signatureBase64 = Convert.ToBase64String(signature);

        // Send to server
        var requestBody = new
        {
            userId,
            message,
            signature = signatureBase64
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(serverUrl, content);
        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Server response: {responseBody}");
    }
}