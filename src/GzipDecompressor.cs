using System.IO.Compression;
using System.Threading.Tasks;

namespace GrokCLI.Decompressors
{
    public class GzipDecompressor : IDecompressor
    {
        public async Task<byte[]> DecompressAsync(byte[] compressedData)
        {
            using (var memoryStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                await gzipStream.CopyToAsync(decompressedStream);
                return decompressedStream.ToArray();
            }
        }

        public bool SupportsEncoding(string encoding)
        {
            return encoding?.Contains("gzip", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}