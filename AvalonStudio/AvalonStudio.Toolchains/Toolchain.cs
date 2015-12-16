namespace AvalonStudio.Toolchains
{
    using AvalonStudio.Utils;
    using Projects;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public abstract class ToolChain
    {
        public static string AppendPath(string path, string addition)
        {
            if (path[path.Length - 1] != ';')
            {
                path += ";";
            }

            return path + addition;
        }

        [XmlIgnore]
        public abstract string GDBExecutable { get; }

        public abstract Task<bool> Build(IConsole console, Project project);

        public abstract Task Clean(IConsole console, Project project);

        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }        
    }

    public class ProcessResult
    {
        public int ExitCode { get; set; }
    }

    public class CompileResult : ProcessResult
    {
        public CompileResult()
        {
            ObjectLocations = new List<string>();
            LibraryLocations = new List<string>();
            ExecutableLocations = new List<string>();
        }

        public Project Project { get; set; }
        public List<string> ObjectLocations { get; set; }
        public List<string> LibraryLocations { get; set; }
        public List<string> ExecutableLocations { get; set; }
        public int NumberOfObjectsCompiled { get; set; }

        public int Count
        {
            get
            {
                return ObjectLocations.Count + LibraryLocations.Count + ExecutableLocations.Count;
            }
        }
    }

    public class LinkResult : ProcessResult
    {
        public string Executable { get; set; }
    }
}
