using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using Mono.Debugging.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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

            console.Clear();

            if (!string.IsNullOrEmpty(settings.PreInitCommand?.Trim()))
            {
                console.WriteLine("[Remote GDB] - Starting GDB Server...");


                var gdbServerStartInfo = new ProcessStartInfo();
                gdbServerStartInfo.Arguments = settings.PreInitCommandArgs;
                gdbServerStartInfo.FileName = settings.PreInitCommand;
                gdbServerStartInfo.WorkingDirectory = _project.CurrentDirectory;

                // Hide console window
                gdbServerStartInfo.RedirectStandardOutput = true;
                gdbServerStartInfo.RedirectStandardError = true;
                gdbServerStartInfo.UseShellExecute = false;
                gdbServerStartInfo.CreateNoWindow = true;

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

                if (!string.IsNullOrEmpty(settings.PostInitCommand?.Trim()))
                {
                    var console = IoC.Get<IConsole>();

                    var exitCode = PlatformSupport.ExecuteShellCommand(settings.PostInitCommand, settings.PostInitCommandArgs, (s, e) =>
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

            var commands = settings.GDBExitCommands?.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            foreach (var command in commands)
            {
                var commandParts = command.Split(' ');
                var args = command.Remove(0, commandParts[0].Length);

                var arguments = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                console.WriteLine($"Running GDB Command: {command}");
                RunCommand(commandParts[0], arguments);
            }

            base.OnExit();
        }

        private void RemoteGdbSession_TargetReady(object sender, TargetEventArgs e)
        {
            var settings = _project.GetDebuggerSettings<RemoteGdbSettings>();

            bool result = RunCommand("-target-select", "extended-remote", $"{settings.Host}:{settings.Port}").Status == CommandStatus.Done;

            if (result)
            {
                var commands = settings.GDBInitCommands?.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                foreach(var command in commands)
                {
                    var commandParts = command.Split(' ');
                    var args = command.Remove(0, commandParts[0].Length);

                    var arguments = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    console.WriteLine($"Running GDB Command: {command}");
                    RunCommand(commandParts[0], arguments);
                }

                console.WriteLine("[JLink] - Connected.");
            }
        }
    }
}
