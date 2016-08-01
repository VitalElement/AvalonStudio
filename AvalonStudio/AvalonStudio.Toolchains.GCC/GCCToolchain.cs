using AvalonStudio.Toolchains;
using AvalonStudio.Toolchains.Standard;

namespace AvalonStudio.Toolchains.GCC
{
    public abstract class GCCToolchain : StandardToolChain, IGDBToolchain
	{
		public abstract string GDBExecutable { get; }
	}
}