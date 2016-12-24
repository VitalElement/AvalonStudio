namespace AvalonStudio.Debugging.GDB
{
	public abstract class GDBDebugAdaptor : GDBDebugger
	{
		public abstract string Name { get; }
	}
}