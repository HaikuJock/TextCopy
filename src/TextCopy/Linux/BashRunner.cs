using System.Threading;
using System.Threading.Tasks;

static class BashRunner
{
    public static string Run(string commandLine)
    {
        return ProcessRunner.Run("bash", $"-c \"{commandLine}\"");
    }

    public static Task<string> RunAsync(string commandLine)
    {
        return ProcessRunner.RunAsync("bash", $"-c \"{commandLine}\"", CancellationToken.None);
    }
}