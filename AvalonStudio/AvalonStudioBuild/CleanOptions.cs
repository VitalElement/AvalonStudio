using CommandLine;

namespace AvalonStudio
{
    [Verb("clean", HelpText = "Cleans the specified project.")]
    internal class CleanOptions : ProjectOption
    {
    }
}