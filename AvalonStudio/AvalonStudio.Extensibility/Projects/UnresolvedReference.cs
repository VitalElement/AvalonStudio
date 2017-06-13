namespace AvalonStudio.Projects
{
    using AvalonStudio.Debugging;
    using AvalonStudio.TestFrameworks;
    using AvalonStudio.Toolchains;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    public class UnresolvedReference : IProject
    {
        public UnresolvedReference(ISolution solution, string location)
        {
            Location = location;
        }

        public ISolution Solution {get; set;}

        public ObservableCollection<IProject> References => null;

        public IToolChain ToolChain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDebugger Debugger2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITestFramework TestFramework { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Hidden { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string CurrentDirectory => throw new NotImplementedException();

        public IList<object> ConfigurationPages => throw new NotImplementedException();

        public string Executable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public dynamic ToolchainSettings => throw new NotImplementedException();

        public dynamic DebugSettings => throw new NotImplementedException();

        public dynamic Settings => throw new NotImplementedException();

        public ObservableCollection<IProjectItem> Items => throw new NotImplementedException();

        public string Location {get; set;}

        public string LocationDirectory => throw new NotImplementedException();

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
        }

        public IProject Project { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IProjectFolder Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
