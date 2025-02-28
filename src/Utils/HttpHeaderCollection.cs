// GrokCLI/Utilities/HttpHeaders.cs
namespace GrokCLI.Utils
{
    public static class HttpHeaderCollection
    {
        // Base headers common to all requests
        private static readonly IReadOnlyDictionary<string, string> BaseHeaders = 
            new Dictionary<string, string>
            {
                { "host", "grok.com" },
                { "connection", "keep-alive" },
                { "sec-ch-ua", "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"" },
                { "sec-ch-ua-mobile", "?0" },
                { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36" },
                { "accept", "*/*" },
                { "origin", "https://grok.com" },
                { "cookie", "sso-rw=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; sso=eyJhbGciOiJIUzI1NiJ9.eyJzZXNzaW9uX2lkIjoiZDgyYzRiNDItYzNjNC00ZGU4LTkyOWUtNjk4MDM1ZThkYTU4In0.Q8ntpCHdYPauieIsaXufN1bRn1IUzyWUTLmLOOpW3Lc; _ga=GA1.1.828449324.1740173126" }
            };

        public static readonly IReadOnlyDictionary<string, string> RateLimitHeaders = 
            new Dictionary<string, string>(BaseHeaders)
            {
                { "sec-ch-ua-platform", "\"Windows\"" },
                { "baggage", "sentry-environment=production,sentry-release=oVPBj5ez-07C9D12RX46Y,sentry-public_key=b311e0f2690c81f25e2c4cf6d4f7ce1c,sentry-trace_id=9bb3e1da066d4358a708b5757ae21902,sentry-sample_rate=1,sentry-sampled=true" },
                { "sentry-trace", "9bb3e1da066d4358a708b5757ae21902-8a007787523ceebd-1" },
                { "dnt", "1" },
                { "sec-fetch-site", "same-origin" },
                { "sec-fetch-mode", "cors" },
                { "sec-fetch-dest", "empty" },
                { "referer", "https://grok.com/chat/ededb21f-7f79-45c7-bbcb-efa48b0c1143?referrer=website" },
                { "accept-encoding", "gzip, deflate, br, zstd" },
                { "accept-language", "en-US,en;q=0.9" },
                { "cookie", BaseHeaders["cookie"] + "; _ga_8FEWB057YH=GS1.1.1740624830.33.1.1740624862.0.0.0" }
            };

        public static readonly IReadOnlyDictionary<string, string> GrokHeaders = 
            new Dictionary<string, string>(BaseHeaders)
            {
                { "referer", "https://grok.com/?referrer=website" },
                { "cookie", BaseHeaders["cookie"] + "; _ga_8FEWB057YH=GS1.1.1740581925.26.1.1740582579.0.0.0" }
            };

        public static readonly IReadOnlyDictionary<string, string> UploadHeaders = 
            new Dictionary<string, string>(BaseHeaders)
            {
                { "sec-ch-ua-platform", "\"Windows\"" },
                { "baggage", "sentry-environment=production,sentry-release=hPZj-aOLuqQ3QUlWq1JLg,sentry-public_key=b311e0f2690c81f25e2c4cf6d4f7ce1c,sentry-trace_id=bb94ef0929f8420e9bbbff532f4307fe,sentry-sample_rate=1,sentry-sampled=true" },
                { "sentry-trace", "bb94ef0929f8420e9bbbff532f4307fe-bfcd1ea726cb36a3-1" },
                { "dnt", "1" },
                { "sec-fetch-site", "same-origin" },
                { "sec-fetch-mode", "cors" },
                { "sec-fetch-dest", "empty" },
                { "referer", "https://grok.com/chat/f429b6cc-3c5a-4994-a5e0-db7bcd53d0af?referrer=website" },
                { "accept-encoding", "gzip, deflate, br, zstd" },
                { "accept-language", "en-US,en;q=0.9" },
                { "cookie", BaseHeaders["cookie"] + "; _ga_8FEWB057YH=GS1.1.1740664903.36.1.1740667676.0.0.0" }
            };
    }
}