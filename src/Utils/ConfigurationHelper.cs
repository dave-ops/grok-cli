using Microsoft.Extensions.Configuration;

namespace GrokCLI.Utils
{
    public static class ConfigurationHelper
    {
        private static readonly Lazy<IConfiguration> _configuration = new(() =>
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        });

        /// <summary>
        /// Gets the configuration instance for the application.
        /// </summary>
        /// <returns>An IConfiguration instance initialized with appsettings.json.</returns>
        public static IConfiguration GetConfiguration()
        {
            return _configuration.Value;
        }

        /// <summary>
        /// Retrieves a section from the configuration as a dictionary of key-value pairs.
        /// </summary>
        /// <param name="sectionPath">The configuration section path (e.g., "HttpHeaders:BaseHeaders").</param>
        /// <returns>A read-only dictionary of the section's key-value pairs, or an empty dictionary if not found.</returns>
        public static IReadOnlyDictionary<string, string> GetSectionAsDictionary(string sectionPath)
        {
            var config = GetConfiguration();
            var section = config.GetSection(sectionPath).Get<Dictionary<string, string>>();
            return section ?? new Dictionary<string, string>();
        }
    }
}