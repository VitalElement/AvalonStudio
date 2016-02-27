namespace AvalonStudio.Languages
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using TextEditor.Document;
    using TextEditor.Indentation;
    using TextEditor.Rendering;
    using TextEditor;

    [InheritedExport(typeof(ILanguageService))]
    public interface ILanguageService
    {
        List<CodeCompletionData> CodeCompleteAt(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles, string filter);

        CodeAnalysisResults RunCodeAnalysis(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file);

        IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file);

        void RegisterSourceFile(TextEditor editor, ISourceFile file, TextDocument textDocument);

        void UnregisterSourceFile(TextEditor editor, ISourceFile file);

        bool CanHandle(ISourceFile file);

        int Format(ISourceFile file, TextDocument textDocument, uint offset, uint length, int cursor);

        IIndentationStrategy IndentationStrategy { get; }

        Symbol GetSymbol(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset);

        Symbol GetSymbol(ISourceFile file, List<UnsavedFile> unsavedFiles, string name);

        /// <summary>
        /// A description of the language supported by the service, i.e. C/C++
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The base type that all Project templates for this language must inherit. This base class must implement IProjectTemplate.
        /// </summary>
        Type BaseTemplateType { get; }
    } 
}
