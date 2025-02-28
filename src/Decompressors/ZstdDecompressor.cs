using System.Threading.Tasks;
using GrokCLI.Helpers;

namespace GrokCLI.Decompressors
{
    public class ZstdDecompressor : IDecompressor
    {
        public async Task<byte[]> DecompressAsync(byte[] compressedData)
        {
            // Zstandard is not natively supported in .NET, so log and return null
            Logger.Info("Zstandard (zstd) compression detected, but not supported in this code.");
            return await Task.FromResult<byte[]>(null); // Indicates unsupported compression
        }

        public bool SupportsEncoding(string encoding)
        {
            return encoding?.Contains("zstd", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}