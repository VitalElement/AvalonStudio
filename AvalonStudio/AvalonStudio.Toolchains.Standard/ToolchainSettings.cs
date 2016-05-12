namespace AvalonStudio.Toolchains
{
    using System.Collections.Generic;

    public class ToolchainSettings
    {
        public ToolchainSettings()
        {
            IncludePaths = new List<string>();
        }

        public string ToolChainLocation { get; set; }
        public List<string> IncludePaths { get; set; }
    }
}
