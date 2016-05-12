namespace AvalonStudio.Toolchains.GCC
{
    using AvalonStudio.Toolchains.Standard;

    public abstract class GCCToolchain : StandardToolChain
    {
        public abstract string GDBExecutable { get; }
    }
}
