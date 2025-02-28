using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class Upload
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> Execute(FileInfo file)
    {

        Console.WriteLine("executing...");

        // Read file content and convert to base64
        byte[] fileBytes = await File.ReadAllBytesAsync(file.FullName);
        string base64Content = Convert.ToBase64String(fileBytes);

        // Create JSON payload
        var jsonPayload = new
        {
            fileName = file.Name,
            fileMimeType = GetMimeType(file.Extension),
            content = base64Content
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

        // Add headers from the working curl command
        request.Headers.Add("host", "grok.com");
        request.Headers.Add("connection", "keep-alive");
        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
        request.Headers.Add("sec-ch-ua", "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"");
        request.Headers.Add("sec-ch-ua-mobile", "?0");
        request.Headers.Add("baggage", "sentry-environment=production,sentry-release=hPZj-aOLuqQ3QUlWq1JLg,sentry-public_key=b311e0f2690c81f25e2c4cf6d4f7ce1c,sentry-trace_id=bb94ef0929f8420e9bbbff532f4307fe,sentry-sample_rate=1,sentry-sampled=true");
        request.Headers.Add("sentry-trace", "bb94ef0929f8420e9bbbff532f4307fe-bfcd1ea726cb36a3-1");
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");
        request.Headers.Add("dnt", "1");
        request.Headers.Add("accept", "*/*");
        request.Headers.Add("origin", "https://grok.com");
        request.Headers.Add("sec-fetch-site", "same-origin");
        request.Headers.Add("sec-fetch-mode", "cors");
        request.Headers.Add("sec-fetch-dest", "empty");
        request.Headers.Add("referer", "https://grok.com/chat/f429b6cc-3c5a-4994-a5e0-db7bcd53d0af?referrer=website");
        request.Headers.Add("accept-encoding", "gzip, deflate, br, zstd");
        request.Headers.Add("accept-language", "en-US,en;q=0.9");
        request.Headers.Add("cookie", "sso-rw=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; sso=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; _ga=GA1.1.828449324.1740173126; _ga_8FEWB057YH=GS1.1.1740664903.36.1.1740667676.0.0.0");
        request.Headers.Add("x-postman-captr", "4020342");

        // Send the request and get response
        Console.WriteLine("sending...");
        HttpResponseMessage response = await client.SendAsync(request);
        
        // Check if the response is successful before calling EnsureSuccessStatusCode
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: HTTP {response.StatusCode} - {errorContent}");
            return $"Error: HTTP {response.StatusCode} - {errorContent}";
        }

        response.EnsureSuccessStatusCode();
        Console.WriteLine("sent.");
        Console.WriteLine("receiving....");
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("received.");
        return responseBody;
    }

    private string GetMimeType(string extension)
    {
        switch (extension.ToLower())
        {
            case ".png":
                return "image/png";
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".pdf":
                return "application/pdf";
            default:
                return "application/octet-stream";
        }
    }
}