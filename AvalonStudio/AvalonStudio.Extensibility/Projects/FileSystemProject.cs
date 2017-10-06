namespace AvalonStudio.Projects
{
    using Avalonia.Threading;
    using AvalonStudio.Debugging;
    using AvalonStudio.Platforms;
    using AvalonStudio.TestFrameworks;
    using AvalonStudio.Toolchains;
    using AvalonStudio.Utils;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.IO;
    using System.Linq;

    public abstract class FileSystemProject : IProject, IDisposable
    {
        private FileSystemWatcher fileSystemWatcher;
        private FileSystemWatcher folderSystemWatcher;
        private Dispatcher uiDispatcher;

        public event EventHandler<ISourceFile> FileAdded;

        public FileSystemProject(bool useDispatcher)
        {
            Folders = new ObservableCollection<IProjectFolder>();

            SourceFiles = new List<ISourceFile>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Project = this;

            if (useDispatcher)
            {
                uiDispatcher = Dispatcher.UIThread;
            }
        }

        ~FileSystemProject()
        {
            Dispose();
        }

        public void PopulateFiles(IProjectFolder folder)
        {
            var files = Directory.EnumerateFiles(folder.Location);

            files = files.Where(f => !IsExcluded(ExcludedFiles, CurrentDirectory.MakeRelativePath(f).ToAvalonPath()) && f != Location);

            foreach (var file in files)
            {
                var sourceFile = File.FromPath(this, folder, file.ToPlatformPath().NormalizePath());
                SourceFiles.InsertSorted(sourceFile);
                folder.Items.InsertSorted(sourceFile);

                FileAdded?.Invoke(this, sourceFile);
            }
        }

        private static bool IsExcluded(List<string> exclusionFilters, string path)
        {
            var result = false;

            var filter = exclusionFilters.FirstOrDefault(f => path.Contains(f));

            result = !string.IsNullOrEmpty(filter);

            return result;
        }

        public IProjectFolder GetSubFolders(IProjectFolder parent, string path)
        {
            var result = new StandardProjectFolder(path);

            try
            {
                var folders = Directory.GetDirectories(path);

                if (folders.Count() > 0)
                {
                    foreach (var folder in folders.Where(f => !IsExcluded(ExcludedFiles, CurrentDirectory.MakeRelativePath(f).ToAvalonPath())))
                    {
                        result.Items.InsertSorted(GetSubFolders(result, folder));
                    }
                }

                PopulateFiles(result);

                Folders.InsertSorted(result);
                result.Parent = parent;
                result.Project = this;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public void LoadFiles()
        {
            var folders = GetSubFolders(this, CurrentDirectory);

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
                // Workaround for https://github.com/dotnet/corefx/issues/21934 if we are on unix we need to check if the
                // file exists becuase the notification filters dont work.
                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT || System.IO.File.Exists(e.FullPath))
                {
                    RemoveFile(e.OldFullPath);

                    AddFile(e.FullPath);
                }
            });
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                // Workaround for https://github.com/dotnet/corefx/issues/21934 if we are on unix we need to check if the
                // file exists becuase the notification filters dont work.
                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT || System.IO.File.Exists(e.FullPath))
                {
                    AddFile(e.FullPath);
                }
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
                // Workaround for https://github.com/dotnet/corefx/issues/21934 if we are on unix we need to check if the
                // file exists becuase the notification filters dont work.
                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT || System.IO.Directory.Exists(e.FullPath))
                {
                    RemoveFolder(e.OldFullPath);

                    AddFolder(e.FullPath);
                }
            });
        }

        private void FolderSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                // Workaround for https://github.com/dotnet/corefx/issues/21934 if we are on unix we need to check if the
                // file exists becuase the notification filters dont work.
                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT || System.IO.Directory.Exists(e.FullPath))
                {
                    AddFolder(e.FullPath);
                }
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

            var sourceFile = File.FromPath(this, folder, fullPath.ToPlatformPath().NormalizePath());
            SourceFiles.InsertSorted(sourceFile);

            if (folder != null)
            {
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

                FileAdded?.Invoke(this, sourceFile);
            }
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

            if (folder != null)
            {
                var existing = FindFolder(fullPath);

                if (existing == null)
                {
                    var newFolder = GetSubFolders(folder, fullPath);

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
            file.Parent?.Items.Remove(file);
            SourceFiles.Remove(file);
        }

        public void RemoveFolder(IProjectFolder folder)
        {
            folder.Parent.Items.Remove(folder);
            RemoveFiles(this, folder);

            Folders.Remove(folder);
        }

        private void RemoveFiles(FileSystemProject project, IProjectFolder folder)
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

        private void Invoke(Action action)
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

        [JsonIgnore]
        public IList<ISourceFile> SourceFiles { get; private set; }

        [JsonIgnore]
        public IList<IProjectFolder> Folders { get; private set; }

        #region IProject Implementation

        public abstract IList<object> ConfigurationPages { get; }

        public abstract string CurrentDirectory { get; }

        public abstract IDebugger Debugger2 { get; set; }

        public abstract dynamic DebugSettings { get; set; }

        public abstract dynamic Settings { get; set; }

        public abstract string Executable { get; set; }

        public abstract string Extension { get; }

        public abstract bool Hidden { get; set; }

        public abstract ObservableCollection<IProjectItem> Items { get; }

        public abstract List<string> ExcludedFiles { get; set; }

        public abstract string Location { get; set; }

        public abstract string LocationDirectory { get; }

        public abstract string Name { get; }

        public abstract IProjectFolder Parent { get; set; }

        public abstract IProject Project { get; set; }

        public abstract ObservableCollection<IProject> References { get; }

        public abstract ISolution Solution { get; set; }

        public abstract ITestFramework TestFramework
        {
            get; set;
        }

        public abstract IToolChain ToolChain
        {
            get; set;
        }

        public abstract dynamic ToolchainSettings { get; set; }

        public abstract void AddReference(IProject project);

        public abstract int CompareTo(IProjectFolder other);

        public abstract int CompareTo(string other);

        public abstract int CompareTo(IProject other);

        public abstract int CompareTo(IProjectItem other);

        public abstract void ExcludeFile(ISourceFile file);

        public abstract void ExcludeFolder(IProjectFolder folder);

        public abstract void RemoveReference(IProject project);

        public abstract void ResolveReferences();

        public abstract void Save();

        public abstract IProject Load(ISolution solution, string filePath);

        public void Dispose()
        {
            folderSystemWatcher.Created -= FolderSystemWatcher_Created;
            folderSystemWatcher.Renamed -= FolderSystemWatcher_Renamed;
            folderSystemWatcher.Deleted -= FolderSystemWatcher_Deleted;
            fileSystemWatcher.Changed -= FileSystemWatcher_Changed;
            fileSystemWatcher.Created -= FileSystemWatcher_Created;
            fileSystemWatcher.Renamed -= FileSystemWatcher_Renamed;
            fileSystemWatcher.Deleted -= FileSystemWatcher_Deleted;
        }

        #endregion IProject Implementation
    }
}