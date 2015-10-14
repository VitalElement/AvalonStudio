namespace AvalonStudio.Models.LanguageServices
{
    using TextEditor;
    using System;
    using System.Collections.Generic;


    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(uint line, uint column);

        List<SyntaxHighlightingData> RunCodeAnalysis(Func<bool> interruptRequested);
    }
}
