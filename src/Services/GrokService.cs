namespace GrokCLI.Services;

using System.Net.Http;
using System.Text;
using GrokCLI.Helpers;
using GrokCLI.Renderers;
using GrokCLI.Utils;

public class GrokService : IService
{
    public async Task<byte[]> Execute(string prompt)
    {
        var message = prompt ?? Grok.GetDefaultUserPrompt();
        Logger.Info($"Grok service executing with message: {message}");
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = true, // Follow redirects like curl --location
            MaxAutomaticRedirections = 10
        };
        
        using (var client = new HttpClient(handler))
        {
            var url = Grok.GetConversationUri();

            // Replace {message} placeholder with the actual message
            var jsonPayload = @"{
                ""temporary"":false,
                ""modelName"":""{version}"",
                ""message"":""{message}"",
                ""fileAttachments"":[],
                ""imageAttachments"":[],
                ""disableSearch"":false,
                ""enableImageGeneration"":true,
                ""returnImageBytes"":false,
                ""returnRawGrokInXaiRequest"":false,
                ""enableImageStreaming"":true,
                ""imageGenerationCount"":2,
                ""forceConcise"":false,
                ""toolOverrides"":{},
                ""enableSideBySide"":true,
                ""isPreset"":false,
                ""sendFinalMetadata"":true,
                ""customInstructions"":"""",
                ""deepsearchPreset"":"""",
                ""isReasoning"":false
            }".Replace("{message}", message)
              .Replace("{version}", Grok.GetVersion());

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Add headers from HttpHeaderCollection.GrokHeaders
            foreach (var header in HttpHeaderCollection.GrokHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            try
            {
                Logger.Info($"Sending request to {url}");
                Logger.Debug($"Request headers: {request.Headers}");
                Logger.Debug($"Request body: {jsonPayload}");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Log response headers to understand encoding and content type
                Logger.Debug("Content-Type: " + response.Content.Headers.ContentType);
                Logger.Debug("Content-Encoding: " + (response.Content.Headers.ContentEncoding?.FirstOrDefault() ?? "none"));

                // Read the raw response as a byte array
                byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();

                // Handle decompression using the reusable method
                string? contentEncoding = response.Content.Headers.ContentEncoding?.FirstOrDefault();
                responseBytes = DecompressionHelper.DecompressResponse(responseBytes, contentEncoding) ?? responseBytes;

                // Convert bytes to string for rendering
                string responseString = Encoding.UTF8.GetString(responseBytes);

                // Render the response using GrokResponseRenderer
                await new GrokResponseRenderer().Render(responseString);

                // Return the decompressed bytes directly
                return responseBytes;
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"HTTP request failed: {ex.Message}");
                throw;
            }
        }
    }
}