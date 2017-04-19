namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    using System.Collections.Generic;
    public class MsBuild
    {
        public string SolutionPath { get; set; }
        public List<Project> Projects { get; set; }
    }
}