﻿namespace AvalonStudio
{
    using Platform;
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

    public enum Perspective
    {
        Editor,
        Debug
    }

    [Export(typeof(WorkspaceViewModel))]
    public class WorkspaceViewModel : ViewModel<Workspace>
    {
        public static WorkspaceViewModel Instance = null;

        [ImportingConstructor]
        public WorkspaceViewModel([Import] Workspace workspace) : base(workspace)
        {
            CurrentPerspective = Perspective.Editor;

            MainMenu = new MainMenuViewModel();
            SolutionExplorer = new SolutionExplorerViewModel();
            Console = new ConsoleViewModel();
            ErrorList = new ErrorListViewModel();
            ToolBar = new ToolBarViewModel();
            StatusBar = new StatusBarViewModel();

            Documents = new ObservableCollection<EditorViewModel>();

            DebugManager = new DebugManager();

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platform.Platform.PlatformString;

            SolutionExplorer.SelectedItemChanged += (sender, e) =>
            {
                if (e is SourceFileViewModel)
                {
                    var currentTab = Documents.FirstOrDefault(t => t.Model.ProjectFile.File == ((ISourceFile)(e as SourceFileViewModel).Model).File);

                    if (currentTab == null)
                    {
                        var newEditor = new EditorViewModel(new EditorModel());
                        Documents.Add(newEditor);
                        SelectedDocument = newEditor;

                        newEditor.Margins.Add(new BreakPointMargin(DebugManager.BreakPointManager));
                        newEditor.Margins.Add(new LineNumberMargin());
                        newEditor.Model.OpenFile((e as SourceFileViewModel).Model as ISourceFile);
                    }
                    else
                    {
                        SelectedDocument = currentTab;
                    }
                }
            };

            ProcessCancellationToken = new CancellationTokenSource();

            ModalDialog = new ModalDialogViewModelBase("Dialog");

            CurrentPerspective = Perspective.Editor;
        }

        public void Save()
        {
            SelectedDocument?.Save();
        }

        public void SaveAll()
        {
            foreach (var document in Documents)
            {
                document.Save();
            }
        }

        public void Clean()
        {
            new Thread(new ThreadStart(new Action(async () =>
            {
                if (SolutionExplorer.Model != null)
                {
                    if (SolutionExplorer.Model.StartupProject != null)
                    {
                        if (SolutionExplorer.Model.StartupProject.ToolChain != null)
                        {
                            await SolutionExplorer.Model.StartupProject.ToolChain.Clean(Console, SolutionExplorer.Model.StartupProject);
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

            new Thread(new ThreadStart(new Action(async () =>
            {
                if (SolutionExplorer.Model != null)
                {
                    if (SolutionExplorer.Model.StartupProject != null)
                    {
                        if (SolutionExplorer.Model.StartupProject.ToolChain != null)
                        {
                            await Task.Factory.StartNew(() => SolutionExplorer.Model.StartupProject.ToolChain.Build(Console, SolutionExplorer.Model.StartupProject));
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
            dlg.InitialDirectory = Platform.Platform.ProjectDirectory;
            var result = await dlg.ShowAsync();

            if (result != null)
            {
                WorkspaceViewModel.Instance.SolutionExplorer.Model = Solution.Load(result[0]);
            }
        }

        public void ShowPackagesDialog()
        {
            ModalDialog = new PackageManagerDialogViewModel();
            ModalDialog.ShowDialog();
        }

        public void ShowProjectPropertiesDialog()
        {
            ModalDialog = new ProjectConfigurationDialogViewModel(WorkspaceViewModel.Instance.SolutionExplorer.SelectedProject, () => { });
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

        public void StartDebugSession()
        {
            if (CurrentPerspective == Perspective.Editor)
            {
                if (SolutionExplorer.Model?.StartupProject != null)
                {
                    DebugManager.StartDebug(SolutionExplorer.Model.StartupProject);
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
                    if (CurrentPerspective == Perspective.Editor)
                    {
                        if (SolutionExplorer.Solution?.FirstOrDefault()?.Model.StartupProject != null)
                        {
                            DebugManager.StartDebug(SolutionExplorer.Solution.FirstOrDefault()?.Model.StartupProject);
                        }
                    }
                    else
                    {
                        DebugManager.Continue();
                    }
                    break;
            }
        }

        public void InvalidateErrors()
        {
            var allErrors = new List<ErrorViewModel>();
            var toRemove = new List<ErrorViewModel>();

            foreach (var document in documents)
            {
                foreach (var diagnostic in document.Model.CodeAnalysisResults.Diagnostics)
                {
                    //if (diagnostic.Location.FileLocation.File.FileName.NormalizePath() == document.FilePath.NormalizePath())
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
                ErrorList.Errors.Remove(error);
            }

            foreach (var error in allErrors)
            {
                var matching = ErrorList.Errors.SingleOrDefault((err) => err.IsEqual(error));

                if (matching == null)
                {
                    //hasChanged = true;
                    ErrorList.Errors.Add(error);
                }
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

        public SolutionExplorerViewModel SolutionExplorer { get; private set; }

        private ObservableCollection<EditorViewModel> documents;
        public ObservableCollection<EditorViewModel> Documents
        {
            get { return documents; }
            set { this.RaiseAndSetIfChanged(ref documents, value); }
        }

        private EditorViewModel selectedDocument;
        public EditorViewModel SelectedDocument
        {
            get { return selectedDocument; }
            set { this.RaiseAndSetIfChanged(ref selectedDocument, value); value?.Model.Editor?.Focus(); }
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
            foreach(var document in Documents)
            {
                //TODO implement code analysis trigger.
            }
        }

        private bool hideWhenModalVisibility = true;
        public bool HideWhenModalVisibility
        {
            get { return hideWhenModalVisibility; }
            set { hideWhenModalVisibility = value; this.RaisePropertyChanged(); }
        }

        public void Cleanup()
        {
            foreach(var document in Documents)
            {
                document.Model.ShutdownBackgroundWorkers();
            }
        }
    }
}
