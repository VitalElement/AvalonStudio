namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    using System.Collections.Generic;
    public class Project
    {
        public string ProjectGuid { get; set; }
        public string Path { get; set; }
        public string AssemblyName { get; set; }
        public string TargetPath { get; set; }
        public string TargetFramework { get; set; }
        public List<string> SourceFiles { get; set; }
    }
}