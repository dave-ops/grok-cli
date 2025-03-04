using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using GrokCLI;

namespace GrokCLI.Commands
{
    public static class ClipboardHelper
    {
        public static (string contentType, string text) GetClipboardContentInfo()
        {
            string contentType = Constants.UNKNOWN;
            string? result = null;

            if (!OpenClipboard(IntPtr.Zero))
            {
                #pragma warning disable CS8619
                return (contentType, result);
            }

            try
            {
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
                    contentType = "Bitmap (DIB)";
                    result = "Clipboard contains a bitmap image (no specific format like JPG/PNG)";
                    // Optionally, process DIB data further (see below)
                }
                else if (IsClipboardFormatAvailable(CF_HDROP))
                {
                    (contentType, result) = GetFileDropInfo();
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

        private static (string contentType, string result) GetFileDropInfo()
        {
            IntPtr hClipboardData = GetClipboardData(CF_HDROP);
            if (hClipboardData == IntPtr.Zero) return (Constants.UNKNOWN, null);

            IntPtr ptr = GlobalLock(hClipboardData);
            if (ptr == IntPtr.Zero) return (Constants.UNKNOWN, null);

            try
            {
                // Get the number of files
                uint fileCount = DragQueryFile(hClipboardData, 0xFFFFFFFF, null, 0);
                if (fileCount == 0) return ("File Drop", "No files found in clipboard");

                // For simplicity, handle only the first file
                StringBuilder filePath = new StringBuilder(260); // MAX_PATH
                DragQueryFile(hClipboardData, 0, filePath, (uint)filePath.Capacity);

                string path = filePath.ToString();
                string extension = Path.GetExtension(path).ToLower();

                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                        return ("JPEG Image File", $"File: {path}");
                    case ".png":
                        return ("PNG Image File", $"File: {path}");
                    case ".gif":
                        return ("GIF Image File", $"File: {path}");
                    case ".bmp":
                        return ("BMP Image File", $"File: {path}");
                    default:
                        return ("File Drop", $"File: {path} (unsupported format)");
                }
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

        [DllImport("shell32.dll")]
        private static extern uint DragQueryFile(IntPtr hDrop, uint iFile, StringBuilder lpszFile, uint cch);

        // Clipboard format constants
        private const uint CF_TEXT = 1;         // ANSI text
        private const uint CF_UNICODETEXT = 13; // Unicode text
        private const uint CF_DIB = 8;          // Device-independent bitmap
        private const uint CF_HDROP = 15;       // File drop (list of filenames)
    }
}