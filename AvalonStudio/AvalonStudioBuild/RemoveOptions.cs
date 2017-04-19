using CommandLine;

namespace AvalonStudio
{
    [Verb("remove", HelpText = "Removes source files to the specified project file.")]
    internal class RemoveOptions : ProjectOption
    {
        [Value(0, Required = true, HelpText = "File name to add to directory.")]
        public string File { get; set; }
    }
}