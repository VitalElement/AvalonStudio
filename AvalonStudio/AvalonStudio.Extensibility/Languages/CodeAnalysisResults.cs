using AvalonStudio.Extensibility.Languages;
using AvalonStudio.TextEditor.Document;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public class CodeAnalysisResults
    {
        public CodeAnalysisResults()
        {
            SyntaxHighlightingData = new SyntaxHighlightDataList();
            Diagnostics = new TextSegmentCollection<Diagnostic>();
            IndexItems = new List<IndexEntry>();
        }

        public SyntaxHighlightDataList SyntaxHighlightingData { get; set; }
        public TextSegmentCollection<Diagnostic> Diagnostics { get; set; }
        public List<IndexEntry> IndexItems { get; set; }
    }
}