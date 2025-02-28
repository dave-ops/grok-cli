namespace GrokCLI.Decompressors
{
    public interface IDecompressor
    {
        Task<byte[]> DecompressAsync(byte[] compressedData);
        bool SupportsEncoding(string encoding);
    }
}