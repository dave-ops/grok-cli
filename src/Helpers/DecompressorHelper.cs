using System.IO.Compression;
using System.Text;
using GrokCLI.Helpers;

namespace GrokCLI
{
    public static class DecompressionHelper // You can name this class differently or move it to src/Decompressors
    {
        public static byte[] DecompressResponse(byte[] responseBytes, string? contentEncoding)
        {
            if (string.IsNullOrEmpty(contentEncoding))
            {
                return responseBytes; // No decompression needed
            }

            Logger.Info($"Decompressing response with encoding: {contentEncoding}");

            if (contentEncoding.Contains("gzip", StringComparison.OrdinalIgnoreCase))
            {
                using (var memoryStream = new MemoryStream(responseBytes))
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                using (var decompressedStream = new MemoryStream())
                {
                    gzipStream.CopyTo(decompressedStream);
                    return decompressedStream.ToArray();
                }
            }
            else if (contentEncoding.Contains("deflate", StringComparison.OrdinalIgnoreCase))
            {
                using (var memoryStream = new MemoryStream(responseBytes))
                using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                using (var decompressedStream = new MemoryStream())
                {
                    deflateStream.CopyTo(decompressedStream);
                    return decompressedStream.ToArray();
                }
            }
            else if (contentEncoding.Contains("br", StringComparison.OrdinalIgnoreCase)) // Brotli
            {
                using (var memoryStream = new MemoryStream(responseBytes))
                using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                using (var decompressedStream = new MemoryStream())
                {
                    brotliStream.CopyTo(decompressedStream);
                    byte[] decompressedBytes = decompressedStream.ToArray();
                    Logger.Info(decompressedBytes.Length.ToString() + "");
                    return decompressedBytes;
                }
            }
            else if (contentEncoding.Contains("zstd", StringComparison.OrdinalIgnoreCase))
            {
                Logger.Info("Zstandard (zstd) compression detected, but not supported in this code.");
                throw new NotSupportedException("Unsupported compression: zstd");
            }

            return responseBytes; // Return original bytes if no matching encoding is found
        }
    }
}