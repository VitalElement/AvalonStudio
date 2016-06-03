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
    using System.Threading.Tasks;

    [InheritedExport(typeof(ILanguageService))]
    public interface ILanguageService
    {
        Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles, string filter);

        Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file);

        IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file);

        void RegisterSourceFile(IIntellisenseControl intellisenseControl, ICompletionAdviceControl completionAdviceControl, TextEditor editor, ISourceFile file, TextDocument textDocument);

        void UnregisterSourceFile(TextEditor editor, ISourceFile file);

        bool CanHandle(ISourceFile file);

        int Format(TextDocument textDocument, uint offset, uint length, int cursor);

        int Comment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true);
        int UnComment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true);

        IIndentationStrategy IndentationStrategy { get; }

        Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset);

        Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name);

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
