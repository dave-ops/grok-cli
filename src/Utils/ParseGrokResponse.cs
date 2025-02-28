using System;
using System.Text.Json;
using System.Text;

namespace GrokCLI.Models
{
    public static class ParseGrokResponse
    {
        public static string ParseResponse(byte[] responseBytes)
        {
            try
            {
                string responseText = Encoding.UTF8.GetString(responseBytes);
                string[] jsonObjects = responseText.Split(new[] { "}" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s + "}")
                    .ToArray();

                string fullMessage = "";
                foreach (var json in jsonObjects)
                {
                    if (string.IsNullOrWhiteSpace(json)) continue;

                    var grokResponse = JsonSerializer.Deserialize<GrokResponse>(json);
                    if (grokResponse?.result?.modelResponse?.message != null)
                    {
                        fullMessage = grokResponse.result.modelResponse.message;
                    }
                }

                return string.IsNullOrEmpty(fullMessage) ? "No message found" : fullMessage;
            }
            catch (JsonException ex)
            {
                return $"Error parsing JSON: {ex.Message}";
            }
        }
    }
}