using System;
using GrokCLI.Services;

namespace GrokCLI.Commands
{
    public class ClipboardCommand : ICommand
    {
        public const string CommandName = "clip";
        private readonly IClipboardService _clipboardService;

        public ClipboardCommand(IClipboardService clipboardService = null)
        {
            _clipboardService = clipboardService ?? new ClipboardService();
        }

        public async Task Execute(string? parameter = null)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                return;
            }

            (string contentType, string text) = _clipboardService.GetClipboardContentInfo();
            if (text != null)
            {
                Console.WriteLine($"Clipboard content type: {contentType}");
                Console.Write("what do you want to call your clip: ");
                string? name = Console.ReadLine();
                string prompt = $"{name}\n" +
                              $"{Constants.MD_CODE_BRACKET} " +
                              $"{text}" +
                              $"{Constants.MD_CODE_BRACKET} " +
                              "prompt: " +
                              $"{Constants.MD_CODE_BRACKET}" +
                              $"{parameter}" +
                              $"{Constants.MD_CODE_BRACKET}";
                string escaped = StringUtils.EscapeJsonString(prompt);
                await new GrokService().Execute(escaped);
            }
            else
            {
                Console.WriteLine("No supported content found in clipboard or clipboard is not available.");
                await new GrokService().Execute(parameter);
            }
        }
    }
}