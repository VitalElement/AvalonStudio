using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using D_Parser.Dom;
using D_Parser.Misc;

namespace AvalonStudio.Projects.DUB
{
    public class DUBProject : FileSystemProject
    {
        public static DUBProject Create(ISolution solution, string path)
        {
            var result = new DUBProject();
            result.Solution = solution;
            result.Location = path;

            result.LoadFiles();

            return result;
        }

        public DUBProject() : base(true)
        {
            ExcludedFiles = new List<string>();
            Items = new ObservableCollection<IProjectItem>();
            References = new ObservableCollection<IProject>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            ParseCache = new LegacyParseCacheView(new List<string>());   
            Project = this;
        }

        public ParseCacheView ParseCache { get; set; }

        public override IList<object> ConfigurationPages
        {
            get
            {
                return new List<object>();
            }
        }

        public override string Extension
        {
            get { return "json"; }
        }

        public override ITestFramework TestFramework { get; set; }
        public override bool Hidden { get; set; }
        public override ObservableCollection<IProjectItem> Items { get; }
        public override string Location { get; set; }
        public override string LocationDirectory => CurrentDirectory;

        public override string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
        }

        public override IProject Project { get; set; }

        public override ObservableCollection<IProject> References { get; }

        public override IToolChain ToolChain
        {
            get
            {
                var result = IoC.Get<IShell>().ToolChains.FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Toolchains.LDC.LDCToolchain");
                return result;
            }
            
            set { }
        }

        public override ISolution Solution { get; set; }

        public override IProjectFolder Parent { get; set; }

        public override List<string> ExcludedFiles { get; set; }

        public override string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location) + Platform.DirectorySeperator; }
        }

        public override IDebugger Debugger { get; set; }
        public override dynamic ToolchainSettings { get; set; }
        public override dynamic DebugSettings { get; set; }

        public override void AddReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public override void RemoveReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public override void ResolveReferences()
        {
            //throw new NotImplementedException();
        }

        public override IProject Load(ISolution solution, string filePath)
        {
            var result = new DUBProject();
            result.Solution = solution;
            result.Location = filePath;

            result.LoadFiles();

            return result;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public override int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public override int CompareTo(IProject other)
        {
            return Name.CompareTo(other.Name);
        }

        public override int CompareTo(IProjectItem other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void ExcludeFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public override void ExcludeFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }

        public override string Executable { get; set; }
    }
}
