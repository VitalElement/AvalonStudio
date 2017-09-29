using AvalonStudio.Languages;
using System;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.Tests
{
    class TestLanguageService : ILanguageService
    {
        public TestLanguageService()
        {
        }

        public global::AvaloniaEdit.Indentation.IIndentationStrategy IndentationStrategy => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public Type BaseTemplateType => throw new NotImplementedException();

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators { get; set; } = new Dictionary<string, Func<string, string>>();

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables { get; set; } = new Dictionary<string, Func<int, int, int, string>>();

        public IEnumerable<char> IntellisenseSearchCharacters => throw new NotImplementedException();

        public IEnumerable<char> IntellisenseCompleteCharacters => throw new NotImplementedException();

        public IEnumerable<char> ValidIdentifierCharacters => throw new NotImplementedException();

        public string LanguageId => "cpp";

        public void Activation()
        {
            throw new NotImplementedException();
        }

        public void BeforeActivation()
        {
            throw new NotImplementedException();
        }

        public bool CanHandle(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            throw new NotImplementedException();
        }

        public Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            throw new NotImplementedException();
        }

        public int Comment(ISourceFile file, global::AvaloniaEdit.Document.TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public int Format(ISourceFile file, global::AvaloniaEdit.Document.TextDocument textDocument, uint offset, uint length, int cursor)
        {
            throw new NotImplementedException();
        }

        public IList<global::AvaloniaEdit.Rendering.IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public IList<global::AvaloniaEdit.Rendering.IVisualLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            throw new NotImplementedException();
        }

        public Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterSourceFile(global::AvaloniaEdit.TextEditor editor, ISourceFile file, global::AvaloniaEdit.Document.TextDocument textDocument)
        {
            throw new NotImplementedException();
        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            throw new NotImplementedException();
        }

        public Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            throw new NotImplementedException();
        }

        public int UnComment(ISourceFile file, global::AvaloniaEdit.Document.TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public void UnregisterSourceFile(global::AvaloniaEdit.TextEditor editor, ISourceFile file)
        {
            throw new NotImplementedException();
        }
    }
}
