using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AvalonStudio.Projects
{
    class UnsupportedProjectType : IProject
    {
        public UnsupportedProjectType(ISolutionFolder parent, string location)
        {
            Name = Path.GetFileName(location);
            Location = location;
            LocationDirectory = Path.GetDirectoryName(location);
            Project = this;
            Parent = this;

            (this as ISolutionItem).Parent = parent;
        }

        public ObservableCollection<IProject> References => null;

        public IToolChain ToolChain { get; set; }
        public IDebugger Debugger2 { get; set; }
        public ITestFramework TestFramework { get; set; }
        public bool Hidden { get; set; }

        public string CurrentDirectory { get; }

        public IList<object> ConfigurationPages => null;

        public string Executable { get; set; }

        public dynamic ToolchainSettings => null;

        public dynamic Settings => null;

        public dynamic DebugSettings => null;

        public ObservableCollection<IProjectItem> Items => null;

        public string Location { get; }

        public string LocationDirectory { get; }

        public IProject Project { get; set; }

        public IProjectFolder Parent { get; set; }
        ISolutionFolder ISolutionItem.Parent { get; set; }

        public Guid Id { get; set; }

        public ISolution Solution { get; set; }

        public string Name { get; }        

        public event EventHandler FileAdded;

        public void AddReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProjectItem other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProjectFolder other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(string other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProject other)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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

        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }
    }
}
