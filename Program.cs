using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static bool stopAnimation = false;
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run -- <message>");
            return;
        }

        string message = string.Join(" ", args);
        string? response = await MakeGrokRequest(message);
        if (response != null)
        {
            ParseGrokResponse(response);
        }
    }

    static void ShowThinking()
    {
        string[] animation = new string[] { ".", "..", "..." };
        int index = 0;

        while (!stopAnimation)
        {
            Console.Write($"\rThinking{animation[index]}");
            index = (index + 1) % animation.Length;
            Thread.Sleep(300);
        }
        Console.Write("\r" + new string(' ', 20) + "\r");
    }

    static void ParseGrokResponse(string responseText)
    {
        var lines = responseText.Trim().Split('\n');
        var conversationInfo = new Dictionary<string, string>();
        string userMessage = "";
        var responseTokens = new List<string>();
        string fullResponse = "";
        var metadata = new Dictionary<string, object>();
        string finalTitle = "";

        foreach (var line in lines)
        {
            try
            {
                using var document = JsonDocument.Parse(line);
                var result = document.RootElement.GetProperty("result");

                if (result.TryGetProperty("conversation", out var conv))
                {
                    conversationInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(conv.GetRawText()) ?? new Dictionary<string, string>();
                }

                if (result.TryGetProperty("response", out var resp))
                {
                    if (resp.TryGetProperty("userResponse", out var userResp))
                    {
                        userMessage = userResp.GetProperty("message").GetString() ?? "";
                    }

                    if (resp.TryGetProperty("token", out var token))
                    {
                        string? tokenStr = token.GetString();
                        if (!string.IsNullOrEmpty(tokenStr))
                        {
                            responseTokens.Add(tokenStr);
                        }
                    }

                    if (resp.TryGetProperty("finalMetadata", out var meta))
                    {
                        metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(meta.GetRawText()) ?? new Dictionary<string, object>();
                    }

                    if (resp.TryGetProperty("modelResponse", out var modelResp))
                    {
                        fullResponse = modelResp.GetProperty("message").GetString() ?? "";
                    }
                }

                if (result.TryGetProperty("title", out var title))
                {
                    finalTitle = title.GetProperty("newTitle").GetString() ?? "";
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Error decoding JSON line: {line}\nError: {e.Message}");
                continue;
            }
        }

        string assembledResponse = string.IsNullOrEmpty(fullResponse) 
            ? string.Join("", responseTokens).Trim() 
            : fullResponse;

        Console.WriteLine("=== Grok Response ===");
        Console.WriteLine($"Conversation ID: {conversationInfo.GetValueOrDefault("conversationId", "N/A")}");
        Console.WriteLine($"Created: {conversationInfo.GetValueOrDefault("createTime", "N/A")}");
        Console.WriteLine($"Your Message: {userMessage}");
        Console.WriteLine("\nGrok Says:");
        Console.WriteLine(assembledResponse);

        if (metadata.TryGetValue("followUpSuggestions", out var suggestionsObj) && suggestionsObj != null)
        {
            Console.WriteLine("\nFollow-up Suggestions:");
            var suggestions = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(suggestionsObj.ToString() ?? "[]");
            if (suggestions != null)
            {
                foreach (var suggestion in suggestions)
                {
                    Console.WriteLine($"- {suggestion["label"]}");
                }
            }
        }

        if (!string.IsNullOrEmpty(finalTitle))
        {
            Console.WriteLine($"\nConversation Title: {finalTitle}");
        }
    }

    static async Task<string?> MakeGrokRequest(string message)
    {
        var thinkingThread = new Thread(ShowThinking);
        thinkingThread.Start();

        try
        {
            var url = "https://grok.com/rest/app-chat/conversations/new";
            var payload = new
            {
                temporary = false,
                modelName = "grok-latest",
                message,
                fileAttachments = new string[] { },
                imageAttachments = new string[] { },
                disableSearch = false,
                enableImageGeneration = true,
                returnImageBytes = false,
                returnRawGrokInXaiRequest = false,
                enableImageStreaming = true,
                imageGenerationCount = 2,
                forceConcise = false,
                toolOverrides = new { },
                enableSideBySide = true,
                isPreset = false,
                sendFinalMetadata = true,
                customInstructions = "",
                deepsearchPreset = "",
                isReasoning = false
            };

            var jsonContent = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonContent, Encoding.UTF8);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var requestHeaders = new Dictionary<string, string>
            {
                { "host", "grok.com" },
                { "connection", "keep-alive" },
                { "sec-ch-ua-platform", "\"Windows\"" },
                { "sec-ch-ua", "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"" },
                { "sec-ch-ua-mobile", "?0" },
                { "baggage", "sentry-environment=production,sentry-release=oVPBj5ez-07C9D12RX46Y,sentry-public_key=b311e0f2690c81f25e2c4cf6d4f7ce1c,sentry-trace_id=a635555ada924b96a2721ee436d61e1b,sentry-sample_rate=1,sentry-sampled=true" },
                { "sentry-trace", "a635555ada924b96a2721ee436d61e1b-9d2b4d894110a440-1" },
                { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36" },
                { "dnt", "1" },
                { "accept", "*/*" },
                { "origin", "https://grok.com" },
                { "sec-fetch-site", "same-origin" },
                { "sec-fetch-mode", "cors" },
                { "sec-fetch-dest", "empty" },
                { "referer", "https://grok.com/?referrer=website" },
                { "accept-encoding", "gzip, deflate, br, zstd" },
                { "accept-language", "en-US,en;q=0.9" },
                { "cookie", "sso-rw=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; sso=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; _ga=GA1.1.828449324.1740173126; _ga_8FEWB057YH=GS1.1.1740581925.26.1.1740583454.0.0.0" },
                { "x-postman-captr", "7330362" }
            };

            client.DefaultRequestHeaders.Clear();
            foreach (var header in requestHeaders)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");
            return null;
        }
        finally
        {
            stopAnimation = true;
            thinkingThread.Join();
        }
    }
}