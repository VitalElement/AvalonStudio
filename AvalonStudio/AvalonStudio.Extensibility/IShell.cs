﻿namespace AvalonStudio.Extensibility
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

    public interface IShell
    {
        event EventHandler SolutionChanged;
        ISolution CurrentSolution { get; }

        Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false, bool selectLine = false);
        ObservableCollection<object> Tools { get; }
        
        void InvalidateCodeAnalysis();
        void Debug();
        void Build();
        void Clean();
    }
}
