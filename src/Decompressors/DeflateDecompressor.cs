sing System.IO.Compression;
using System.Threading.Tasks;

namespace GrokCLI.Decompressors
{
    public class DeflateDecompressor : IDecompressor
    {
        public async Task<byte[]> DecompressAsync(byte[] compressedData)
        {
            using (var memoryStream = new MemoryStream(compressedData))
            using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                await deflateStream.CopyToAsync(decompressedStream);
                return decompressedStream.ToArray();
            }
        }

        public bool SupportsEncoding(string encoding)
        {
            return encoding?.Contains("deflate", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}