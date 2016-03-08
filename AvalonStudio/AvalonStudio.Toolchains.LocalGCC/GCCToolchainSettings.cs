namespace AvalonStudio.Toolchains.LocalGCC
{
    public class LocalGccToolchainSettings
    {
        public LocalGccToolchainSettings()
        {
            CompileSettings = new CompileSettings();
            LinkSettings = new LinkSettings();
        }

        public CompileSettings CompileSettings { get; set; }
        public LinkSettings LinkSettings { get; set; }
    }
}
