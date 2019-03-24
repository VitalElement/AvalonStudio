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

        [Option('j', "jobs", Required = false, Default = 0, HelpText = "Number of jobs for compiling.")]
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

    [Verb("create-package", HelpText = "Creates a package.")]
    internal class CreatePackageOptions
    {
        [Value(0, MetaName = "Package", HelpText = "the name of the package to create", Required = true)]
        public string PackageName { get; set; }


        [Value(0, MetaName = "Type", HelpText = "the type of the package (package, toolchain)", Required = true)]
        public string Type { get; set; }

        [Value(1, MetaName = "ConnectionString", HelpText = "the connection string.", Required = true)]
        public string ConnectionString { get; set; }
    }

    [Verb("push-package", HelpText = "Pushes a package.")]
    internal class PushPackageOptions
    {
        [Value(0, MetaName = "File", HelpText = "validated package file to push.", Required = true)]
        public string File { get; set; }

        [Value(0, MetaName = "Package", HelpText = "the name of the package to push", Required = true)]
        public string PackageName { get; set; }

        [Value(0, MetaName = "Platform", HelpText = "the platform the package supports (any, osx-x64, linux-x64, win-x64)", Required = true)]
        public string Platform { get; set; }

        [Value(0, MetaName = "Version", HelpText = "the type of the version of the packages (Maj.Min.Rev.Build)", Required = true)]
        public string Version { get; set; }

        [Value(1, MetaName = "ConnectionString", HelpText = "the connection string.", Required = true)]
        public string ConnectionString { get; set; }
    }

    [Verb("package", HelpText = "Compresses a directory into a valid package. Requires 7z to be available.")]
    internal class PackageOptions
    {
        [Value(0, MetaName = "Source", HelpText = "Directory containing files and manifest.", Required = true)]
        public string SourceDirectory { get; set; }
    }
}