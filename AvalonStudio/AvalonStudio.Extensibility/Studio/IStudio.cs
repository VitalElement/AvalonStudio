using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;

namespace AvalonStudio.Extensibility.Studio
{
    public interface IStudio
    {
        IEnumerable<Lazy<ILanguageServiceProvider, LanguageServiceProviderMetadata>> LanguageServiceProviders { get; }
        IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> SolutionTypes { get; }
        IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes { get; }
        IEnumerable<Lazy<IEditorProvider>> EditorProviders { get; }
        IEnumerable<Lazy<ITestFramework>> TestFrameworks { get; }

        ColorScheme CurrentColorScheme { get; set; }

        IPerspective DebugPerspective { get; }

        Perspective CurrentPerspective { get; set; }

        ISolution CurrentSolution { get; set; }

        Task OpenSolutionAsync(string path);

        Task CloseSolutionAsync();

        IObservable<ISolution> OnSolutionChanged { get; }

        event EventHandler<SolutionChangedEventArgs> SolutionChanged;        

        IWorkspaceTaskRunner TaskRunner { get; }

        event EventHandler<BuildEventArgs> BuildStarting;

        event EventHandler<BuildEventArgs> BuildCompleted;

        ITextEditor GetEditor(string path);

        void RemoveDocument(ISourceFile document);

        Task<ITextDocument> CreateDocumentAsync(string path);

        Task<ITextDocumentTabViewModel> OpenDocumentAsync(ISourceFile file, int line, int startColumn = -1, int endColumn = -1, bool debugHighlight = false, bool selectLine = false, bool focus = true);

        void CloseDocumentsForProject(IProject project);

        void ShowQuickCommander();

        Task<bool> BuildAsync(IProject project);

        void Clean(IProject project);

        void Build();

        void Clean();

        void Save();

        void SaveAll();

        IProject GetDefaultProject();

        void InvalidateCodeAnalysis();

        bool DebugMode { get; }

        double GlobalZoomLevel { get; set; }
    }
}