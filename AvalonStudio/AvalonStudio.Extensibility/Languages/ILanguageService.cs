using AvaloniaEdit.Document;
using AvaloniaEdit.Indentation;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Languages
{
    public interface ILanguageService : IExtension
    {
        IIndentationStrategy IndentationStrategy { get; }

        /// <summary>
        ///     A description of the language supported by the service, i.e. C/C++
        /// </summary>
        string Title { get; }

        /// <summary>
        /// A file path compatible name for the language, i.e. cs, cpp, ts, css, go, vb, fsharp
        /// </summary>
        string LanguageId { get; }

        /// <summary>
        ///     The base type that all Project templates for this language must inherit. This base class must implement
        ///     IProjectTemplate.
        /// </summary>
        Type BaseTemplateType { get; }

        /// <summary>
        /// Dictionary of functions for transforming snippet variables. Key is function name, the arugment is the string to transform.
        /// i.e. (propertyName) => "_" + propertyName
        /// </summary>
        IDictionary<string, Func<string, string>> SnippetCodeGenerators { get; }

        /// <summary>
        /// Dictionary of dynamic varables that can be evaluated by snippets. i.e. ClassName, arguments are CaretIndex, Line, Column.
        /// </summary>
        IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables { get; }

        Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "");

        bool CanTriggerIntellisense(char currentChar, char previousChar);
        IEnumerable<char> IntellisenseSearchCharacters { get; }
        IEnumerable<char> IntellisenseCompleteCharacters { get; }

        bool IsValidIdentifierCharacter(char data);

        Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, TextDocument textDocument, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        void RegisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file, TextDocument textDocument);

        void UnregisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file);

        bool CanHandle(ISourceFile file);

        int Format(ISourceFile file, TextDocument textDocument, uint offset, uint length, int cursor);

        int Comment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true);

        int UnComment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true);

        Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName);

        Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset);

        Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name);
    }
}