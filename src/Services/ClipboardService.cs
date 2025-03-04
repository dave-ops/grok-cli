namespace GrokCLI.Services;

using System;
using System.Runtime.InteropServices;

public class ClipboardService : IService
    {
        public Execute(string contentType, string text)
        {
            if (!OpenClipboard(IntPtr.Zero))
                return (Constants.UNKNOWN, null);

            string contentType = Constants.UNKNOWN;
            string result = null;

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
                    contentType = "Bitmap";
                }
                else if (IsClipboardFormatAvailable(CF_HDROP))
                {
                    contentType = "File Drop";
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

        private const uint CF_TEXT = 1;
        private const uint CF_UNICODETEXT = 13;
        private const uint CF_DIB = 8;
        private const uint CF_HDROP = 15;
    }
}