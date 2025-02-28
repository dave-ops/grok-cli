using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrokCLI
{
    public class PreviewImage
    {
        public string ImageUrl { get; private set; }
        public byte[] ImageData { get; private set; }
        public string ContentType { get; private set; }
        private readonly HttpClient _httpClient;

        public PreviewImage(string imageUrl)
        {
            ImageUrl = imageUrl;
            _httpClient = new HttpClient();
        }
        public async Task LoadImageAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(ImageUrl);
                response.EnsureSuccessStatusCode();

                ContentType = response.Content.Headers.ContentType?.MediaType;
                ImageData = await response.Content.ReadAsByteArrayAsync();
            }
            catch (HttpRequestException ex)
            {
                // Handle network or HTTP request errors
                Console.WriteLine($"Error loading image: {ex.Message}");
                ImageData = null;
            }
            catch (Exception ex)
            {
                // Handle other potential errors
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                ImageData = null;
            }
        }

         public static async Task Main(string[] args)
        {
            string imageUrl = "https://assets.grok.com/users/fa5c83b9-b2c1-4bbc-8d3a-81f4c18f2d9b/67648c26-51a8-4051-97cc-a390250ce503/preview-image";
            PreviewImage preview = new PreviewImage(imageUrl);

            await preview.LoadImageAsync();

            if (preview.ImageData != null)
            {
                Console.WriteLine($"Image downloaded successfully. {preview.ImageData.Length} bytes. ContentType: {preview.ContentType}");
            }
            else
            {
                Console.WriteLine("Failed to download image.");
            }
        }
    }
}