using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using AvalonStudio.Projects.Raw;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System.IO;
using System.Dynamic;

namespace AvalonStudio.Projects.OmniSharp
{
    public class OmniSharpProject : IProject
    {
        public static OmniSharpProject Create(ISolution solution, string path, AvalonStudio.Languages.CSharp.OmniSharp.Project project)
        {
            OmniSharpProject result = new OmniSharpProject();
            result.Solution = solution;
            result.Location = path;

            foreach(var file in project.SourceFiles)
            {
                var sourceFile = RawFile.FromPath(result, result, file.ToPlatformPath());
                result.SourceFiles.InsertSorted(sourceFile);
                result.Items.Add(sourceFile);
            }

            return result;
        }

        public OmniSharpProject()
        {
            Items = new ObservableCollection<IProjectItem>();
            //Folders = new ObservableCollection<IProjectFolder>();
            References = new ObservableCollection<IProject>();
            SourceFiles = new List<ISourceFile>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Project = this;
        }

        public IList<ISourceFile> SourceFiles { get; }

        public IList<object> ConfigurationPages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location) + Platform.DirectorySeperator; }
        }

        public IDebugger Debugger
        {
            get; set;
        }

        public dynamic DebugSettings { get; set; }

        public string Executable { get; set; }

        public string Extension
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Hidden
        {
            get; set;
        }

        public ObservableCollection<IProjectItem> Items { get; }

        public string Location
        {
            get; private set;
        }

        public string LocationDirectory => CurrentDirectory;

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
        }

        public IProjectFolder Parent { get; set; }

        public IProject Project { get; set; }

        public ObservableCollection<IProject> References { get; }

        public ISolution Solution
        {
            get; private set;
        }

        public ITestFramework TestFramework
        {
            get; set;
        }

        public IToolChain ToolChain
        {
            get; set;
        }

        public dynamic ToolchainSettings { get; set; }

        public event EventHandler FileAdded;

        public void AddReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public int CompareTo(IProject other)
        {
            return Name.CompareTo(other.Name);
        }

        public int CompareTo(IProjectItem other)
        {
            return Name.CompareTo(other.Name);
        }

        public void ExcludeFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public void ExcludeFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }

        public ISourceFile FindFile(string path)
        {
            throw new NotImplementedException();
        }

        public IProject Load(ISolution solution, string filePath)
        {
            return null;
        }

        public void RemoveReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public void ResolveReferences()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
