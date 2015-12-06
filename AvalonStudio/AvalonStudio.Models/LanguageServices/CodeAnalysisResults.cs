namespace AvalonStudio.Models.LanguageServices
{
    using System.Collections.Generic;

    public class CodeAnalysisResults
    {
        public CodeAnalysisResults()
        {
            SyntaxHighlightingData = new SyntaxHighlightDataList();
            Diagnostics = new List<Diagnostic>();
        }

        public SyntaxHighlightDataList SyntaxHighlightingData { get; set; }
        public List<Diagnostic> Diagnostics { get; set; }
    }
}
