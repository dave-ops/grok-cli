namespace GrokCLI.Models
{
    public class TokenResponse
    {
        public string? token { get; set; }
        public bool isThinking { get; set; }
        public bool isSoftStop { get; set; }
        public string? responseId { get; set; }
    }
}