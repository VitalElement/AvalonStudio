using AvalonStudio.Extensibility.Languages;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public class CodeAnalysisResults
    {
        public CodeAnalysisResults()
        {
            SyntaxHighlightingData = new SyntaxHighlightDataList();            
            IndexItems = new List<IndexEntry>();
        }

        public SyntaxHighlightDataList SyntaxHighlightingData { get; set; }        
        public List<IndexEntry> IndexItems { get; set; }
    }
}