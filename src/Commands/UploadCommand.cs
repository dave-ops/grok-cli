namespace GrokCLI;

public static class UploadCommand
{
    public static async Task Execute(string? parameter)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            Console.WriteLine("Error: Please provide a filepath for upload (e.g., grok upload C:\\temp\\screenshot.png)");
            return;
        }

        FileInfo? fileInfo = FileHelper.GetFileInfo(parameter);
        if (fileInfo != null)
        {
            _ = await new Upload().Execute(fileInfo);
        }
        else
        {
            Console.WriteLine($"File not found: {parameter}");
        }
    }
}