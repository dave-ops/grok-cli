using System;
using System.Runtime.InteropServices;
using System.Text;
using GrokCLI.Services;

namespace GrokCLI.Commands
{
    public class ClipboardCommand : ICommand
    {
        public const string CommandName = "clip";

        public async Task Execute(string? parameter = null)
        {
            if (string.IsNullOrEmpty(parameter)) {
                return;
            }

            (string contentType, string text) = GetClipboardContentInfo();
            if (text != null)
            {
                Console.WriteLine($"Clipboard content type: {contentType}");
                Console.Write("what do you want to call your clip: ");
                string? name = Console.ReadLine();
                Console.WriteLine($"clipping to {name}");
                string prompt = $"{name}\n" +
                                        $"{Constants.MD_CODE_BRACKET}\n" +
                                        $"{GetClipboardUnicodeText()}\n" +
                                        $"{Constants.MD_CODE_BRACKET}\n" +
                                        "prompt:\n" +
                                        $"{Constants.MD_CODE_BRACKET}\n" +
                                        $"{parameter}\n" +
                                        $"{Constants.MD_CODE_BRACKET}\n";
                await new GrokService().Execute(prompt);
            }
            else
            {
                Console.WriteLine("No supported content found in clipboard or clipboard is not available.");
                await new GrokService().Execute(parameter);
            }
        }

        private static (string contentType, string text) GetClipboardContentInfo()
        {
            if (!OpenClipboard(IntPtr.Zero))
                return ("Unknown", null);

            string contentType = "Unknown";
            string result = null;

            try
            {
                // Check for common formats
                if (IsClipboardFormatAvailable(CF_UNICODETEXT))
                {
                    contentType = "Unicode Text";
                    result = GetClipboardUnicodeText();
                }
                else if (IsClipboardFormatAvailable(CF_TEXT))
                {
                    contentType = "ANSI Text";
                    result = GetClipboardAnsiText();
                }
                else if (IsClipboardFormatAvailable(CF_DIB))
                {
                    contentType = "Bitmap";
                    // Handle bitmap data if needed (not returning text)
                }
                else if (IsClipboardFormatAvailable(CF_HDROP))
                {
                    contentType = "File Drop";
                    // Handle file list if needed (not returning text)
                }
            }
            finally
            {
                CloseClipboard();
            }

            return (contentType, result);
        }

        private static string GetClipboardAnsiText()
        {
            IntPtr hClipboardData = GetClipboardData(CF_TEXT);
            if (hClipboardData == IntPtr.Zero) return null;

            IntPtr ptr = GlobalLock(hClipboardData);
            if (ptr == IntPtr.Zero) return null;

            try
            {
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                GlobalUnlock(ptr);
            }
        }

        private static string GetClipboardUnicodeText()
        {
            IntPtr hClipboardData = GetClipboardData(CF_UNICODETEXT);
            if (hClipboardData == IntPtr.Zero) return null;

            IntPtr ptr = GlobalLock(hClipboardData);
            if (ptr == IntPtr.Zero) return null;

            try
            {
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                GlobalUnlock(ptr);
            }
        }

        // Win32 API imports
        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        // Clipboard format constants
        private const uint CF_TEXT = 1;         // ANSI text
        private const uint CF_UNICODETEXT = 13; // Unicode text
        private const uint CF_DIB = 8;          // Device-independent bitmap
        private const uint CF_HDROP = 15;       // File drop (list of filenames)
    }
}