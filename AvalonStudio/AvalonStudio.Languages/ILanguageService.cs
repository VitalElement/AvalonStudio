﻿namespace AvalonStudio.Languages
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    [InheritedExport(typeof(ILanguageService))]
    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(string fileName, int line, int column, List<UnsavedFile> unsavedFiles, string filter);

        CodeAnalysisResults RunCodeAnalysis(List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        bool SupportsFile(ISourceFile file);
    }
}
