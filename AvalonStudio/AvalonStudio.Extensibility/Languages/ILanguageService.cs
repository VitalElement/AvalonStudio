using AvaloniaEdit.Indentation;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Extensibility.Plugin;
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
        /// An identifier compatible with Dot CLI language identifiers i.e. C#, F#, VB, etc
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Dictionary of functions for transforming snippet variables. Key is function name, the arugment is the string to transform.
        /// i.e. (propertyName) => "_" + propertyName
        /// </summary>
        IDictionary<string, Func<string, string>> SnippetCodeGenerators { get; }

        /// <summary>
        /// Dictionary of dynamic varables that can be evaluated by snippets. i.e. ClassName, arguments are CaretIndex, Line, Column.
        /// </summary>
        IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables { get; }

        bool CanTriggerIntellisense(char currentChar, char previousChar);
        IEnumerable<char> IntellisenseSearchCharacters { get; }
        IEnumerable<char> IntellisenseCompleteCharacters { get; }
        IEnumerable<ICodeEditorInputHelper> InputHelpers { get; }

        //IObservable<TextSegmentCollection<Diagnostic>> Diagnostics { get; }

        bool IsValidIdentifierCharacter(char data);

        Task<CodeCompletionResults> CodeCompleteAtAsync(IEditor editor, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "");

        Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEditor editor, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        Task<SignatureHelp> SignatureHelp(IEditor editor, List<UnsavedFile> unsavedFiles, int offset, string methodName);

        Task<Symbol> GetSymbolAsync(IEditor editor, List<UnsavedFile> unsavedFiles, int offset);

        Task<List<Symbol>> GetSymbolsAsync(IEditor editor, List<UnsavedFile> unsavedFiles, string name);

        void RegisterSourceFile(IEditor editor);

        void UnregisterSourceFile(IEditor editor);

        bool CanHandle(IEditor editor);

        int Format(IEditor editor, uint offset, uint length, int cursor);

        int Comment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true);

        int UnComment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true);
    }
}