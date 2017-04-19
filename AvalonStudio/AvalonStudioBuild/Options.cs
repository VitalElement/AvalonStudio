using CommandLine;

namespace AvalonStudio
{
    internal class Options
    {
        [Option('p', "Project", Required = true, HelpText = "Name of project to build.")]
        public string Project { get; set; }

        [Option('j', "jobs", Required = false, HelpText = "Number of jobs for compiling.")]
        public int Jobs { get; set; }
    }
}