using Microsoft.Extensions.Configuration;

namespace GrokCLI.Utils
{
    public static class LoggingHelper
    {
        public static string GetLogLevel()
        {
            var config = ConfigurationHelper.GetConfiguration();
            return config.GetValue<string>("Logging:Level") ?? "Information";
        }

        public static IReadOnlyDictionary<string, string> GetLoggingSettings()
        {
            return ConfigurationHelper.GetSectionAsDictionary("Logging");
        }
    }
}