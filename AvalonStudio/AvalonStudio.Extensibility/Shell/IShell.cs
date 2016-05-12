namespace AvalonStudio.Shell
{
    using AvalonStudio.Projects;
    using Documents;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Avalonia.Reactive;
    using TestFrameworks;
    using Debugging;
    using Toolchains;
    using Languages;
    using Extensibility.Dialogs;

    public enum Perspective
    {
        Editor,
        Debug
    }


    public interface IShell
    {
        Perspective CurrentPerspective { get; set; }

        event EventHandler SolutionChanged;
        ISolution CurrentSolution { get; set; }

        IEditor GetDocument(string path);
        IEditor SelectedDocument { get; }

        Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false, bool selectLine = false);
        ObservableCollection<object> Tools { get; }
        ModalDialogViewModelBase ModalDialog { get; set; }
        object BottomSelectedTool { get; set; }
        void InvalidateCodeAnalysis();
        
        void Build(IProject project);
        void Clean(IProject project);
        
        void Build();
        void Clean();

        void Save();
        void SaveAll();

        IProject GetDefaultProject();

        IEnumerable<IProject> ProjectTypes { get; }

        IEnumerable<IProjectTemplate> ProjectTemplates { get; }

        IEnumerable<ICodeTemplate> CodeTemplates { get; }

        IEnumerable<ILanguageService> LanguageServices { get; }

        IEnumerable<IToolChain> ToolChains { get; }

        IEnumerable<IDebugger> Debuggers { get; }

        IEnumerable<ITestFramework> TestFrameworks { get; }
    }
}
