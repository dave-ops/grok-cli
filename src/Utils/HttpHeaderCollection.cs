using Microsoft.Extensions.Configuration;

namespace GrokCLI.Utils
{
    public static class HttpHeaderCollection
    {
        private static readonly Lazy<IReadOnlyDictionary<string, string>> BaseHeaders = new(() =>
        {
            var headers = ConfigurationHelper.GetSectionAsDictionary("HttpHeaders:BaseHeaders");
            return headers;
        });

        public static IReadOnlyDictionary<string, string> RateLimitHeaders => 
            BuildHeaders("RateLimitHeaders");

        public static IReadOnlyDictionary<string, string> GrokHeaders => 
            BuildHeaders("GrokHeaders");

        public static IReadOnlyDictionary<string, string> UploadHeaders => 
            BuildHeaders("UploadHeaders");

        private static IReadOnlyDictionary<string, string> BuildHeaders(string sectionName)
        {
            var specificHeaders = ConfigurationHelper.GetSectionAsDictionary($"HttpHeaders:{sectionName}");

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