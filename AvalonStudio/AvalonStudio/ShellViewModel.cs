namespace AvalonStudio
{
    using Platforms;
    using Controls;
    using Controls.ViewModels;
    using Debugging;
    using Extensibility;
    using MVVM;
    using Perspex.Controls;
    using Perspex.Input;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TextEditor;
    using Utils;
    using Perspex.Threading;
    using Documents;
    using Extensibility.Dialogs;
    using Languages;
    using Toolchains;
    using TestFrameworks;
    using Extensibility.Plugin;
    using Splat;
    using Controls.Standard.ErrorList;
    using Shell;
    using Extensibility.MainMenu;
    using Extensibility.Commands;
    using ToolBars;
    using Extensibility.ToolBars;
    using ToolBars.Models;
    public enum Perspective
    {
        Editor,
        Debug
    }

    [Export(typeof(IShell))]
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : ViewModel, IShell
    {
        public static ShellViewModel Instance = null;

        private readonly IMenu mainMenu;
        private readonly IEnumerable<ILanguageService> languageServices;
        private readonly IEnumerable<IProjectTemplate> projectTemplates;
        private readonly IEnumerable<ICodeTemplate> codeTemplates;
        private readonly IEnumerable<IToolChain> toolChains;
        private readonly IEnumerable<IDebugger> debuggers;
        private readonly IEnumerable<IProject> projectTypes;
        private readonly IEnumerable<ITestFramework> testFrameworks;        
        private readonly IEnumerable<IPlugin> plugins;

        public IEnumerable<IProject> ProjectTypes
        {
            get
            {
                return projectTypes;
            }
        }

        public IEnumerable<IProjectTemplate> ProjectTemplates
        {
            get
            {
                return projectTemplates;
            }
        }

        public IEnumerable<ICodeTemplate> CodeTemplates
        {
            get
            {
                return codeTemplates;
            }
        }

        public IEnumerable<ILanguageService> LanguageServices
        {
            get
            {
                return languageServices;
            }
        }

        public IEnumerable<IToolChain> ToolChains
        {
            get
            {
                return toolChains;
            }
        }

        public IEnumerable<IDebugger> Debuggers
        {
            get
            {
                return debuggers;
            }
        }

        public IEnumerable<ITestFramework> TestFrameworks
        {
            get
            {
                return testFrameworks;
            }
        }

        public IMenu MainMenu
        {
            get
            {
                return mainMenu;
            }
        }

        [ImportingConstructor]
        public ShellViewModel([ImportMany] IEnumerable<ToolViewModel> importedTools,
            [ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<IProject> projectTypes, [ImportMany] IEnumerable<IProjectTemplate> projectTemplates, [ImportMany] IEnumerable<IToolChain> toolChains, [ImportMany] IEnumerable<IDebugger> debuggers, [ImportMany] IEnumerable<ITestFramework> testFrameworks, [ImportMany] IEnumerable<ICodeTemplate> codeTemplates, [ImportMany] IEnumerable<IPlugin> plugins, [Import]IMenu mainMenu)
        {
            this.mainMenu = mainMenu;
            this.languageServices = languageServices;
            this.projectTemplates = projectTemplates;
            this.toolChains = toolChains;
            this.debuggers = debuggers;
            this.projectTypes = projectTypes;
            this.testFrameworks = testFrameworks;
            this.codeTemplates = codeTemplates;

            IoC.RegisterConstant(this, typeof(IShell));

            foreach (var plugin in plugins)
            {
                plugin.BeforeActivation();
            }

            foreach (var plugin in plugins)
            {
                plugin.Activation();
            }

            CurrentPerspective = Perspective.Editor;

            //MainMenu = new MainMenuViewModel();
            //ToolBar = new ToolBarViewModel();
            StatusBar = new StatusBarViewModel();
            DocumentTabs = new DocumentTabsViewModel();

            Console = IoC.Get<IConsole>();
            ErrorList = IoC.Get<IErrorList>();

            DebugManager = new DebugManager();

            tools = new ObservableCollection<object>();
            leftTools = new ObservableCollection<object>();
            rightTools = new ObservableCollection<object>();
            bottomTools = new ObservableCollection<object>();

            foreach (var tool in importedTools)
            {
                tools.Add(tool);

                switch (tool.DefaultLocation)
                {
                    case Location.Bottom:
                        bottomTools.Add(tool);                        
                        break;

                    case Location.Left:
                        leftTools.Add(tool);
                        break;

                    case Location.Right:
                        rightTools.Add(tool);                        
                        break;
                }
            }

            LeftSelectedTool = LeftTools.FirstOrDefault();
            RightSelectedTool = RightTools.FirstOrDefault();
            BottomSelectedTool = BottomTools.FirstOrDefault();


            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platforms.Platform.PlatformString;

            ProcessCancellationToken = new CancellationTokenSource();

            ModalDialog = new ModalDialogViewModelBase("Dialog");

            CurrentPerspective = Perspective.Editor;

            ToolBarDefinition = AvalonStudio.ToolBars.ToolBarDefinitions.MainToolBar;
            var toolBar = this.ToolBar;
        }

        public async Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false, bool selectLine = false)
        {
            var currentTab = DocumentTabs.Documents.FirstOrDefault(t => t.Model.ProjectFile.File == file.File);

            if (currentTab == null)
            {
                if (DocumentTabs.TemporaryDocument != null)
                {
                    await DocumentTabs.TemporaryDocument.CloseCommand.ExecuteAsyncTask(null);
                    DocumentTabs.TemporaryDocument = null;
                }

                var newEditor = new EditorViewModel(new EditorModel());
                newEditor.Margins.Add(new BreakPointMargin(DebugManager.BreakPointManager));
                newEditor.Margins.Add(new LineNumberMargin());

                DocumentTabs.Documents.Add(newEditor);
                DocumentTabs.TemporaryDocument = newEditor;
                DocumentTabs.SelectedDocument = newEditor;
                newEditor.Model.OpenFile(file, newEditor.Intellisense);
            }
            else
            {
                DocumentTabs.SelectedDocument = currentTab;
            }

            if (debugHighlight)
            {
                DocumentTabs.SelectedDocument.DebugLineHighlighter.Line = line;
            }

            Dispatcher.UIThread.InvokeAsync(() => DocumentTabs.SelectedDocument.Model.ScrollToLine(line));

            if (selectLine)
            {
                DocumentTabs.SelectedDocument.GotoPosition(line, column);
            }

            return DocumentTabs.SelectedDocument;
        }

        public IEditor GetDocument(string path)
        {
            return DocumentTabs.Documents.FirstOrDefault(d => d.Model.ProjectFile.File == path);
        }

        public void Save()
        {
            DocumentTabs.SelectedDocument?.Save();
        }

        public void SaveAll()
        {
            foreach (var document in DocumentTabs.Documents)
            {
                document.Save();
            }
        }

        public void Clean()
        {
            var project = GetDefaultProject();

            if(project != null)
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

        public void Debug()
        {
            var project = GetDefaultProject();

            if (project != null)
            {
                Debug(project);
            }
        }

        public void Debug(IProject project)
        {
            if (CurrentPerspective == Perspective.Editor)
            {
                Console.Clear();
                DebugManager.StartDebug(project);
            }
            else
            {
                DebugManager.Continue();
            }
        }

        public void Clean(IProject project)
        {
            Console.Clear();

            new Thread(new ThreadStart(new Action(async () =>
            {
                await project.ToolChain.Clean(Console, project);
            }))).Start();
        }

        public void Build(IProject project)
        {
            SaveAll();

            Console.Clear();

            new Thread(new ThreadStart(new Action(async () =>
            {
                await Task.Factory.StartNew(() => project.ToolChain.Build(Console, project));
            }))).Start();
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
                case Key.F9:
                    DebugManager.StepInstruction();
                    break;

                case Key.F10:
                    DebugManager.StepOver();
                    break;

                case Key.F11:
                    DebugManager.StepInto();
                    break;

                case Key.F5:
                    if (CurrentSolution?.StartupProject != null)
                    {
                        Debug(CurrentSolution.StartupProject);
                    }
                    break;

                case Key.F6:
                    Build();
                    break;
            }
        }

        public void InvalidateErrors()
        {
            var allErrors = new List<ErrorViewModel>();
            var toRemove = new List<ErrorViewModel>();
            bool hasChanged = false;

            foreach (var document in DocumentTabs.Documents)
            {
                if (document.Model.CodeAnalysisResults != null)
                {
                    foreach (var diagnostic in document.Model.CodeAnalysisResults.Diagnostics)
                    {

                        var error = new ErrorViewModel(diagnostic);
                        var matching = allErrors.FirstOrDefault((err) => err.IsEqual(error));

                        if (matching == null)
                        {
                            allErrors.Add(error);
                        }
                    }
                }
            }

            foreach (var error in ErrorList.Errors)
            {
                var matching = allErrors.SingleOrDefault((err) => err.IsEqual(error));

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
                var matching = ErrorList.Errors.SingleOrDefault((err) => err.IsEqual(error));

                if (matching == null)
                {
                    hasChanged = true;
                    ErrorList.Errors.Add(error);
                }
            }

            if (hasChanged)
            {
                BottomSelectedTool = ErrorList;
            }
        }

        private bool debugControlsVisible;
        public bool DebugVisible
        {
            get { return debugControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
        }


        public DebugManager DebugManager { get; private set; }

        //public MainMenuViewModel MainMenu { get; private set; }

        private ToolBarDefinition _toolBarDefinition;
        public ToolBarDefinition ToolBarDefinition
        {
            get { return _toolBarDefinition; }
            protected set
            {
                this.RaiseAndSetIfChanged(ref _toolBarDefinition, value);
                // Might need to do a global raise property change (NPC(string.Empty))
            }
        }

        private IToolBar _toolBar;
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

        public DocumentTabsViewModel DocumentTabs { get; private set; }

        private ObservableCollection<object> rightTools;
        public ObservableCollection<object> RightTools
        {
            get { return rightTools; }
            set { this.RaiseAndSetIfChanged(ref rightTools, value); }
        }

        private ObservableCollection<object> bottomTools;
        public ObservableCollection<object> BottomTools
        {
            get { return bottomTools; }
            set { this.RaiseAndSetIfChanged(ref bottomTools, value); }
        }

        private ObservableCollection<object> leftTools;
        public ObservableCollection<object> LeftTools
        {
            get { return leftTools; }
            set { this.RaiseAndSetIfChanged(ref leftTools, value); }
        }

        private ObservableCollection<object> tools;
        public ObservableCollection<object> Tools
        {
            get { return tools; }
            set { this.RaiseAndSetIfChanged(ref tools, value); }
        }

        private object rightSelectedTool;
        public object RightSelectedTool
        {
            get { return rightSelectedTool; }
            set { this.RaiseAndSetIfChanged(ref rightSelectedTool, value); }
        }

        private object bottomSelectedTool;
        public object BottomSelectedTool
        {
            get { return bottomSelectedTool; }
            set { this.RaiseAndSetIfChanged(ref bottomSelectedTool, value); }
        }

        private object leftSelectedTool;
        public object LeftSelectedTool
        {
            get { return leftSelectedTool; }
            set { this.RaiseAndSetIfChanged(ref leftSelectedTool, value); }
        }
        
        public IConsole Console { get; private set; }

        public IErrorList ErrorList { get; private set; }

        public StatusBarViewModel StatusBar { get; private set; }

        public CancellationTokenSource ProcessCancellationToken { get; private set; }

        private Perspective currentPerspective;
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

        private ModalDialogViewModelBase modalDialog;
        public ModalDialogViewModelBase ModalDialog
        {
            get { return modalDialog; }
            set { modalDialog = value; this.RaisePropertyChanged(); }
        }

        public void InvalidateCodeAnalysis()
        {
            foreach (var document in DocumentTabs.Documents)
            {
                //TODO implement code analysis trigger.
            }
        }

        private ISolution currentSolution;

        public event EventHandler SolutionChanged;

        public ISolution CurrentSolution
        {
            get { return currentSolution; }
            set
            {
                this.RaiseAndSetIfChanged(ref currentSolution, value);

                if (SolutionChanged != null)
                {
                    SolutionChanged(this, new EventArgs());
                }
            }
        }


        public void Cleanup()
        {
            foreach (var document in DocumentTabs.Documents)
            {
                document.Model.ShutdownBackgroundWorkers();
            }
        }

        public IProject GetDefaultProject()
        {
            IProject result = null;

            if (CurrentSolution != null)
            {
                if(CurrentSolution.StartupProject != null)
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
    }
}
