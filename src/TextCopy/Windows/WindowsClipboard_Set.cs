using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

static partial class WindowsClipboard
{
    public static void SetText(string text)
    {
        OpenClipboard();
        try
        {
            InnerSetText(text);
        }
        finally
        {
            CloseClipboard();
        }
    }
    public static async Task SetTextAsync(string text)
    {
        await OpenClipboardAsync();
        try
        {
            InnerSetText(text);
        }
        finally
        {
            CloseClipboard();
        }
    }
    static void InnerSetText(string text)
    {
        EmptyClipboard();
        IntPtr hGlobal = default;
        try
        {
            var bytes = (text.Length + 1) * 2;
            hGlobal = Marshal.AllocHGlobal(bytes);

            if (hGlobal == default)
            {
                ThrowWin32();
            }

            var target = GlobalLock(hGlobal);

            if (target == default)
            {
                ThrowWin32();
            }

            try
            {
                Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
            }
            finally
            {
                GlobalUnlock(target);
            }

            if (SetClipboardData(cfUnicodeText, hGlobal) == default)
            {
                ThrowWin32();
            }

            hGlobal = default;
        }
        finally
        {
            if (hGlobal != default)
            {
                Marshal.FreeHGlobal(hGlobal);
            }
        }
    }

}