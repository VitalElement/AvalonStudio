using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;

namespace AvalonStudio.Shell
{
	public enum Perspective
	{
		Editor,
		Debug
	}


	public interface IShell
	{
		Perspective CurrentPerspective { get; set; }
		ISolution CurrentSolution { get; set; }
		IEditor SelectedDocument { get; }
		ObservableCollection<object> Tools { get; }
		ModalDialogViewModelBase ModalDialog { get; set; }
		object BottomSelectedTool { get; set; }

        IEnumerable<ISolutionType> SolutionTypes { get; }

        IEnumerable<IProject> ProjectTypes { get; }

		IEnumerable<IProjectTemplate> ProjectTemplates { get; }

		IEnumerable<ICodeTemplate> CodeTemplates { get; }

		IEnumerable<ILanguageService> LanguageServices { get; }

		IEnumerable<IToolChain> ToolChains { get; }        

		IEnumerable<IDebugger> Debuggers { get; }

		IEnumerable<ITestFramework> TestFrameworks { get; }

		event EventHandler SolutionChanged;

		IEditor GetDocument(string path);

		Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false,
			bool selectLine = false);

		void InvalidateCodeAnalysis();

		void Build(IProject project);
		void Clean(IProject project);

		void Build();
		void Clean();

		void Save();
		void SaveAll();

		IProject GetDefaultProject();
	}
}