using AvalonStudio.Debugging;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Tests
{
    public class TestProject : IProject
    {
        public static readonly Guid TypeId = Guid.Parse("{42C950AB-82C7-447B-9B4A-31E8C85F8083}");

        public TestProject(string location)
        {
            Location = location;
        }

        public ObservableCollection<IProject> References => throw new NotImplementedException();

        public Guid ProjectTypeId => TypeId;

        public IToolChain ToolChain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDebugger Debugger2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITestFramework TestFramework { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Hidden { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string CurrentDirectory => Path.GetDirectoryName(Location) + Platform.DirectorySeperator;

        public IList<object> ConfigurationPages => throw new NotImplementedException();

        public string Executable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public dynamic ToolchainSettings => throw new NotImplementedException();

        public dynamic Settings => throw new NotImplementedException();

        public dynamic DebugSettings => throw new NotImplementedException();

        public ObservableCollection<IProjectItem> Items => throw new NotImplementedException();

        public string Location { get; private set; }

        public string LocationDirectory => throw new NotImplementedException();

        public IProject Project { get; set; }
        public IProjectFolder Parent { get; set; }
        public Guid Id { get; set; }
        public ISolution Solution { get; set; }
        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
            set
            {
                if (value != Name)
                {
                    var newLocation = Path.Combine(CurrentDirectory, value + Path.GetExtension(Location));

                    var current = Location;

                    Location = newLocation;

                    Solution?.UpdateItem(this);
                }
            }
        }

        public bool CanRename => true;

        public IReadOnlyList<ISourceFile> SourceFiles => throw new NotImplementedException();

        ISolutionFolder ISolutionItem.Parent { get; set; }        

        public event EventHandler<ISourceFile> FileAdded;

        public void AddReference(IProject project)
        {
        }
        
        public int CompareTo (IProject other)
        {
            return Name.CompareTo(other.Name);
        }

        public int CompareTo (ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
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

        public void Dispose()
        {
        }

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

        public Task LoadFilesAsync()
        {
            throw new NotImplementedException();
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
    }
}
