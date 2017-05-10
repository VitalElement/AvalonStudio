namespace AvalonStudio.Projects.OmniSharp
{
    using AvalonStudio.GlobalSettings;

    public class DotNetToolchainSettings : SettingsBase
    {
        public DotNetToolchainSettings()
        {
            DotNetPath = "dotnet";
        }

        public string DotNetPath { get; set; }
    }
}