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
}