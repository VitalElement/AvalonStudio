using AvalonStudio.Languages;
using System;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaEdit.Document;
using AvalonStudio.Editor;
using AvalonStudio.Documents;
using AvaloniaEdit.Indentation;

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

        public IIndentationStrategy IndentationStrategy => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public string Identifier => throw new NotImplementedException();

        public IEnumerable<char> IntellisenseSearchCharacters => throw new NotImplementedException();

        public IEnumerable<char> IntellisenseCompleteCharacters => throw new NotImplementedException();

        public IEnumerable<ICodeEditorInputHelper> InputHelpers => throw new NotImplementedException();

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

        public int Comment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public int Format(IEditor editor, uint offset, uint length, int cursor)
        {
            throw new NotImplementedException();
        }

        public Task<Symbol> GetSymbolAsync(IEditor editor, List<UnsavedFile> unsavedFiles, int offset)
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

        public void RegisterSourceFile(IEditor editor)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(IEditor editor, string renameTo)
        {
            throw new NotImplementedException();
        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEditor editor, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            throw new NotImplementedException();
        }

        public Task<SignatureHelp> SignatureHelp(IEditor editor, List<UnsavedFile> unsavedFiles, int offset, string methodName)
        {
            throw new NotImplementedException();
        }

        public int UnComment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public void UnregisterSourceFile(IEditor editor)
        {
            throw new NotImplementedException();
        }
    }
}
