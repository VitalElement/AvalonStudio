using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;

namespace AvalonStudio.Shell
{
	[Export(typeof (IShell))]
	public class MinimalShell : IShell
	{
		public static IShell Instance = null;

		private readonly IEnumerable<IPlugin> plugins;

		[ImportingConstructor]
		public MinimalShell([ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<ISolutionType> solutionTypes,
			[ImportMany] IEnumerable<IProject> projectTypes, [ImportMany] IEnumerable<IProjectTemplate> projectTemplates,
			[ImportMany] IEnumerable<IToolChain> toolChains, [ImportMany] IEnumerable<IDebugger> debuggers,
			[ImportMany] IEnumerable<ITestFramework> testFrameworks, [ImportMany] IEnumerable<ICodeTemplate> codeTemplates)
		{
			LanguageServices = languageServices;
			ProjectTemplates = projectTemplates;
			ToolChains = toolChains;
			Debuggers = debuggers;
            SolutionTypes = solutionTypes;
			ProjectTypes = projectTypes;
			TestFrameworks = testFrameworks;
			CodeTemplates = codeTemplates;

			IoC.RegisterConstant(this, typeof (IShell));
		}

		public event EventHandler SolutionChanged;

		public IEnumerable<IProject> ProjectTypes { get; }

        public IEnumerable<ISolutionType> SolutionTypes { get; }

        public IEnumerable<IProjectTemplate> ProjectTemplates { get; }

		public IEnumerable<ICodeTemplate> CodeTemplates { get; }

		public IEnumerable<ILanguageService> LanguageServices { get; }

		public IEnumerable<IToolChain> ToolChains { get; }

		public IEnumerable<IDebugger> Debuggers { get; }

		public IEnumerable<ITestFramework> TestFrameworks { get; }

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

		public IEditor SelectedDocument
		{
			get { throw new NotImplementedException(); }
		}        

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
	}
}