using CommandLine;

namespace AvalonStudio
{
    [Verb("add", HelpText = "Adds source files to the specified project file.")]
    internal class AddOptions : ProjectOption
    {
        [Value(0, Required = true, HelpText = "File name to add to directory.")]
        public string File { get; set; }
    }
}