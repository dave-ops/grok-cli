using GrokCLI.Helpers;

namespace GrokCLI {

    public static class FileHelper
    {
        /// <summary>
        /// Converts a file path string to a FileInfo object.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>A FileInfo object representing the file, or null if the file path is invalid or the file does not exist.</returns>
        /// <exception cref="ArgumentException">Thrown when the file path is null, empty, or whitespace.</exception>
        public static FileInfo? GetFileInfo(string filePath)
        {
            // Validate the file path
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null, empty, or whitespace.", nameof(filePath));
            }

            // Check if the file exists before creating FileInfo
            if (!File.Exists(filePath))
            {
                Logger.Info($"Warning: File does not exist at path: {filePath}");
                return null; 
            }

            try
            {
                // Create and return a FileInfo object
                return new FileInfo(filePath);
            }
            catch (Exception ex)
            {
                //Catch any unhandled exceptions
                Logger.Info($"Error getting FileInfo for path {filePath}: {ex.Message}");
                return null;
            }
        }
    }
}