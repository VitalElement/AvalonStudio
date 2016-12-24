using System.Threading.Tasks;
using AvalonStudio.Debugging.GDB;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;

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
                await new SetCommand("new-console", "on").Execute(this);
			}

			return result;
		}
	}
}