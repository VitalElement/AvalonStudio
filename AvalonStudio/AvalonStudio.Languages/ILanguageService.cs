namespace AvalonStudio.Languages
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(string fileName, int line, int column, List<UnsavedFile> unsavedFiles, string filter);

        CodeAnalysisResults RunCodeAnalysis(List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        bool SupportsFile(ISourceFile file);
    }

    [Export(typeof(ILanguageService))]
    public class LanguageService : ILanguageService
    {
        public List<CodeCompletionData> CodeCompleteAt(string fileName, int line, int column, List<UnsavedFile> unsavedFiles, string filter)
        {
            throw new NotImplementedException();
        }

        public CodeAnalysisResults RunCodeAnalysis(List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            throw new NotImplementedException();
        }

        public bool SupportsFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }
    }
}
