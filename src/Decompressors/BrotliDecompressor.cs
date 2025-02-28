using System.IO.Compression;
using System.Threading.Tasks;

namespace GrokCLI.Decompressors
{
    public class BrotliDecompressor : IDecompressor
    {
        public async Task<byte[]> DecompressAsync(byte[] compressedData)
        {
            using (var memoryStream = new MemoryStream(compressedData))
            using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                await brotliStream.CopyToAsync(decompressedStream);
                return decompressedStream.ToArray();
            }
        }

        public bool SupportsEncoding(string encoding)
        {
            return encoding?.Contains("br", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}