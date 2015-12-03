namespace AvalonStudio.Models.LanguageServices
{
    using TextEditor;
    using System;
    using System.Collections.Generic;


    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(uint line, uint column);

        SyntaxHighlightDataList RunCodeAnalysis(Func<bool> interruptRequested);
    }
}
