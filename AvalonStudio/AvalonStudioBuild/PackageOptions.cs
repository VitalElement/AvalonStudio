using CommandLine;

namespace AvalonStudio
{
    [Verb("install-package", HelpText = "Installs a package manually.")]
    internal class PackageOptions
    {
        [Value(0, MetaName = "PackageName", HelpText = "Package Name i.e. AvalonStudio.Toolchains.STM32")]
        public string Package { get; set; }

        [Value(1, MetaName = "Tag", HelpText = "")]
        public string Tag { get; set; }
    }
}