using Microsoft.Extensions.Configuration;

namespace GrokCLI.Utils
{
    public static class Grok
    {
        public static string GetVersion()
        {
            var config = ConfigurationHelper.GetConfiguration();
            return config.GetValue<string>("Grok:version") ?? "grok-latest";
        }

        public static string GetDefaultUserPrompt()
        {
            var config = ConfigurationHelper.GetConfiguration();
            return config.GetValue<string>("Grok:DefaultUserPrompt") ?? "say your name";
        }

        public static string GetConversationUri()
        {
            var config = ConfigurationHelper.GetConfiguration();
            return config.GetValue<string>("Grok:Conversation:Uri") ?? "https://grok.com/rest/app-chat/conversations/new";
        }


        public static IReadOnlyDictionary<string, string> GetLoggingSettings()
        {
            return ConfigurationHelper.GetSectionAsDictionary("Grok");
        }
    }
}