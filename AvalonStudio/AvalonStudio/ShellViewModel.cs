using Avalonia.Input;
using Avalonia.Threading;
using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.ErrorList;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.MainMenu;
using AvalonStudio.Extensibility.MainToolBar;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.ToolBars;
using AvalonStudio.Extensibility.ToolBars.Models;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AvalonStudio.IdeSettings;

namespace AvalonStudio
{
    [Export]
    public class ShellViewModel : ViewModel, IShell
    {
        public static ShellViewModel Instance { get; internal set; }
        private IToolBar _toolBar;
        private ToolBarDefinition _toolBarDefinition;
        private double _globalZoomLevel;
        private List<ILanguageService> _languageServices;
        private List<IProjectTemplate> _projectTemplates;
        private List<ISolutionType> _solutionTypes;
        private List<IProjectType> _projectTypes;
        private List<IToolChain> _toolChains;
        private List<IDebugger> _debugger2s;
        private List<ITestFramework> _testFrameworks;
        private List<ICodeTemplate> _codeTemplates;
        private List<MenuBarDefinition> _menuBarDefinitions;
        private List<MenuDefinition> _menuDefinitions;
        private List<MenuItemGroupDefinition> _menuItemGroupDefinitions;
        private List<MenuItemDefinition> _menuItemDefinitions;
        private List<CommandDefinition> _commandDefinitions;
        private List<KeyBinding> _keyBindings;
        private List<ToolBarDefinition> _toolBarDefinitions;
        private List<ToolBarItemGroupDefinition> _toolBarItemGroupDefinitions;
        private List<ToolBarItemDefinition> _toolBarItemDefinitions;

        private Perspective currentPerspective;

        private ISolution currentSolution;

        private bool debugControlsVisible;

        private ModalDialogViewModelBase modalDialog;

        private QuickCommanderViewModel _quickCommander;

        private ObservableCollection<object> tools;

        [ImportingConstructor]
        public ShellViewModel([ImportMany] IEnumerable<IExtension> extensions, [ImportMany] IEnumerable<ICodeTemplate> codeTemplates)
        {
            _languageServices = new List<ILanguageService>();
            _projectTemplates = new List<IProjectTemplate>();
            _debugger2s = new List<IDebugger>();
            _codeTemplates = new List<ICodeTemplate>();
            _projectTypes = new List<IProjectType>();
            _solutionTypes = new List<ISolutionType>();
            _testFrameworks = new List<ITestFramework>();
            _toolChains = new List<IToolChain>();
            _menuBarDefinitions = new List<MenuBarDefinition>();
            _menuDefinitions = new List<MenuDefinition>();
            _menuItemGroupDefinitions = new List<MenuItemGroupDefinition>();
            _menuItemDefinitions = new List<MenuItemDefinition>();
            _commandDefinitions = new List<CommandDefinition>();
            _keyBindings = new List<KeyBinding>();
            _toolBarDefinitions = new List<ToolBarDefinition>();
            _toolBarItemGroupDefinitions = new List<ToolBarItemGroupDefinition>();
            _toolBarItemDefinitions = new List<ToolBarItemDefinition>();

            IoC.RegisterConstant<IShell>(this);
            IoC.RegisterConstant(this);

            foreach (var extension in extensions)
            {
                extension.BeforeActivation();
            }

            _codeTemplates.AddRange(codeTemplates);

            CurrentPerspective = Perspective.Editor;

            StatusBar = new StatusBarViewModel();
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

            QuickCommander = new QuickCommanderViewModel();

            foreach (var extension in extensions)
            {
                extension.Activation();

                _languageServices.ConsumeExtension(extension);
                _toolChains.ConsumeExtension(extension);
                _projectTemplates.ConsumeExtension(extension);
                _debugger2s.ConsumeExtension(extension);
                _solutionTypes.ConsumeExtension(extension);
                _projectTypes.ConsumeExtension(extension);
                _testFrameworks.ConsumeExtension(extension);

                _commandDefinitions.ConsumeExtension(extension);
            }

            _menuBarDefinitions.AddRange(IoC.GetServices<MenuBarDefinition>(typeof(MenuBarDefinition)));
            _menuDefinitions.AddRange(IoC.GetServices<MenuDefinition>(typeof(MenuDefinition)));
            _menuItemGroupDefinitions.AddRange(IoC.GetServices<MenuItemGroupDefinition>(typeof(MenuItemGroupDefinition)));
            _menuItemDefinitions.AddRange(IoC.GetServices<MenuItemDefinition>(typeof(MenuItemDefinition)));

            _toolBarDefinitions.AddRange(IoC.GetServices<ToolBarDefinition>(typeof(ToolBarDefinition)));
            _toolBarItemDefinitions.AddRange(IoC.GetServices<ToolBarItemDefinition>(typeof(ToolBarItemDefinition)));
            _toolBarItemGroupDefinitions.AddRange(IoC.GetServices<ToolBarItemGroupDefinition>(typeof(ToolBarItemGroupDefinition)));

            foreach (var definition in _toolBarItemDefinitions)
            {
                definition.Activation();
            }

            foreach (var menuItemDefinition in _menuDefinitions)
            {
                menuItemDefinition.Activation();
            }

            foreach (var menuItemDefinition in _menuItemGroupDefinitions)
            {
                menuItemDefinition.Activation();
            }

            foreach (var extension in _menuItemDefinitions)
            {
                extension.Activation();
            }

            var menuBar = IoC.Get<MenuBarDefinition>("MainMenu");

            foreach (var commandDefinition in _commandDefinitions)
            {
                if (commandDefinition.Command != null && commandDefinition.Gesture != null)
                {
                    _keyBindings.Add(new KeyBinding { Gesture = commandDefinition.Gesture, Command = commandDefinition.Command });
                }
            }

            ToolBarDefinition = ToolBarDefinitions.MainToolBar;

            var menuBuilder = new MenuBuilder(_menuBarDefinitions.ToArray(), _menuDefinitions.ToArray(), _menuItemGroupDefinitions.ToArray(), _menuItemDefinitions.ToArray(), new ExcludeMenuDefinition[0], new ExcludeMenuItemGroupDefinition[0], new ExcludeMenuItemDefinition[0]);

            var mainMenu = new Extensibility.MainMenu.ViewModels.MainMenuViewModel(menuBuilder);

            menuBuilder.BuildMenuBar(menuBar, mainMenu.Model);

            MainMenu = mainMenu;

            foreach (var tool in extensions.OfType<ToolViewModel>())
            {
                tools.Add(tool);

                switch (tool.DefaultLocation)
                {
                    case Location.Bottom:
                        BottomTabs.Tools.Add(tool);
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
                        LeftTabs.Tools.Add(tool);
                        break;

                    case Location.Right:
                        RightTabs.Tools.Add(tool);
                        break;
                }
            }

            LeftTabs.SelectedTool = LeftTabs.Tools.FirstOrDefault();
            RightTabs.SelectedTool = RightTabs.Tools.FirstOrDefault();
            BottomTabs.SelectedTool = BottomTabs.Tools.FirstOrDefault();
            BottomRightTabs.SelectedTool = BottomRightTabs.Tools.FirstOrDefault();
            RightTopTabs.SelectedTool = RightTopTabs.Tools.FirstOrDefault();
            RightMiddleTabs.SelectedTool = RightMiddleTabs.Tools.FirstOrDefault();
            RightBottomTabs.SelectedTool = RightBottomTabs.Tools.FirstOrDefault();
            MiddleTopTabs.SelectedTool = MiddleTopTabs.Tools.FirstOrDefault();

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platform.OSDescription + " " + Platform.AvalonRID;

            ProcessCancellationToken = new CancellationTokenSource();

            CurrentPerspective = Perspective.Editor;
            
            var editorSettings = Settings.GetSettings<EditorSettings>();

            _globalZoomLevel = editorSettings.GlobalZoomLevel;

            IoC.RegisterConstant(this);

            this.WhenAnyValue(x => x.GlobalZoomLevel).Subscribe(zoomLevel=>
            {
                foreach(var document in DocumentTabs.Documents.OfType<EditorViewModel>())
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
        }

        public event EventHandler<SolutionChangedEventArgs> SolutionChanged;

        public IMenu MainMenu { get; }

        public bool DebugVisible
        {
            get { return debugControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
        }

        public ToolBarDefinition ToolBarDefinition
        {
            get
            {
                return _toolBarDefinition;
            }
            protected set
            {
                this.RaiseAndSetIfChanged(ref _toolBarDefinition, value);
                // Might need to do a global raise property change (NPC(string.Empty))
            }
        }

        public IToolBar ToolBar
        {
            get
            {
                if (_toolBar != null)
                    return _toolBar;

                if (ToolBarDefinition == null)
                    return null;

                var toolBarBuilder = new ToolBarBuilder(_toolBarDefinitions.ToArray(), _toolBarItemGroupDefinitions.ToArray(), _toolBarItemDefinitions.ToArray(), new ExcludeToolBarDefinition[0], new ExcludeToolBarItemGroupDefinition[0], new ExcludeToolBarItemDefinition[0]);

                var mainToolBar = new Extensibility.ToolBars.ViewModels.ToolBarsViewModel(toolBarBuilder);

                toolBarBuilder.BuildToolBars(mainToolBar);

                _toolBar = new ToolBarModel();

                toolBarBuilder.BuildToolBar(ToolBarDefinition, _toolBar);
                return _toolBar;
            }
        }

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

        public StatusBarViewModel StatusBar { get; }

        public CancellationTokenSource ProcessCancellationToken { get; private set; }

        public IEnumerable<ISolutionType> SolutionTypes => _solutionTypes;

        public IEnumerable<IProjectType> ProjectTypes => _projectTypes;

        public IEnumerable<IProjectTemplate> ProjectTemplates => _projectTemplates;

        public IEnumerable<ICodeTemplate> CodeTemplates => _codeTemplates;

        public IEnumerable<ILanguageService> LanguageServices => _languageServices;

        public IEnumerable<IToolChain> ToolChains => _toolChains;

        public IEnumerable<IDebugger> Debugger2s => _debugger2s;

        public IEnumerable<ITestFramework> TestFrameworks => _testFrameworks;

        public void AddDocument(IDocumentTabViewModel document)
        {
            DocumentTabs.Documents.Add(document);
            DocumentTabs.SelectedDocument = document;

            DocumentTabs.InvalidateSeperatorVisibility();
        }

        public void RemoveDocument(IDocumentTabViewModel document)
        {
            if(document == null)
            {
                return;
            }

            IDocumentTabViewModel newSelectedTab = DocumentTabs.SelectedDocument;

            if (DocumentTabs.SelectedDocument == document)
            {
                var index = DocumentTabs.Documents.IndexOf(document);
                var current = index;

                bool foundTab = false;

                while (!foundTab)
                {
                    index++;

                    if (index < DocumentTabs.Documents.Count)
                    {
                        if (DocumentTabs.Documents[index].IsVisible)
                        {
                            foundTab = true;
                            newSelectedTab = DocumentTabs.Documents[index];
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                index = current;

                while (!foundTab)
                {
                    index--;

                    if (index >= 0)
                    {
                        if (DocumentTabs.Documents[index].IsVisible)
                        {
                            foundTab = true;
                            newSelectedTab = DocumentTabs.Documents[index];
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            DocumentTabs.SelectedDocument = newSelectedTab;

            document.IsVisible = false;

            if (document is EditorViewModel doc)
            {
                doc.Save();                
            }

            if (DocumentTabs.TemporaryDocument == document)
            {
                DocumentTabs.TemporaryDocument = null;
            }

            if (DocumentTabs.Documents.OfType<EditorViewModel>().Where(d => !d.IsVisible).Count() == 5)
            {
                var toRemove = DocumentTabs.Documents.OfType<EditorViewModel>().Where(d => !d.IsVisible).First();

                DocumentTabs.Documents.Remove(toRemove);
                toRemove.OnClose();

                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await Task.Delay(25);
                    GC.Collect();
                });
            }

            DocumentTabs.InvalidateSeperatorVisibility();
        }

        public async Task<IEditor> OpenDocument(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true)
        {
            bool restoreFromCache = false;

            var currentTab = DocumentTabs.Documents.OfType<EditorViewModel>().FirstOrDefault(t => t.ProjectFile?.FilePath == file.FilePath);

            if (currentTab != null && !currentTab.IsVisible)
            {
                restoreFromCache = true;
            }

            if (currentTab == null || restoreFromCache)
            {
                if (DocumentTabs.TemporaryDocument != null)
                {
                    var documentToClose = DocumentTabs.TemporaryDocument;

                    await documentToClose.CloseCommand.ExecuteAsyncTask(null);
                }

                EditorViewModel newEditor = null;

                if (restoreFromCache)
                {
                    newEditor = currentTab;
                }
                else
                {
                    newEditor = new EditorViewModel();
                }

                if (restoreFromCache)
                {
                    newEditor.IsVisible = true;
                    newEditor.Dock = Avalonia.Controls.Dock.Right;
                    DocumentTabs.SelectedDocument = newEditor;
                }
                else
                {
                    newEditor.OpenFile(file);
                    AddDocument(newEditor);
                }

                newEditor.IsTemporary = true;
                DocumentTabs.TemporaryDocument = newEditor;
            }
            else
            {
                DocumentTabs.SelectedDocument = currentTab;
            }

            if (DocumentTabs.SelectedDocument is IEditor editor)
            {
                // ensures that the document has been opened and created.
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (debugHighlight)
                    {
                        editor.SetDebugHighlight(line, startColumn, endColumn);
                    }

                    if (selectLine || debugHighlight)
                    {
                        editor.GotoPosition(line, startColumn != -1 ? 1 : startColumn);
                    }

                    if (focus)
                    {
                        editor.Focus();
                    }
                });
            }

            return DocumentTabs.SelectedDocument as IEditor;
        }

        public IEditor GetDocument(string path)
        {
            return DocumentTabs.Documents.OfType<IEditor>().FirstOrDefault(d => d.ProjectFile?.FilePath == path);
        }

        public void Save()
        {
            if (SelectedDocument is IEditor)
            {
                (SelectedDocument as IEditor).Save();
            }
        }

        public void SaveAll()
        {
            foreach (var document in DocumentTabs.Documents.OfType<EditorViewModel>().Where(d => d.IsDirty && d.IsVisible))
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
                Build(project);
            }
        }

        public void Clean(IProject project)
        {
            Console.Clear();

            if (project.ToolChain != null)
            {
                new Thread(async () => { await project.ToolChain.Clean(Console, project); }).Start();
            }
            else
            {
                Console.WriteLine($"No toolchain selected for {project.Name}");
            }
        }

        public void Build(IProject project)
        {
            SaveAll();

            Console.Clear();

            if (project.ToolChain != null)
            {
                new Thread(async () => { await Task.Factory.StartNew(() => project.ToolChain.Build(Console, project)); }).Start();
            }
            else
            {
                Console.WriteLine($"No toolchain selected for {project.Name}");
            }
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
                    if (value == null || (DocumentTabs.TemporaryDocument == value && !value.IsTemporary))
                    {
                        DocumentTabs.TemporaryDocument = null;
                    }

                    DocumentTabs.SelectedDocument = value;
                }
            }
        }

        public object BottomSelectedTool
        {
            get { return BottomTabs.SelectedTool; }

            set { BottomTabs.SelectedTool = value; }
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

        public void InvalidateErrors()
        {
            var allErrors = new List<ErrorViewModel>();
            var toRemove = new List<ErrorViewModel>();
            var hasChanged = false;

            foreach (var document in DocumentTabs.Documents.Where(d => d.IsVisible).OfType<EditorViewModel>())
            {
                if (document.Diagnostics != null)
                {
                    foreach (var diagnostic in document.Diagnostics)
                    {
                        var error = new ErrorViewModel(diagnostic);
                        var matching = allErrors.FirstOrDefault(err => err.IsEqual(error));

                        if (matching == null)
                        {
                            allErrors.Add(error);
                        }
                    }
                }
            }

            foreach (var error in ErrorList.Errors)
            {
                var matching = allErrors.SingleOrDefault(err => err.IsEqual(error));

                if (matching == null)
                {
                    toRemove.Add(error);
                }
            }

            foreach (var error in toRemove)
            {
                hasChanged = true;
                ErrorList.Errors.Remove(error);
            }

            foreach (var error in allErrors)
            {
                var matching = ErrorList.Errors.SingleOrDefault(err => err.IsEqual(error));

                if (matching == null)
                {
                    hasChanged = true;
                    ErrorList.Errors.Add(error);
                }
            }

            if (hasChanged)
            {
                BottomTabs.SelectedTool = ErrorList;
            }
        }

        public void Cleanup()
        {
            foreach (var document in DocumentTabs.Documents.OfType<EditorViewModel>())
            {
                // document.Model.ShutdownBackgroundWorkers();
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
                var solutionType = SolutionTypes.FirstOrDefault(st => st.Extensions.Contains(System.IO.Path.GetExtension(path).Substring(1)));

                if (solutionType != null)
                {
                    CurrentSolution = await solutionType.LoadAsync(path);
                }
            }
        }

        public async Task CloseSolutionAsync()
        {
            var documentsToClose = DocumentTabs.Documents.ToList();

            foreach (var document in documentsToClose)
            {
                if (document is EditorViewModel)
                {
                    await (document as EditorViewModel).CloseCommand.ExecuteAsyncTask();
                }
            }

            CurrentSolution = null;
        }

        public async Task CloseDocumentsForProjectAsync(IProject project)
        {
            var documentsToClose = DocumentTabs.Documents.ToList();

            foreach (var document in documentsToClose)
            {
                if (document is EditorViewModel evm && evm.ProjectFile.Project == project)
                {
                    await evm.CloseCommand.ExecuteAsyncTask();

                    evm.OnClose();
                }
            }
        }

        public double GlobalZoomLevel
        {
            get { return _globalZoomLevel; }
            set { this.RaiseAndSetIfChanged(ref _globalZoomLevel, value); }
        }

    }
}