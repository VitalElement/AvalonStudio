using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Projects
{
    public class DotNetCliService
    {       
        private readonly ConcurrentDictionary<string, object> _locks;
        private readonly SemaphoreSlim _semaphore;
        private readonly IConsole _console;
        private DotNetInfo _info;

        private string _dotnetPath = "dotnet";

        public string DotNetPath => _dotnetPath;

        public static DotNetCliService Instance { get; } = new DotNetCliService();

        public DotNetInfo Info => _info;
        
        private DotNetCliService(string dotnetPath = null)
        {            
            this._locks = new ConcurrentDictionary<string, object>();
            this._semaphore = new SemaphoreSlim(Environment.ProcessorCount / 2);
            this._console = IoC.Get<IConsole>();

            if (dotnetPath != null)
            {
                _dotnetPath = dotnetPath;
            }
            else
            {
                SetDotNetPath("");
            }

            _info = GetInfo();
        }

        private static void RemoveMSBuildEnvironmentVariables(IDictionary<string, string> environment)
        {
            // Remove various MSBuild environment variables set by OmniSharp to ensure that
            // the .NET CLI is not launched with the wrong values.
            environment.Remove("MSBUILD_EXE_PATH");
            environment.Remove("MSBuildExtensionsPath");
        }

        public void SetDotNetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "dotnet";
            }

            if (string.Equals(_dotnetPath, path, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _dotnetPath = path;

            _console.WriteLine($"DotNetPath set to {_dotnetPath}");

        }

        public Task RestoreAsync(string workingDirectory, string arguments = null, Action onFailure = null)
        {
            return Task.Factory.StartNew(() =>
            {
                _console.WriteLine($"Begin dotnet restore in '{workingDirectory}'");

                var restoreLock = _locks.GetOrAdd(workingDirectory, new object());
                lock (restoreLock)
                {
                    ShellExecuteResult? exitStatus = null;
                    //_eventEmitter.RestoreStarted(workingDirectory);
                    _semaphore.Wait();
                    try
                    {
                        // A successful restore will update the project lock file which is monitored
                        // by the dotnet project system which eventually update the Roslyn model
                        exitStatus = PlatformSupport.ExecuteShellCommand(Path.Combine(workingDirectory, _dotnetPath), $"restore {arguments}");// updateEnvironment: RemoveMSBuildEnvironmentVariables);
                    }
                    finally
                    {
                        _semaphore.Release();

                        _locks.TryRemove(workingDirectory, out _);

                        //_eventEmitter.RestoreFinished(workingDirectory, exitStatus.Succeeded);

                        if (exitStatus?.ExitCode != 0 && onFailure != null)
                        {
                            onFailure();
                        }

                        _console.WriteLine($"Finish restoring project {workingDirectory}. Exit code {exitStatus}");
                    }
                }
            });
        }

        public Process Start(string arguments, string workingDirectory)
        {
            var startInfo = new ProcessStartInfo(_dotnetPath, arguments)
            {
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            RemoveMSBuildEnvironmentVariables(startInfo.Environment);

            return Process.Start(startInfo);
        }

        public SemanticVersion GetVersion(string workingDirectory = null)
        {
            var shellResult = PlatformSupport.ExecuteShellCommand(Path.Combine(workingDirectory, _dotnetPath), "--version");

            return SemanticVersion.Parse(shellResult.Output);
        }

        public DotNetInfo GetInfo(string workingDirectory = null)
        {
            const string DOTNET_CLI_UI_LANGUAGE = nameof(DOTNET_CLI_UI_LANGUAGE);

            // Ensure that we set the DOTNET_CLI_UI_LANGUAGE environment variable to "en-US" before
            // running 'dotnet --info'. Otherwise, we may get localized results.
            var originalValue = Environment.GetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE);
            Environment.SetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE, "en-US");

            try
            {
                Process process;
                try
                {
                    process = Start("--info", workingDirectory);
                }
                catch
                {
                    return DotNetInfo.Empty;
                }

                if (process.HasExited)
                {
                    return DotNetInfo.Empty;
                }

                var lines = new List<string>();
                process.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        lines.Add(e.Data);
                    }
                };

                process.BeginOutputReadLine();

                process.WaitForExit();

                return DotNetInfo.Parse(lines);
            }
            finally
            {
                Environment.SetEnvironmentVariable(DOTNET_CLI_UI_LANGUAGE, originalValue);
            }
        }

        /// <summary>
        /// Checks to see if this is a "legacy" .NET CLI. If true, this .NET CLI supports project.json
        /// development; otherwise, it supports .csproj development.
        /// </summary>
        public bool IsLegacy(string workingDirectory = null)
        {
            var version = GetVersion(workingDirectory);

            if (version.Major < 1)
            {
                return true;
            }

            if (version.Major == 1 &&
                version.Minor == 0 &&
                version.Patch == 0)
            {
                if (version.Release.StartsWith("preview1") ||
                    version.Release.StartsWith("preview2"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
