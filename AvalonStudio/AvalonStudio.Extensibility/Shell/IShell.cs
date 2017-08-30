using AvalonStudio.Controls;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AvalonStudio.Shell
{
    public enum Perspective
    {
        Editor,
        Debug
    }

    public interface IShell
    {        
        ISolution CurrentSolution { get; set; }

        IObservable<ISolution> OnSolutionChanged { get; }

        event EventHandler<SolutionChangedEventArgs> SolutionChanged;

        IWorkspaceTaskRunner TaskRunner { get; }

        Perspective CurrentPerspective { get; set; }
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

        IEnumerable<IDebugger> Debugger2s { get; }

        IEnumerable<ITestFramework> TestFrameworks { get; }

        IEditor GetDocument(string path);

        Task<IEditor> OpenDocument(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true);

        Task CloseDocumentsForProjectAsync(IProject project);

        Task OpenSolutionAsync(string path);

        Task CloseSolutionAsync();

        void AddDocument(IDocumentTabViewModel document);

        void RemoveDocument(IDocumentTabViewModel document);

        void InvalidateCodeAnalysis();

        void InvalidateErrors();

        void Build(IProject project);

        void Clean(IProject project);

        void Build();

        void Clean();

        void Save();

        void SaveAll();

        IProject GetDefaultProject();

        bool DebugMode { get; }
    }
}
