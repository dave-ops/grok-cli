namespace GrokCLI.Renderers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

public class RateLimitResponseRenderer : IRenderer
{
    public async Task Render(string jsonInput)
    {
        try
        {
            JObject obj = JObject.Parse(jsonInput);
            StringBuilder output = new StringBuilder();

            output.AppendLine("Rate Limit Status:");
            output.AppendLine("------------------");
            output.AppendLine($"Window Size (seconds): {obj["windowSizeSeconds"]?.ToString() ?? "N/A"}");
            output.AppendLine($"Remaining Queries: {obj["remainingQueries"]?.ToString() ?? "N/A"}");
            output.AppendLine($"Total Queries: {obj["totalQueries"]?.ToString() ?? "N/A"}");

            Console.WriteLine(output.ToString());
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
        }
    }
}