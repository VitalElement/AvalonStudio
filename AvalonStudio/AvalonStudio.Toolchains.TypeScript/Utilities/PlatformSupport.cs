using AvalonStudio.Platforms;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AvalonStudio.Toolchains.TypeScript.Utilities
{
    public static class PlatformSupport
    {
        private static ShellExecutorType executorType;

        static PlatformSupport()
        {
            switch (Platform.PlatformIdentifier)
            {
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                    executorType = ShellExecutorType.Windows;
                    break;

                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    executorType = ShellExecutorType.Unix;
                    break;
            }
        }

        public static ShellExecuteResult ExecuteShellCommand(string commandName, string args)
        {
            var outputBuilder = new StringBuilder();
            var exitCode = ExecuteShellCommand(commandName, args, (s, e) =>
            {
                outputBuilder.AppendLine(e.Data);
            }, false);
            var procOutput = outputBuilder.ToString().Trim();
            return new ShellExecuteResult()
            {
                ExitCode = exitCode,
                Output = procOutput
            };
        }

        public static int ExecuteShellCommand(string commandName, string args, Action<object, DataReceivedEventArgs> outputReceivedCallback, bool resolveExecutable = true)
        {
            var shellProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                }
            };
            if (executorType == ShellExecutorType.Windows)
            {
                shellProc.StartInfo.FileName = ResolveFullExecutablePath("cmd.exe");
                shellProc.StartInfo.Arguments = $"/C {(resolveExecutable ? ResolveFullExecutablePath(commandName) : commandName)} {args}";
                shellProc.StartInfo.CreateNoWindow = true;
            }
            else //Unix
            {
                shellProc.StartInfo.FileName = "sh";
                shellProc.StartInfo.Arguments = $"-c \"{(resolveExecutable ? ResolveFullExecutablePath(commandName) : commandName)} {args}\"";
            }
            shellProc.OutputDataReceived += (s, a) => outputReceivedCallback(s, a);
            shellProc.Start();
            shellProc.BeginOutputReadLine();
            shellProc.WaitForExit();
            return shellProc.ExitCode;
        }

        /// <summary>
        /// Checks whether a script executable is available in the user's shell
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CheckExecutableAvailability(string fileName)
        {
            return ResolveFullExecutablePath(fileName) != null;
        }

        /// <summary>
        /// Attempts to locate the full path to a script
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ResolveFullExecutablePath(string fileName, bool returnNullOnFailure = true)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            if (executorType == ShellExecutorType.Windows)
            {
                var values = Environment.GetEnvironmentVariable("PATH");
                foreach (var path in values.Split(';'))
                {
                    var fullPath = Path.Combine(path, fileName);
                    if (File.Exists(fullPath))
                        return fullPath;
                }
            }
            else
            {
                //Use the which command
                var outputBuilder = new StringBuilder();
                ExecuteShellCommand("which", $"\"{fileName}\"", (s, e) =>
                {
                    outputBuilder.AppendLine(e.Data);
                }, false);
                var procOutput = outputBuilder.ToString();
                if (string.IsNullOrWhiteSpace(procOutput))
                {
                    return returnNullOnFailure ? null : fileName;
                }
                return procOutput.Trim();
            }
            return returnNullOnFailure ? null : fileName;
        }
    }
}