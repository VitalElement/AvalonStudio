namespace AvalonStudio.Models.Tools.Debuggers.Local
{
    using AvalonStudio.Debugging.GDB;
    using AvalonStudio.Projects.Standard;
    using AvalonStudio.Toolchains.Standard;
    using AvalonStudio.Utils;

    public class LocalDebugAdaptor : GDBDebugger
    {
        public LocalDebugAdaptor ()
        {

        }

        new public bool Start(StandardToolChain toolchain, IConsole console, IStandardProject project)
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
