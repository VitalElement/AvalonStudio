namespace AvalonStudio.Debugging.GDB.JLink
{
    using JLink;
    using Projects;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Dynamic;
    using System.IO;
    using System.Threading.Tasks;
    using Toolchains;
    using Utils;
    using Platform;
    using Perspex.Controls;
    using System.Threading;
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


    public class JLinkDebugAdaptor : GDBDebugAdaptor
    {   
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
            catch (Exception e)
            {
                result = project.DebugSettings.JLinkSettings = new JLinkSettings();
            }

            return result;
        }

        public static void SetSettings(IProject project, JLinkSettings settings)
        {
            project.DebugSettings.JLinkSettings = settings;
        }

        public static string BaseDirectory
        {
            get
            {
                return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Debugging.JLink\\").ToPlatformPath();
            }
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

        private Process jlinkProcess;

        public override UserControl GetSettingsControl(IProject project)
        {
            return new JLinkSettingsForm() { DataContext = new JLinkSettingsFormViewModel(project) };
        }

        public override void ProvisionSettings(IProject project)
        {         
            JLinkSettings result = GetSettings(project);

            if (result == null)
            {
                project.DebugSettings.JLinkSettings = new JLinkSettings();
                result = project.DebugSettings.JLinkSettings;
                project.Save();
            }
        }

        public override bool Start(IToolChain toolchain, IConsole console, IProject project)
        {
            bool result = true;
            //DebugMode = true;

            var settings = GetSettings(project);

            console.Clear();
            console.WriteLine("[JLink] - Starting GDB Server...");
            // TODO allow people to select the device.
            var startInfo = new ProcessStartInfo();
            startInfo.Arguments = string.Format("-select USB -device {0} -if {1} -speed 12000 -noir", "STM32F407VG", Enum.GetName(typeof(JlinkInterfaceType), settings.Interface));
            startInfo.FileName = Path.Combine(BaseDirectory, "JLinkGDBServerCL" + Platform.ExecutableExtension);
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
                           console.WriteLine("[JLink] - " + e.Data);
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

            while (jlinkProcess == null)
            {
                Thread.Sleep(10);
            }

            StoppedEventIsEnabled = false;

            if (result)
            {
                result = base.Start(toolchain, console, project);

                if (result)
                {
                    console.WriteLine("[JLink] - Connecting...");
                    result = new TargetSelectCommand(":2331").Execute(this).Response == ResponseCode.Done;

                    if (result)
                    {
                        new MonitorCommand("halt").Execute(this);
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

        new public void Stop()
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
