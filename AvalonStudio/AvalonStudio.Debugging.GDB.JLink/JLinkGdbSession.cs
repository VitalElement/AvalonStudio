using AvalonStudio.Extensibility;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Repositories;
using AvalonStudio.Utils;
using Mono.Debugging.Client;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB.JLink
{
    internal class JLinkGdbSession : GdbSession
    {
        private IConsole console;
        private IProject _project;
        private Process jlinkProcess;
        public bool DebugMode { get; set; }

        public JLinkGdbSession(IProject project, string gdbExecutable) : base(gdbExecutable, "-exec-continue")
        {
            this._project = project;
            console = IoC.Get<IConsole>();

            TargetReady += JLinkGdbSession_TargetReady;
        }

        private void JLinkGdbSession_TargetReady(object sender, TargetEventArgs e)
        {
            bool result = RunCommand("-target-select", "remote", ":2331").Status == CommandStatus.Done;

            if (result)
            {
                RunCommand("monitor", "halt");
                RunCommand("monitor", "reset");

                console.WriteLine(RunCommand("-target-download").Status.ToString());

                console.WriteLine("[JLink] - Connected.");
            }
        }

        protected override void OnRun(DebuggerStartInfo startInfo)
        {
            var result = true;
            var settings = _project.GetDebuggerSettings<JLinkSettings>();

            console.Clear();
            console.WriteLine("[JLink] - Starting GDB Server...");
            // TODO allow people to select the device.
            var jlinkStartInfo = new ProcessStartInfo();
            jlinkStartInfo.Arguments = string.Format("-select USB -device {0} -if {1} -speed {2} -noir", settings.TargetDevice, Enum.GetName(typeof(JlinkInterfaceType), settings.Interface), settings.SpeedkHz);
            jlinkStartInfo.FileName = Path.Combine(JLinkDebugger.BaseDirectory, "JLinkGDBServerCL" + Platform.ExecutableExtension);

            if (Platform.OSDescription == "Unix")
            {
                jlinkStartInfo.FileName = Path.Combine(JLinkDebugger.BaseDirectory, "JLinkGDBServer" + Platform.ExecutableExtension);
            }

            if (Path.IsPathRooted(jlinkStartInfo.FileName) && !System.IO.File.Exists(jlinkStartInfo.FileName))
            {
                console.WriteLine("[JLink] - Error unable to find executable.");
                return;
            }

            // Hide console window
            jlinkStartInfo.RedirectStandardOutput = true;
            jlinkStartInfo.RedirectStandardError = true;
            jlinkStartInfo.UseShellExecute = false;
            jlinkStartInfo.CreateNoWindow = true;

            var processes = Process.GetProcessesByName("JLinkGDBServerCL");

            foreach (var process in processes)
            {
                process.Kill();
            }

            Task.Run(() =>
            {
                using (var process = Process.Start(jlinkStartInfo))
                {
                    jlinkProcess = process;

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (DebugMode && !string.IsNullOrEmpty(e.Data))
                        {
                            console.WriteLine("[JLink] - " + e.Data);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            console.WriteLine("[JLink] - " + e.Data);
                        }
                    };

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    Dispose();

                    console.WriteLine("[JLink] - GDB Server Closed.");

                    jlinkProcess = null;

                    result = false;
                }
            });

            while (jlinkProcess == null)
            {
                Thread.Sleep(10);
            }

            TargetExited += (sender, e) =>
            {
                jlinkProcess?.Kill();
                jlinkProcess = null;
            };

            if (result)
            {
                base.OnRun(startInfo);
                console.WriteLine("[JLink] - Connecting...");
            }
        }
    }
}