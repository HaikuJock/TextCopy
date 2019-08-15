using System;
using System.Threading.Tasks;

static partial class OsxClipboard
{
    public static Task SetTextAsync(string text)
    {
        SetText(text);
        return Task.CompletedTask;
    }

    public static void SetText(string text)
    {
        IntPtr str = default;
        try
        {
            str = objc_msgSend(objc_msgSend(nsString, allocRegister), initWithUtf8Register, text);
            objc_msgSend(generalPasteboard, clearContentsRegister);
            objc_msgSend(generalPasteboard, setStringRegister, str, utfTextType);
        }
        finally
        {
            if (str != default)
            {
                objc_msgSend(str, sel_registerName("release"));
            }
        }
    }
}