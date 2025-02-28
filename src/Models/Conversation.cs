using System;
using System.Text.Json;
using System.Text;

namespace GrokCLI.Models
{
public class Conversation
    {
        public string? conversationId { get; set; }
        public string? title { get; set; }
        public bool starred { get; set; }
        public DateTime createTime { get; set; }
        public DateTime modifyTime { get; set; }
        public string? systemPromptName { get; set; }
        public bool temporary { get; set; }
        public string?[]? mediaTypes { get; set; }
    }
}
