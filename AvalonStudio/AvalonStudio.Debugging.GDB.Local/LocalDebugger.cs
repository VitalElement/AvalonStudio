using System.Threading.Tasks;
using System;
using AvalonStudio.Debugging.GDB;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;

namespace AvalonStudio.Debuggers.GDB.Local
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
                SetAsyncMode((await new GDBSetCommand("mi-async", "on").Execute(this)).Response == ResponseCode.Done);

				if (Platform.PlatformIdentifier == PlatformID.Win32NT) {
					await new SetCommand ("new-console", "on").Execute (this);
				}
			}

			return result;
		}
	}
}