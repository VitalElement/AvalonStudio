using AvalonStudio.Projects.Standard;
using System.Collections.Generic;

namespace AvalonStudio.Toolchains.Standard
{
    public class CompileResult : ProcessResult
    {
        public CompileResult()
        {
            ObjectLocations = new List<string>();
            LibraryLocations = new List<string>();
            ExecutableLocations = new List<string>();
        }

        public IStandardProject Project { get; set; }
        public List<string> ObjectLocations { get; set; }
        public List<string> LibraryLocations { get; set; }
        public List<string> ExecutableLocations { get; set; }
        public int NumberOfObjectsCompiled { get; set; }

        public int Count
        {
            get { return ObjectLocations.Count + LibraryLocations.Count + ExecutableLocations.Count; }
        }
    }
}