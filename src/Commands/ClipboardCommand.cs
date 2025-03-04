using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GrokCLI.Commands;

public class ClipboardCommand : ICommand
{
    public const string CommandName = "clip";

    // Synchronous method since GetClipboardText() is not async
    public Task Execute(string? parameter = null)
    {
        string clipboardText = GetClipboardText();
        if (clipboardText != null)
        {
            Console.WriteLine("Clipboard contents: " + clipboardText);
        }
        else
        {
            Console.WriteLine("No text found in clipboard or clipboard is not available.");
        }

        return Task.CompletedTask; // Return a completed task for interface compatibility
    }

    private static string GetClipboardText()
    {
        if (!IsClipboardFormatAvailable(CF_TEXT))
            return null;

        if (!OpenClipboard(IntPtr.Zero))
            return null;

        string result = null;
        try
        {
            IntPtr hClipboardData = GetClipboardData(CF_TEXT);
            if (hClipboardData != IntPtr.Zero)
            {
                IntPtr ptr = GlobalLock(hClipboardData);
                if (ptr != IntPtr.Zero)
                {
                    try
                    {
                        string text = Marshal.PtrToStringAnsi(ptr);
                        result = text;
                    }
                    finally
                    {
                        GlobalUnlock(ptr);
                    }
                }
            }
        }
        finally
        {
            CloseClipboard();
        }

        return result;
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

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalFree(IntPtr hMem);

    private const uint CF_TEXT = 1; // Text format
}