using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Utils;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public class CodeAnalysisResults
    {
        public CodeAnalysisResults()
        {
            SyntaxHighlightingData = new SyntaxHighlightDataList();
            FoldingInfo = new List<IndexEntry>();
            IndexTree = new IndexTree();
        }

        public SyntaxHighlightDataList SyntaxHighlightingData { get; set; }
        public IndexTree IndexTree { get; set; }
        public List<IndexEntry> FoldingInfo { get; set; }
    }
}