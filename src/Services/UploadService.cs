using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GrokCLI.Decompressors;
using GrokCLI.Utils;
using GrokCLI.Helpers;
using GrokCLI.Renderers;

public class UploadService
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string url = "https://grok.com/rest/app-chat/upload-file";

    public async Task<byte[]> Execute(FileInfo file)
    {
        Logger.Info($"executing... uploading {file.Name}");

        // Step 1: Validate and process the file
        byte[] fileBytes;
        string finalFileName;
        string mimeType;

        (fileBytes, finalFileName, mimeType) = await ProcessFile(file);
        if (fileBytes == null)
        {
            Logger.Error($"File processing failed for {file.Name}. Upload aborted.");
        }

        // Create JSON payload
        var jsonPayload = new
        {
            fileName = finalFileName,
            fileMimeType = mimeType, // Use the corrected MIME type (image/png for converted files)
            content = Convert.ToBase64String(fileBytes)
        };

        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonPayload);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        // Set up the request
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        foreach (var header in HttpHeaderCollection.UploadHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        Logger.Info($"Sending request to {url}");
        Logger.Info($"Request headers: {request.Headers}");
        Logger.Info($"Request body: {jsonPayload}");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

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

    private async Task<(byte[]?, string, string)> ProcessFile(FileInfo file)
    {
        // Step: Check if the file is valid for upload or needs conversion
        string extension = file.Extension.ToLowerInvariant();
        string[] allowedImageExtensions = { ".png", ".jpg", ".jpeg" };
        string[] allowedTextExtensions = { ".txt", ".cs", ".js", ".md", ".json" }; // Add more text/code extensions as needed

        if (allowedImageExtensions.Contains(extension))
        {
            // Direct upload for image files
            byte[] fileBytes = await File.ReadAllBytesAsync(file.FullName);
            Logger.Info($"Image file {file.Name} validated (MIME type: image/{extension.TrimStart('.')})");
            return (fileBytes, file.Name, $"image/{extension.TrimStart('.')}");
        }
        else if (allowedTextExtensions.Contains(extension))
        {
            // Convert text file to image and upload as PNG
            Logger.Info($"Converting text file {file.Name} to image for upload");
            byte[] textBytes = await File.ReadAllBytesAsync(file.FullName);
            string textContent = Encoding.UTF8.GetString(textBytes);
            byte[] imageBytes = ConvertTextToImage(textContent, file.Name);

            if (imageBytes == null)
            {
                Logger.Error($"Failed to convert {file.Name} to image");
                return (null, "", "");
            }

            string convertedFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}_converted.png";
            Logger.Info($"Text file {file.Name} converted to {convertedFileName} (MIME type: image/png)");
            return (imageBytes, convertedFileName, "image/png");
        }
        else
        {
            // Block all other file types
            Logger.Error($"File {file.Name} has an unsupported extension: {extension}. Only image files or plain text files are allowed.");
            return (null, "", "");
        }
    }

    private byte[] ConvertTextToImage(string text, string fileName)
    {
        try
        {
            // Create a bitmap (e.g., 800x600 pixels, adjustable based on text length)
            int width = 800;
            int height = 600;
            using Bitmap bitmap = new Bitmap(width, height);
            using Graphics graphics = Graphics.FromImage(bitmap);
            using Font font = new Font("Arial", 12);
            using SolidBrush brush = new SolidBrush(Color.Black);

            // Clear the bitmap with white background
            graphics.Clear(Color.White);

            // Measure and draw the text, wrapping if necessary
            StringFormat format = new StringFormat();
            RectangleF rect = new RectangleF(10, 10, width - 20, height - 20);
            graphics.DrawString(text, font, brush, rect, format);

            // Save the bitmap as PNG and convert to byte array
            using MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            Logger.Error($"Error converting {fileName} to image: {ex.Message}");
            return null;
        }
    }

    private string CleanResponse(string response)
    {
        // Remove leading/trailing whitespace and check if it starts with '!'
        if (string.IsNullOrEmpty(response))
            return string.Empty;

        string trimmed = response.Trim();
        if (trimmed.StartsWith("!"))
        {
            // Try to extract JSON if it's wrapped or preceded by non-JSON content
            int jsonStart = trimmed.IndexOf('{');
            if (jsonStart >= 0)
            {
                trimmed = trimmed.Substring(jsonStart);
            }
            else
            {
                Logger.Error($"Non-JSON response detected: {trimmed}");
                return string.Empty;
            }
        }

        // Validate if the trimmed response is valid JSON
        try
        {
            JObject.Parse(trimmed);
            return trimmed;
        }
        catch (JsonException)
        {
            Logger.Error($"Invalid JSON response after cleaning: {trimmed}");
            return string.Empty;
        }
    }
}