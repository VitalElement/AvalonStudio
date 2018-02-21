using Avalonia.Threading;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.OmniSharp.DotnetCli;
using AvalonStudio.Projects.OmniSharp.ProjectTypes;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using RoslynPad.Roslyn;
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
        public static async Task<OmniSharpProject> Create(ISolution solution, string path)
        {
            IoC.Get<IStatusBar>().SetText($"Loading projects for solution: {solution.Name}");

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

            var fileWatcher = new FileSystemWatcher(CurrentDirectory, Path.GetFileName(Location))
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,

                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            fileWatcher.Changed += async (sender, e) =>
            {
                RestoreRequired = true;
                // todo restore packages and re-evaluate.
                await RoslynWorkspace.GetWorkspace(Solution).ReevaluateProject(this);
            };

            FileAdded += (sender, e) =>
            {
                switch (e.Extension)
                {
                    case ".cs":
                        RoslynWorkspace.GetWorkspace(Solution).AddDocument(RoslynProject, e);
                        break;
                }
            };
        }

        public async Task<bool> Restore(IConsole console, IStatusBar statusBar = null)
        {
            return await Task.Factory.StartNew(() =>
            {
                var exitCode = PlatformSupport.ExecuteShellCommand(DotNetCliService.Instance.Info.Executable, $"restore {Path.GetFileName(Location)}", (s, e) =>
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
                return new List<object>() { new ToolchainSettingsFormViewModel() };
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
                var shell = IoC.Get<IShell>();

                var debugger = shell.Debugger2s.FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Debugging.DotNetCore.DotNetCoreDebugger");

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

        public override IToolChain ToolChain
        {
            get
            {
                var shell = IoC.Get<IShell>();

                var toolchain = shell.ToolChains.FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Toolchains.MSBuild.MSBuildToolchain");

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
            throw new NotImplementedException();
        }

        public override List<string> ExcludedFiles { get; set; }

        public override Guid ProjectTypeId => DotNetCoreCSharpProjectType.DotNetCoreCSharpTypeId;
    }
}