namespace AvalonStudio.Models.LanguageServices
{
    using System.Collections.Generic;

    interface LanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(uint line, uint column);
    }
}
