namespace AvalonStudio.Toolchains.STM32
{
    public class STM32ToolchainSettings
    {
        public STM32ToolchainSettings()
        {
            CompileSettings = new CompileSettings();
        }

        public CompileSettings CompileSettings { get; set; }
        public LinkSettings LinkSettings { get; set; }
    }
}
