namespace GrokCLI.Helpers;
public static class MimeHelper
{
    static public string GetMimeType(string extension)
    {
        switch (extension.ToLower())
        {
            case ".png":
                return "image/png";
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".pdf":
                return "application/pdf";
            case ".json": // Add this case for JSON files
                return "application/json";
            default:
                return "application/octet-stream";
        }
    }
}
