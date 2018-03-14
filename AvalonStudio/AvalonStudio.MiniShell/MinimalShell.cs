namespace AvalonStudio.Shell
{
    using AvalonStudio.Debugging;
    using AvalonStudio.Documents;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Dialogs;
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.Languages;
    using AvalonStudio.Projects;
    using AvalonStudio.TestFrameworks;
    using AvalonStudio.Toolchains;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Composition;
    using System.Threading.Tasks;
    using AvalonStudio.Extensibility.Editor;
    using AvalonStudio.Extensibility.MainMenu;

    [Export]
    [Export(typeof(IShell))]
    [Shared]
    public class MinimalShell : IShell
    {
        public static IShell Instance { get; set; }

        private List<IToolChain> _toolChains;

        private IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> _languageServices;

        private IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> _solutionTypes;
        private IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> _projectTypes;

        private IEnumerable<IDebugger> _debugger2s;

        private IEnumerable<Lazy<ITestFramework>> _testFrameworks;

        public event EventHandler<FileOpenedEventArgs> FileOpened;
        public event EventHandler<FileOpenedEventArgs> FileClosed;
        public event EventHandler<BuildEventArgs> BuildStarting;
        public event EventHandler<BuildEventArgs> BuildCompleted;

        [ImportingConstructor]
        public MinimalShell(
            [ImportMany] IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> languageServices,
            [ImportMany] IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> solutionTypes,
            [ImportMany] IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> projectTypes,
            [ImportMany] IEnumerable<IDebugger> debugger2s,
            [ImportMany] IEnumerable<Lazy<ITestFramework>> testFrameworks,
            [ImportMany] IEnumerable<IExtension> extensions)
        {
            _languageServices = languageServices;

            _solutionTypes = solutionTypes;
            _projectTypes = projectTypes;

            _debugger2s = debugger2s;

            _testFrameworks = testFrameworks;

            _toolChains = new List<IToolChain>();

            IoC.RegisterConstant(this, typeof(IShell));

            foreach (var extension in extensions)
            {
                extension.BeforeActivation();
            }

            foreach (var extension in extensions)
            {
                extension.Activation();

                _toolChains.ConsumeExtension(extension);
            }

            IoC.RegisterConstant(this);
        }

        event EventHandler<SolutionChangedEventArgs> IShell.SolutionChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler SolutionChanged
        {
            add { throw new NotSupportedException(); }
            remove { }
        }

        public bool DebugMode { get; set; }

        public IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes => _projectTypes;

        public IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> LanguageServices => _languageServices;

        public IEnumerable<IToolChain> ToolChains => _toolChains;

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

        public ISolution CurrentSolution
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }

        public ObservableCollection<object> Tools
        {
            get { throw new NotImplementedException(); }
        }

        public object BottomSelectedTool
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }

        public ModalDialogViewModelBase ModalDialog
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }

        public Perspective CurrentPerspective
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
        }

        public IDocumentTabViewModel SelectedDocument
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IObservable<ISolution> OnSolutionChanged => throw new NotImplementedException();

        public IWorkspaceTaskRunner TaskRunner => throw new NotImplementedException();

        public ColorScheme CurrentColorScheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEditor OpenDocument(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false,
            bool selectLine = false, bool focus = true)
        {
            throw new NotImplementedException();
        }

        public void InvalidateCodeAnalysis()
        {
            throw new NotImplementedException();
        }

        public Task<bool> BuildAsync(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Clean(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Build()
        {
            throw new NotImplementedException();
        }

        public void Clean()
        {
            throw new NotImplementedException();
        }

        public IProject GetDefaultProject()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void SaveAll()
        {
            throw new NotImplementedException();
        }

        public IEditor GetDocument(string path)
        {
            throw new NotImplementedException();
        }

        public void Debug(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Debug()
        {
            throw new NotImplementedException();
        }

        public void AddDocument(IDocumentTabViewModel document)
        {
            throw new NotImplementedException();
        }

        public void RemoveDocument(IDocumentTabViewModel document)
        {
            throw new NotImplementedException();
        }

        public Task OpenSolutionAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task CloseDocumentsForProjectAsync(IProject project)
        {
            throw new NotImplementedException();
        }

        public Task CloseSolutionAsync()
        {
            throw new NotImplementedException();
        }        

        public void CloseDocument(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public void CloseDocumentsForProject(IProject project)
        {
            throw new NotImplementedException();
        }

        public void CloseSolution()
        {
            throw new NotImplementedException();
        }

        public void AddDocument(IDocumentTabViewModel document, bool temporary = true)
        {
            throw new NotImplementedException();
        }

        public IFileDocumentTabViewModel OpenDocument(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public Task<IEditor> OpenDocumentAsync(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true)
        {
            throw new NotImplementedException();
        }

        public IMenu BuildEditorContextMenu()
        {
            throw new NotImplementedException();
        }

        public void UpdateDiagnostics(DiagnosticsUpdatedEventArgs diagnostics)
        {
            throw new NotImplementedException();
        }
    }
}