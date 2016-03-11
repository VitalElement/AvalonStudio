namespace AvalonStudio.Debugging.GDB.OpenOCD
{
    using Debugging.GDB;
    using Platform;
    using Utils;
    using Perspex.Controls;
    using Projects;
    using System;
    using System.Diagnostics;
    using System.Dynamic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Toolchains;
    using Utils;

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

        public static string BaseDirectory
        {
            get
            {
                return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Debugging.OpenOCD\\").ToPlatformPath();
            }
        }

        public override UserControl GetSettingsControl(IProject project)
        {
            return new OpenOCDSettingsForm() { DataContext = new OpenOCDSettingsFormViewModel(project) };
        }

        public override void ProvisionSettings(IProject project)
        {
            ProvisionOpenOCDSettings(project);
        }

        public static OpenOCDSettings ProvisionOpenOCDSettings(IProject project)
        {
            OpenOCDSettings result = GetSettings(project);

            if (result == null)
            {
                project.DebugSettings.OpenOCDSettings = new OpenOCDSettings();
                result = project.DebugSettings.OpenOCDSettings;
                project.Save();
            }

            return result;
        }

        public static OpenOCDSettings GetSettings(IProject project)
        {
            OpenOCDSettings result = null;

            try
            {
                if (project.DebugSettings.OpenOCDSettings is ExpandoObject)
                {
                    result = (project.DebugSettings.OpenOCDSettings as ExpandoObject).GetConcreteType<OpenOCDSettings>();
                }
                else
                {
                    result = project.DebugSettings.OpenOCDSettings;
                }
            }
            catch (Exception e)
            {
                result = project.DebugSettings.OpenOCDSettings = new OpenOCDSettings();
            }

            return result;
        }

        public static void SetSettings (IProject project, OpenOCDSettings settings)
        {
            project.DebugSettings.OpenOCDSettings = settings;
        }        

        new public void Reset(bool runAfter)
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

        private Process openOcdProcess;

        public override bool Start(IToolChain toolchain, IConsole console, IProject project)
        {
            bool result = true;            
            console.WriteLine("[OpenOCD] - Starting GDB Server...");

            DebugMode = true;
            var settings = GetSettings(project);

            if(settings == null)
            {
                console.WriteLine("[OpenOCD] - No configuration found for open ocd, check debugger settings for the selected project.");
            }

            if (settings.InterfaceConfigFile == null || settings.TargetConfigFile == string.Empty)
            {
                console.WriteLine("[OpenOCD] - No configuration file selected. Please configure debug settings for the project.");
                return false;   //TODO implement error message on the console.
            }

            var startInfo = new ProcessStartInfo();
            startInfo.Arguments = string.Format("-f \"{0}\" -f \"{1}\"", settings.InterfaceConfigFile, settings.TargetConfigFile);
            startInfo.FileName = Path.Combine(BaseDirectory, "bin", "openocd" + Platform.ExecutableExtension);

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
            startInfo.WorkingDirectory = BaseDirectory;

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
            Continue();
        }        

        new public void Stop()
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

