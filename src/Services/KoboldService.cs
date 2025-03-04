using System.Text;
using System.IO.Compression;
using GrokCLI.Helpers;
using GrokCLI.Utils;

namespace GrokCLI.Services;
public class KoboldService
{
    public async Task<string> Execute()
    {
        using (var client = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:3000/api");
            foreach (var header in HttpHeaderCollection.RateLimitHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var content = new StringContent("application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Log response headers to understand encoding and content type
            Logger.Info("Content-Type: " + response.Content.Headers.ContentType);
            Logger.Info("Content-Encoding: " + (response.Content.Headers.ContentEncoding?.FirstOrDefault() ?? "none"));

            // Read the raw response as a byte array
            byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();

            // Handle decompression using the reusable method
            string? contentEncoding = response.Content.Headers.ContentEncoding?.FirstOrDefault();
            responseBytes = DecompressionHelper.DecompressResponse(responseBytes, contentEncoding);

            // Convert the decompressed bytes to a string
            string resultString = Encoding.UTF8.GetString(responseBytes);
            Logger.Output(resultString);
            return resultString;
        }
    }
}