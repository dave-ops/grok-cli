using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrokCS;
using static GrokCS.ParseGrokResponse;
using static GrokCS.GetRateLimit;

Console.WriteLine(new GetRateLimit().Execute().Result);
return;

var client = new HttpClient();
var url = "https://grok.com/rest/app-chat/conversations/new";

var jsonPayload = @"{""temporary"":false,""modelName"":""grok-3"",""message"":""say your name"",""fileAttachments"":[],""imageAttachments"":[],""disableSearch"":false,""enableImageGeneration"":true,""returnImageBytes"":false,""returnRawGrokInXaiRequest"":false,""enableImageStreaming"":true,""imageGenerationCount"":2,""forceConcise"":false,""toolOverrides"":{},""enableSideBySide"":true,""isPreset"":false,""sendFinalMetadata"":true,""customInstructions"":"""",""deepsearchPreset"":"""",""isReasoning"":false}";
var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"); // Sets content-type automatically

var request = new HttpRequestMessage(HttpMethod.Post, url);
request.Headers.Add("host", "grok.com");
request.Headers.Add("connection", "keep-alive");
request.Headers.Add("sec-ch-ua", "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"");
request.Headers.Add("sec-ch-ua-mobile", "?0");
request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");
request.Headers.Add("accept", "*/*");
request.Headers.Add("origin", "https://grok.com");
request.Headers.Add("referer", "https://grok.com/?referrer=website");
request.Headers.Add("cookie", "sso-rw=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; sso=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; _ga=GA1.1.828449324.1740173126; _ga_8FEWB057YH=GS1.1.1740581925.26.1.1740582579.0.0.0");

// DO NOT add "content-type" here—it's handled by StringContent
request.Content = content;

try
{
    var response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();

    var responseBytes = await response.Content.ReadAsByteArrayAsync();
    Console.WriteLine($"Status Code: {(int)response.StatusCode}");
    Console.WriteLine($"Response Length: {responseBytes.Length} bytes");

    var responseText = Encoding.UTF8.GetString(responseBytes);
    Console.WriteLine($"Raw Text: {responseText}");

    var parsedMessage = GrokCS.ParseGrokResponse.ParseResponse(responseBytes);
    Console.WriteLine($"Parsed Message: {parsedMessage}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}