using System.Threading.Tasks;
using TextCopy;
using Xunit;
using Xunit.Abstractions;

public class ClipboardTests :
    XunitLoggingBase
{
    [Fact]
    public void Simple()
    {
        Verify("Foo");
        Verify("🅢");
    }

    static void Verify(string expected)
    {
        Clipboard.SetText(expected);

        var actual = Clipboard.GetText();
        Assert.Equal(expected, actual);
    }
    [Fact]
    public async Task Async()
    {
        await VerifyAsync("Foo");
        await VerifyAsync("🅢");
    }

    static async Task VerifyAsync(string expected)
    {
        await Clipboard.SetTextAsync(expected);

        var actual = await Clipboard.GetTextAsync();
        Assert.Equal(expected, actual);
    }

    public ClipboardTests(ITestOutputHelper output) :
        base(output)
    {
    }
}