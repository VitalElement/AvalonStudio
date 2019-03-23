using CommandLine;

namespace AvalonStudio
{
    [Verb("test", HelpText = "Runs all the test project inside a solutions.")]
    internal class TestOptions : ProjectOption
    {
    }
}