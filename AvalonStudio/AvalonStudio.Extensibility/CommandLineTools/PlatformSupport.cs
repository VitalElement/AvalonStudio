using Avalonia;
using AvalonStudio.Platforms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AvalonStudio.CommandLineTools
{
    public static class PlatformSupport
    {
        private static ShellExecutorType executorType;

        static PlatformSupport()
        {
            switch (Platform.PlatformIdentifier)
            {
                case Platforms.PlatformID.Win32NT:
                    executorType = ShellExecutorType.Windows;
                    break;

                case Platforms.PlatformID.Unix:
                case Platforms.PlatformID.MacOSX:
                    executorType = ShellExecutorType.Unix;
                    break;
            }
        }

        public static ShellExecuteResult ExecuteShellCommand(string commandName, string args)
        {
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            var exitCode = ExecuteShellCommand(commandName, args,
            (s, e) =>
            {
                outputBuilder.AppendLine(e.Data);
            },
            (s, e) =>
            {
                errorBuilder = new StringBuilder();
            },
            false, "");

            return new ShellExecuteResult()
            {
                ExitCode = exitCode,
                Output = outputBuilder.ToString().Trim(),
                ErrorOutput = errorBuilder.ToString().Trim()
            };
        }

        public static void LaunchShell(string workingDirectory, params string[] paths)
        {
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
            };

            foreach (var extraPath in paths)
            {
                if (extraPath != null)
                {
                    startInfo.Environment["PATH"] += $"{Platform.PathSeperator}{extraPath}";
                }
            }

            if (executorType == ShellExecutorType.Windows)
            {
                startInfo.FileName = ResolveFullExecutablePath("cmd.exe");
                startInfo.Arguments = $"/c start {startInfo.FileName}";
            }
            else //Unix
            {
                startInfo.FileName = "sh";
            }

            Process.Start(startInfo);
        }

        public static Process LaunchShellCommand(string commandName, string args, Action<object, DataReceivedEventArgs>
           outputReceivedCallback, Action<object, DataReceivedEventArgs> errorReceivedCallback = null, bool resolveExecutable = true,
           string workingDirectory = "", bool executeInShell = true, bool includeSystemPaths = true, params string[] extraPaths)
        {
            var shellProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,                    
                }                
            };

            if(!includeSystemPaths)
            {
                shellProc.StartInfo.Environment["PATH"] = "";
            }


            foreach (var extraPath in extraPaths)
            {
                if (extraPath != null)
                {
                    shellProc.StartInfo.Environment["PATH"] += $"{Platform.PathSeperator}{extraPath}";
                }
            }

            if (executeInShell)
            {
                if (executorType == ShellExecutorType.Windows)
                {
                    shellProc.StartInfo.FileName = ResolveFullExecutablePath("cmd.exe");
                    shellProc.StartInfo.Arguments = $"/C {(resolveExecutable ? ResolveFullExecutablePath(commandName, true, extraPaths) : commandName)} {args}";
                    shellProc.StartInfo.CreateNoWindow = true;
                }
                else //Unix
                {
                    shellProc.StartInfo.FileName = "sh";
                    shellProc.StartInfo.Arguments = $"-c \"{(resolveExecutable ? ResolveFullExecutablePath(commandName) : commandName)} {args}\"";
                    shellProc.StartInfo.CreateNoWindow = true;
                }
            }
            else
            {
                shellProc.StartInfo.FileName = (resolveExecutable ? ResolveFullExecutablePath(commandName, true, extraPaths) : commandName);
                shellProc.StartInfo.Arguments = args;
                shellProc.StartInfo.CreateNoWindow = true;
            }

            shellProc.OutputDataReceived += (s, a) => outputReceivedCallback(s, a);

            if (errorReceivedCallback != null)
            {
                shellProc.ErrorDataReceived += (s, a) => errorReceivedCallback(s, a);
            }

            shellProc.EnableRaisingEvents = true;
            new ProcessManager(shellProc);

            try
            {
                shellProc.Start();

                shellProc.BeginOutputReadLine();
                shellProc.BeginErrorReadLine();
            }
            catch { }

            return shellProc;
        }

        public class ProcessManager
        {
            private Process _process;

            public ProcessManager (Process process)
            {
                _process = process;

                if (Application.Current != null)
                {
                    Application.Current.OnExit += Current_OnExit;

                    _process.Exited += _process_Exited;
                }
            }

            private void Current_OnExit(object sender, EventArgs e)
            {
                try
                {
                    if (!_process.HasExited)
                    {
                        _process.Kill();
                    }
                }
                catch
                {

                }
            }

            private void _process_Exited(object sender, EventArgs e)
            {
                if (Application.Current != null)
                {
                    _process.Exited -= _process_Exited;

                    Application.Current.OnExit -= Current_OnExit;
                }

                _process = null;
            }
        }

        public static string[] GetSystemPaths ()
        {
            var result = ExecuteShellCommand("/bin/bash", "-l -c 'echo $PATH'");

            return result.Output.Split(':');
        }


        public static int ExecuteShellCommand(string commandName, string args, Action<object, DataReceivedEventArgs>
            outputReceivedCallback, Action<object, DataReceivedEventArgs> errorReceivedCallback = null, bool resolveExecutable = true,
            string workingDirectory = "", bool executeInShell = true, bool includeSystemPaths = true, params string[] extraPaths)
        {
            using (var shellProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory
                }
            })

            {
                if(!includeSystemPaths)
                {
                    shellProc.StartInfo.Environment["PATH"] = "";
                }
                foreach (var extraPath in extraPaths)
                {
                    if (extraPath != null)
                    {
                        shellProc.StartInfo.Environment["PATH"] += $"{Platform.PathSeperator}{extraPath}";
                    }
                }

                if (executeInShell)
                {
                    if (executorType == ShellExecutorType.Windows)
                    {
                        shellProc.StartInfo.FileName = ResolveFullExecutablePath("cmd.exe");
                        shellProc.StartInfo.Arguments = $"/C {(resolveExecutable ? ResolveFullExecutablePath(commandName, true, extraPaths) : commandName)} {args}";
                        shellProc.StartInfo.CreateNoWindow = true;
                    }
                    else //Unix
                    {
                        shellProc.StartInfo.FileName = "sh";
                        shellProc.StartInfo.Arguments = $"-c \"{(resolveExecutable ? ResolveFullExecutablePath(commandName) : commandName)} {args}\"";
                        shellProc.StartInfo.CreateNoWindow = true;
                    }
                }
                else
                {
                    shellProc.StartInfo.FileName = (resolveExecutable ? ResolveFullExecutablePath(commandName, true, extraPaths) : commandName);
                    shellProc.StartInfo.Arguments = args;
                    shellProc.StartInfo.CreateNoWindow = true;
                }

                shellProc.OutputDataReceived += (s, a) => outputReceivedCallback(s, a);

                if (errorReceivedCallback != null)
                {
                    shellProc.ErrorDataReceived += (s, a) => errorReceivedCallback(s, a);
                }

                shellProc.Start();

                shellProc.BeginOutputReadLine();
                shellProc.BeginErrorReadLine();

                shellProc.WaitForExit();

                return shellProc.ExitCode;
            }
        }

        /// <summary>
        /// Checks whether a script executable is available in the user's shell
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CheckExecutableAvailability(string fileName, params string[] extraPaths)
        {
            return ResolveFullExecutablePath(fileName, true, extraPaths) != null;
        }

        /// <summary>
        /// Attempts to locate the full path to a script
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ResolveFullExecutablePath(string fileName, bool returnNullOnFailure = true, params string[] extraPaths)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            if (executorType == ShellExecutorType.Windows)
            {
                var values = new List<string>(extraPaths);
                values.AddRange(new List<string>(Environment.GetEnvironmentVariable("PATH").Split(';')));

                foreach (var path in values)
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
                }, (s, e) => { }, false);
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