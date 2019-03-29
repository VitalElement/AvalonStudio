namespace AvalonStudio.Projects
{
    using AvalonStudio.Debugging;
    using AvalonStudio.Platforms;
    using AvalonStudio.TestFrameworks;
    using AvalonStudio.Toolchains;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading.Tasks;

    public class PlaceHolderProject : IProject
    {
        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }

        public PlaceHolderProject(ISolution solution, string location)
        {
            Location = Path.Combine(solution.CurrentDirectory, location).NormalizePath();
            CurrentDirectory = Path.GetDirectoryName(Location);
        }

        public IProjectFolder Parent { get; set; }

        public ISolution Solution { get; set; }

        public ObservableCollection<IProject> References => null;

        public IToolchain ToolChain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDebugger Debugger2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITestFramework TestFramework { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Hidden { get; set; }

        public string CurrentDirectory { get; }

        public IList<object> ConfigurationPages => throw new NotImplementedException();

        public string Executable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public dynamic ToolchainSettings => throw new NotImplementedException();

        public dynamic DebugSettings => throw new NotImplementedException();

        public dynamic Settings => throw new NotImplementedException();

        public ObservableCollection<IProjectItem> Items => null;

        public string Location { get; set; }

        public string LocationDirectory => throw new NotImplementedException();

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Location);
            }
            set { }
        }

        public bool CanRename => false;

        public IProject Project { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Id { get; set; }

        ISolutionFolder ISolutionItem.Parent { get; set; }

        public IReadOnlyList<ISourceFile> SourceFiles => null;

        public event EventHandler FileAdded;

        event EventHandler<ISourceFile> IProject.FileAdded
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

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
            if (GetType() == other.GetType())
            {
                return Name.CompareTo(other.Name);
            }
            else
            {
                return GetType().FullName.CompareTo(other.GetType().FullName);
            }
        }

        public void Dispose()
        { }

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
            return null;
        }

        public bool RemoveReference(IProject project)
        {
            return false;
        }

        public Task ResolveReferencesAsync()
        {
            return Task.CompletedTask;
        }

        public void Save()
        {

        }

        public Task LoadFilesAsync()
        {
            return Task.CompletedTask;
        }

        public Task UnloadAsync()
        {
            return Task.CompletedTask;
        }

        public bool IsItemSupported(string languageName)
        {
            return false;
        }
    }

    public class NotFoundProject : PlaceHolderProject
    {
        public NotFoundProject(ISolution solution, string location) : base(solution, location)
        {
        }
    }

    public class LoadingProject : PlaceHolderProject
    {
        public LoadingProject(ISolution solution, string location) : base(solution, location)
        {
        }
    }

    public class UnresolvedReference : PlaceHolderProject
    {
        public UnresolvedReference(ISolution solution, string location) : base(solution, location)
        {
        }
    }
}
