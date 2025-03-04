using Microsoft.Extensions.Configuration;

namespace GrokCLI.Utils
{
    public static class Kobold
    {
        public static string GetUri()
        {
            var config = ConfigurationHelper.GetConfiguration();
            return config.GetValue<string>("Kobold:Uri") ?? "http://localhost:3000/api";
        }

        public static IReadOnlyDictionary<string, string> GetLoggingSettings()
        {
            return ConfigurationHelper.GetSectionAsDictionary("Kobold");
        }
    }
}