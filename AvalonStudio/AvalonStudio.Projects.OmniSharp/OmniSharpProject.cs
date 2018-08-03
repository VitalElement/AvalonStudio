using Avalonia.Threading;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.OmniSharp.Roslyn;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp
{
    public class OmniSharpProject : FileSystemProject
    {
        private string detectedTargetPath;
        private FileSystemWatcher fileWatcher;
        private DateTime lastProjectFileRead = DateTime.MinValue;

        public static async Task<OmniSharpProject> Create(ISolution solution, string path)
        {
            var (project, projectReferences, targetPath) = await RoslynWorkspace.GetWorkspace(solution).AddProject(solution.CurrentDirectory, path);

            if (project == null)
            {
                return null;
            }

            var roslynProject = project;
            var references = projectReferences;
            OmniSharpProject result = new OmniSharpProject(path)
            {
                Solution = solution,
                RoslynProject = roslynProject,
                UnresolvedReferences = references,
                detectedTargetPath = targetPath
            };

            return result;
        }

        public OmniSharpProject(string location) : base(true)
        {
            Location = location;
            ExcludedFiles = new List<string>();
            Items = new ObservableCollection<IProjectItem>();
            References = new ObservableCollection<IProject>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Settings = new ExpandoObject();
            Project = this;

            Items.InsertSorted(new ReferenceFolder(this));

            ExcludedFiles.Add("bin");
            ExcludedFiles.Add("obj");

            try
            {
                fileWatcher = new FileSystemWatcher(CurrentDirectory, Path.GetFileName(Location))
                {
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = false,

                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
                };

                fileWatcher.Changed += async (sender, e) =>
                {
                    var lastWriteTime = File.GetLastWriteTime(e.FullPath);

                    if (lastWriteTime != lastProjectFileRead)
                    {
                        lastProjectFileRead = lastWriteTime;

                        RestoreRequired = true;

                        var statusBar = IoC.Get<IStatusBar>();

                        statusBar.SetText($"Project: {Name} has changed, running restore...");

                        await Restore(null, statusBar);

                        RestoreRequired = false;

                        statusBar.SetText($"Project: {Name} has changed, re-evaluating project...");

                        try
                        {
                            // todo restore packages and re-evaluate.
                            var (project, projectReferences, targetPath) = await RoslynWorkspace.GetWorkspace(Solution).ReevaluateProject(this);
                            detectedTargetPath = targetPath;
                            statusBar.ClearText();
                        }
                        catch(Exception)
                        {
                            statusBar.SetText("Error parsing Project File: Please check for valid syntax.");
                        }                        
                    }
                };
            }
            catch (System.IO.IOException e)
            {
                var console = IoC.Get<IConsole>();

                console.WriteLine("Reached Max INotify Limit, to use AvalonStudio on Unix increase the INotify Limit");
                console.WriteLine("often it is set here: '/proc/sys/fs/inotify/max_user_watches'");

                console.WriteLine(e.Message);
            }

            FileAdded += (sender, e) =>
            {
                switch (e.Extension)
                {
                    case ".cs":
                        RoslynWorkspace.GetWorkspace(Solution).AddDocument(RoslynProject, e);
                        break;
                }
            };

            FileRemoved += (sender, e) =>
            {
                switch (e.Extension)
                {
                    case ".cs":
                        RoslynWorkspace.GetWorkspace(Solution).RemoveDocument(RoslynProject, e);
                        break;
                }
            };
        }

        public async Task<bool> Restore(IConsole console, IStatusBar statusBar = null)
        {
            return await Task.Factory.StartNew(() =>
            {
                var exitCode = PlatformSupport.ExecuteShellCommand(DotNetCliService.Instance.Info.Executable, $"restore /m {Path.GetFileName(Location)}", (s, e) =>
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

                return exitCode == 0;
            });
        }

        public bool RestoreRequired { get; set; } = false;

        protected override bool FilterProjectFile => false;

        public List<string> UnresolvedReferences { get; set; }

        public Microsoft.CodeAnalysis.Project RoslynProject { get; set; }

        public override IList<object> ConfigurationPages
        {
            get
            {
                return new List<object>();
            }
        }

        public override string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location) + Platforms.Platform.DirectorySeperator; }
        }

        public override IDebugger Debugger2
        {
            get
            {
                var debugger = IoC.GetInstances<IDebugger>().FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Debugging.DotNetCore.DotNetCoreDebugger");

                return debugger;
            }
            set
            {
            }
        }

        public override dynamic Settings { get; set; }

        public override dynamic DebugSettings { get; set; }

        public override string Executable
        {
            get
            {
                if (detectedTargetPath != null)
                    return detectedTargetPath;
                if (RoslynProject.OutputFilePath == null)
                {
                    return null;
                }

                var objPath = Path.Combine(CurrentDirectory, RoslynProject.OutputFilePath);

                return objPath.Replace("obj", "bin");
            }
            set { }
        }

        public override string Extension
        {
            get
            {
                return ".csproj";
            }
        }

        public override bool Hidden
        {
            get; set;
        }

        public override ObservableCollection<IProjectItem> Items { get; }

        public override string Location
        {
            get; set;
        }

        public override string LocationDirectory => CurrentDirectory;

        public override bool CanRename => false;

        public override string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
            set { }
        }

        public override IProjectFolder Parent { get; set; }

        public override IProject Project { get; set; }

        public override ObservableCollection<IProject> References { get; }

        public override ISolution Solution
        {
            get; set;
        }

        public override ITestFramework TestFramework
        {
            get; set;
        }

        public override IToolchain ToolChain
        {
            get
            {
                var toolchain = IoC.GetInstances<IToolchain>().FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Toolchains.MSBuild.MSBuildToolchain");

                return toolchain;
            }
            set
            {
            }
        }

        public override dynamic ToolchainSettings { get; set; }

        public override void AddReference(IProject project)
        {
            References.Add(project);
        }

        public override int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public override int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public override int CompareTo(IProject other)
        {
            return Name.CompareTo(other.Name);
        }

        public override int CompareTo(IProjectItem other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void ExcludeFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public override void ExcludeFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }

        public override IProject Load(string filePath)
        {
            return null;
        }

        public override bool RemoveReference(IProject project)
        {
            return false;
        }

        public override Task ResolveReferencesAsync()
        {
            if (UnresolvedReferences != null)
            {
                foreach (var unresolvedReference in UnresolvedReferences)
                {
                    var fullReferencePath = this.ResolveReferencePath(unresolvedReference);

                    if (RoslynWorkspace.GetWorkspace(Solution).ResolveReference(this, unresolvedReference))
                    {
                        var currentProject = Solution.FindProjectByPath(fullReferencePath);

                        if (currentProject == null)
                        {
                            throw new Exception("Error loading msbuild project, out of sync");
                        }

                        AddReference(currentProject);
                    }
                    else
                    {
                        AddReference(new UnresolvedReference(Solution, Path.Combine(Solution.CurrentDirectory, Path.GetFileNameWithoutExtension(fullReferencePath))));
                    }
                }
            }

            return Task.CompletedTask;
        }

        internal void MarkRestored()
        {
            foreach (var reference in References)
            {
                if (reference is OmniSharpProject netProject)
                {
                    netProject.MarkRestored();
                }
            }

            RestoreRequired = false;
        }

        public override void Save()
        {
        }

        private static object s_unloadLock = new object();

        public override async Task UnloadAsync()
        {
            await base.UnloadAsync();

            fileWatcher?.Dispose();

            lock (s_unloadLock)
            {
                RoslynProject = null;

                var workspace = RoslynWorkspace.GetWorkspace(Solution, false);

                if (workspace != null)
                {
                    RoslynWorkspace.DisposeWorkspace(Solution);
                }
            }
        }

        public override bool IsItemSupported(string languageName)
        {
            switch(languageName)
            {
                case "C#":
                    return true;

                default:
                    return false;
            }
        }

        public override List<string> ExcludedFiles { get; set; }
    }
}