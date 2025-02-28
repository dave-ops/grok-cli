using System.Text;
using GrokCLI.Helpers;
using GrokCLI.Utils;

namespace GrokCLI
{
    public class GrokService
    {

        public async Task<byte[]> Execute(string message = "say your name")
        {
            Logger.Info($"grok service {message}");
            using (var client = new HttpClient())
            {
                var url = "https://grok.com/rest/app-chat/conversations/new";

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
                }";
                
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                foreach (var header in HttpHeaderCollection.GrokHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                request.Content = content;

                try
                {
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsByteArrayAsync();
                }
                catch (HttpRequestException ex)
                {
                    Logger.Info($"Error: {ex.Message}");
                    throw;
                }
            }
        }
    }

}