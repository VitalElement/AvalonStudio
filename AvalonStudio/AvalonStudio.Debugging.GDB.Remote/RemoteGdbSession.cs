using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using Mono.Debugging.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB.Remote
{
    class RemoteGdbSession : GdbSession
    {
        private IConsole console;
        private IProject _project;
        private Process _gdbServerProcess;

        public RemoteGdbSession(IProject project, string gdbExecutable) : base(gdbExecutable)
        {
            _project = project;
            console = IoC.Get<IConsole>();

            TargetReady += RemoteGdbSession_TargetReady;
        }

        protected override void OnRun(DebuggerStartInfo startInfo)
        {
            var settings = _project.GetDebuggerSettings<RemoteGdbSettings>();
            var environment = _project.GetEnvironmentVariables().AppendRange(Platform.EnvironmentVariables);

            console.Clear();

            var preInitCommand = settings.PreInitCommand?.Trim().ExpandVariables(environment);
            var preInitCommandArguments = settings.PreInitCommandArgs?.Trim().ExpandVariables(environment);
            var postInitCommand = settings.PostInitCommand?.Trim().ExpandVariables(environment);
            var postInitCommandArguments = settings.PostInitCommandArgs?.Trim().ExpandVariables(environment);

            if (!string.IsNullOrEmpty(preInitCommand))
            {
                console.WriteLine("[Remote GDB] - Starting GDB Server...");

                var gdbServerStartInfo = new ProcessStartInfo
                {
                    Arguments = preInitCommandArguments ?? "",
                    FileName = preInitCommand,
                    WorkingDirectory = _project.CurrentDirectory,

                    // Hide console window
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Task.Run(() =>
                {
                    using (var process = Process.Start(gdbServerStartInfo))
                    {
                        _gdbServerProcess = process;

                        process.OutputDataReceived += (sender, e) =>
                        {
                            console.WriteLine("[GDB Server] - " + e.Data);
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                console.WriteLine("[GDB Server] - " + e.Data);
                            }
                        };

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        process.WaitForExit();

                        Dispose();

                        console.WriteLine("[GDB Server] - GDB Server Closed.");

                        _gdbServerProcess = null;
                    }
                });

                TargetExited += (sender, e) =>
                {
                    _gdbServerProcess?.Kill();
                    _gdbServerProcess = null;
                };
            }

            base.OnRun(startInfo);

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(250);

                if (!string.IsNullOrEmpty(postInitCommand))
                {
                    var console = IoC.Get<IConsole>();

                    var exitCode = PlatformSupport.ExecuteShellCommand(postInitCommand, postInitCommandArguments, (s, e) =>
                    {
                        console.WriteLine(e.Data);
                    }, (s, ee) =>
                    {
                        if (ee.Data != null)
                        {
                            console.WriteLine(ee.Data);
                        }
                    }, false, _project.CurrentDirectory, false);
                }
            });
        }

        protected override void OnExit()
        {
            InsideStop();

            var settings = _project.GetDebuggerSettings<RemoteGdbSettings>();

            var commands = settings.GDBExitCommands?.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (commands != null)
            {
                foreach (var command in commands)
                {
                    var commandParts = command.Split(' ');
                    var args = command.Remove(0, commandParts[0].Length);

                    var arguments = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    console.WriteLine($"Running GDB Command: {command}");
                    RunCommand(commandParts[0], arguments);
                }
            }

            base.OnExit();
        }

        private void RemoteGdbSession_TargetReady(object sender, TargetEventArgs e)
        {
            var settings = _project.GetDebuggerSettings<RemoteGdbSettings>();

            bool result = RunCommand("-target-select", "extended-remote", $"{settings.Host}:{settings.Port}").Status == CommandStatus.Done;

            if (result)
            {
                var environment = _project.GetEnvironmentVariables().AppendRange(Platform.EnvironmentVariables);

                var commands = settings.GDBInitCommands?.ExpandVariables(environment).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                if (commands != null)
                {
                    foreach (var command in commands)
                    {
                        var commandParts = command.Split(' ');
                        var args = command.Remove(0, commandParts[0].Length);

                        var arguments = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        console.WriteLine($"Running GDB Command: {command}");
                        RunCommand(commandParts[0], arguments);
                    }
                }

                console.WriteLine("[JLink] - Connected.");
            }
        }
    }
}
