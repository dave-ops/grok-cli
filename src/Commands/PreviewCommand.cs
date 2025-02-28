using GrokCLI.Helpers;

namespace GrokCLI;

public static class PreviewCommand
{
    public static async Task Execute()
    {
        string imageUrl = "https://assets.grok.com/users/fa5c83b9-b2c1-4bbc-8d3a-81f4c18f2d9b/67648c26-51a8-4051-97cc-a390250ce503/preview-image";
        PreviewImage preview = new PreviewImage(imageUrl);
        await preview.LoadImageAsync();
        
        if (preview.ImageData != null)
        {
            Logger.Info($"Image downloaded successfully. {preview.ImageData.Length} bytes. ContentType: {preview.ContentType}");
        }
        else
        {
            Logger.Info("Failed to download image.");
        }
    }
}