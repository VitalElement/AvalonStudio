using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using Mono.Debugging.Client;
using System;
using System.Diagnostics;
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

        public JLinkGdbSession(IProject project, string gdbExecutable) : base(gdbExecutable, "-exec-continue", waitForStopBeforeRunning: true)
        {
            this._project = project;
            console = IoC.Get<IConsole>();

            TargetReady += JLinkGdbSession_TargetReady;
        }

        private void JLinkGdbSession_TargetReady(object sender, TargetEventArgs e)
        {
            var settings = _project.GetDebuggerSettings<JLinkSettings>();

            bool result = RunCommand("-target-select", "remote", ":2331").Status == CommandStatus.Done;

            if (result)
            {
                InsideStop();

                RunCommand("monitor", "halt");

                if (settings.Reset)
                {
                    RunCommand("monitor", "reset");
                }

                if (settings.Download)
                {
                    console.WriteLine(RunCommand("-target-download", 60000).Status.ToString());

                    if (settings.PostDownloadReset)
                    {
                        RunCommand("monitor", "reset");
                    }
                    else
                    {
                        RunCommand("monitor", "halt");
                    }
                }

                console.WriteLine("[JLink] - Connected.");
            }
        }

        protected override void OnRun(DebuggerStartInfo startInfo)
        {
            var result = true;
            var settings = _project.GetDebuggerSettings<JLinkSettings>();

            console.Clear();
            console.WriteLine("[JLink] - Starting GDB Server...");

            string processName = "JLinkGDBServer";

            if (Platform.PlatformIdentifier != Platforms.PlatformID.Unix)
            {
                processName += "CL";
            }

            var jlinkStartInfo = new ProcessStartInfo();

            if (settings.UseRemote)
            {
                jlinkStartInfo.Arguments = string.Format($"-select IP={settings.RemoteIPAddress} -device {settings.TargetDevice} -if {Enum.GetName(typeof(JlinkInterfaceType), settings.Interface)} -speed {settings.SpeedkHz}");
            }
            else
            {
                jlinkStartInfo.Arguments = string.Format($"-select USB -device {settings.TargetDevice} -if {Enum.GetName(typeof(JlinkInterfaceType), settings.Interface)} -speed {settings.SpeedkHz}");
            }

            jlinkStartInfo.FileName = Path.Combine(JLinkDebugger.GetBaseDirectory(_project), processName + Platform.ExecutableExtension);

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

            var processes = Process.GetProcessesByName(processName);

            foreach (var process in processes)
            {
                process.Kill();
            }

            if (!Path.IsPathRooted(jlinkStartInfo.FileName) || File.Exists(jlinkStartInfo.FileName))
            {
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

                        base.OnExit();
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
            else
            {
                console.WriteLine("[JLink] - Unable to start GDBServer.");
                base.OnExit();
            }
        }
    }
}