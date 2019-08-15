#if (NETSTANDARD)
using System;
using System.Runtime.InteropServices;
#endif

namespace TextCopy
{
    /// <summary>
    /// Provides methods to place text on and retrieve text from the system Clipboard.
    /// </summary>
    public static partial class Clipboard
    {
        static Clipboard()
        {
#if (NETSTANDARD)
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                getFunc = WindowsClipboard.GetText;
                setAction = WindowsClipboard.SetText;
                getFuncAsync = WindowsClipboard.GetTextAsync;
                setActionAsync = WindowsClipboard.SetTextAsync;
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                getFunc = OsxClipboard.GetText;
                setAction = OsxClipboard.SetText;
                getFuncAsync = OsxClipboard.GetTextAsync;
                setActionAsync = OsxClipboard.SetTextAsync;
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                getFunc = LinuxClipboard.GetText;
                setAction = LinuxClipboard.SetText;
                getFuncAsync = LinuxClipboard.GetTextAsync;
                setActionAsync = LinuxClipboard.SetTextAsync;
                return;
            }
            throw new NotSupportedException();
#else
            getFunc = WindowsClipboard.GetText;
            setAction = WindowsClipboard.SetText;
            getFuncAsync = WindowsClipboard.GetTextAsync;
            setActionAsync = WindowsClipboard.SetTextAsync;
#endif
        }
    }
}