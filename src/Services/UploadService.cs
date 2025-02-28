using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCLI.Utils;
using GrokCLI.Helpers;

public class UploadService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> Execute(FileInfo file)
    {
        Logger.Info($"executing... uploading {file.Name}");

        // Read file content and convert to base64
        byte[] fileBytes = await File.ReadAllBytesAsync(file.FullName);
        string base64Content = Convert.ToBase64String(fileBytes);

        // Determine the correct MIME type for the file
        string mimeType = MimeHelper.GetMimeType(file.Extension) ?? "text/plain"; // Default to text/plain if unknown
        Logger.Info($"Detected MIME type for {file.Name}: {mimeType}");

        // Create JSON payload
        var jsonPayload = new
        {
            fileName = file.Name,
            fileMimeType = mimeType, // Use the corrected MIME type
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
            Logger.Error($"Error: HTTP {response.StatusCode} - {errorContent}");
            return $"Error: HTTP {response.StatusCode} - {errorContent}";
        }

         // Log response headers to understand encoding and content type
        Logger.Info("Content-Type: " + response.Content.Headers.ContentType);
        Logger.Info("Content-Encoding: " + (response.Content.Headers.ContentEncoding?.FirstOrDefault() ?? "none"));

        // Read the raw response as a byte array
        byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();

        // Handle decompression using the reusable method
        string? contentEncoding = response.Content.Headers.ContentEncoding?.FirstOrDefault();
        responseBytes = DecompressionHelper.DecompressResponse(responseBytes, contentEncoding) ?? responseBytes;

        // Convert bytes to string for rendering
        string responseString = Encoding.UTF8.GetString(responseBytes);

        // Render the response using GrokResponseRenderer
        await new UploadResponseRenderer().Render(responseString);

        // Return the decompressed bytes directly
        return responseBytes;
    }
}