using AvalonStudio.Languages;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public interface IErrorList
    {
        void AddDiagnostic(ErrorViewModel error);

        void RemoveDiagnostic(ErrorViewModel error);

        ReadOnlyCollection<Diagnostic> FindDiagnosticsAtOffset(int offset);
        
        IReadOnlyCollection<ErrorViewModel> Errors { get; }

        IReadOnlyCollection<ErrorViewModel> FixIts { get; }
    }
}