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

    public interface IShell
    {
        event EventHandler SolutionChanged;
        ISolution CurrentSolution { get; set; }

        Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false, bool selectLine = false);
        ObservableCollection<object> Tools { get; }
        ModalDialogViewModelBase ModalDialog { get; set; }
        Shell Model { get; }

        void InvalidateCodeAnalysis();
        void Debug();
        void Build();
        void Clean();
    }
}
