using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace GrokCS
{
    public class GetRateLimit
    {
        public async Task<string> Execute() 
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://grok.com/rest/rate-limits");
                request.Headers.Add("host", "grok.com");
                request.Headers.Add("connection", "keep-alive");
                request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                request.Headers.Add("sec-ch-ua", "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"");
                request.Headers.Add("sec-ch-ua-mobile", "?0");
                request.Headers.Add("baggage", "sentry-environment=production,sentry-release=oVPBj5ez-07C9D12RX46Y,sentry-public_key=b311e0f2690c81f25e2c4cf6d4f7ce1c,sentry-trace_id=9bb3e1da066d4358a708b5757ae21902,sentry-sample_rate=1,sentry-sampled=true");
                request.Headers.Add("sentry-trace", "9bb3e1da066d4358a708b5757ae21902-8a007787523ceebd-1");
                request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");
                request.Headers.Add("dnt", "1");
                request.Headers.Add("accept", "*/*");
                request.Headers.Add("origin", "https://grok.com");
                request.Headers.Add("sec-fetch-site", "same-origin");
                request.Headers.Add("sec-fetch-mode", "cors");
                request.Headers.Add("sec-fetch-dest", "empty");
                request.Headers.Add("referer", "https://grok.com/chat/ededb21f-7f79-45c7-bbcb-efa48b0c1143?referrer=website");
                request.Headers.Add("accept-encoding", "gzip, deflate, br, zstd");
                request.Headers.Add("accept-language", "en-US,en;q=0.9");
                request.Headers.Add("cookie", "sso-rw=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; sso=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; _ga=GA1.1.828449324.1740173126; _ga_8FEWB057YH=GS1.1.1740624830.33.1.1740624862.0.0.0");
                request.Headers.Add("x-postman-captr", "8799857");
                
                var content = new StringContent("{\"requestKind\":\"DEFAULT\",\"modelName\":\"grok-latest\"}", null, "application/json");
                request.Content = content;
                // DO NOT add "content-type" here—it's handled by StringContent
                
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task Main(string[] args)
        {
            var rateLimit = new GetRateLimit();
            string result = await rateLimit.Execute(); // Await the async method
            Console.WriteLine(result);
        }
    }
}