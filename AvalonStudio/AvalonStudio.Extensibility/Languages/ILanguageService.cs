using AvaloniaEdit.Indentation;
using AvalonStudio.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Languages
{
    public enum DiagnosticsUpdatedKind
    {
        DiagnosticsRemoved,
        DiagnosticsCreated
    }

    public class DiagnosticsUpdatedEventArgs : EventArgs
    {
        public DiagnosticsUpdatedEventArgs(object tag, ISourceFile associatedFile, DiagnosticsUpdatedKind kind, ImmutableArray<Diagnostic> diagnostics, SyntaxHighlightDataList diagnosticHighlights = null)
        {
            Tag = tag;
            AssociatedSourceFile = associatedFile;
            Kind = kind;
            Diagnostics = diagnostics;
            DiagnosticHighlights = diagnosticHighlights;
        }

        public object Tag { get; }
        public ISourceFile AssociatedSourceFile { get; }
        public DiagnosticsUpdatedKind Kind { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public SyntaxHighlightDataList DiagnosticHighlights { get; }
    }

    public interface ILanguageService
    {
        IIndentationStrategy IndentationStrategy { get; }

        ISyntaxHighlightingProvider SyntaxHighlighter { get; }

        /// <summary>
        /// A file path compatible name for the language, i.e. cs, cpp, ts, css, go, vb, fsharp
        /// </summary>
        string LanguageId { get; }

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

        event EventHandler<DiagnosticsUpdatedEventArgs> DiagnosticsUpdated;        

        bool IsValidIdentifierCharacter(char data);

        Task<CodeCompletionResults> CodeCompleteAtAsync(IEditor editor, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "");

        Task<CodeAnalysisResults> RunCodeAnalysisAsync(ITextEditor editor, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);        

        IEnumerable<IContextActionProvider> GetContextActionProviders(IEditor editor);

        Task<SignatureHelp> SignatureHelp(IEditor editor, List<UnsavedFile> unsavedFiles, int offset, string methodName);

        Task<QuickInfoResult> QuickInfo(IEditor editor, List<UnsavedFile> unsavedFiles, int offset);

        Task<List<Symbol>> GetSymbolsAsync(IEditor editor, List<UnsavedFile> unsavedFiles, string name);

        Task<GotoDefinitionInfo> GotoDefinition(IEditor editor, int offset);

        Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(IEditor editor, string renameTo);

        void RegisterSourceFile(ITextEditor editor);

        void UnregisterSourceFile(IEditor editor);

        bool CanHandle(IEditor editor);

        int Format(IEditor editor, uint offset, uint length, int cursor);

        int Comment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true);

        int UnComment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true);
    }
}