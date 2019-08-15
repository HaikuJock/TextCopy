using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

static class ProcessExtensions
{
    public static void BeginRead(this Process process)
    {
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
    }

    public static TaskCompletionSource<string> SetErrorData(this Process process)
    {
        var builder = new StringBuilder();
        var source = new TaskCompletionSource<string>();
        process.ErrorDataReceived += SetOrAppend(source, builder);
        return source;
    }

    public static TaskCompletionSource<string> SetOutputData(this Process process)
    {
        var builder = new StringBuilder();
        var source = new TaskCompletionSource<string>();
        process.OutputDataReceived += SetOrAppend(source, builder);
        return source;
    }

    static DataReceivedEventHandler SetOrAppend(TaskCompletionSource<string> source, StringBuilder builder)
    {
        return (sender, args) =>
        {
            if (args.Data == null)
            {
                source.SetResult(builder.ToString());
            }
            else
            {
                builder.AppendLine(args.Data);
            }
        };
    }
}