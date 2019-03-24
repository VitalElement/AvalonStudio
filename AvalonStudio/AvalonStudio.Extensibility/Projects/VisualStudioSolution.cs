using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Utils;
using Microsoft.DotNet.Cli.Sln.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AvalonStudio.Platforms;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility.Threading;
using Avalonia.Platform;
using Avalonia;

namespace AvalonStudio.Projects
{
    public class VisualStudioSolution : ISolution
    {
        private SlnFile _solutionModel;
        private Dictionary<Guid, ISolutionItem> _solutionItems;

        public const string Extension = "sln";

        public static VisualStudioSolution Load(string fileName)
        {
            return new VisualStudioSolution(SlnFile.Read(fileName));
        }

        public bool IsRestored { get; set; } = false;

        private object _restoreLock = new object();

        private TaskCompletionSource<bool> _restoreTaskCompletionSource;

        /// <summary>
        /// Allows disabling of serialization to disk. Useful for UnitTesting.
        /// </summary>
        public bool SaveEnabled { get; set; } = true;

        public SlnFile Model => _solutionModel;

        public static VisualStudioSolution Create(string location, string name, bool save = true, string extension = Extension)
        {
            var filePath = Path.Combine(location, name + "." + extension);

            var result = new VisualStudioSolution(new SlnFile { FullPath = filePath, FormatVersion = "12.00", MinimumVisualStudioVersion = "10.0.40219.1", VisualStudioVersion = "15.0.27009.1" });

            if (save)
            {
                result.Save();
            }

            return result;
        }

        private VisualStudioSolution(SlnFile solutionModel)
        {
            _solutionModel = solutionModel;
            _solutionItems = new Dictionary<Guid, ISolutionItem>();

            _solutionModel.SolutionConfigurationsSection["Debug|Any CPU"] = "Debug|Any CPU";
            _solutionModel.SolutionConfigurationsSection["Release|Any CPU"] = "Release|Any CPU";

            Parent = Solution = this;

            Id = Guid.NewGuid();

            Items = new ObservableCollection<ISolutionItem>();
        }

        public async Task LoadSolutionAsync()
        {
            await Task.Run(async () =>
            {
                await LoadFoldersAsync();

                await LoadProjectLoadingPlaceholdersAsync();

                var platform = AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>();

                if (platform != null)
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        BuildTree();
                    });
                }
                else
                {
                    BuildTree();
                }
            });
        }

        public async Task RestoreSolutionAsync()
        {
            if (!string.IsNullOrEmpty(DotNetCliService.Instance.DotNetPath))
            {
                await Restore(null, IoC.Get<IStatusBar>());
            }
        }

        public async Task LoadProjectsAsync()
        {
            await LoadProjectsImplAsync();

            await ResolveReferencesAsync();
        }

        private async Task LoadFilesAsync()
        {
            foreach (var project in Projects)
            {
                await project.LoadFilesAsync();
            }
        }

        private void BuildTree()
        {
            var nestedProjects = _solutionModel.Sections.FirstOrDefault(section => section.Id == "NestedProjects");

            if (nestedProjects != null)
            {
                foreach (var nestedProject in nestedProjects.Properties)
                {
                    _solutionItems[Guid.Parse(nestedProject.Key)].SetParentInternal(_solutionItems[Guid.Parse(nestedProject.Value)] as ISolutionFolder);
                }
            }
        }

        private async Task LoadFoldersAsync()
        {
            var solutionFolders = _solutionModel.Projects.Where(p => p.TypeGuid == ProjectTypeGuids.SolutionFolderGuid);

            var newItems = new List<ISolutionItem>();

            foreach (var solutionFolder in solutionFolders)
            {
                var newItem = new SolutionFolder(solutionFolder.Name);
                newItem.Id = Guid.Parse(solutionFolder.Id);
                newItem.Parent = this;
                newItem.Solution = this;
                _solutionItems.Add(newItem.Id, newItem);

                newItems.Add(newItem);
            }

            var platform = AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>();

            if (platform != null)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var newItem in newItems)
                    {
                        Items.InsertSorted(newItem, false);
                    }
                });
            }
            else
            {
                foreach (var newItem in newItems)
                {
                    Items.InsertSorted(newItem, false);
                }
            }
        }

        private async Task ResolveReferencesAsync()
        {
            var statusBar = IoC.Get<IStatusBar>();

            foreach (var project in Projects)
            {
                statusBar.SetText($"Resolving References: {project.Name}");

                await project.ResolveReferencesAsync();
            }

            statusBar.ClearText();
        }

        public Task<bool> Restore(IConsole console, IStatusBar statusBar = null, bool checkLock = false)
        {
            bool restore = !checkLock;

            if (checkLock)
            {
                lock (_restoreLock)
                {
                    restore = !IsRestored;

                    if (restore)
                    {
                        IsRestored = true;

                        _restoreTaskCompletionSource = new TaskCompletionSource<bool>();
                    }
                }
            }

            if (restore)
            {
                return Task.Factory.StartNew(() =>
                {
                    statusBar.SetText($"Restoring Packages for solution: {Name}");

                    var exitCode = PlatformSupport.ExecuteShellCommand(DotNetCliService.Instance.DotNetPath, $"restore /m {Path.GetFileName(Location)}", (s, e) =>
                    {
                        if (statusBar != null)
                        {
                            if (!string.IsNullOrWhiteSpace(e.Data))
                            {
                                Dispatcher.UIThread.InvokeAsync(() =>
                                {
                                    statusBar.SetText(e.Data.Trim());
                                });
                            }
                        }

                        console?.WriteLine(e.Data);
                    }, (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            if (console != null)
                            {
                                console.WriteLine();
                                console.WriteLine(e.Data);
                            }
                        }
                    },
                    false, CurrentDirectory, false);

                    IsRestored = true;

                    var result = exitCode == 0;

                    _restoreTaskCompletionSource?.SetResult(result);

                    lock (_restoreLock)
                    {
                        _restoreTaskCompletionSource = null;
                    }

                    return result;
                });
            }
            else
            {
                lock (_restoreLock)
                {
                    if (_restoreTaskCompletionSource != null)
                    {
                        return _restoreTaskCompletionSource.Task;
                    }
                    else
                    {
                        return Task.FromResult(true);
                    }
                }
            }
        }

        private async Task LoadProjectsImplAsync()
        {
            var statusBar = IoC.Get<IStatusBar>();

            var solutionProjects = _solutionModel.Projects.Where(p => p.TypeGuid != ProjectTypeGuids.SolutionFolderGuid);

            var tasks = new List<Task<IProject>>();

            var jobrunner = new JobRunner<(ISolutionItem placeHolder, SlnProject projectInfo, string path), IProject>(Environment.ProcessorCount, loadInfo =>
            {
                var threadingPlatform = AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>();

                if (threadingPlatform != null)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        statusBar.SetText($"Loading Project: {loadInfo.path}");
                    });
                }

                var task = ProjectUtils.LoadProjectFileAsync(this, Guid.Parse(loadInfo.projectInfo.TypeGuid), loadInfo.path);

                var result = task.GetAwaiter().GetResult();

                if (result != null)
                {
                    result.Id = loadInfo.placeHolder.Id;
                    result.Solution = this;

                    result.LoadFilesAsync().Wait();
                }

                return result;
            });

            foreach (var project in solutionProjects)
            {
                var placeHolder = _solutionItems[Guid.Parse(project.Id)];

                tasks.Add(jobrunner.Add((placeHolder, project, Path.Combine(CurrentDirectory, project.FilePath))));
            }

            while (true)
            {
                var currentProjectTask = await Task.WhenAny(tasks);

                tasks.Remove(currentProjectTask);

                if (currentProjectTask.IsCompleted)
                {
                    var newProject = currentProjectTask.Result;

                    if (newProject != null)
                    {
                        var placeHolder = _solutionItems[newProject.Id];

                        if (newProject != null)
                        {
                            SetItemParent(newProject, placeHolder.Parent);

                            placeHolder.SetParentInternal(null);
                            _solutionItems.Remove(placeHolder.Id);

                            _solutionItems.Add(newProject.Id, newProject);
                        }
                    }
                }

                if (tasks.Count == 0)
                {
                    break;
                }
            }

            var platform = AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>();

            if (platform != null)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    statusBar.ClearText();
                });
            }

            jobrunner.Stop();
        }

        private async Task LoadProjectLoadingPlaceholdersAsync()
        {
            var solutionProjects = _solutionModel.Projects.Where(p => p.TypeGuid != ProjectTypeGuids.SolutionFolderGuid);

            var newItems = new List<ISolutionItem>();

            foreach (var project in solutionProjects)
            {
                if (File.Exists(project.FilePath))
                {
                    var newProject = new LoadingProject(this, project.FilePath)
                    {
                        Id = Guid.Parse(project.Id),
                        Solution = this
                    };

                    (newProject as ISolutionItem).Parent = this;

                    _solutionItems.Add(newProject.Id, newProject);
                    newItems.Add(newProject);
                }
                else
                {
                    var newProject = new NotFoundProject(this, project.FilePath)
                    {
                        Id = Guid.Parse(project.Id),
                        Solution = this
                    };

                    (newProject as ISolutionItem).Parent = this;

                    _solutionItems.Add(newProject.Id, newProject);
                    newItems.Add(newProject);
                }
            }

            var platform = AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>();

            if (platform != null)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var newItem in newItems)
                    {
                        Items.InsertSorted(newItem);
                    }
                });
            }
            else
            {
                foreach (var newItem in newItems)
                {
                    Items.InsertSorted(newItem);
                }
            }
        }

        public bool CanRename => true;

        public string Name
        {
            get => Path.GetFileNameWithoutExtension(_solutionModel.FullPath);
            set
            {
                if (value != Name)
                {
                    var newLocation = Path.Combine(CurrentDirectory, value + Path.GetExtension(Location));

                    System.IO.File.Move(Location, newLocation);

                    _solutionModel.FullPath = newLocation;
                }

                Save();
            }
        }

        public string Location => _solutionModel.FullPath;

        public IProject StartupProject
        {
            get
            {
                var avalonStudioProperties = _solutionModel.Sections.FirstOrDefault(section => section.Id == "AvalonStudioProperties");

                IProject result = null;

                if (avalonStudioProperties != null && avalonStudioProperties.Properties.ContainsKey("StartupItem"))
                {
                    result = _solutionItems[Guid.Parse(avalonStudioProperties.Properties["StartupItem"])] as IProject;
                }

                return result;
            }

            set
            {
                var avalonStudioProperties = _solutionModel.Sections.FirstOrDefault(section => section.Id == "AvalonStudioProperties");

                if (value == null)
                {
                    if (avalonStudioProperties != null)
                    {
                        _solutionModel.Sections.Remove(avalonStudioProperties);
                    }
                }
                else
                {
                    if (avalonStudioProperties == null)
                    {
                        _solutionModel.Sections.Add(new SlnSection() { Id = "AvalonStudioProperties", SectionType = SlnSectionType.PreProcess });
                        avalonStudioProperties = _solutionModel.Sections.FirstOrDefault(section => section.Id == "AvalonStudioProperties");
                    }

                    avalonStudioProperties.Properties["StartupItem"] = value.Id.GetGuidString();
                }

                Save();
            }
        }

        public IEnumerable<IProject> Projects => _solutionItems.Select(kv => kv.Value).OfType<IProject>();

        public string CurrentDirectory => Path.GetDirectoryName(_solutionModel.FullPath) + Platforms.Platform.DirectorySeperator;

        public ObservableCollection<ISolutionItem> Items { get; private set; }

        public ISolution Solution { get; set; }

        public ISolutionFolder Parent { get; set; }

        public Guid Id { get; set; }

        public void UpdateItem(ISolutionItem item)
        {
            var slnProject = _solutionModel.Projects.FirstOrDefault(p => Guid.Parse(p.Id) == item.Id);

            if (slnProject != null)
            {
                if (item is ISolutionFolder)
                {
                    slnProject.FilePath = slnProject.Name = item.Name;
                }
                else if (item is IProject project)
                {
                    slnProject.FilePath = CurrentDirectory.MakeRelativePath(project.Location);
                    slnProject.Name = project.Name;
                }

                Save();
            }
        }

        public T AddItem<T>(T item, Guid? itemGuid = null, ISolutionFolder parent = null) where T : ISolutionItem
        {
            item.Id = Guid.NewGuid();
            item.Solution = this;

            if (item is IProject project)
            {
                var currentProject = Projects.FirstOrDefault(p => p.Location.IsSamePathAs(project.Location));

                if (currentProject == null)
                {
                    SetItemParent(project, parent ?? this);

                    _solutionItems.Add(project.Id, project);

                    _solutionModel.Projects.Add(new SlnProject
                    {
                        Id = project.Id.GetGuidString(),
                        TypeGuid = itemGuid?.GetGuidString(),
                        Name = project.Name,
                        FilePath = CurrentDirectory.MakeRelativePath(project.Location)
                    });

                    var debug1 = new SlnPropertySet(project.Id.GetGuidString()); debug1["Debug|Any CPU.ActiveCfg"] = "Debug|Any CPU";
                    var debug2 = new SlnPropertySet(project.Id.GetGuidString()); debug2["Debug|Any CPU.Build.0"] = "Debug|Any CPU";
                    var release1 = new SlnPropertySet(project.Id.GetGuidString()); release1["Release|Any CPU.ActiveCfg"] = "Release|Any CPU";
                    var release2 = new SlnPropertySet(project.Id.GetGuidString()); release2["Release|Any CPU.Build.0"] = "Release|Any CPU";

                    _solutionModel.ProjectConfigurationsSection.Add(debug1);
                    _solutionModel.ProjectConfigurationsSection.Add(debug2);
                    _solutionModel.ProjectConfigurationsSection.Add(release1);
                    _solutionModel.ProjectConfigurationsSection.Add(release2);

                    return (T)project;
                }
                else
                {
                    SetItemParent(currentProject, parent ?? this);

                    return (T)currentProject;
                }
            }
            else if (item is ISolutionFolder folder)
            {
                SetItemParent(folder, parent ?? this);

                _solutionModel.Projects.Add(new SlnProject
                {
                    Id = folder.Id.GetGuidString(),
                    TypeGuid = ProjectTypeGuids.SolutionFolderGuid,
                    Name = folder.Name,
                    FilePath = folder.Name
                });

                return (T)folder;
            }

            return item;
        }

        public void RemoveItem(ISolutionItem item)
        {
            if (item is ISolution)
            {
                throw new InvalidOperationException();
            }

            if (item is IProject project)
            {
                foreach (var parent in Projects.Where(p => p != project))
                {
                    if (parent.RemoveReference(project))
                    {
                        parent.Save();
                    }
                }
            }

            if (item is ISolutionFolder folder)
            {
                foreach (var child in folder.Items.ToList())
                {
                    RemoveItem(child);
                }
            }

            if (item == StartupProject)
            {
                StartupProject = null;
            }

            SetItemParent(item, null);

            _solutionItems.Remove(item.Id);

            var currentSlnProject = _solutionModel.Projects.FirstOrDefault(slnProj => Guid.Parse(slnProj.Id) == item.Id);

            if (currentSlnProject != null)
            {
                _solutionModel.Projects.Remove(currentSlnProject);
            }

            var propsToRemove = _solutionModel.ProjectConfigurationsSection.Where(props => Guid.Parse(props.Id) == item.Id).ToList();

            foreach (var prop in propsToRemove)
            {
                _solutionModel.ProjectConfigurationsSection.Remove(prop);
            }
        }

        public ISourceFile FindFile(string path)
        {
            ISourceFile result = null;

            foreach (var project in Projects)
            {
                result = project.FindFile(path);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public void Save()
        {
            if (SaveEnabled)
            {
                _solutionModel.Write();
            }
        }

        private void SetItemParent(ISolutionItem item, ISolutionFolder parent)
        {
            var nestedProjects = _solutionModel.Sections.FirstOrDefault(section => section.Id == "NestedProjects");

            if (nestedProjects != null)
            {
                nestedProjects.Properties.Remove(item.Id.GetGuidString());
            }

            item.SetParentInternal(parent);

            if (parent != this)
            {
                if (nestedProjects == null)
                {
                    _solutionModel.Sections.Add(new SlnSection() { Id = "NestedProjects", SectionType = SlnSectionType.PreProcess });
                    nestedProjects = _solutionModel.Sections.FirstOrDefault(section => section.Id == "NestedProjects");
                }

                if (parent != null)
                {
                    nestedProjects.Properties[item.Id.GetGuidString()] = parent.Id.GetGuidString();
                }
            }
        }

        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }

        public void VisitChildren(Action<ISolutionItem> visitor)
        {
            foreach (var child in Items)
            {
                if (child is ISolutionFolder folder)
                {
                    folder.VisitChildren(visitor);
                }

                visitor(child);
            }
        }

        public IProject FindProject(string name)
        {
            return Projects.FirstOrDefault(p => p.Name == name);
        }

        public IProject FindProjectByPath(string absolutePath)
        {
            return Projects.FirstOrDefault(p => p.Location.NormalizePath() == absolutePath.NormalizePath());
        }

        public Task UnloadSolutionAsync()
        {
            return Task.CompletedTask;
        }

        public async Task UnloadProjectsAsync()
        {
            // TODO parallelise this on a job runner.
            foreach (var project in Projects)
            {
                await project.UnloadAsync();
            }
        }
    }
}
