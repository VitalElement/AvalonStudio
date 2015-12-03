namespace AvalonStudio.Models.LanguageServices
{
    using TextEditor;
    using System;
    using System.Collections.Generic;


    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(uint line, uint column);

        SyntaxHighlightDataList RunCodeAnalysis(List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);
    }
}
