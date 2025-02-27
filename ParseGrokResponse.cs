using System;
using System.Text.Json;
using System.Text;

namespace GrokCS
{
    // Classes to match the JSON structure
    public class Conversation
    {
        public string conversationId { get; set; }
        public string title { get; set; }
        public bool starred { get; set; }
        public DateTime createTime { get; set; }
        public DateTime modifyTime { get; set; }
        public string systemPromptName { get; set; }
        public bool temporary { get; set; }
        public string[] mediaTypes { get; set; }
    }

    public class UserResponse
    {
        public string responseId { get; set; }
        public string message { get; set; }
        public string sender { get; set; }
        public DateTime createTime { get; set; }
        public bool manual { get; set; }
        public bool partial { get; set; }
        public bool shared { get; set; }
        public string query { get; set; }
        public string queryType { get; set; }
        public object[] webSearchResults { get; set; }
        public string[] xpostIds { get; set; }
        public object[] xposts { get; set; }
        public string[] generatedImageUrls { get; set; }
        public object[] imageAttachments { get; set; }
        public object[] fileAttachments { get; set; }
        public object[] cardAttachmentsJson { get; set; }
        public string[] fileUris { get; set; }
        public object[] fileAttachmentsMetadata { get; set; }
        public bool isControl { get; set; }
        public object[] steps { get; set; }
        public string[] mediaTypes { get; set; }
    }

    public class ResponseData
    {
        public UserResponse userResponse { get; set; }
        public bool isThinking { get; set; }
        public bool isSoftStop { get; set; }
        public string responseId { get; set; }
    }

    public class TokenResponse
    {
        public string token { get; set; }
        public bool isThinking { get; set; }
        public bool isSoftStop { get; set; }
        public string responseId { get; set; }
    }

    public class FinalMetadata
    {
        public string[] followUpSuggestions { get; set; }
        public string[] feedbackLabels { get; set; }
        public object toolsUsed { get; set; }
        public string disclaimer { get; set; }
    }

    public class ModelResponse
    {
        public string responseId { get; set; }
        public string message { get; set; }
        public string sender { get; set; }
        public DateTime createTime { get; set; }
        public string parentResponseId { get; set; }
        public bool manual { get; set; }
        public bool partial { get; set; }
        public bool shared { get; set; }
        public string query { get; set; }
        public string queryType { get; set; }
        public object[] webSearchResults { get; set; }
        public string[] xpostIds { get; set; }
        public object[] xposts { get; set; }
        public string[] generatedImageUrls { get; set; }
        public object[] imageAttachments { get; set; }
        public object[] fileAttachments { get; set; }
        public object[] cardAttachmentsJson { get; set; }
        public string[] fileUris { get; set; }
        public object[] fileAttachmentsMetadata { get; set; }
        public bool isControl { get; set; }
        public object[] steps { get; set; }
        public string[] mediaTypes { get; set; }
    }

    public class Result
    {
        public Conversation conversation { get; set; }
        public ResponseData response { get; set; }
        public TokenResponse token { get; set; }
        public FinalMetadata finalMetadata { get; set; }
        public ModelResponse modelResponse { get; set; }
        public NewTitle title { get; set; }
    }

    public class NewTitle
    {
        public string newTitle { get; set; }
    }

    public class GrokResponse
    {
        public Result result { get; set; }
    }

    public static class ParseGrokResponse
    {
        public static string ParseResponse(byte[] responseBytes)
        {
            try
            {
                // Convert bytes to string
                string responseText = Encoding.UTF8.GetString(responseBytes);

                // Split the response into individual JSON objects (assuming each line is a separate object)
                string[] jsonObjects = responseText.Split(new[] { "}" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s + "}")
                    .ToArray();

                string fullMessage = "";
                foreach (var json in jsonObjects)
                {
                    if (string.IsNullOrWhiteSpace(json)) continue;

                    // Try to deserialize each object
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