namespace GrokCLI.Renderers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using GrokCLI.Helpers;

public class UploadResponseRenderer : IRenderer
{
    public async Task Render(string jsonInput)
    {
        Logger.Info(jsonInput);
        try
        {
            JObject obj = JObject.Parse(jsonInput);
            StringBuilder output = new StringBuilder();

            output.AppendLine("Upload Response:");
            output.AppendLine("----------------");
            output.AppendLine($"File Metadata ID: {obj["fileMetadataId"]?.ToString() ?? "N/A"}");
            output.AppendLine($"File Metadata: {obj["fileMetadata"]?.ToString() ?? "N/A"}");
            output.AppendLine($"File MIME Type: {obj["fileMimeType"]?.ToString() ?? "N/A"}");
            output.AppendLine($"File Name: {obj["fileName"]?.ToString() ?? "N/A"}");
            output.AppendLine($"File URI: {obj["fileUri"]?.ToString() ?? "N/A"}");
            output.AppendLine($"Parsed File URI: {obj["parsedFileUri"]?.ToString() ?? "N/A"}");
            output.AppendLine($"Create Time: {obj["createTime"]?.ToString() ?? "N/A"}");

            Console.WriteLine(output.ToString());
        }
        catch (JsonException ex)
        {
            Logger.Error($"Error parsing JSON: {ex.Message}");
        }
    }
}