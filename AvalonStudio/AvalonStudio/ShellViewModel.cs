using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Threading;
using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.ErrorList;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.MainMenu;
using AvalonStudio.Extensibility.MainToolBar;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.ToolBars;
using AvalonStudio.Extensibility.ToolBars.Models;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.TextEditor;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using ReactiveUI;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio
{
    static class ListExtensions
    {
        public static void ConsumeExtension<T>(this List<T> destination, IExtension extension) where T : class, IExtension
        {
            if (extension is T)
            {
                destination.Add(extension as T);
            }
        }
    }

    [Export]
    public class ShellViewModel : ViewModel, IShell
    {
        public static ShellViewModel Instance = null;

        private IToolBar _toolBar;

        private ToolBarDefinition _toolBarDefinition;

        private List<ILanguageService> _languageServices;
        private List<IProjectTemplate> _projectTemplates;
        private List<ISolutionType> _solutionTypes;
        private List<IProjectType> _projectTypes;
        private List<IToolChain> _toolChains;
        private List<IDebugger> _debuggers;
        private List<ITestFramework> _testFrameworks;
        private List<ICodeTemplate> _codeTemplates;
        private List<MenuBarDefinition> _menuBarDefinitions;
        private List<MenuDefinition> _menuDefinitions;
        private List<MenuItemGroupDefinition> _menuItemGroupDefinitions;
        private List<MenuItemDefinition> _menuItemDefinitions;

        private Perspective currentPerspective;

        private ISolution currentSolution;

        private bool debugControlsVisible;

        private ModalDialogViewModelBase modalDialog;

        private ObservableCollection<object> tools;

        

        [ImportingConstructor]
        public ShellViewModel([ImportMany] IEnumerable<IExtension> extensions,
            [Import] ICommandKeyGestureService keyGestureService, [Import] ICommandService commandService, [Import] IToolBarBuilder toolBarBuilder)
        {
            _languageServices = new List<ILanguageService>();
            _projectTemplates = new List<IProjectTemplate>();
            _debuggers = new List<IDebugger>();
            _codeTemplates = new List<ICodeTemplate> ();
            _projectTypes = new List<IProjectType>();
            _solutionTypes = new List<ISolutionType>();
            _testFrameworks = new List<ITestFramework>();
            _toolChains = new List<IToolChain>();
            _menuBarDefinitions = new List<MenuBarDefinition>();
            _menuDefinitions = new List<MenuDefinition>();
            _menuItemGroupDefinitions = new List<MenuItemGroupDefinition>();
            _menuItemDefinitions = new List<MenuItemDefinition>();
            
            IoC.RegisterConstant(this, typeof(IShell));

            foreach (var extension in extensions)
            {
                extension.BeforeActivation();
            }

            IoC.RegisterConstant(commandService, typeof(ICommandService));
            IoC.RegisterConstant(keyGestureService, typeof(ICommandKeyGestureService));            
            IoC.RegisterConstant(toolBarBuilder, typeof(IToolBarBuilder));

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

            foreach (var extension in extensions)
            {
                extension.Activation();

                _languageServices.ConsumeExtension(extension);
                _toolChains.ConsumeExtension(extension);
                _projectTemplates.ConsumeExtension(extension);
                _debuggers.ConsumeExtension(extension);
                _solutionTypes.ConsumeExtension(extension);
                _projectTypes.ConsumeExtension(extension);
                _menuBarDefinitions.ConsumeExtension(extension);
                _menuDefinitions.ConsumeExtension(extension);
                _menuItemGroupDefinitions.ConsumeExtension(extension);
                _menuItemDefinitions.ConsumeExtension(extension);
            }

            var menuBar = IoC.Get<MenuBarDefinition>("MainMenu");

            var menuBuilder = new MenuBuilder(null, _menuBarDefinitions.ToArray(), _menuDefinitions.ToArray(), _menuItemGroupDefinitions.ToArray(), _menuItemDefinitions.ToArray(), new ExcludeMenuDefinition[0], new ExcludeMenuItemGroupDefinition[0], new ExcludeMenuItemDefinition[0]);
                
            var mainMenu = new AvalonStudio.Extensibility.MainMenu.ViewModels.MainMenuViewModel(menuBuilder);

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
            StatusBar.PlatformString = Platform.OSDescription;

            ProcessCancellationToken = new CancellationTokenSource();

            CurrentPerspective = Perspective.Editor;

            ToolBarDefinition = ToolBarDefinitions.MainToolBar;
        }

        public event EventHandler<SolutionChangedEventArgs> SolutionChanged;

        public IMenu MainMenu { get; }

        public bool DebugVisible
        {
            get { return debugControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
        }

        public DebugManager DebugManager { get; private set; }

        public ToolBarDefinition ToolBarDefinition
        {
            get { return _toolBarDefinition; }
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

                var toolBarBuilder = IoC.Get<IToolBarBuilder>();
                _toolBar = new ToolBarModel();

                toolBarBuilder.BuildToolBar(ToolBarDefinition, _toolBar);
                return _toolBar;
            }
        }

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

        public IEnumerable<IDebugger> Debuggers => _debuggers;

        public IEnumerable<ITestFramework> TestFrameworks => _testFrameworks;

        public void AddDocument(IDocumentTabViewModel document)
        {
            DocumentTabs.Documents.Add(document);
            DocumentTabs.SelectedDocument = document;
        }

        public void RemoveDocument(IDocumentTabViewModel document)
        {
            IDocumentTabViewModel newSelectedTab = DocumentTabs.SelectedDocument;

            if (DocumentTabs.SelectedDocument == document)
            {
                if (DocumentTabs.SelectedDocument != DocumentTabs.Documents.Last())
                {
                    newSelectedTab = DocumentTabs.Documents.SkipWhile(d => d == document).FirstOrDefault();
                }
                else
                {
                    newSelectedTab = DocumentTabs.Documents.Reverse().Skip(1).FirstOrDefault();
                }
            }

            DocumentTabs.SelectedDocument = newSelectedTab;

            DocumentTabs.Documents.Remove(document);

            if (DocumentTabs.TemporaryDocument == document)
            {
                DocumentTabs.TemporaryDocument = null;
            }
        }

        public async Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false,
            bool selectLine = false)
        {
            var currentTab = DocumentTabs.Documents.OfType<EditorViewModel>().FirstOrDefault(t => t.Model.ProjectFile.FilePath == file.FilePath);

            var selectedDocumentTCS = new TaskCompletionSource<IDocumentTabViewModel>();

            if (currentTab == null)
            {
                await Dispatcher.UIThread.InvokeTaskAsync(async () =>
                {
                    if (DocumentTabs.TemporaryDocument != null)
                    {
                        var documentToClose = DocumentTabs.TemporaryDocument;
                        DocumentTabs.TemporaryDocument = null;
                        await documentToClose.CloseCommand.ExecuteAsyncTask(null);
                        SelectedDocument = null;
                    }
                });

                EditorViewModel newEditor = null;
                await Dispatcher.UIThread.InvokeTaskAsync(async () =>
                {
                    newEditor = new EditorViewModel(new EditorModel());

                    newEditor.Margins.Add(new BreakPointMargin(IoC.Get<IDebugManager>().BreakPointManager));
                    newEditor.Margins.Add(new LineNumberMargin());

                    await Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        DocumentTabs.Documents.Add(newEditor);
                        DocumentTabs.TemporaryDocument = newEditor;
                    });

                    DocumentTabs.SelectedDocument = newEditor;

                    await Dispatcher.UIThread.InvokeTaskAsync(() => { newEditor.Model.OpenFile(file, newEditor.Intellisense, newEditor.Intellisense.CompletionAssistant); });

                    selectedDocumentTCS.SetResult(DocumentTabs.SelectedDocument);
                });
            }
            else
            {
                await Dispatcher.UIThread.InvokeTaskAsync(() => { DocumentTabs.SelectedDocument = currentTab; });

                selectedDocumentTCS.SetResult(DocumentTabs.SelectedDocument);
            }

            await selectedDocumentTCS.Task;

            if (DocumentTabs.SelectedDocument is EditorViewModel)
            {
                if (debugHighlight)
                {
                    (DocumentTabs.SelectedDocument as EditorViewModel).DebugLineHighlighter.Line = line;
                }

                if (selectLine || debugHighlight)
                {
                    Dispatcher.UIThread.InvokeAsync(() => (DocumentTabs.SelectedDocument as EditorViewModel).Model.ScrollToLine(line));
                    (DocumentTabs.SelectedDocument as EditorViewModel).GotoPosition(line, column);
                }
            }

            return DocumentTabs.SelectedDocument as EditorViewModel;
        }

        public IEditor GetDocument(string path)
        {
            return DocumentTabs.Documents.OfType<EditorViewModel>().FirstOrDefault(d => d.Model.ProjectFile?.FilePath == path);
        }

        public void Save()
        {
            if (SelectedDocument is EditorViewModel)
            {
                (SelectedDocument as EditorViewModel).Save();
            }
        }

        public void SaveAll()
        {
            foreach (var document in DocumentTabs.Documents.OfType<EditorViewModel>())
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

        public ObservableCollection<object> Tools
        {
            get { return tools; }
            set { this.RaiseAndSetIfChanged(ref tools, value); }
        }

        public Perspective CurrentPerspective
        {
            get { return currentPerspective; }
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
            set
            {
                modalDialog = value;
                this.RaisePropertyChanged();
            }
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
            get { return currentSolution; }
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

        public void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                //case Key.F9:
                //    DebugManager.StepInstruction();
                //    break;

                //case Key.F10:
                //    DebugManager.StepOver();
                //    break;

                //case Key.F11:
                //    DebugManager.StepInto();
                //    break;

                //case Key.F5:
                //    if (CurrentSolution?.StartupProject != null)
                //    {
                //        Debug(CurrentSolution.StartupProject);
                //    }
                //    break;

                case Key.F6:
                    Build();
                    break;
            }
        }

        public void InvalidateErrors()
        {
            var allErrors = new List<ErrorViewModel>();
            var toRemove = new List<ErrorViewModel>();
            var hasChanged = false;

            foreach (var document in DocumentTabs.Documents.OfType<EditorViewModel>())
            {
                if (document.Model.CodeAnalysisResults != null)
                {
                    foreach (var diagnostic in document.Model.CodeAnalysisResults.Diagnostics)
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
                document.Model.ShutdownBackgroundWorkers();
            }
        }

        public async Task OpenSolutionAsync(string path)
        {
            if(CurrentSolution != null)
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

        public async Task CloseSolutionAsync ()
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

        public async Task CloseDocumentsForProjectAsync (IProject project)
        {
            var documentsToClose = DocumentTabs.Documents.ToList();

            foreach (var document in documentsToClose)
            {
                if (document is EditorViewModel && (document as EditorViewModel).ProjectFile.Project == project)
                {
                    await (document as EditorViewModel).CloseCommand.ExecuteAsyncTask();
                }
            }
        }
    }
}