namespace AvalonStudio.Models.LanguageServices
{
    using System;
    using System.Collections.Generic;

    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(uint line, uint column);

        void RunCodeAnalysis(Func<bool> interruptRequested);
    }
}
