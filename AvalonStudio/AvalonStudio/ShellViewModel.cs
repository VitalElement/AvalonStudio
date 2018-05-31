using Avalonia.Input;
using Avalonia.Threading;
using AvalonStudio.Commands;
using AvalonStudio.Commands.Settings;
using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.ErrorList;
using AvalonStudio.Controls.Standard.SolutionExplorer;
using AvalonStudio.Debugging;
using AvalonStudio.Docking;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.MainMenu;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Languages;
using AvalonStudio.MainMenu;
using AvalonStudio.Menus.Models;
using AvalonStudio.Menus.ViewModels;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolbars;
using AvalonStudio.Toolbars.ViewModels;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using Dock.Model;
using Dock.Model.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio
{
    [Export]
    [Export(typeof(IShell))]
    [Shared]
    internal class ShellViewModel : ViewModel, IShell
    {
        public static ShellViewModel Instance { get; internal set; }
        private WorkspaceTaskRunner _taskRunner;
        private double _globalZoomLevel;
        private List<KeyBinding> _keyBindings;

        private IEnumerable<ToolbarViewModel> _toolbars;

        private Lazy<StatusBarViewModel> _statusBar;

        private IEnumerable<Lazy<IEditorProvider>> _editorProviders;
        private IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> _languageServices;

        private IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> _solutionTypes;
        private IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> _projectTypes;

        private IEnumerable<Lazy<IToolchain>> _toolChains;
        private IEnumerable<IDebugger> _debugger2s;

        private IEnumerable<Lazy<ITestFramework>> _testFrameworks;

        private Perspective currentPerspective;

        private ISolution currentSolution;

        private bool debugControlsVisible;

        private ModalDialogViewModelBase modalDialog;

        private QuickCommanderViewModel _quickCommander;

        private ObservableCollection<object> tools;

        [ImportingConstructor]
        public ShellViewModel(
            CommandService commandService,
            Lazy<StatusBarViewModel> statusBar,
            IContentTypeService contentTypeService,
            MainMenuService mainMenuService,
            ToolbarService toolbarService,
            [ImportMany] IEnumerable<Lazy<IEditorProvider>> editorProviders,
            [ImportMany] IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> languageServices,
            [ImportMany] IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> solutionTypes,
            [ImportMany] IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> projectTypes,
            [ImportMany] IEnumerable<Lazy<IToolchain>> toolChains,
            [ImportMany] IEnumerable<IDebugger> debugger2s,
            [ImportMany] IEnumerable<Lazy<ITestFramework>> testFrameworks,
            [ImportMany] IEnumerable<Lazy<IExtension>> extensions)
        {  
            MainMenu = mainMenuService.GetMainMenu();

            var toolbars = toolbarService.GetToolbars();
            StandardToolbar = toolbars.Single(t => t.Key == "Standard").Value;

            _statusBar = statusBar;
            IoC.RegisterConstant<IStatusBar>(_statusBar.Value);

            _editorProviders = editorProviders;
            _languageServices = languageServices;

            _solutionTypes = solutionTypes;
            _projectTypes = projectTypes;

            _toolChains = toolChains;
            _debugger2s = debugger2s;

            _testFrameworks = testFrameworks;
            
            _keyBindings = new List<KeyBinding>();

            IoC.RegisterConstant<IShell>(this);
            IoC.RegisterConstant(this);

            var factory = new DefaultLayoutFactory();
            Factory = factory;
            Layout = Factory.CreateLayout();
            Factory.InitLayout(Layout, this);

            _leftPane = factory.LeftDock;
            _documentDock = factory.DocumentDock;
            _rightPane = factory.RightDock;
            _bottomPane = factory.BottomDock;

            foreach (var extension in extensions)
            {
                extension.Value.BeforeActivation();
            }

            CurrentPerspective = Perspective.Editor;

            DocumentTabs = new DocumentTabControlViewModel();

            Console = IoC.Get<IConsole>();
            ErrorList = IoC.Get<IErrorList>();

            tools = new ObservableCollection<object>();

            LeftTabs = new TabControlViewModel();
            RightTabs = new TabControlViewModel();
            BottomTabs = new TabControlViewModel();
            BottomRightTabs = new TabControlViewModel();
            RightBottomTabs = new TabControlViewModel();
            RightMiddleTabs = new TabControlViewModel();
            RightTopTabs = new TabControlViewModel();
            MiddleTopTabs = new TabControlViewModel();

            ModalDialog = new ModalDialogViewModelBase("Dialog");

            OnSolutionChanged = Observable.FromEventPattern<SolutionChangedEventArgs>(this, nameof(SolutionChanged)).Select(s => s.EventArgs.NewValue);

            _taskRunner = new WorkspaceTaskRunner();

            QuickCommander = new QuickCommanderViewModel();

            foreach (var extension in extensions)
            {
                extension.Value.Activation();
            }

            foreach (var command in commandService.GetKeyGestures())
            {
                foreach (var keyGesture in command.Value)
                {
                    _keyBindings.Add(new KeyBinding { Command = command.Key.Command, Gesture = KeyGesture.Parse(keyGesture) });
                }
            }                        

            foreach (var tool in extensions.Select(e => e.Value).OfType<ToolViewModel>())
            {
                tools.Add(tool);

                switch (tool.DefaultLocation)
                {
                    case Location.Bottom:
                        _bottomPane.Views.Add(tool);

                        factory.Update(tool, tool, _bottomPane);
                        break;

                    case Location.BottomRight:
                        BottomRightTabs.Tools.Add(tool);
                        break;

                    case Location.RightBottom:
                        RightBottomTabs.Tools.Add(tool);
                        break;

                    case Location.RightMiddle:
                        RightMiddleTabs.Tools.Add(tool);
                        break;

                    case Location.RightTop:
                        RightTopTabs.Tools.Add(tool);
                        break;

                    case Location.MiddleTop:
                        MiddleTopTabs.Tools.Add(tool);
                        break;

                    case Location.Left:
                        _leftPane.Views.Add(tool);

                        factory.Update(tool, tool, _leftPane);
                        break;

                    case Location.Right:
                        _rightPane.Views.Add(tool);

                        factory.Update(tool, tool, _rightPane);
                        break;
                }
            }

            LeftTabs.SelectedTool = LeftTabs.Tools.Where(t=>t.IsVisible).FirstOrDefault();
            RightTabs.SelectedTool = RightTabs.Tools.Where(t => t.IsVisible).FirstOrDefault();
            BottomTabs.SelectedTool = BottomTabs.Tools.Where(t => t.IsVisible).FirstOrDefault();
            BottomRightTabs.SelectedTool = BottomRightTabs.Tools.Where(t => t.IsVisible).FirstOrDefault();
            RightTopTabs.SelectedTool = RightTopTabs.Tools.Where(t => t.IsVisible).FirstOrDefault();
            RightMiddleTabs.SelectedTool = RightMiddleTabs.Tools.Where(t => t.IsVisible).FirstOrDefault();
            RightBottomTabs.SelectedTool = RightBottomTabs.Tools.Where(t => t.IsVisible).FirstOrDefault();
            MiddleTopTabs.SelectedTool = MiddleTopTabs.Tools.Where(t => t.IsVisible).FirstOrDefault();

            IoC.Get<IStatusBar>().ClearText();

            ProcessCancellationToken = new CancellationTokenSource();

            CurrentPerspective = Perspective.Editor;

            var editorSettings = Settings.GetSettings<EditorSettings>();

            _globalZoomLevel = editorSettings.GlobalZoomLevel;

            this.WhenAnyValue(x => x.GlobalZoomLevel).Subscribe(zoomLevel =>
            {
                foreach (var document in DocumentTabs.Documents.OfType<EditorViewModel>())
                {
                    document.ZoomLevel = zoomLevel;
                }
            });

            this.WhenAnyValue(x => x.GlobalZoomLevel).Throttle(TimeSpan.FromSeconds(2)).Subscribe(zoomLevel =>
            {
                var settings = Settings.GetSettings<EditorSettings>();

                settings.GlobalZoomLevel = zoomLevel;

                Settings.SetSettings(settings);
            });

            EnableDebugModeCommand = ReactiveCommand.Create(() =>
            {
                DebugMode = !DebugMode;
            });            
        }

        private DocumentDock _documentDock;
        private ToolDock _leftPane;
        private ToolDock _rightPane;
        private ToolDock _bottomPane;

        public ReactiveCommand EnableDebugModeCommand { get; }

        public event EventHandler<SolutionChangedEventArgs> SolutionChanged;
        public event EventHandler<BuildEventArgs> BuildStarting;
        public event EventHandler<BuildEventArgs> BuildCompleted;

        private IDockFactory _factory;
        private IView _layout;

        public IDockFactory Factory
        {
            get => _factory;
            set => this.RaiseAndSetIfChanged(ref _factory, value);
        }

        public IView Layout
        {
            get => _layout;
            set => this.RaiseAndSetIfChanged(ref _layout, value);
        }

        public IObservable<ISolution> OnSolutionChanged { get; }

        public MenuViewModel MainMenu { get; }

        public StatusBarViewModel StatusBar => _statusBar.Value;

        public bool DebugVisible
        {
            get { return debugControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
        }

        public IEnumerable<ToolbarViewModel> Toolbars => _toolbars;

        private ToolbarViewModel StandardToolbar { get; }

        public IEnumerable<KeyBinding> KeyBindings => _keyBindings;

        public DocumentTabControlViewModel DocumentTabs { get; }

        public TabControlViewModel LeftTabs { get; }

        public TabControlViewModel RightTabs { get; }

        public TabControlViewModel RightTopTabs { get; }
        public TabControlViewModel RightMiddleTabs { get; }
        public TabControlViewModel RightBottomTabs { get; }
        public TabControlViewModel BottomTabs { get; }
        public TabControlViewModel BottomRightTabs { get; }
        public TabControlViewModel MiddleTopTabs { get; }

        public IConsole Console { get; }

        public IErrorList ErrorList { get; }

        public CancellationTokenSource ProcessCancellationToken { get; private set; }

        public IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes => _projectTypes;

        public IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> LanguageServices => _languageServices;

        public IEnumerable<IToolchain> ToolChains => _toolChains.Select(t => t.Value);

        public IEnumerable<IDebugger> Debugger2s => _debugger2s;

        public IEnumerable<ITestFramework> TestFrameworks
        {
            get
            {
                foreach (var testFramework in _testFrameworks)
                {
                    yield return testFramework.Value;
                }
            }
        }

        public void AddDocument(IDocumentTabViewModel document, bool temporary = false)
        {
            _documentDock.Views.Add(document);
            DocumentTabs.OpenDocument(document, temporary);
        }

        public void RemoveDocument (ISourceFile file)
        {
            var document = DocumentTabs.Documents.OfType<IFileDocumentTabViewModel>().FirstOrDefault(d => d.SourceFile == file);

            if(document != null)
            {
                RemoveDocument(document);
            }            
        }

        public void RemoveDocument(IDocumentTabViewModel document)
        {
            if (document == null)
            {
                return;
            }

            if (document is EditorViewModel doc)
            {
                doc.Editor?.Save();
            }

            DocumentTabs.CloseDocument(document);
        }

        public IFileDocumentTabViewModel OpenDocument(ISourceFile file)
        {
            var currentTab = DocumentTabs.Documents.OfType<IFileDocumentTabViewModel>().FirstOrDefault(t => t.SourceFile?.FilePath == file.FilePath);

            if (currentTab == null)
            {
                var provider = _editorProviders.FirstOrDefault(p => p.Value.CanEdit(file))?.Value;

                if (provider != null)
                {
                    currentTab = provider.CreateViewModel(file);

                    AddDocument(currentTab);
                }
                else
                {
                    var newTab = new TextEditorViewModel(file);

                    AddDocument(newTab);

                    currentTab = newTab;
                }
            }
            else
            {
                AddDocument(currentTab);
            }

            return currentTab;
        }

        public async Task<IEditor> OpenDocumentAsync(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true)
        {
            var currentTab = OpenDocument(file);

            if (DocumentTabs.SelectedDocument is IFileDocumentTabViewModel fileTab)
            {
                await fileTab.WaitForEditorToLoadAsync();

                if (debugHighlight)
                {
                    fileTab.Editor.SetDebugHighlight(line, startColumn, endColumn);
                }

                if (selectLine || debugHighlight)
                {
                    fileTab.Editor.GotoPosition(line, startColumn != -1 ? startColumn : 1);
                }

                if (focus)
                {
                    fileTab.Editor.Focus();
                }

                if (currentTab is TextEditorViewModel editor)
                {
                    return editor.DocumentAccessor;
                }
            }

            return null;
        }

        public IEditor GetDocument(string path)
        {
            return DocumentTabs.Documents.OfType<TextEditorViewModel>().Where(d => d.SourceFile?.FilePath == path).Select(d => d.DocumentAccessor).FirstOrDefault();
        }

        public void Save()
        {
            if (SelectedDocument is IFileDocumentTabViewModel document)
            {
                document.Editor.Save();
            }
        }

        public void SaveAll()
        {
            foreach (var document in DocumentTabs.Documents.OfType<IFileDocumentTabViewModel>())
            {
                document.Editor?.Save();
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
            Console.Clear();

            if (project.ToolChain != null)
            {
                BuildStarting?.Invoke(this, new BuildEventArgs(BuildType.Clean, project));

                TaskRunner.RunTask(() =>
                {
                    project.ToolChain.Clean(Console, project).Wait();

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        BuildCompleted?.Invoke(this, new BuildEventArgs(BuildType.Clean, project));
                    });
                });
            }
            else
            {
                Console.WriteLine($"No toolchain selected for {project.Name}");
            }
        }

        public async Task<bool> BuildAsync(IProject project)
        {
            bool result = false;

            SaveAll();

            Console.Clear();

            if (project.ToolChain != null)
            {
                BuildStarting?.Invoke(this, new BuildEventArgs(BuildType.Build, project));

                await TaskRunner.RunTask(() =>
                {
                    result = project.ToolChain.BuildAsync(Console, project).Result;
                });

                Dispatcher.UIThread.Post(() =>
                {
                    BuildCompleted?.Invoke(this, new BuildEventArgs(BuildType.Build, project));
                });
            }
            else
            {
                Console.WriteLine($"No toolchain selected for {project.Name}");
            }

            return result;
        }

        public void ShowQuickCommander()
        {
            this._quickCommander.IsVisible = true;
        }

        public ObservableCollection<object> Tools
        {
            get { return tools; }
            set { this.RaiseAndSetIfChanged(ref tools, value); }
        }

        public Perspective CurrentPerspective
        {
            get
            {
                return currentPerspective;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref currentPerspective, value);

                switch (value)
                {
                    case Perspective.Editor:
                        DebugVisible = false;
                        break;

                    case Perspective.Debug:
                        // TODO close intellisense, and tooltips.
                        // disable documents, get rid of error list, solution explorer, etc.    (isreadonly)
                        DebugVisible = true;
                        break;
                }
            }
        }

        public ModalDialogViewModelBase ModalDialog
        {
            get { return modalDialog; }
            set { this.RaiseAndSetIfChanged(ref modalDialog, value); }
        }

        public QuickCommanderViewModel QuickCommander
        {
            get { return _quickCommander; }
            set { this.RaiseAndSetIfChanged(ref _quickCommander, value); }
        }


        public void InvalidateCodeAnalysis()
        {
            foreach (var document in DocumentTabs.Documents)
            {
                //TODO implement code analysis trigger.
            }
        }

        private ColorScheme _currentColorScheme;

        public ColorScheme CurrentColorScheme
        {
            get { return _currentColorScheme; }

            set
            {
                this.RaiseAndSetIfChanged(ref _currentColorScheme, value);

                foreach (var document in DocumentTabs.Documents.OfType<AvalonStudio.Controls.EditorViewModel>())
                {
                    document.ColorScheme = value;
                }
            }
        }

        public ISolution CurrentSolution
        {
            get
            {
                return currentSolution;
            }
            private set
            {
                var oldValue = CurrentSolution;

                this.RaiseAndSetIfChanged(ref currentSolution, value);

                SolutionChanged?.Invoke(this, new SolutionChangedEventArgs() { OldValue = oldValue, NewValue = currentSolution });
            }
        }

        public IDocumentTabViewModel SelectedDocument
        {
            get
            {
                return DocumentTabs?.SelectedDocument;
            }
            set
            {
                if (DocumentTabs != null)
                {
                    DocumentTabs.SelectedDocument = value;
                }
            }
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
                    Console.WriteLine("No Default project is set in the solution.");
                }
            }
            else
            {
                Console.WriteLine("No Solution is loaded.");
            }

            return result;
        }

        public void ShowProjectPropertiesDialog()
        {
            //ModalDialog = new ProjectConfigurationDialogViewModel(CurrentSolution.SelectedProject, () => { });
            //ModalDialog.ShowDialog();
        }

        public void ShowPackagesDialog()
        {
            ModalDialog = new PackageManagerDialogViewModel();
            ModalDialog.ShowDialog();
        }

        public void ExitApplication()
        {
            Environment.Exit(1);
        }

        public void UpdateDiagnostics(DiagnosticsUpdatedEventArgs diagnostics)
        {
            var toRemove = ErrorList.Errors.Where(e => Equals(e.Tag, diagnostics.Tag) && e.AssociatedFile == diagnostics.AssociatedSourceFile).ToList();

            foreach (var error in toRemove)
            {
                ErrorList.Errors.Remove(error);
            }

            foreach (var diagnostic in diagnostics.Diagnostics)
            {
                if (diagnostic.Level != DiagnosticLevel.Hidden)
                {
                    ErrorList.Errors.InsertSorted(new ErrorViewModel(diagnostic, diagnostics.Tag, diagnostics.AssociatedSourceFile));
                }
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
                var solutionType = _solutionTypes.FirstOrDefault(
                    s => s.Metadata.SupportedExtensions.Any(e => extension.EndsWith(e)));

                if (solutionType != null)
                {
                    StatusBar.SetText($"Loading Solution: {path}");

                    var solution = await solutionType.Value.LoadAsync(path);

                    await solution.LoadSolutionAsync();

                    await solution.RestoreSolutionAsync();

                    StatusBar.ClearText();

                    CurrentSolution = solution;

                    await CurrentSolution.LoadProjectsAsync();
                }
            }
        }

        public async Task CloseSolutionAsync()
        {
            ErrorList.Errors.Clear();

            var documentsToClose = DocumentTabs.Documents.ToList();

            foreach (var document in documentsToClose)
            {
                if (document is EditorViewModel evm)
                {
                    DocumentTabs.CloseDocument(evm);
                }
            }

            await CurrentSolution.UnloadProjectsAsync();

            await CurrentSolution.UnloadSolutionAsync();

            CurrentSolution = null;
        }

        public void CloseDocumentsForProject(IProject project)
        {
            var documentsToClose = DocumentTabs.Documents.ToList();

            foreach (var document in documentsToClose)
            {
                if (document is EditorViewModel evm && evm.SourceFile.Project == project)
                {
                    DocumentTabs.CloseDocument(evm);
                }
            }
        }

        public double GlobalZoomLevel
        {
            get { return _globalZoomLevel; }
            set { this.RaiseAndSetIfChanged(ref _globalZoomLevel, value); }
        }

        public bool DebugMode { get; set; }

        public IWorkspaceTaskRunner TaskRunner => _taskRunner;
    }
}