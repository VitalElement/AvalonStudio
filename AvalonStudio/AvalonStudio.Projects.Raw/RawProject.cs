using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using System.IO;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using Avalonia.Threading;
using AvalonStudio.Projects.Standard;
using System.Dynamic;

namespace AvalonStudio.Projects.Raw
{
    public class RawProject : IProject
    {
        private FileSystemWatcher fileSystemWatcher;
        private FileSystemWatcher folderSystemWatcher;
        private Dispatcher uiDispatcher;

        public const string ProjectExtension = "aproj";

        public RawProject() : this(true)
        {

        }

        public RawProject(bool useDispatcher)
        {            
            Items = new ObservableCollection<IProjectItem>();
            Folders = new ObservableCollection<IProjectFolder>();            
            References = new ObservableCollection<IProject>();            
            SourceFiles = new List<ISourceFile>();            
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Project = this;

            if (useDispatcher)
            {
                uiDispatcher = Dispatcher.UIThread;
            }
        }

        public static Solution CreateRawSolution(string path)
        {
            var result = new Solution();

            result.Name = Path.GetFileName(path);
            result.CurrentDirectory = path + Platform.DirectorySeperator;

            result.AddProject(RawProject.Create(result, path));

            return result;
        }

        public static RawProject Create(ISolution solution, string path)
        {
            RawProject project = new RawProject();
            project.Solution = solution;
            project.Location = path;

            project.LoadFiles();

            return project;
        }

        public static void PopulateFiles(RawProject project, StandardProjectFolder folder)
        {
            var files = Directory.EnumerateFiles(folder.Location);            

            foreach (var file in files)
            {
                var sourceFile = RawFile.FromPath(project, folder, file.ToPlatformPath());
                project.SourceFiles.InsertSorted(sourceFile);
                folder.Items.InsertSorted(sourceFile);
            }
        }

        public static StandardProjectFolder GetSubFolders(RawProject project, IProjectFolder parent, string path)
        {
            var result = new StandardProjectFolder(path);

            var folders = Directory.GetDirectories(path);

            if (folders.Count() > 0)
            {
                foreach (var folder in folders)
                {
                    result.Items.InsertSorted(GetSubFolders(project, result, folder));
                }
            }

            PopulateFiles(project, result);

            project.Folders.InsertSorted(result);
            result.Parent = parent;
            result.Project = project;

            return result;
        }

        internal void LoadFiles()
        {
            var folders = GetSubFolders(this, this, CurrentDirectory);

            //Items = new ObservableCollection<IProjectItem>();

            foreach (var item in folders.Items)
            {
                item.Parent = this;
                Items.InsertSorted(item);
            }

            foreach (var file in SourceFiles)
            {
                file.Project = this;
            }

            folderSystemWatcher = new FileSystemWatcher(CurrentDirectory);
            folderSystemWatcher.Created += FolderSystemWatcher_Created;
            folderSystemWatcher.Renamed += FolderSystemWatcher_Renamed;
            folderSystemWatcher.Deleted += FolderSystemWatcher_Deleted;
            folderSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName;
            folderSystemWatcher.IncludeSubdirectories = true;
            folderSystemWatcher.EnableRaisingEvents = true;

            fileSystemWatcher = new FileSystemWatcher(CurrentDirectory);
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                RemoveFile(e.FullPath);
            });
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Invoke(() =>
            {
                RemoveFile(e.OldFullPath);

                AddFile(e.FullPath);
            });
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                AddFile(e.FullPath);
            });
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                FileChanged(e.FullPath);
            });
        }

        private void FolderSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                RemoveFolder(e.FullPath);
            });
        }

        private void FolderSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Invoke(() =>
            {
                RemoveFolder(e.OldFullPath);

                AddFolder(e.FullPath);
            });
        }

        private void FolderSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                AddFolder(e.FullPath);
            });
        }

        public void RemoveFile(string fullPath)
        {
            var file = FindFile(fullPath);

            if (file != null)
            {
                RemoveFile(file);
            }
        }

        public void AddFile(string fullPath)
        {
            var folder = FindFolder(Path.GetDirectoryName(fullPath) + "\\");

            var sourceFile = RawFile.FromPath(this, folder, fullPath.ToPlatformPath());
            SourceFiles.InsertSorted(sourceFile);

            if (folder.Location == Project.CurrentDirectory)
            {
                Project.Items.InsertSorted(sourceFile);
                sourceFile.Parent = Project;
            }
            else
            {
                folder.Items.InsertSorted(sourceFile);
                sourceFile.Parent = folder;
            }

            FileAdded?.Invoke(this, new EventArgs());
        }

        public void FileChanged(string fullPath)
        {
            var file = FindFile(fullPath);

            if (file != null)
            {
                file.RaiseFileModifiedEvent();
            }
        }

        public void RemoveFolder(string folder)
        {
            var existingFolder = FindFolder(folder);

            if (existingFolder != null)
            {
                RemoveFolder(existingFolder);
            }
        }

        public void AddFolder(string fullPath)
        {
            var folder = FindFolder(Path.GetDirectoryName(fullPath) + "\\");

            var existing = FindFolder(fullPath);

            if (existing == null)
            {
                var newFolder = GetSubFolders(this, folder, fullPath);

                if (folder.Location == Project.CurrentDirectory)
                {
                    newFolder.Parent = Project;
                    Project.Items.InsertSorted(newFolder);
                }
                else
                {
                    newFolder.Parent = folder;
                    folder.Items.InsertSorted(newFolder);
                }
            }
        }

        public ISourceFile FindFile(string path)
        {
            return SourceFiles.BinarySearch(path);
        }

        public IProjectFolder FindFolder(string path)
        {
            return Folders.BinarySearch(path);
        }

        public void RemoveFile(ISourceFile file)
        {
            file.Parent.Items.Remove(file);
            SourceFiles.Remove(file);
        }

        public void RemoveFolder(IProjectFolder folder)
        {
            folder.Parent.Items.Remove(folder);
            RemoveFiles(this, folder);

            Folders.Remove(folder);
        }

        private void RemoveFiles(RawProject project, IProjectFolder folder)
        {
            foreach (var item in folder.Items)
            {
                if (item is IProjectFolder)
                {
                    RemoveFiles(project, item as IProjectFolder);
                    project.Folders.Remove(item as IProjectFolder);
                }

                if (item is ISourceFile)
                {
                    project.SourceFiles.Remove(item as ISourceFile);
                }
            }
        }

        void Invoke(Action action)
        {
            if (uiDispatcher != null)
            {
                uiDispatcher.InvokeTaskAsync(() =>
                {
                    action();
                });
            }
            else
            {
                action();
            }
        }

        public IList<ISourceFile> SourceFiles { get; }

        public IList<IProjectFolder> Folders { get; }

        #region IProject Implementation
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
            get { return ProjectExtension; }
        }

        public bool Hidden { get; set; }

        public ObservableCollection<IProjectItem> Items { get; }

        public string Location { get; internal set; }

        public string LocationDirectory => CurrentDirectory;

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
        }

        public IProjectFolder Parent { get; set; }

        public IProject Project { get; set; }

        public ObservableCollection<IProject> References { get; }

        public ISolution Solution { get; set; }

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
            References.InsertSorted(project);
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

        public IProject Load(ISolution solution, string filePath)
        {
            //var result = Load(filePath, solution);

            //return result;

            return null;
        }

        public void RemoveReference(IProject project)
        {
            References.Remove(project);
        }

        public void ResolveReferences()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
