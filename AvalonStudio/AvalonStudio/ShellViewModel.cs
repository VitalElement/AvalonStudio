﻿namespace AvalonStudio
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
    public enum Perspective
    {
        Editor,
        Debug
    }

    [Export(typeof(IShell))]
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : ViewModel<Shell>, IShell
    {
        public static ShellViewModel Instance = null;

        [ImportingConstructor]
        public ShellViewModel([Import] Shell workspace, [ImportMany] IEnumerable<ToolViewModel> importedTools) : base(workspace)
        {
            CurrentPerspective = Perspective.Editor;

            MainMenu = new MainMenuViewModel();            
            ToolBar = new ToolBarViewModel();
            StatusBar = new StatusBarViewModel();
            DocumentTabs = new DocumentTabsViewModel();

            DebugManager = new DebugManager();

            tools = new ObservableCollection<object>();
            leftTools = new ObservableCollection<object>();
            rightTools = new ObservableCollection<object>();
            bottomTools = new ObservableCollection<object>();

            foreach(var tool in importedTools)
            {
                tools.Add(tool);

                switch(tool.DefaultLocation)
                {
                    case Location.Bottom:
                        bottomTools.Add(tool);
                        BottomSelectedTool = tool;
                        break;

                    case Location.Left:
                        leftTools.Add(tool);
                        break;

                    case Location.Right:
                        rightTools.Add(tool);
                        RightSelectedTool = tool;
                        break;
                } 
                
                if(tool is ErrorListViewModel)
                {
                    ErrorList = tool as ErrorListViewModel;
                }               

                if(tool is ConsoleViewModel)
                {
                    Console = tool as ConsoleViewModel;
                }
            }

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platforms.Platform.PlatformString;

            ProcessCancellationToken = new CancellationTokenSource();

            ModalDialog = new ModalDialogViewModelBase("Dialog");

            CurrentPerspective = Perspective.Editor;
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
            Console.Clear();

            new Thread(new ThreadStart(new Action(async () =>
            {
                if (CurrentSolution != null)
                {
                    if (CurrentSolution.StartupProject != null)
                    {
                        if (CurrentSolution.StartupProject.ToolChain != null)
                        {
                            await CurrentSolution.StartupProject.ToolChain.Clean(Console, CurrentSolution.StartupProject);
                        }
                        else
                        {
                            Console.WriteLine("No toolchain selected for project.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Startup Project defined.");
                    }
                }
                else
                {
                    Console.WriteLine("No project loaded.");
                }
            }))).Start();
        }        

        public void Build()
        {
            SaveAll();

            Console.Clear();

            new Thread(new ThreadStart(new Action(async () =>
            {
                if (CurrentSolution != null)
                {
                    if (CurrentSolution.StartupProject != null)
                    {
                        if (CurrentSolution.StartupProject.ToolChain != null)
                        {
                            await Task.Factory.StartNew(() => CurrentSolution.StartupProject.ToolChain.Build(Console, CurrentSolution.StartupProject));
                        }
                        else
                        {
                            Console.WriteLine("No toolchain selected for project.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Startup Project defined.");
                    }
                }
                else
                {
                    Console.WriteLine("No project loaded.");
                }
            }))).Start();
        }

        public async void LoadSolution()
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Open Solution";
            dlg.Filters.Add(new FileDialogFilter { Name = "AvalonStudio Solution", Extensions = new List<string> { Solution.Extension } });
            dlg.InitialFileName = string.Empty;
            dlg.InitialDirectory = Platforms.Platform.ProjectDirectory;
            var result = await dlg.ShowAsync();

            if (result != null)
            {
                CurrentSolution = Solution.Load(result[0]);
            }
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

       

        public void ShowNewProjectDialog()
        {
            ModalDialog = new NewProjectDialogViewModel();
            ModalDialog.ShowDialog();
        }

        public void ExitApplication()
        {
            Environment.Exit(1);
        }

        public void Debug()
        {
            if (CurrentPerspective == Perspective.Editor)
            {
                if (CurrentSolution.StartupProject != null)
                {
                    Console.Clear();
                    DebugManager.StartDebug(CurrentSolution.StartupProject);
                }
            }
            else
            {
                DebugManager.Continue();
            }
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
                    Debug();
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

        public MainMenuViewModel MainMenu { get; private set; }

        public ToolBarViewModel ToolBar { get; private set; }

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

        public ErrorListViewModel ErrorList { get; private set; }

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

                if(SolutionChanged != null)
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
    }
}
