using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GrokCLI.Renderers;
public class GrokResponseRenderer
{
    public static void RenderResponses(string jsonInput)
    {
        // Split the input into individual JSON objects (assuming newline-separated JSON)
        string[] jsonObjects = jsonInput.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        StringBuilder conversation = new StringBuilder();
        string currentResponseId = null;
        string userMessage = null;

        foreach (string json in jsonObjects)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                JToken result = obj["result"];

                if (result != null)
                {
                    if (result["conversation"] != null)
                    {
                        // Handle conversation initialization
                        string title = result["conversation"]["title"]?.ToString() ?? "New Conversation";
                        conversation.AppendLine($"Conversation: {title}");
                        conversation.AppendLine("--------------------------------");
                    }
                    else if (result["response"] != null)
                    {
                        JToken response = result["response"];
                        string responseId = response["responseId"]?.ToString();

                        if (response["userResponse"] != null)
                        {
                            // User message
                            userMessage = response["userResponse"]["message"]?.ToString();
                            if (!string.IsNullOrEmpty(userMessage))
                            {
                                conversation.AppendLine($"User: {userMessage}");
                            }
                        }
                        else if (response["token"] != null)
                        {
                            // Grok token (part of the AI response)
                            string token = response["token"]?.ToString();
                            if (!string.IsNullOrEmpty(token) && response["isSoftStop"]?.ToObject<bool>() != true)
                            {
                                if (currentResponseId != responseId)
                                {
                                    if (currentResponseId != null && conversation.Length > 0)
                                    {
                                        conversation.AppendLine(); // Add a newline between responses
                                    }
                                    currentResponseId = responseId;
                                    conversation.Append("Grok: ");
                                }
                                conversation.Append(token);
                            }
                        }
                        else if (response["modelResponse"] != null)
                        {
                            // Final Grok response
                            string finalMessage = response["modelResponse"]["message"]?.ToString();
                            if (!string.IsNullOrEmpty(finalMessage))
                            {
                                conversation.AppendLine(finalMessage);
                            }
                        }
                    }
                    else if (result["title"] != null)
                    {
                        // Update conversation title
                        string newTitle = result["title"]["newTitle"]?.ToString();
                        if (!string.IsNullOrEmpty(newTitle))
                        {
                            conversation.AppendLine($"\nConversation Title Updated: {newTitle}");
                            conversation.AppendLine("--------------------------------");
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }
        }

        // Output the rendered conversation
        Console.WriteLine(conversation.ToString());
    }
}