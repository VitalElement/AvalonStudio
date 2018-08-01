namespace AvalonStudio.Projects
{
    using Avalonia.Threading;
    using AvalonStudio.Debugging;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Studio;
    using AvalonStudio.Platforms;
    using AvalonStudio.Shell;
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
    using System.Threading.Tasks;

    public abstract class FileSystemProject : IProject, IDisposable
    {
        private FileSystemWatcher fileSystemWatcher;
        private FileSystemWatcher folderSystemWatcher;
        private Dispatcher uiDispatcher;

        public event EventHandler<ISourceFile> FileAdded;
        public event EventHandler<ISourceFile> FileRemoved;

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

        protected virtual bool FilterProjectFile => true;

        [JsonIgnore]
        public Guid Id { get; set; }

        private async Task PopulateFilesAsync(IProjectFolder folder)
        {
            await Task.Run(() =>
            {
                var files = Directory.EnumerateFiles(folder.Location);

                files = files.Where(f => !IsExcluded(f));

                foreach (var file in files)
                {
                    var sourceFile = FileSystemFile.FromPath(this, folder, file.ToPlatformPath().NormalizePath());
                    SourceFiles.InsertSorted(sourceFile);
                    folder.Items.InsertSorted(sourceFile);

                    FileAdded?.Invoke(this, sourceFile);
                }
            });
        }

        private bool IsExcluded(string fullPath)
        {
            var result = false;

            if(FilterProjectFile && fullPath == Location)
            {
                result = true;
            }
            else
            {
                var path = CurrentDirectory.MakeRelativePath(fullPath).ToAvalonPath();

                var filter = ExcludedFiles.FirstOrDefault(f => path.Contains(f));

                result = !string.IsNullOrEmpty(filter);
            }

            return result;
        }

        private async Task<IProjectFolder> GetSubFoldersAsync(IProjectFolder parent, string path)
        {
            var result = new FileSystemFolder(path);

            try
            {
                var folders = Directory.GetDirectories(path);

                if (folders.Count() > 0)
                {
                    foreach (var folder in folders.Where(f => !IsExcluded(f)))
                    {
                        result.Items.InsertSorted(await GetSubFoldersAsync(result, folder));
                    }
                }

                await PopulateFilesAsync(result);

                Folders.InsertSorted(result);
                result.Parent = parent;
                result.Project = this;
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task LoadFilesAsync()
        {
            var folders = await GetSubFoldersAsync(this, CurrentDirectory);

            foreach (var item in folders.Items)
            {
                item.Parent = this;
                Items.InsertSorted(item);                
            }

            foreach (var file in SourceFiles)
            {
                file.Project = this;
            }

            try
            {
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
            catch(System.IO.IOException e)
            {
                var console = IoC.Get<IConsole>();

                console.WriteLine("Reached Max INotify Limit, to use AvalonStudio on Unix increase the INotify Limit");
                console.WriteLine("often it is set here: '/proc/sys/fs/inotify/max_user_watches'");
                
                console.WriteLine(e.Message);
            }
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
            Invoke(async () =>
            {
                // Workaround for https://github.com/dotnet/corefx/issues/21934 if we are on unix we need to check if the
                // file exists becuase the notification filters dont work.
                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT || System.IO.Directory.Exists(e.FullPath))
                {
                    RemoveFolder(e.OldFullPath);

                    await AddFolderAsync(e.FullPath);
                }
            });
        }

        private void FolderSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Invoke(async () =>
            {
                // Workaround for https://github.com/dotnet/corefx/issues/21934 if we are on unix we need to check if the
                // file exists becuase the notification filters dont work.
                if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT || System.IO.Directory.Exists(e.FullPath))
                {
                    await AddFolderAsync(e.FullPath);
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
            if(!IsExcluded(fullPath))
            {
                var folder = FindFolder(Path.GetDirectoryName(fullPath) + "\\");

                var sourceFile = FileSystemFile.FromPath(this, folder, fullPath.ToPlatformPath().NormalizePath());
                
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

        private async Task AddFolderAsync(string fullPath)
        {
            if (!IsExcluded(fullPath))
            {
                var folder = FindFolder(Path.GetDirectoryName(fullPath) + "\\");

                if (folder != null)
                {
                    var existing = FindFolder(fullPath);

                    if (existing == null)
                    {
                        var newFolder = await GetSubFoldersAsync(folder, fullPath);

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

            IoC.Get<IStudio>().RemoveDocument(file);

            FileRemoved?.Invoke(this, file);
        }

        public void RemoveFolder(IProjectFolder folder)
        {
            folder.Parent.Items.Remove(folder);
            RemoveFiles(this, folder);

            Folders.Remove(folder);
        }

        private void RemoveFiles(FileSystemProject project, IProjectFolder folder)
        {
            foreach (var item in folder.Items.ToList())
            {
                if (item is IProjectFolder subfolder)
                {
                    RemoveFiles(project, subfolder);                    
                    project.Folders.Remove(subfolder);
                }

                if (item is ISourceFile file)
                {
                    project.RemoveFile(file);                    
                }
            }
        }

        private void Invoke(Action action)
        {
            if (uiDispatcher != null)
            {
                uiDispatcher.InvokeAsync(() =>
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
        public List<ISourceFile> SourceFiles { get; private set; }

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

        public abstract bool CanRename { get; }

        public abstract string Name { get; set; }

        public abstract IProjectFolder Parent { get; set; }

        public abstract IProject Project { get; set; }

        public abstract ObservableCollection<IProject> References { get; }

        public abstract ISolution Solution { get; set; }

        public abstract ITestFramework TestFramework
        {
            get; set;
        }

        public abstract IToolchain ToolChain
        {
            get; set;
        }

        public abstract dynamic ToolchainSettings { get; set; }
        ISolutionFolder ISolutionItem.Parent { get; set; }

        IReadOnlyList<ISourceFile> IProject.SourceFiles => SourceFiles.AsReadOnly();

        public abstract void AddReference(IProject project);

        public abstract int CompareTo(IProjectFolder other);

        public abstract int CompareTo(string other);

        public abstract int CompareTo(IProject other);

        public abstract int CompareTo(IProjectItem other);

        public abstract void ExcludeFile(ISourceFile file);

        public abstract void ExcludeFolder(IProjectFolder folder);

        public abstract bool RemoveReference(IProject project);

        public abstract Task ResolveReferencesAsync();

        public abstract void Save();

        public abstract IProject Load(string filePath);

        public void Dispose()
        {
            if (folderSystemWatcher != null)
            {
                folderSystemWatcher.Created -= FolderSystemWatcher_Created;
                folderSystemWatcher.Renamed -= FolderSystemWatcher_Renamed;
                folderSystemWatcher.Deleted -= FolderSystemWatcher_Deleted;
            }

            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.Changed -= FileSystemWatcher_Changed;
                fileSystemWatcher.Created -= FileSystemWatcher_Created;
                fileSystemWatcher.Renamed -= FileSystemWatcher_Renamed;
                fileSystemWatcher.Deleted -= FileSystemWatcher_Deleted;
            }
        }

        #endregion IProject Implementation

        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }

        public virtual Task UnloadAsync()
        {
            fileSystemWatcher?.Dispose();
            folderSystemWatcher?.Dispose();

            return Task.CompletedTask;
        }

        public abstract bool IsItemSupported(string languageName);
    }
}