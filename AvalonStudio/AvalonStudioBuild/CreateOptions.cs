using AvalonStudio.Projects.Standard;
using CommandLine;

namespace AvalonStudio
{
    [Verb("create", HelpText = "Creates new projects.")]
    internal class CreateOptions
    {
        [Value(0, Required = true, MetaName = "Project Name", HelpText = "Name of project to create")]
        public string Project { get; set; }

        [Option('t', "Type", Required = true, HelpText = "Options are Executable, StaticLibrary or SuperProject")]
        public ProjectType Type { get; set; }
    }
}