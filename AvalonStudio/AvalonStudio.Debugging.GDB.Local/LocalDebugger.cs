namespace AvalonStudio.Models.Tools.Debuggers.Local
{
    using AvalonStudio.Debugging.GDB;
    using AvalonStudio.Utils;
    using Projects;
    using System.Threading.Tasks;
    using Toolchains;

    public class LocalDebugAdaptor : GDBDebugger
    {
        public LocalDebugAdaptor ()
        {

        }

        public override async Task<bool> StartAsync(IToolChain toolchain, IConsole console, IProject project)
        {
            bool result = await base.StartAsync(toolchain, console, project);

            if (result)
            {
                await new SetCommand("new-console", "on").Execute(this);
            }

            return result;
        }

        public string Name
        {
            get { return "Local Executable Debug Adaptor"; }
        }
    }
}
