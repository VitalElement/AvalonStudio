using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Extensibility.Editor
{
    public class CodeEditorViewModel : TextEditorViewModel, IDebugLineDocumentTabViewModel
    {
        private DebugHighlightLocation _debugHighlight;

        public CodeEditorViewModel(ITextDocument document, ISourceFile file) : base (document, file)
        {

        }

        public DebugHighlightLocation DebugHighlight
        {
            get { return _debugHighlight; }
            set { this.RaiseAndSetIfChanged(ref _debugHighlight, value); }
        }

        // HighlightingProvider

        // CompletionProvider

        // Formatter

        // Indentation Strategy
    }
}
