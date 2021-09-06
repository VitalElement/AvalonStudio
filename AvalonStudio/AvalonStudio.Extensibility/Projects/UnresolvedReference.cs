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

        public ObservableCollection<IProject> References { get; } = new ObservableCollection<IProject>();

        public IToolchain ToolChain { get; set; }
        public IDebugger Debugger2 { get; set; }
        public ITestFramework TestFramework { get; set; }
        public bool Hidden { get; set; }

        public string CurrentDirectory { get; }

        public IList<object> ConfigurationPages => null;

        public string Executable { get; set; }

        public dynamic ToolchainSettings { get; }

        public dynamic DebugSettings { get; }

        public dynamic Settings { get; }

        public ObservableCollection<IProjectItem> Items => null;

        public string Location { get; set; }

        public string LocationDirectory { get; }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Location);
            }
            set { }
        }

        public bool CanRename => false;

        public IProject Project { get; set; }

        public Guid Id { get; set; }

        ISolutionFolder ISolutionItem.Parent { get; set; }

        public IReadOnlyList<ISourceFile> SourceFiles => null;

        public event EventHandler<ISourceFile> FileAdded;


        public void AddReference(IProject project)
        {
        }

        public int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public int CompareTo(IProjectItem other)
        {
            return Name.CompareTo(other.Name);
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
        }

        public void ExcludeFolder(IProjectFolder folder)
        {
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
