using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;

namespace AvalonStudio.Debugging.GDB.JLink
{
	public enum JlinkInterfaceType
	{
		[Description("JTAG")] JTAG,
		[Description("SWD")] SWD,
		[Description("FINE")] FINE,
		[Description("2-wire-JTAG-PIC32")] JTAGPic32
	}


	public class JLinkDebugAdaptor : GDBDebugAdaptor
	{
		private Process jlinkProcess;

		public static string BaseDirectory
		{
			get {
				if (Platform.OSDescription == "Unix") {
					return string.Empty;
				} else {
					return Path.Combine (Platform.ReposDirectory, "AvalonStudio.Debugging.JLink\\").ToPlatformPath ();
				}
			}
		}

		public override string Name
		{
			get { return "JLink"; }
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

		public static void SetSettings(IProject project, JLinkSettings settings)
		{
			project.DebugSettings.JLinkSettings = settings;
		}

		~JLinkDebugAdaptor()
		{
			if (jlinkProcess != null && !jlinkProcess.HasExited)
			{
				jlinkProcess.Kill();
			}
		}

		public override object GetSettingsControl(IProject project)
		{
			return new JLinkSettingsFormViewModel(project);
		}

		public override void ProvisionSettings(IProject project)
		{
			var result = GetSettings(project);

			if (result == null)
			{
				project.DebugSettings.JLinkSettings = new JLinkSettings();
				result = project.DebugSettings.JLinkSettings;
				project.Save();
			}
		}

		public override async Task<bool> StartAsync(IToolChain toolchain, IConsole console, IProject project)
		{
			var result = true;
			var settings = GetSettings(project);

			console.Clear();
			console.WriteLine("[JLink] - Starting GDB Server...");
			// TODO allow people to select the device.
			var startInfo = new ProcessStartInfo();
			startInfo.Arguments = string.Format("-select USB -device {0} -if {1} -speed 12000 -noir", settings.TargetDevice, Enum.GetName(typeof (JlinkInterfaceType), settings.Interface));
			startInfo.FileName = Path.Combine(BaseDirectory, "JLinkGDBServerCL" + Platform.ExecutableExtension);

			if (Platform.OSDescription == "Unix") {
				startInfo.FileName = Path.Combine(BaseDirectory, "JLinkGDBServer" + Platform.ExecutableExtension);	
			}

			if (Path.IsPathRooted(startInfo.FileName) && !System.IO.File.Exists(startInfo.FileName))
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

			Task.Factory.StartNew(async () =>
			{
				using (var process = Process.Start(startInfo))
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

					await CloseAsync();

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
				result = await base.StartAsync(toolchain, console, project);

				if (result)
				{
					console.WriteLine("[JLink] - Connecting...");
                    asyncModeEnabled = (await new GDBSetCommand("mi-async", "on").Execute(this)).Response == ResponseCode.Done;
					result = (await new TargetSelectCommand(":2331").Execute(this)).Response == ResponseCode.Done;

					if (result)
					{
						await new MonitorCommand("halt").Execute(this);
						await new MonitorCommand("reset").Execute(this);
						//new MonitorCommand("reg r13 = (0x00000000)").Execute(this);
						//new MonitorCommand("reg pc = (0x00000004)").Execute(this);

						await new TargetDownloadCommand().Execute(this);

						console.WriteLine("[JLink] - Connected.");
					}

					StoppedEventIsEnabled = true;
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

			return result;
		}


		public override async Task RunAsync()
		{
			await ContinueAsync();
		}

		public override async Task StopAsync()
		{
			await base.StopAsync();

			if (jlinkProcess != null && !jlinkProcess.HasExited)
			{
				jlinkProcess.Kill();
			}
		}
	}
}