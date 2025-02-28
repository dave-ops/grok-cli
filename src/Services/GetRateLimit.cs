using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Net.Http.Headers;
using GrokCLI.Utils;

namespace GrokCLI
{
    public class GetRateLimit
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
                Console.WriteLine("Content-Type: " + response.Content.Headers.ContentType);
                Console.WriteLine("Content-Encoding: " + (response.Content.Headers.ContentEncoding?.FirstOrDefault() ?? "none"));

                // Read the raw response as a byte array
                byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();

                // Handle decompression if the response is compressed
                if (response.Content.Headers.ContentEncoding != null && response.Content.Headers.ContentEncoding.Any())
                {
                    string? contentEncoding = response.Content.Headers.ContentEncoding.FirstOrDefault();
                    Console.WriteLine($"Decompressing response with encoding: {contentEncoding}");

                    if (contentEncoding?.Contains("gzip", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        using (var memoryStream = new MemoryStream(responseBytes))
                        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                        using (var decompressedStream = new MemoryStream())
                        {
                            gzipStream.CopyTo(decompressedStream);
                            responseBytes = decompressedStream.ToArray();
                        }
                    }
                    else if (contentEncoding?.Contains("deflate", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        using (var memoryStream = new MemoryStream(responseBytes))
                        using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                        using (var decompressedStream = new MemoryStream())
                        {
                            deflateStream.CopyTo(decompressedStream);
                            responseBytes = decompressedStream.ToArray();
                        }
                    }
                    else if (contentEncoding?.Contains("br", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        // Note: Brotli requires the System.IO.Compression.Brotli NuGet package
                        // Install it via: dotnet add package System.IO.Compression.Brotli
                        using (var memoryStream = new MemoryStream(responseBytes))
                        using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                        using (var decompressedStream = new MemoryStream())
                        {
                            brotliStream.CopyTo(decompressedStream);
                            responseBytes = decompressedStream.ToArray();
                        }
                    }
                    else if (contentEncoding?.Contains("zstd", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Zstandard (zstd) compression detected, but not supported in this code.");
                        return "Unsupported compression: zstd";
                    }
                }

                // Convert the decompressed bytes to a string
                string resultString = Encoding.UTF8.GetString(responseBytes);
                return resultString;
            }
        }
    }
}