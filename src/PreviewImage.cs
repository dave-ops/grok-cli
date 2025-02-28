using GrokCLI.Helpers;

namespace GrokCLI
{
    public class PreviewImage(string imageUrl)
    {
        public string ImageUrl { get; private set; } = imageUrl;
        public byte[]? ImageData { get; private set; }
        public string? ContentType { get; private set; }
        private readonly HttpClient _httpClient = new HttpClient();

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
                Logger.Info($"Error loading image: {ex.Message}");
                ImageData = null;
            }
            catch (Exception ex)
            {
                // Handle other potential errors
                Logger.Info($"An unexpected error occurred: {ex.Message}");
                ImageData = null;
            }
        }
    }
}