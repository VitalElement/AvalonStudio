using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AvalonStudio.Models.Solutions;
using AvalonStudio.Models.Tools.Compiler;

namespace AvalonStudio.Models.Tools.Debuggers
{
    [XmlInclude(typeof(OpenOCDDebugAdaptor))]
    [XmlInclude(typeof(JLinkDebugAdaptor))]
    [XmlInclude(typeof(LocalDebugAdaptor))]
    public abstract class GDBDebugAdaptor : GDBDebugger
    {
        public GDBDebugAdaptor () 
        {

        }

        public abstract string Name { get; }
    }

    public class OpenOCDDebugAdaptor : GDBDebugAdaptor
    {
        public OpenOCDDebugAdaptor()
        {

        }

		~OpenOCDDebugAdaptor ()
		{
			if (openOcdProcess != null && !openOcdProcess.HasExited)
			{
				openOcdProcess.Kill ();
			}
		}

        public string InterfaceConfiguration { get; set; }
        public string TargetConfiguration { get; set; }

        public override void Reset(bool runAfter)
        {
            SafelyExecuteCommandWithoutResume(() =>
            {
                new MonitorCommand("reset halt").Execute(this);

                if(runAfter)
                {
                    Continue();
                }
                else
                {
                    StepInstruction();
                }
            });
		}        
        
        public string Location { get; set; }

        private Process openOcdProcess;

        public override bool Start(ToolChain toolchain, IConsole console, Project project)
        {
            bool result = true;            
            console.WriteLine("[OpenOCD] - Starting GDB Server...");

            if (InterfaceConfiguration == null || InterfaceConfiguration == string.Empty)
            {
                console.WriteLine("[OpenOCD] - No configuration file selected. Please configure debug settings for the project.");
                return false;   //TODO implement error message on the console.
            }

            var startInfo = new ProcessStartInfo();
            startInfo.Arguments = string.Format("-f \"{0}\" -f \"{1}\"", InterfaceConfiguration, TargetConfiguration);
            startInfo.FileName = Location;

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("[OpenOCD] - Error unable to find executable.");
                return false;
            }

            // Hide console window
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            var processes = Process.GetProcessesByName("openocd");

            foreach (var process in processes)
            {
                process.Kill();
            }

            Task.Factory.StartNew(() =>
           {
               using (var process = Process.Start(startInfo))
               {
                   openOcdProcess = process;


                   process.OutputDataReceived += (sender, e) =>
                   {
                       if (!string.IsNullOrEmpty(e.Data))
                       {
                           console.WriteLine("[OpenOCD] - " + e.Data);
                       }
                   };

                   process.ErrorDataReceived += (sender, e) =>
                   {
                       if (DebugMode && !string.IsNullOrEmpty(e.Data))
                       {
                           console.WriteLine("[OpenOCD] - " + e.Data);
                       };
                   };

                   process.BeginOutputReadLine();
                   process.BeginErrorReadLine();

                   process.WaitForExit();

                   base.Close();

                   console.WriteLine("[OpenOCD] - GDB Server Closed.");
				   openOcdProcess = null;

                   result = false;
               }
           });

            //Prevent stopped events until we have completed download and reset.            
            StoppedEventIsEnabled = false;

            Thread.Sleep(500);  //Only way to wait if openocd timesout. i.e. because already running?

            if (result)
            {
                result = base.Start(toolchain, console, project);

                if (result)
                {
                    console.WriteLine("[OpenOCD] - Connecting...");

                    result = new TargetSelectCommand(":3333").Execute(this).Response == ResponseCode.Done;

                    if (result)
                    {
                        new MonitorCommand("reset halt").Execute(this);

                        new MonitorCommand("speed 8000").Execute(this);

                        new TargetDownloadCommand().Execute(this);

                        new MonitorCommand("reset halt").Execute(this);

                        console.WriteLine("[OpenOCD] - Connected.");
                    }

                    StoppedEventIsEnabled = true;
                }
            }
           
            if(!result)
            {
                console.WriteLine("[OpenOCD] - Unable to connect. Ensure there no process named openocd.exe is running.");

                if (openOcdProcess != null && !openOcdProcess.HasExited)
                {
                    openOcdProcess.Kill();
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

            if (openOcdProcess != null && !openOcdProcess.HasExited)
            {
                openOcdProcess.Kill();
            }
        }

        public override string Name
        {
            get { return "OpenOCD"; }
        }
    }
}

