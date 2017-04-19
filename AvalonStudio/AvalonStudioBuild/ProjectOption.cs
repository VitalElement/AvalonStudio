using CommandLine;

namespace AvalonStudio
{
    internal abstract class ProjectOption
    {
        [Value(0, MetaName = "Solution", HelpText = "Solution file (asln)", Required = true)]
        public string Solution { get; set; }

        [Value(1, MetaName = "Project", HelpText = "Name of project to run command on")]
        public string Project { get; set; }
    }
}