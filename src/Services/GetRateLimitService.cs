using System.Text;
using System.Net.Http;
using GrokCLI.Helpers;
using GrokCLI.Utils;
using GrokCLI.Decompressors;

namespace GrokCLI
{
    public class GetRateLimitService
    {
        private readonly IDecompressor[] _decompressors;

        public GetRateLimitService()
        {
            _decompressors = new IDecompressor[]
            {
                new GzipDecompressor(),
                new DeflateDecompressor(),
                new BrotliDecompressor(),
                new ZstdDecompressor()
            };
        }

        public async Task<string> Execute()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://grok.com/rest/rate-limits");
                foreach (var header in HttpHeaderCollection.RateLimitHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                var content = new StringContent("{\"requestKind\":\"DEFAULT\",\"modelName\":\"grok-latest\"}", null, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Log response headers to understand encoding and content type
                Logger.Info("Content-Type: " + response.Content.Headers.ContentType);
                Logger.Info("Content-Encoding: " + (response.Content.Headers.ContentEncoding?.FirstOrDefault() ?? "none"));

                // Read the raw response as a byte array
                byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();

                // Handle decompression if the response is compressed
                if (response.Content.Headers.ContentEncoding != null && response.Content.Headers.ContentEncoding.Any())
                {
                    string? contentEncoding = response.Content.Headers.ContentEncoding.FirstOrDefault();
                    Logger.Info($"Decompressing response with encoding: {contentEncoding}");

                    var decompressor = _decompressors.FirstOrDefault(d => d.SupportsEncoding(contentEncoding));
                    if (decompressor != null)
                    {
                        responseBytes = await decompressor.DecompressAsync(responseBytes);
                        if (responseBytes == null) // Handle unsupported compression (e.g., zstd)
                        {
                            return $"Unsupported compression: {contentEncoding}";
                        }
                    }
                    else
                    {
                        Logger.Info($"No decompressor found for encoding: {contentEncoding}");
                        return $"Unknown compression: {contentEncoding}";
                    }
                }

                // Convert the decompressed bytes to a string
                string resultString = Encoding.UTF8.GetString(responseBytes);
                Logger.Output(resultString);
                return resultString;
            }
        }
    }
}