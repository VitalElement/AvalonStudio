using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
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

namespace AvalonStudio
{
    [Export(typeof(IShell))]
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : ViewModel, IShell
    {
        public static ShellViewModel Instance = null;

        private IToolBar _toolBar;

        private ToolBarDefinition _toolBarDefinition;

        private Perspective currentPerspective;

        private ISolution currentSolution;

        private bool debugControlsVisible;

        private ModalDialogViewModelBase modalDialog;

        private ObservableCollection<object> tools;

        [ImportingConstructor]
        public ShellViewModel([ImportMany] IEnumerable<ToolViewModel> importedTools,
            [ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<ISolutionType> solutionTypes, [ImportMany] IEnumerable<IProject> projectTypes,
            [ImportMany] IEnumerable<IProjectTemplate> projectTemplates, [ImportMany] IEnumerable<IToolChain> toolChains,
            [ImportMany] IEnumerable<IDebugger> debuggers, [ImportMany] IEnumerable<ITestFramework> testFrameworks,
            [ImportMany] IEnumerable<ICodeTemplate> codeTemplates, [ImportMany] IEnumerable<IExtension> extensions,
            [Import] IMenu mainMenu)
        {
            MainMenu = mainMenu;
            LanguageServices = languageServices;
            ProjectTemplates = projectTemplates;
            ToolChains = toolChains;
            Debuggers = debuggers;
            SolutionTypes = solutionTypes;
            ProjectTypes = projectTypes;
            TestFrameworks = testFrameworks;
            CodeTemplates = codeTemplates;

            IoC.RegisterConstant(this, typeof(IShell));

            foreach (var extension in extensions)
            {
                extension.BeforeActivation();
            }

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
            }

            foreach (var tool in importedTools)
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
            StatusBar.PlatformString = Platform.PlatformString;

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

        public IEnumerable<ISolutionType> SolutionTypes { get; }

        public IEnumerable<IProject> ProjectTypes { get; }

        public IEnumerable<IProjectTemplate> ProjectTemplates { get; }

        public IEnumerable<ICodeTemplate> CodeTemplates { get; }

        public IEnumerable<ILanguageService> LanguageServices { get; }

        public IEnumerable<IToolChain> ToolChains { get; }

        public IEnumerable<IDebugger> Debuggers { get; }

        public IEnumerable<ITestFramework> TestFrameworks { get; }

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

        public async Task OpenSolution(string path)
        {
            // TODO implement closing down current solution cleanly.

            if (System.IO.File.Exists(path))
            {
                var solutionType = SolutionTypes.FirstOrDefault(st => st.Extensions.Contains(System.IO.Path.GetExtension(path).Substring(1)));

                if (solutionType != null)
                {
                    CurrentSolution = await solutionType.LoadAsync(path);
                }
            }
        }
    }
}