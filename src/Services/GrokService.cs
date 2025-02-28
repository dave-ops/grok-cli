using System.Net.Http;
using System.Text;
using GrokCLI.Helpers;
using GrokCLI.Utils;

namespace GrokCLI
{
    public class GrokService
    {
        public async Task<byte[]> Execute(string message = "say your name")
        {
            Logger.Info($"Grok service executing with message: {message}");
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true, // Follow redirects like curl --location
                MaxAutomaticRedirections = 10
            };
            
            using (var client = new HttpClient(handler))
            {
                var url = "https://grok.com/rest/app-chat/conversations/new";

                // Replace {message} placeholder with the actual message
                var jsonPayload = @"{
                    ""temporary"":false,
                    ""modelName"":""grok-latest"",
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
                }".Replace("{message}", message);

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
                    Logger.Info($"Request headers: {request.Headers}");
                    Logger.Info($"Request body: {jsonPayload}");

                    var response = await client.SendAsync(request);

                    // Log response details before checking status
                    Logger.Info($"Response status: {response.StatusCode}");
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Logger.Info($"Response content: {responseContent}");

                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsByteArrayAsync();
                }
                catch (HttpRequestException ex)
                {
                    Logger.Error($"HTTP request failed: {ex.Message}");
                    throw;
                }
            }
        }
    }
}