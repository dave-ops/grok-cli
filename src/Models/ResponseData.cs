using GrokCLI.Models;

namespace GrokCS.Models {

    public class ResponseData
    {
        public UserResponse? userResponse { get; set; }
        public bool isThinking { get; set; }
        public bool isSoftStop { get; set; }
        public string? responseId { get; set; }
    }
}
