using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AvalonStudio.Models.Solutions;
using AvalonStudio.Models.Tools.Compiler;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class JLinkDebugAdaptor : GDBDebugAdaptor
    {        
        public enum JlinkInterfaceType
        {
            [Description("JTAG")]
            JTAG,
            [Description("SWD")]
            SWD,
            [Description("FINE")]
            FINE,
            [Description("2-wire-JTAG-PIC32")]
            JTAGPic32
        }

        public JLinkDebugAdaptor ()
        {
        }

		~JLinkDebugAdaptor ()
		{
			if (jlinkProcess != null && !jlinkProcess.HasExited)
			{
				jlinkProcess.Kill ();
			}
		}

        public JlinkInterfaceType Interface { get; set; }

        public string Location { get; set; }

        private Process jlinkProcess;

        public override bool Start(ToolChain toolchain, IConsole console, Project project)
        {
            bool result = true;
            console.Clear();
            console.WriteLine("[JLink] - Starting GDB Server...");
            // TODO allow people to select the device.
            var startInfo = new ProcessStartInfo();
            startInfo.Arguments = string.Format("-select USB -device {0} -if {1} -speed 12000", project.SelectedConfiguration.SelectedDeviceName, Enum.GetName(typeof(JlinkInterfaceType), Interface));
            startInfo.FileName = Location;

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("[JLink] - Error unable to find executable.");
                return false;
            }

            // Hide console window
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            var processes = Process.GetProcessesByName("JLinkGDBServerCL");

            foreach (var process in processes)
            {
                process.Kill();
            }

            Task.Factory.StartNew(() =>
            {
                using (var process = Process.Start(startInfo))
                {
                    jlinkProcess = process;

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (base.DebugMode && !string.IsNullOrEmpty(e.Data))
                        {
                           // console.WriteLine("[JLink] - " + e.Data);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            console.WriteLine("[JLink] - " + e.Data);
                        };
                    };

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    base.Close();

                    console.WriteLine("[JLink] - GDB Server Closed.");

					jlinkProcess = null;

                    result = false;
                }
            });

            StoppedEventIsEnabled = false;

            //Thread.Sleep(500);

            if (result)
            {
                result = base.Start(toolchain, console, project);

                if (result)
                {
                    console.WriteLine("[JLink] - Connecting...");
                    result = new TargetSelectCommand("localhost:2331").Execute(this).Response == ResponseCode.Done;

                    if (result)
                    {           
                        new MonitorCommand("reset").Execute(this);
                        //new MonitorCommand("reg r13 = (0x00000000)").Execute(this);
                        //new MonitorCommand("reg pc = (0x00000004)").Execute(this);

                        new TargetDownloadCommand().Execute(this);

                        console.WriteLine("[JLink] - Connected.");
                    }

                    StoppedEventIsEnabled = true;
                }
            }
            
            if(!result)
            {
                console.WriteLine("[JLink] - Unable to connect. Ensure there no process named jlinkgdbservercl.exe is running.");

                if (jlinkProcess != null && !jlinkProcess.HasExited)
                {
                    jlinkProcess.Kill();
                }
            }

            return result;
        }

        public override void Run()
        {
            base.Continue();
        }

        public override void Stop()
        {
            base.Stop();

            if(jlinkProcess != null && !jlinkProcess.HasExited)
            {
                jlinkProcess.Kill();
            }            
        }

        public override string Name
        {
            get { return "JLink"; }
        }
    }
}
