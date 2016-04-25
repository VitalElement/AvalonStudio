namespace AvalonStudio.Extensibility
{
    using AvalonStudio.Projects;
    using Documents;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Perspex.Reactive;
    using Dialogs;
    using TestFrameworks;
    using Debugging;
    using Toolchains;
    using Languages;
    public interface IShell
    {
        event EventHandler SolutionChanged;
        ISolution CurrentSolution { get; set; }

        Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false, bool selectLine = false);
        ObservableCollection<object> Tools { get; }

        object BottomSelectedTool { get; set; }
        ModalDialogViewModelBase ModalDialog { get; set; }

        void InvalidateCodeAnalysis();
        void Debug(IProject project);
        void Build(IProject project);
        void Clean(IProject project);

        void Debug();
        void Build();
        void Clean();

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
