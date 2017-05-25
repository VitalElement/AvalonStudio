using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public interface IErrorList
    {
        void AddDiagnostic(ErrorViewModel error);

        void RemoveDiagnostic(ErrorViewModel error);

        void AddFixIt(FixIt fixit);

        void ClearFixits(Predicate<Diagnostic> predicate);

        ReadOnlyCollection<Diagnostic> FindDiagnosticsAtOffset(ISourceFile file, int offset);
        
        IReadOnlyCollection<ErrorViewModel> Errors { get; }

        IReadOnlyCollection<ErrorViewModel> FixIts { get; }
    }
}