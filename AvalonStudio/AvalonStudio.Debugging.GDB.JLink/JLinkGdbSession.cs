using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Mono.Debugging.Client;
using AvalonStudio.Extensibility;
using System.Diagnostics;
using AvalonStudio.Platforms;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace AvalonStudio.Debugging.GDB.JLink
{
    class JLinkGdbSession : GdbSession
    {
        private IConsole console;
        private IProject _project;
        private Process jlinkProcess;
        public bool DebugMode { get; set; }

        public static string BaseDirectory
        {
            get
            {
                if (Platform.OSDescription == "Unix")
                {
                    return string.Empty;
                }
                else
                {
                    return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Debugging.JLink\\").ToPlatformPath();
                }
            }
        }

        public static JLinkSettings GetSettings(IProject project)
        {
            JLinkSettings result = null;

            try
            {
                if (project.DebugSettings.JLinkSettings is ExpandoObject)
                {
                    result = (project.DebugSettings.JLinkSettings as ExpandoObject).GetConcreteType<JLinkSettings>();
                }
                else
                {
                    result = project.DebugSettings.JLinkSettings;
                }
            }
            catch (Exception)
            {
                result = project.DebugSettings.JLinkSettings = new JLinkSettings();
            }

            return result;
        }

        public JLinkGdbSession(IProject project, string gdbExecutable) : base(gdbExecutable)
        {
            this._project = project;
            console = IoC.Get<IConsole>();
        }

        protected override void OnRun(DebuggerStartInfo startInfo)
        {
            var result = true;
            var settings = GetSettings(_project);

            console.Clear();
            console.WriteLine("[JLink] - Starting GDB Server...");
            // TODO allow people to select the device.
            var jlinkStartInfo = new ProcessStartInfo();
            jlinkStartInfo.Arguments = string.Format("-select USB -device {0} -if {1} -speed {2} -noir", settings.TargetDevice, Enum.GetName(typeof(JlinkInterfaceType), settings.Interface), settings.SpeedkHz);
            jlinkStartInfo.FileName = Path.Combine(BaseDirectory, "JLinkGDBServerCL" + Platform.ExecutableExtension);

            if (Platform.OSDescription == "Unix")
            {
                jlinkStartInfo.FileName = Path.Combine(BaseDirectory, "JLinkGDBServer" + Platform.ExecutableExtension);
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
                        ;
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

            if (result)
            {
                base.OnRun(startInfo);
                console.WriteLine("[JLink] - Connecting...");

                RunCommand("mi-async", "on");


                result = (RunCommand("-target-select", "remote", ":2331").Status == CommandStatus.Done);

                if (result)
                {
                    RunCommand("monitor", "halt");
                    RunCommand("monitor", "reset");

                    console.WriteLine(RunCommand("-target-download").Status.ToString());

                    RunCommand("-exec-continue");

                    console.WriteLine("[JLink] - Connected.");
                }
            }

            if (!result)
            {
                console.WriteLine(
                    "[JLink] - Unable to connect. Ensure target is powered, connected and that debug settings are correct.");

                if (jlinkProcess != null && !jlinkProcess.HasExited)
                {
                    jlinkProcess.Kill();
                }
            }
        }
    }
}
