namespace AvalonStudio.Shell
{
    using AvalonStudio.Controls;
    using AvalonStudio.Debugging;
    using AvalonStudio.Documents;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Dialogs;
    using AvalonStudio.Languages;
    using AvalonStudio.Projects;
    using AvalonStudio.TestFrameworks;
    using AvalonStudio.Toolchains;
    using System;
    using System.Composition;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;    
    using System.Threading.Tasks;
    using AvalonStudio.Extensibility.Plugin;

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
    public class MinimalShell : IShell
	{
		public static IShell Instance = null;

        private List<ILanguageService> _languageServices;
        private List<IProjectTemplate> _projectTemplates;
        private List<ISolutionType> _solutionTypes;
        private List<IProjectType> _projectTypes;
        private List<IToolChain> _toolChains;
        private List<IDebugger> _debuggers;
        private List<ITestFramework> _testFrameworks;

        [ImportingConstructor]
		public MinimalShell([ImportMany] IEnumerable<IExtension> extensions)
		{
            _languageServices = new List<ILanguageService>();
            _projectTemplates = new List<IProjectTemplate>();
            _debuggers = new List<IDebugger>();
            _projectTypes = new List<IProjectType>();
            _solutionTypes = new List<ISolutionType>();
            _testFrameworks = new List<ITestFramework>();
            _toolChains = new List<IToolChain>();

            IoC.RegisterConstant(this, typeof (IShell));

            foreach (var extension in extensions)
            {
                extension.BeforeActivation();
            }

            foreach (var extension in extensions)
            {
                extension.Activation();

                _languageServices.ConsumeExtension(extension);
                _toolChains.ConsumeExtension(extension);
                _projectTemplates.ConsumeExtension(extension);
                _debuggers.ConsumeExtension(extension);
                _solutionTypes.ConsumeExtension(extension);
                _projectTypes.ConsumeExtension(extension);
                _testFrameworks.ConsumeExtension(extension);
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

        public IEnumerable<ISolutionType> SolutionTypes => _solutionTypes;

        public IEnumerable<IProjectType> ProjectTypes => _projectTypes;

        public IEnumerable<IProjectTemplate> ProjectTemplates => _projectTemplates;

        public IEnumerable<ILanguageService> LanguageServices => _languageServices;

        public IEnumerable<IToolChain> ToolChains => _toolChains;

        public IEnumerable<IDebugger> Debuggers => _debuggers;

        public IEnumerable<ITestFramework> TestFrameworks => _testFrameworks;

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

        public IEnumerable<ICodeTemplate> CodeTemplates { get; }

        public Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false,
			bool selectLine = false)
		{
			throw new NotImplementedException();
		}

		public void InvalidateCodeAnalysis()
		{
			throw new NotImplementedException();
		}

		public void Build(IProject project)
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
    }
}