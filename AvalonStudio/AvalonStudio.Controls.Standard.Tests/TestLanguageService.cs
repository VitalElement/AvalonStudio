using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.Tests
{
    class TestLanguageService : ILanguageService
    {
        public TestLanguageService()
        {
        }

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators { get; set; } = new Dictionary<string, Func<string, string>>();

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables { get; set; } = new Dictionary<string, Func<int, int, int, string>>(); 

        public string LanguageId => "cpp";

        public IEnumerable<char> IntellisenseSearchCharacters => throw new NotImplementedException();

        public IEnumerable<char> IntellisenseCompleteCharacters => throw new NotImplementedException();

        public IEnumerable<ITextEditorInputHelper> InputHelpers => throw new NotImplementedException();

        public ISyntaxHighlightingProvider SyntaxHighlighter => throw new NotImplementedException();

        public bool CanHandle(IEditor editor)
        {
            throw new NotImplementedException();
        }

        public bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            throw new NotImplementedException();
        }

        public Task<CodeCompletionResults> CodeCompleteAtAsync(IEditor editor, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            throw new NotImplementedException();
        }

        public int Comment(ITextEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public int Format(ITextEditor editor, uint offset, uint length, int cursor)
        {
            throw new NotImplementedException();
        }        

        public IEnumerable<IContextActionProvider> GetContextActionProviders(IEditor editor)
        {
            throw new NotImplementedException();
        }

        public Task<QuickInfoResult> QuickInfo(IEditor editor, List<UnsavedFile> unsavedFiles, int offset)
        {
            throw new NotImplementedException();
        }

        public Task<List<Symbol>> GetSymbolsAsync(IEditor editor, List<UnsavedFile> unsavedFiles, string name)
        {
            throw new NotImplementedException();
        }

        public Task<GotoDefinitionInfo> GotoDefinition(IEditor editor, int offset)
        {
            throw new NotImplementedException();
        }

        public bool IsValidIdentifierCharacter(char data)
        {
            throw new NotImplementedException();
        }

        public void RegisterSourceFile(ITextEditor editor)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(IEditor editor, string renameTo)
        {
            throw new NotImplementedException();
        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(ITextEditor editor, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            throw new NotImplementedException();
        }

        public Task<SignatureHelp> SignatureHelp(IEditor editor, List<UnsavedFile> unsavedFiles, int offset, string methodName)
        {
            throw new NotImplementedException();
        }

        public int UnComment(ITextEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public void UnregisterSourceFile(IEditor editor)
        {
            throw new NotImplementedException();
        }
    }
}
