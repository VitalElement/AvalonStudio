namespace AvalonStudio.Models.Tools.Debuggers.Local
{
    using AvalonStudio.Debugging.GDB;
    using AvalonStudio.Utils;
    using Projects;
    using Toolchains;

    public class LocalDebugAdaptor : GDBDebugger
    {
        public LocalDebugAdaptor ()
        {

        }

        public override bool Start(IToolChain toolchain, IConsole console, IProject project)
        {
            bool result = base.Start(toolchain, console, project);

            if (result)
            {
                new SetCommand("new-console", "on").Execute(this);
            }

            return result;
        }

        public string Name
        {
            get { return "Local Executable Debug Adaptor"; }
        }
    }
}
