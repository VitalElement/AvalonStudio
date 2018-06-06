using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.MainMenu;
using AvalonStudio.Extensibility.Plugin;
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
        // Shell
        IWorkspaceTaskRunner TaskRunner { get; }

        Perspective CurrentPerspective { get; set; }

        IDocumentTabViewModel SelectedDocument { get; set; }

        ModalDialogViewModelBase ModalDialog { get; set; }

        ColorScheme CurrentColorScheme { get; set; }

        IEnumerable<T> GetExtensions<T>();

        void AddDocument(IDocumentTabViewModel document, bool temporary = true);

        void RemoveDocument(IDocumentTabViewModel document);

        // Workspace
        ISolution CurrentSolution { get; }

        IObservable<ISolution> OnSolutionChanged { get; }

        event EventHandler<SolutionChangedEventArgs> SolutionChanged;

        event EventHandler<BuildEventArgs> BuildStarting;

        event EventHandler<BuildEventArgs> BuildCompleted;

        IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes { get; }

        IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> LanguageServices { get; }

        IEnumerable<ITestFramework> TestFrameworks { get; }

        IFileDocumentTabViewModel OpenDocument(ISourceFile file);

        void RemoveDocument(ISourceFile document);

        Task<IEditor> OpenDocumentAsync(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true);

        void CloseDocumentsForProject(IProject project);

        Task OpenSolutionAsync(string path);

        Task CloseSolutionAsync();

        Task<bool> BuildAsync(IProject project);

        void Clean(IProject project);

        void Build();

        void Clean();

        void Save();

        void SaveAll();

        IProject GetDefaultProject();


        // Misc?
        IEditor GetDocument(string path);

        void InvalidateCodeAnalysis();

        bool DebugMode { get; }

        double GlobalZoomLevel { get; set; }
    }
}