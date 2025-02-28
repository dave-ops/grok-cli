// GrokCLI/Utilities/HttpHeaders.cs
using Microsoft.Extensions.Configuration;

namespace GrokCLI.Utils
{
    public static class HttpHeaderCollection
    {
        private static readonly Lazy<IReadOnlyDictionary<string, string>> BaseHeaders = new(() =>
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var headers = config.GetSection("HttpHeaders:BaseHeaders")
                .Get<Dictionary<string, string>>();

            return headers ?? new Dictionary<string, string>();
        });

        public static IReadOnlyDictionary<string, string> RateLimitHeaders => 
            BuildHeaders("RateLimitHeaders");

        public static IReadOnlyDictionary<string, string> GrokHeaders => 
            BuildHeaders("GrokHeaders");

        public static IReadOnlyDictionary<string, string> UploadHeaders => 
            BuildHeaders("UploadHeaders");

        private static IReadOnlyDictionary<string, string> BuildHeaders(string sectionName)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var specificHeaders = config.GetSection($"HttpHeaders:{sectionName}")
                .Get<Dictionary<string, string>>() ?? new Dictionary<string, string>();

            var combinedHeaders = new Dictionary<string, string>(BaseHeaders.Value);

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