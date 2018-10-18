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

        public bool CanHandle(ITextEditor editor)
        {
            throw new NotImplementedException();
        }

        public bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            throw new NotImplementedException();
        }

        public Task<CodeCompletionResults> CodeCompleteAtAsync(int index, int line, int column, IEnumerable<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            throw new NotImplementedException();
        }

        public int Comment(int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public int Format(uint offset, uint length, int cursor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContextActionProvider> GetContextActionProviders()
        {
            throw new NotImplementedException();
        }

        public Task<QuickInfoResult> QuickInfo(IEnumerable<UnsavedFile> unsavedFiles, int offset)
        {
            throw new NotImplementedException();
        }

        public Task<List<Symbol>> GetSymbolsAsync(IEnumerable<UnsavedFile> unsavedFiles, string name)
        {
            throw new NotImplementedException();
        }

        public Task<GotoDefinitionInfo> GotoDefinition(int offset)
        {
            throw new NotImplementedException();
        }

        public bool IsValidIdentifierCharacter(char data)
        {
            throw new NotImplementedException();
        }

        public void RegisterEditor(ITextEditor editor)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(string renameTo)
        {
            throw new NotImplementedException();
        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEnumerable<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            throw new NotImplementedException();
        }

        public Task<SignatureHelp> SignatureHelp(IEnumerable<UnsavedFile> unsavedFiles, int offset, string methodName)
        {
            throw new NotImplementedException();
        }

        public int UnComment(int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public void UnregisterEditor()
        {
            throw new NotImplementedException();
        }
    }
}
