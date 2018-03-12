using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Projects;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public class CodeAnalysisResults
    {
        public CodeAnalysisResults(object tag, ISourceFile associatedFile)
        {
            SyntaxHighlightingData = new SyntaxHighlightDataList(tag, associatedFile);            
            IndexItems = new List<IndexEntry>();
        }

        public SyntaxHighlightDataList SyntaxHighlightingData { get; set; }        
        public List<IndexEntry> IndexItems { get; set; }
    }
}