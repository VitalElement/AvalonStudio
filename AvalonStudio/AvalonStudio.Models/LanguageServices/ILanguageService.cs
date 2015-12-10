namespace AvalonStudio.Models.LanguageServices
{
    using System;
    using System.Collections.Generic;


    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(string fileName, int line, int column, List<UnsavedFile> unsavedFiles);

        CodeAnalysisResults RunCodeAnalysis(List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);
    }
}
