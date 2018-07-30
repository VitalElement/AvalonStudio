using AvalonStudio.Languages;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace AvalonStudio.Utils
{
    public interface IErrorList
    {
        ObservableCollection<ErrorViewModel> Errors { get; }

        /// <summary>
        /// Creates a set of diagnostics in the error list.
        /// </summary>
        /// <param name="tag">A unique tag where equality will identify the source.</param>
        /// <param name="sourceKind">The source kind of the diagnostic, misc, build or analysis.</param>
        /// <param name="diagnostics">The diagnostics to add.</param>
        /// <param name="diagnosticHighlights">Any special syntax highlighting for editors to show.</param>
        void Create(object tag, string filePath, DiagnosticSourceKind sourceKind, ImmutableArray<Diagnostic> diagnostics, SyntaxHighlightDataList diagnosticHighlights = null);

        /// <summary>
        /// Removes any diagnostics from the list with a matching tag.
        /// </summary>
        /// <param name="tag">The tag to identify diagnostics to remove.</param>
        void Remove(object tag);

        /// <summary>
        /// Event that fires whenever there are any changes to the error list.
        /// </summary>
        event EventHandler<DiagnosticsUpdatedEventArgs> DiagnosticsUpdated;
    }
}