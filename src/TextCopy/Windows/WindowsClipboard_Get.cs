using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

static partial class WindowsClipboard
{
    public static string GetText()
    {
        if (!IsClipboardFormatAvailable(cfUnicodeText))
        {
            return null;
        }

        try
        {
            OpenClipboard();
            return InnerGet();
        }
        finally
        {
            CloseClipboard();
        }
    }

    public static async Task<string> GetTextAsync()
    {
        if (!IsClipboardFormatAvailable(cfUnicodeText))
        {
            return null;
        }

        try
        {
            await OpenClipboardAsync();
            return InnerGet();
        }
        finally
        {
            CloseClipboard();
        }
    }

    static string InnerGet()
    {
        IntPtr handle = default;

        IntPtr pointer = default;
        try
        {
            handle = GetClipboardData(cfUnicodeText);
            if (handle == default)
            {
                return null;
            }

            pointer = GlobalLock(handle);
            if (pointer == default)
            {
                return null;
            }

            var size = GlobalSize(handle);
            var buff = new byte[size];

            Marshal.Copy(pointer, buff, 0, size);

            return Encoding.Unicode.GetString(buff).TrimEnd('\0');
        }
        finally
        {
            if (pointer != default)
            {
                GlobalUnlock(handle);
            }
        }
    }
}