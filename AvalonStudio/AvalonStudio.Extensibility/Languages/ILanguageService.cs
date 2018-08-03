using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        public DiagnosticsUpdatedEventArgs(object tag, DiagnosticsUpdatedKind kind)
        {
            Tag = tag;
            Kind = kind;
            Source = DiagnosticSourceKind.Misc;
        }

        public DiagnosticsUpdatedEventArgs(object tag, string filePath, DiagnosticsUpdatedKind kind, DiagnosticSourceKind source, ImmutableArray<Diagnostic> diagnostics, SyntaxHighlightDataList diagnosticHighlights = null)
        {
            Tag = tag;
            FilePath = filePath;
            Kind = kind;
            Diagnostics = diagnostics;
            DiagnosticHighlights = diagnosticHighlights;
            Source = source;
        }

        public object Tag { get; }
        public string FilePath { get; }
        public DiagnosticsUpdatedKind Kind { get; }
        public DiagnosticSourceKind Source { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public SyntaxHighlightDataList DiagnosticHighlights { get; }
    }

    public interface ILanguageService
    {
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

        IEnumerable<ITextEditorInputHelper> InputHelpers { get; }

        bool IsValidIdentifierCharacter(char data);

        /// <summary>
        /// Registers the editor that the instance of the language service is associated with.
        /// Called when the editor is opened.
        /// </summary>
        void RegisterEditor(ITextEditor editor);

        /// <summary>
        /// Unregisters the editor. Called when the editor is closed.
        /// </summary>
        void UnregisterEditor();

        int Format(uint offset, uint length, int cursor);

        int Comment(int firstLine, int endLine, int caret = -1, bool format = true);

        int UnComment(int firstLine, int endLine, int caret = -1, bool format = true);

        Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEnumerable<UnsavedFile> unsavedFiles, Func<bool> interruptRequested);

        Task<CodeCompletionResults> CodeCompleteAtAsync(int index, int line, int column, IEnumerable<UnsavedFile> unsavedFiles, char lastChar, string filter = "");

        Task<SignatureHelp> SignatureHelp(IEnumerable<UnsavedFile> unsavedFiles, int offset, string methodName);

        Task<QuickInfoResult> QuickInfo(IEnumerable<UnsavedFile> unsavedFiles, int offset);

        IEnumerable<IContextActionProvider> GetContextActionProviders();

        Task<GotoDefinitionInfo> GotoDefinition(int offset);

        Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(string renameTo);

        Task<List<Symbol>> GetSymbolsAsync(IEnumerable<UnsavedFile> unsavedFiles, string name);
    }
}