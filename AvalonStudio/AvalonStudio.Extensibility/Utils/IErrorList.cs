using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace AvalonStudio.Utils
{
    public interface IErrorList
    {
        ObservableCollection<ErrorViewModel> Errors { get; }

        /// <summary>
        /// Updates the errors in the error list.
        /// </summary>
        /// <param name="diagnostics">Update Args that are associated with a tagged source and a particular file.</param>
        void Create(object tag, DiagnosticSource source, ImmutableArray<Diagnostic> diagnostics, SyntaxHighlightDataList diagnosticHighlights = null);

        void Remove(object tag);

        /// <summary>
        /// Event that fires whenever there are any changes to the error list.
        /// </summary>
        event EventHandler<DiagnosticsUpdatedEventArgs> DiagnosticsUpdated;
    }
}