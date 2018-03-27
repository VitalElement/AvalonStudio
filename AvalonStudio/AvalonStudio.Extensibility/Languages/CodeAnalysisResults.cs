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
        }

        public SyntaxHighlightDataList SyntaxHighlightingData { get; set; }
        public TreeNode<IndexEntry> IndexTree { get; set; }
        public List<IndexEntry> FoldingInfo { get; set; }
    }
}