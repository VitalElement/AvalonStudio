namespace AvalonStudio.Languages
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using TextEditor.Document;
    using TextEditor.Rendering;

    [InheritedExport(typeof(ILanguageService))]
    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles, string filter);

        CodeAnalysisResults RunCodeAnalysis(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file);

        IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file);

        void RegisterSourceFile(ISourceFile file, TextDocument textDocument); 

        bool CanHandle(ISourceFile file);

        /// <summary>
        /// A description of the language supported by the service, i.e. C/C++
        /// </summary>
        string Title { get; }
    } 
}
