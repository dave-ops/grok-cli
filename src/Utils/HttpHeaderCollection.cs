// GrokCLI/Utilities/HttpHeaders.cs
using Microsoft.Extensions.Configuration;

namespace GrokCLI.Utils
{
    public static class HttpHeaderCollection
    {
        private static readonly IConfiguration _config;
        private static readonly IReadOnlyDictionary<string, string> BaseHeaders;

        static HttpHeaderCollection()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            BaseHeaders = _config.GetSection("HttpHeaders:BaseHeaders")
                .Get<Dictionary<string, string>>();
        }

        public static IReadOnlyDictionary<string, string> RateLimitHeaders => 
            BuildHeaders("RateLimitHeaders");

        public static IReadOnlyDictionary<string, string> GrokHeaders => 
            BuildHeaders("GrokHeaders");

        public static IReadOnlyDictionary<string, string> UploadHeaders => 
            BuildHeaders("UploadHeaders");

        private static IReadOnlyDictionary<string, string> BuildHeaders(string sectionName)
        {
            var specificHeaders = _config.GetSection($"HttpHeaders:{sectionName}")
                .Get<Dictionary<string, string>>();

            var combinedHeaders = new Dictionary<string, string>(BaseHeaders);

            foreach (var header in specificHeaders)
            {
                if (header.Key == "cookie-ga")
                {
                    combinedHeaders["cookie"] = $"{combinedHeaders["cookie"]}; {header.Value}";
                }
                else
                {
                    combinedHeaders[header.Key] = header.Value;
                }
            }

            return combinedHeaders;
        }
    }
}