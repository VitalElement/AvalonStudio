using CommandLine;

namespace AvalonStudio
{
    [Verb("addref", HelpText = "Adds a reference to the current project")]
    internal class AddReferenceOptions : ProjectOption
    {
        [Value(1, Required = true, HelpText = "Name of the reference.", MetaName = "Reference Name")]
        public string Name { get; set; }

        [Option('u', "giturl", HelpText = "Url to GitRepository containing a reference.")]
        public string GitUrl { get; set; }

        [Option('r', "revision", HelpText = "Revision to keep the reference at, this can be HEAD, any tag or SHA")]
        public string Revision { get; set; }
    }
}