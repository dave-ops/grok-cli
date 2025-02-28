namespace GrokCLI.Models
{
    public class Result
    {
        public Conversation? conversation { get; set; }
        public ResponseData? response { get; set; }
        public TokenResponse? token { get; set; }
        public FinalMetadata? finalMetadata { get; set; }
        public ModelResponse? modelResponse { get; set; }
        public NewTitle? title { get; set; }
    }
}