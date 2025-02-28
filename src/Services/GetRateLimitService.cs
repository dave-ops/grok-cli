using System.Text;
using System.IO.Compression;
using GrokCLI.Helpers;
using GrokCLI.Utils;

namespace GrokCLI
{
    public class GetRateLimitService
    {
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

                    if (contentEncoding?.Contains("gzip", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        using (var memoryStream = new MemoryStream(responseBytes))
                        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                        using (var decompressedStream = new MemoryStream())
                        {
                            await gzipStream.CopyToAsync(decompressedStream);
                            responseBytes = decompressedStream.ToArray();
                        }
                    }
                    else if (contentEncoding?.Contains("deflate", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        using (var memoryStream = new MemoryStream(responseBytes))
                        using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                        using (var decompressedStream = new MemoryStream())
                        {
                            await deflateStream.CopyToAsync(decompressedStream);
                            responseBytes = decompressedStream.ToArray();
                        }
                    }
                    else if (contentEncoding?.Contains("br", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        using (var memoryStream = new MemoryStream(responseBytes))
                        using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                        using (var decompressedStream = new MemoryStream())
                        {
                            await brotliStream.CopyToAsync(decompressedStream);
                            responseBytes = decompressedStream.ToArray();
                            Logger.Info(responseBytes.Length.ToString() + "");
                        }
                    }
                    else if (contentEncoding?.Contains("zstd", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Logger.Info("Zstandard (zstd) compression detected, but not supported in this code.");
                        return "Unsupported compression: zstd";
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