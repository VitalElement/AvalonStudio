namespace AvalonStudio.Debugging.GDB
{
    public abstract class GDBDebugAdaptor : GDBDebugger
    {
        public GDBDebugAdaptor()
        {

        }

        public abstract string Name { get; }
    }
}
