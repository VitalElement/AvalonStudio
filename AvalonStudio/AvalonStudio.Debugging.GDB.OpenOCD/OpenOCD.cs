using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;

namespace AvalonStudio.Debugging.GDB.OpenOCD
{
	public class OpenOCDDebugAdaptor : GDBDebugAdaptor
	{
		private Process openOcdProcess;

		public static string BaseDirectory
		{
			get { return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Debugging.OpenOCD\\").ToPlatformPath(); }
		}

		public override string Name
		{
			get { return "OpenOCD"; }
		}

		~OpenOCDDebugAdaptor()
		{
			if (openOcdProcess != null && !openOcdProcess.HasExited)
			{
				openOcdProcess.Kill();
			}
		}

		public override object GetSettingsControl(IProject project)
		{
			return new OpenOCDSettingsFormViewModel(project);
		}

		public override void ProvisionSettings(IProject project)
		{
			ProvisionOpenOCDSettings(project);
		}

		public static OpenOCDSettings ProvisionOpenOCDSettings(IProject project)
		{
			var result = GetSettings(project);

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
			catch (Exception)
			{
				result = project.DebugSettings.OpenOCDSettings = new OpenOCDSettings();
			}

			return result;
		}

		public static void SetSettings(IProject project, OpenOCDSettings settings)
		{
			project.DebugSettings.OpenOCDSettings = settings;
		}

		public override async Task ResetAsync(bool runAfter)
		{
			await SafelyExecuteCommandWithoutResume(async () =>
			{
				await new MonitorCommand("reset halt").Execute(this);

				if (runAfter)
				{
					await ContinueAsync();
				}
				else
				{
					await StepInstructionAsync();
				}
			});
		}

		public override async Task<bool> StartAsync(IToolChain toolchain, IConsole console, IProject project)
		{
			var result = true;
			console.WriteLine("[OpenOCD] - Starting GDB Server...");

			var settings = GetSettings(project);

			if (settings == null)
			{
				console.WriteLine(
					"[OpenOCD] - No configuration found for open ocd, check debugger settings for the selected project.");
			}

			if (settings.InterfaceConfigFile == null || settings.TargetConfigFile == string.Empty)
			{
				console.WriteLine("[OpenOCD] - No configuration file selected. Please configure debug settings for the project.");
				return false; //TODO implement error message on the console.
			}

			var startInfo = new ProcessStartInfo();
			startInfo.Arguments = string.Format("-f \"{0}\" -f \"{1}\"", settings.InterfaceConfigFile, settings.TargetConfigFile);
			startInfo.FileName = Path.Combine(BaseDirectory, "bin", "openocd" + Platform.ExecutableExtension);

			if (Path.IsPathRooted(startInfo.FileName) && !System.IO.File.Exists(startInfo.FileName))
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

			Task.Factory.StartNew(async () =>
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
						}
						;
					};

					process.BeginOutputReadLine();
					process.BeginErrorReadLine();

					process.WaitForExit();

					await CloseAsync();

					console.WriteLine("[OpenOCD] - GDB Server Closed.");
					openOcdProcess = null;

					result = false;
				}
			});

			//Prevent stopped events until we have completed download and reset.            
			StoppedEventIsEnabled = false;

			Thread.Sleep(500); //Only way to wait if openocd timesout. i.e. because already running?

			if (result)
			{
				result = await base.StartAsync(toolchain, console, project);

				if (result)
				{
					console.WriteLine("[OpenOCD] - Connecting...");
                    asyncModeEnabled = (await new GDBSetCommand("mi-async", "on").Execute(this)).Response == ResponseCode.Done;
                    result = (await new TargetSelectCommand(":3333").Execute(this)).Response == ResponseCode.Done;

					if (result)
					{
						await new MonitorCommand("reset halt").Execute(this);

						await new MonitorCommand("speed 8000").Execute(this);

						await new TargetDownloadCommand().Execute(this);

						await new MonitorCommand("reset halt").Execute(this);

						console.WriteLine("[OpenOCD] - Connected.");
					}

					StoppedEventIsEnabled = true;
				}
			}

			if (!result)
			{
				console.WriteLine(
					"[OpenOCD] - Unable to connect. Ensure target is powered, connected and that debug settings are correct.");

				if (openOcdProcess != null && !openOcdProcess.HasExited)
				{
					openOcdProcess.Kill();
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

			if (openOcdProcess != null && !openOcdProcess.HasExited)
			{
				openOcdProcess.Kill();
			}
		}
	}
}