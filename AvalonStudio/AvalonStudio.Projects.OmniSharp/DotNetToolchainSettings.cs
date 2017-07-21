namespace AvalonStudio.Projects.OmniSharp
{
    using AvalonStudio.GlobalSettings;

    public class DotNetToolchainSettings
    {
        public DotNetToolchainSettings()
        {
            DotNetPath = "dotnet";
        }

        public string DotNetPath { get; set; }
    }
}