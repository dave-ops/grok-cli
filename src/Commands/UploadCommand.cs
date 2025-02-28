using System.IO;
using GrokCLI.Helpers;
using GrokCLI.Services;
using GrokCLI.Utils;

namespace GrokCLI.Commands;
public class UploadCommand : ICommand
{
    public const string CommandName = "upload";

    public async Task Execute(string? parameter = null)
    {
        if (parameter != null)
        {
            FileInfo file = new FileInfo(parameter);
            if (file.Exists)
            {
                Logger.Info($"{CommandName} command executing with file: {file.Name}");
                await new UploadService().Execute(file);
            }
            else
            {
                Logger.Error($"File not found: {parameter}");
            }
        }
        else
        {
            Logger.Error("No file specified for upload");
        }
    }
}