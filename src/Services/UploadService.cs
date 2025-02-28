using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        // Step 1: Validate and process the file
        byte[] fileBytes;
        string finalFileName;
        string mimeType;

        (fileBytes, finalFileName, mimeType) = await ProcessFile(file);
        if (fileBytes == null)
        {
            Logger.Error($"File processing failed for {file.Name}. Upload aborted.");
            return $"Error: File processing failed for {file.Name}";
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

        response.EnsureSuccessStatusCode();
        Logger.Info("sent.");
        Logger.Info("receiving....");
        string responseBody = await response.Content.ReadAsStringAsync();
        Logger.Info("received.");
        return responseBody;
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
}