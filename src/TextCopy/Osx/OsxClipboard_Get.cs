using System.Runtime.InteropServices;
using System.Threading.Tasks;

static partial class OsxClipboard
{
    public static Task<string> GetTextAsync()
    {
        return Task.FromResult(GetText());
    }

    public static string GetText()
    {
        var ptr = objc_msgSend(generalPasteboard, stringForTypeRegister, nsStringPboardType);
        var charArray = objc_msgSend(ptr, utf8Register);
        return Marshal.PtrToStringAnsi(charArray);
    }
}