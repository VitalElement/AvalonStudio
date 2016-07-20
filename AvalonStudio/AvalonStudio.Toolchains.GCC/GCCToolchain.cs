using AvalonStudio.Toolchains.Standard;

namespace AvalonStudio.Toolchains.GCC
{
	public abstract class GCCToolchain : StandardToolChain
	{
		public abstract string GDBExecutable { get; }
	}
}