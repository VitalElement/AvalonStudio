using Avalonia.Threading;
using AvalonStudio.Controls.Editor;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Languages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Utils;
using Dock.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Studio
{
    [Export(typeof(IStudio))]
    [Export(typeof(IExtension))]
    [Shared]
    public class StudioViewModel : ReactiveObject, IStudio, IActivatableExtension
    {
        private WorkspaceTaskRunner _taskRunner;
        private Perspective currentPerspective;
        private bool debugControlsVisible;
        private QuickCommanderViewModel _quickCommander;
        private double _globalZoomLevel;

        [ImportingConstructor]
        public StudioViewModel([ImportMany] IEnumerable<Lazy<IEditorProvider>> editorProviders,
            [ImportMany] IEnumerable<Lazy<ILanguageServiceProvider, LanguageServiceProviderMetadata>> languageServiceProviders,
            [ImportMany] IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> solutionTypes,
            [ImportMany] IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> projectTypes,
            [ImportMany] IEnumerable<Lazy<ITestFramework>> testFrameworks,
            IContentTypeService contentTypeService)
        {
            EditorProviders = editorProviders;
            LanguageServiceProviders = languageServiceProviders;
            SolutionTypes = solutionTypes;
            ProjectTypes = projectTypes;
            TestFrameworks = testFrameworks;

            _taskRunner = new WorkspaceTaskRunner();

            OnSolutionChanged = Observable.FromEventPattern<SolutionChangedEventArgs>(this, nameof(SolutionChanged)).Select(s => s.EventArgs.NewValue);

            CurrentPerspective = Perspective.Normal;

            var editorSettings = Settings.GetSettings<EditorSettings>();

            _globalZoomLevel = editorSettings.GlobalZoomLevel;

            this.WhenAnyValue(x => x.GlobalZoomLevel).Subscribe(zoomLevel =>
            {
                foreach (var document in IoC.Get<IShell>().Documents.OfType<TextEditorViewModel>())
                {
                    document.ZoomLevel = zoomLevel;
                }
            });

            this.WhenAnyValue(x => x.GlobalZoomLevel).Throttle(TimeSpan.FromSeconds(2)).ObserveOn(RxApp.MainThreadScheduler).Subscribe(zoomLevel =>
            {
                var settings = Settings.GetSettings<EditorSettings>();

                settings.GlobalZoomLevel = zoomLevel;

                Settings.SetSettings(settings);
            });

            QuickCommander = new QuickCommanderViewModel();

            EnableDebugModeCommand = ReactiveCommand.Create(() =>
            {
                DebugMode = !DebugMode;
            });
        }

        public DockBase DebugLayout { get; set; }
        public DockBase MainLayout { get; set; }

        public ReactiveCommand<Unit, Unit> EnableDebugModeCommand { get; }

        public IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> SolutionTypes { get; }

        public IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes { get; }

        public IEnumerable<Lazy<ILanguageServiceProvider, LanguageServiceProviderMetadata>> LanguageServiceProviders { get; }

        public IEnumerable<Lazy<ITestFramework>> TestFrameworks { get; }

        public IEnumerable<Lazy<IEditorProvider>> EditorProviders { get; }

        public IWorkspaceTaskRunner TaskRunner => _taskRunner;

        public event EventHandler<BuildEventArgs> BuildStarting;

        public event EventHandler<BuildEventArgs> BuildCompleted;

        private ISolution currentSolution;

        public event EventHandler<SolutionChangedEventArgs> SolutionChanged;

        public IObservable<ISolution> OnSolutionChanged { get; }

        public bool DebugVisible
        {
            get { return debugControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
        }

        public IPerspective DebugPerspective { get; private set; }

        public Perspective CurrentPerspective
        {
            get
            {
                return currentPerspective;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref currentPerspective, value);

                var shell = IoC.Get<IShell>();

                switch (value)
                {
                    case Perspective.Normal:
                        DebugVisible = false;
                        shell.CurrentPerspective = shell.MainPerspective;
                        break;

                    case Perspective.Debugging:
                        // find all tools with debugging perspective attribute
                        // they must also have IsOpen (i.e. user didnt close them)
                        // re-dock them.
                        shell.CurrentPerspective = DebugPerspective;

                        //IoC.Get<IShell>().Layout?.Navigate(DebugLayout);

                        // TODO close intellisense, and tooltips.
                        // disable documents, get rid of error list, solution explorer, etc.    (isreadonly)
                        DebugVisible = true;
                        break;
                }
            }
        }

        public ISolution CurrentSolution
        {
            get
            {
                return currentSolution;
            }
            set
            {
                var oldValue = CurrentSolution;

                this.RaiseAndSetIfChanged(ref currentSolution, value);

                SolutionChanged?.Invoke(this, new SolutionChangedEventArgs() { OldValue = oldValue, NewValue = currentSolution });
            }
        }

        public void ShowQuickCommander()
        {
            this._quickCommander.IsVisible = true;
        }

        public QuickCommanderViewModel QuickCommander
        {
            get { return _quickCommander; }
            set { this.RaiseAndSetIfChanged(ref _quickCommander, value); }
        }

        private ColorScheme _currentColorScheme;

        public ColorScheme CurrentColorScheme
        {
            get { return _currentColorScheme; }

            set
            {
                this.RaiseAndSetIfChanged(ref _currentColorScheme, value);

                foreach (var document in IoC.Get<IShell>().Documents.OfType<EditorViewModel>())
                {
                    document.ColorScheme = value;
                }
            }
        }

        public void Save()
        {
            var shell = IoC.Get<IShell>();

            if (shell.SelectedDocument is ITextDocumentTabViewModel document && document.IsDirty)
            {
                document.Save();
            }
        }

        public void SaveAll()
        {
            var shell = IoC.Get<IShell>();

            foreach (var document in shell.Documents.OfType<ITextDocumentTabViewModel>().Where(x=>x.IsDirty))
            {
                document.Save();
            }
        }

        public void Clean()
        {
            var project = GetDefaultProject();

            if (project != null)
            {
                Clean(project);
            }
        }

        public void Build()
        {
            var project = GetDefaultProject();

            if (project != null)
            {
                BuildAsync(project).GetAwaiter();
            }
        }

        public void Clean(IProject project)
        {
            var console = IoC.Get<IConsole>();

            console.Clear();

            if (project.ToolChain != null)
            {
                BuildStarting?.Invoke(this, new BuildEventArgs(BuildType.Clean, project));

                TaskRunner.RunTask(() =>
                {
                    project.ToolChain.Clean(IoC.Get<IConsole>(), project).Wait();

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        BuildCompleted?.Invoke(this, new BuildEventArgs(BuildType.Clean, project));
                    });
                });
            }
            else
            {
                console.WriteLine($"No toolchain selected for {project.Name}");
            }
        }

        public async Task<bool> BuildAsync(IProject project)
        {
            bool result = false;

            SaveAll();

            var console = IoC.Get<IConsole>();

            console.Clear();

            if (project.ToolChain != null)
            {
                BuildStarting?.Invoke(this, new BuildEventArgs(BuildType.Build, project));

                await TaskRunner.RunTask(() =>
                {
                    result = project.ToolChain.BuildAsync(IoC.Get<IConsole>(), project).GetAwaiter().GetResult();
                });

                Dispatcher.UIThread.Post(() =>
                {
                    BuildCompleted?.Invoke(this, new BuildEventArgs(BuildType.Build, project));
                });
            }
            else
            {
                console.WriteLine($"No toolchain selected for {project.Name}");
            }

            return result;
        }

        public void CloseDocumentsForProject(IProject project)
        {
            var shell = IoC.Get<IShell>();

            var documentsToClose = shell.Documents.ToList();

            foreach (var document in documentsToClose)
            {
                if (document is EditorViewModel evm && evm.SourceFile.Project == project)
                {
                    shell.RemoveDocument(evm);
                }
            }
        }

        private async Task<ITextDocumentTabViewModel> OpenDocumentAsync(ISourceFile file)
        {
            var shell = IoC.Get<IShell>();

            var currentTab = shell.Documents.OfType<ITextDocumentTabViewModel>().FirstOrDefault(t => t.SourceFile?.FilePath == file.FilePath);

            if (currentTab == null)
            {
                var provider = IoC.Get<IStudio>().EditorProviders.FirstOrDefault(p => p.Value.CanEdit(file))?.Value;

                if (provider != null)
                {
                    currentTab = await provider.CreateViewModel(file);
                }
                else
                {
                    var document = await AvalonStudioTextDocument.CreateAsync(file);
                    currentTab = new TextEditorViewModel(document, file);
                }
            }

            shell.AddOrSelectDocument(currentTab);

            return currentTab;
        }

        public async Task<ITextDocumentTabViewModel> OpenDocumentAsync(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true)
        {
            var shell = IoC.Get<IShell>();

            var currentTab = await OpenDocumentAsync(file);

            if (shell.SelectedDocument is ITextDocumentTabViewModel fileTab)
            {
                if (debugHighlight)
                {
                    if(fileTab is IDebugLineDocumentTabViewModel debugLineTab)
                    {
                        debugLineTab.DebugHighlight = new Debugging.DebugHighlightLocation { Line = line, StartColumn = startColumn, EndColumn = endColumn };
                    }
                }

                if (selectLine || debugHighlight)
                {
                    fileTab.GotoPosition(line, startColumn != -1 ? startColumn : 1);
                }

                if (focus)
                {
                    shell.Select(fileTab);
                    fileTab.Focus();
                }

                if (currentTab is TextEditorViewModel editor)
                {
                    return editor;
                }
            }

            return null;
        }

        public void RemoveDocument(ISourceFile file)
        {
            var shell = IoC.Get<IShell>();

            var document = shell.Documents.OfType<ITextDocumentTabViewModel>().FirstOrDefault(d => d.SourceFile == file);

            if (document != null)
            {
                shell.RemoveDocument(document);
            }
        }

        public Task<ITextDocument> CreateDocumentAsync (string path)
        {
            return AvalonStudioTextDocument.CreateAsync(path);
        }

        public ITextEditor GetEditor(string path)
        {
            var shell = IoC.Get<IShell>();

            return shell.Documents.OfType<TextEditorViewModel>().Where(d => d.SourceFile?.FilePath.CompareFilePath(path) == 0).FirstOrDefault();
        }

        public IProject GetDefaultProject()
        {
            IProject result = null;

            if (CurrentSolution != null)
            {
                if (CurrentSolution.StartupProject != null)
                {
                    result = CurrentSolution.StartupProject;
                }
                else
                {
                    //Console.WriteLine("No Default project is set in the solution.");
                }
            }
            else
            {
                //Console.WriteLine("No Solution is loaded.");
            }

            return result;
        }

        public void InvalidateCodeAnalysis()
        {
            //foreach (var document in Documents)
            {
                //TODO implement code analysis trigger.
            }
        }

        public async Task OpenSolutionAsync(string path)
        {
            if (CurrentSolution != null)
            {
                await CloseSolutionAsync();
            }

            if (System.IO.File.Exists(path))
            {
                var extension = System.IO.Path.GetExtension(path);
                var solutionType = IoC.Get<IStudio>().SolutionTypes.FirstOrDefault(
                    s => s.Metadata.SupportedExtensions.Any(e => extension.EndsWith(e)));

                if (solutionType != null)
                {
                    var statusBar = IoC.Get<IStatusBar>();

                    statusBar.SetText($"Loading Solution: {path}");

                    var solution = await solutionType.Value.LoadAsync(path);

                    await solution.LoadSolutionAsync();

                    await solution.RestoreSolutionAsync();

                    statusBar.ClearText();

                    CurrentSolution = solution;

                    await CurrentSolution.LoadProjectsAsync();
                }
            }
        }

        public async Task CloseSolutionAsync()
        {
            // TODO clear error list?
            //IoC.Get<IErrorList>().Errors.Clear();

            if (CurrentSolution != null)
            {
                foreach (var project in CurrentSolution.Projects)
                {
                    CloseDocumentsForProject(project);
                }

                await CurrentSolution.UnloadProjectsAsync();

                await CurrentSolution.UnloadSolutionAsync();

                CurrentSolution = null;
            }
        }

        public void BeforeActivation()
        {
            var shell = IoC.Get<IShell>();

            var debugPerspective = shell.CreatePerspective();

            DebugPerspective = debugPerspective;
        }

        public void Activation()
        {
        }

        public double GlobalZoomLevel
        {
            get { return _globalZoomLevel; }
            set { this.RaiseAndSetIfChanged(ref _globalZoomLevel, value); }
        }

        public bool DebugMode { get; set; }
    }
}
