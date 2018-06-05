using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.MainMenu;
using AvalonStudio.Extensibility.Templating;
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
        ISolution CurrentSolution { get; }

        IObservable<ISolution> OnSolutionChanged { get; }

        event EventHandler<SolutionChangedEventArgs> SolutionChanged;

        event EventHandler<BuildEventArgs> BuildStarting;

        event EventHandler<BuildEventArgs> BuildCompleted;

        IWorkspaceTaskRunner TaskRunner { get; }

        Perspective CurrentPerspective { get; set; }
        IDocumentTabViewModel SelectedDocument { get; set; }        
        ModalDialogViewModelBase ModalDialog { get; set; }

        ColorScheme CurrentColorScheme { get; set; }

        IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes { get; }

        IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> LanguageServices { get; }

        IEnumerable<IToolchain> ToolChains { get; }

        IEnumerable<IDebugger> Debugger2s { get; }

        IEnumerable<ITestFramework> TestFrameworks { get; }

        IEditor GetDocument(string path);

        IFileDocumentTabViewModel OpenDocument(ISourceFile file);

        Task<IEditor> OpenDocumentAsync(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true);

        void CloseDocumentsForProject(IProject project);

        Task OpenSolutionAsync(string path);

        Task CloseSolutionAsync();

        void AddDocument(IDocumentTabViewModel document, bool temporary = true);

        void RemoveDocument(IDocumentTabViewModel document);

        void RemoveDocument(ISourceFile document);

        void InvalidateCodeAnalysis();

        void UpdateDiagnostics(DiagnosticsUpdatedEventArgs diagnostics);

        Task<bool> BuildAsync(IProject project);

        void Clean(IProject project);

        void Build();

        void Clean();

        void Save();

        void SaveAll();

        IProject GetDefaultProject();

        bool DebugMode { get; }

        double GlobalZoomLevel { get; set; }
    }
}