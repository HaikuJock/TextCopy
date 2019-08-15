using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

static class ProcessRunner
{
    public static string Run(string fileName, string arguments)
    {
        var errorBuilder = new StringBuilder();
        var outputBuilder = new StringBuilder();
        var startInfo = BuildInfo(fileName, arguments);
        using (var process = new Process
        {
            StartInfo = startInfo
        })
        {
            process.Start();
            process.OutputDataReceived += (sender, args) => { outputBuilder.AppendLine(args.Data); };
            process.ErrorDataReceived += (sender, args) => { errorBuilder.AppendLine(args.Data); };
            process.BeginRead();
            if (!process.WaitForExit(500))
            {
                throw BuildException("Process timed out", startInfo, outputBuilder.ToString(), errorBuilder.ToString());
            }

            if (process.ExitCode == 0)
            {
                return outputBuilder.ToString();
            }

            throw BuildException("Could not execute process", startInfo, outputBuilder.ToString(), errorBuilder.ToString());
        }
    }

    public static Task<string> RunAsync(
        string fileName,
        string arguments,
        CancellationToken cancellation)
    {
        var startInfo = BuildInfo(fileName, arguments);

        var completionSource = new TaskCompletionSource<string>();

        var process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        var output = process.SetOutputData();
        var error = process.SetErrorData();

        process.Exited += async (sender, args) =>
        {
            try
            {
                var outputText = await output.Task.ConfigureAwait(false);
                var errorText = await error.Task.ConfigureAwait(false);
                if (process.ExitCode == 0)
                {
                    completionSource.TrySetResult(outputText);
                    return;
                }

                completionSource.TrySetException(BuildException("Could not execute process", startInfo, outputText, errorText));
            }
            catch (Exception exception)
            {
                completionSource.TrySetException(exception);
            }
            finally
            {
                process.Dispose();
            }
        };

        using (cancellation.Register(
            () =>
            {
                completionSource.TrySetCanceled();
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }))
        {
            cancellation.ThrowIfCancellationRequested();

            if (process.Start())
            {
                process.BeginRead();
            }
            else
            {
                completionSource.TrySetException(new Exception("Failed to start process"));
            }

            return completionSource.Task;
        }
    }

    static Exception BuildException(string message, ProcessStartInfo info, string output, string errors)
    {
        message = $@"{message}. Command line: {info.FileName} {info.Arguments}.
Output: {output}
Error: {errors}";
        return new Exception(message);
    }

    static ProcessStartInfo BuildInfo(string fileName, string arguments)
    {
        return new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = false,
        };
    }
}