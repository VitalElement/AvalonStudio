using AvalonStudio.Languages;
using System.Collections.ObjectModel;

namespace AvalonStudio.Utils
{
    public interface IErrorList
    {
        ObservableCollection<ErrorViewModel> Errors { get; }

        void UpdateDiagnostics(DiagnosticsUpdatedEventArgs diagnostics);
    }
}