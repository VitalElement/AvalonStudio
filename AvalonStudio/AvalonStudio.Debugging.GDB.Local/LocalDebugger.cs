using System.Threading.Tasks;
using System;
using AvalonStudio.Debugging.GDB;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;

namespace AvalonStudio.Models.Tools.Debuggers.Local
{
	public class LocalDebugAdaptor : GDBDebugger
	{
		public string Name
		{
			get { return "Local Executable Debug Adaptor"; }
		}

		public override async Task<bool> StartAsync(IToolChain toolchain, IConsole console, IProject project)
		{
			var result = await base.StartAsync(toolchain, console, project);

			if (result)
			{
                asyncModeEnabled = (await new GDBSetCommand("mi-async", "on").Execute(this)).Response == ResponseCode.Done;

				if (Platform.OSDescription == "Windows") {
					await new SetCommand ("new-console", "on").Execute (this);
				}
			}

			return result;
		}
	}
}