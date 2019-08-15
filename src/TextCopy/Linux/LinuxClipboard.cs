using System.IO;
using System.Threading.Tasks;

static class LinuxClipboard
{
    public static void SetText(string text)
    {
        var file = Path.GetTempFileName();
        File.WriteAllText(file, text);
        try
        {
            BashRunner.Run($"cat {file} | xclip -i -selection clipboard");
        }
        finally
        {
            File.Delete(file);
        }
    }

    public static async Task SetTextAsync(string text)
    {
        var file = Path.GetTempFileName();
        File.WriteAllText(file, text);
        try
        {
            await BashRunner.RunAsync($"cat {file} | xclip -i -selection clipboard");
        }
        finally
        {
            File.Delete(file);
        }
    }

    public static string GetText()
    {
        var file = Path.GetTempFileName();
        try
        {
            BashRunner.Run($"xclip -o -selection clipboard > {file}");
            return File.ReadAllText(file);
        }
        finally
        {
            File.Delete(file);
        }
    }

    public static async Task<string> GetTextAsync()
    {
        var file = Path.GetTempFileName();
        try
        {
            await BashRunner.RunAsync($"xclip -o -selection clipboard > {file}");
            return File.ReadAllText(file);
        }
        finally
        {
            File.Delete(file);
        }
    }
}