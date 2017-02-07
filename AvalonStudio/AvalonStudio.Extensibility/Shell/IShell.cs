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
using AvalonStudio.Controls;

namespace AvalonStudio.Shell
{
	public enum Perspective
	{
		Editor,
		Debug
	}


	public interface IShell
	{
        event EventHandler<SolutionChangedEventArgs> SolutionChanged;

		Perspective CurrentPerspective { get; set; }
		ISolution CurrentSolution { get; set; }
        IDocumentTabViewModel SelectedDocument { get; set; }
		ObservableCollection<object> Tools { get; }
		ModalDialogViewModelBase ModalDialog { get; set; }
		object BottomSelectedTool { get; set; }

        IEnumerable<ISolutionType> SolutionTypes { get; }

        IEnumerable<IProjectType> ProjectTypes { get; }

		IEnumerable<IProjectTemplate> ProjectTemplates { get; }

		IEnumerable<ICodeTemplate> CodeTemplates { get; }

		IEnumerable<ILanguageService> LanguageServices { get; }

		IEnumerable<IToolChain> ToolChains { get; }        

		IEnumerable<IDebugger> Debuggers { get; }

		IEnumerable<ITestFramework> TestFrameworks { get; }
        
		IEditor GetDocument(string path);

		Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false, bool selectLine = false);

        Task CloseDocumentsForProjectAsync(IProject project);

        Task OpenSolutionAsync(string path);

        Task CloseSolutionAsync();

        void AddDocument(IDocumentTabViewModel document);
        void RemoveDocument(IDocumentTabViewModel document);

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