using CommandLine;
using System.Collections.Generic;

namespace AvalonStudio
{
    [Verb("build", HelpText = "Builds the project in the current directory.")]
    internal class BuildOptions : ProjectOption
    {
        [Option('l', "label", Required = false, Default = "",
            HelpText = "Provides a label to append to the output file name of the build process. Usually a build number.")]
        public string Label { get; set; }

        [Option('D', "define", Required = false, Separator = ':')]
        public IEnumerable<string> Defines { get; set; }

        [Option('j', "jobs", Required = false, Default = 4, HelpText = "Number of jobs for compiling.")]
        public int Jobs { get; set; }
    }

    [Verb("list", HelpText = "Lists available packages.")]
    internal class ListOptions
    {
        [Value(0, MetaName = "List Command", HelpText = "Command (packages,....)", Required = true)]
        public string Command { get; set; }


        [Value(0, MetaName = "List Parameter", HelpText = "Command (packages,....)", Required = false)]
        public string Parameter { get; set; }
    }

    [Verb("install", HelpText = "Installs a package.")]
    internal class InstallOptions
    {
        [Value(0, MetaName = "Package", HelpText = "the name of the package to install.", Required = true)]
        public string PackageName { get; set; }

        [Value(1, MetaName = "Version", HelpText = "the version of the package to install.", Required = false)]
        public string Version { get; set; }
    }

    [Verb("uninstall", HelpText = "Uninstalls a package.")]
    internal class UninstallOptions
    {
        [Value(0, MetaName = "Package", HelpText = "the name of the package to install.", Required = true)]
        public string PackageName { get; set; }

        [Value(1, MetaName = "Version", HelpText = "the version of the package to install.", Required = true)]
        public string Version { get; set; }
    }

    [Verb("print-env", HelpText = "Prints the environment provided by a package.")]
    internal class PrintEnvironmentOptions
    {
        [Value(0, MetaName = "Package", HelpText = "the name of the package environment to print.", Required = true)]
        public string PackageName { get; set; }

        [Value(1, MetaName = "Version", HelpText = "the version of the package environment to print.", Required = false)]
        public string Version { get; set; }
    }
}