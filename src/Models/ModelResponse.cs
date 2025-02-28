namespace GrokCLI.Models
{
    public class ModelResponse
    {
        public string? responseId { get; set; }
        public string? message { get; set; }
        public string? sender { get; set; }
        public DateTime createTime { get; set; }
        public string? parentResponseId { get; set; }
        public bool manual { get; set; }
        public bool partial { get; set; }
        public bool shared { get; set; }
        public string? query { get; set; }
        public string? queryType { get; set; }
        public object[]? webSearchResults { get; set; }
        public string[]? xpostIds { get; set; }
        public object[]? xposts { get; set; }
        public string[]? generatedImageUrls { get; set; }
        public object[]? imageAttachments { get; set; }
        public object[]? fileAttachments { get; set; }
        public object[]? cardAttachmentsJson { get; set; }
        public string[]? fileUris { get; set; }
        public object[]? fileAttachmentsMetadata { get; set; }
        public bool isControl { get; set; }
        public object[]? steps { get; set; }
        public string[]? mediaTypes { get; set; }
    }

}