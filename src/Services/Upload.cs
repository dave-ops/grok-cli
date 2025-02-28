using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCLI.Utils;

public class Upload
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> Execute(FileInfo file)
    {

        Logger.Info("executing...");

        // Read file content and convert to base64
        byte[] fileBytes = await File.ReadAllBytesAsync(file.FullName);
        string base64Content = Convert.ToBase64String(fileBytes);

        // Create JSON payload
        var jsonPayload = new
        {
            fileName = file.Name,
            fileMimeType = GetMimeType(file.Extension),
            content = base64Content
        };

        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonPayload);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Set up the request
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://grok.com/rest/app-chat/upload-file"),
            Content = content
        };

        foreach (var header in HttpHeaderCollection.UploadHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        // Send the request and get response
        Logger.Info("sending...");
        HttpResponseMessage response = await client.SendAsync(request);
        
        // Check if the response is successful before calling EnsureSuccessStatusCode
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            Logger.Info($"Error: HTTP {response.StatusCode} - {errorContent}");
            return $"Error: HTTP {response.StatusCode} - {errorContent}";
        }

        response.EnsureSuccessStatusCode();
        Logger.Info("sent.");
        Logger.Info("receiving....");
        string responseBody = await response.Content.ReadAsStringAsync();
        Logger.Info("received.");
        return responseBody;
    }

    private string GetMimeType(string extension)
    {
        switch (extension.ToLower())
        {
            case ".png":
                return "image/png";
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".pdf":
                return "application/pdf";
            default:
                return "application/octet-stream";
        }
    }
}